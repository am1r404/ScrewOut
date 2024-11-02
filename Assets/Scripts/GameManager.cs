using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject]
    private LevelLoader _levelLoader;
    
    [Inject] SceneLoader _sceneLoader;
    
    [Header("Screw Panels")]
    public List<ScrewPanel> screwPanels;
    public ScrewPanel baseScrewPanel;

    [Header("Game Settings")]
    public int totalPanels = 4;

    private int currentPanelIndex = 0;
    private bool gameOver = false;

    [SerializeField] public Transform coloredPanels;
    private List<UniTask> ongoingScrewAnimations = new List<UniTask>();
    
    private bool isPanelMoving = false;
    [Inject]
    DiContainer _container;

    [Inject]
    private LevelProgressionService _levelProgressionService; 

    void OnEnable()
    {
        Screw.OnScrewClicked += HandleScrewClicked;

        _levelLoader.OnLevelLoaded += HandleLevelLoaded;
    }

    private void Start()
    {
        
        SetupPanels(_levelLoader.GetCurrentLevelInstance());
    }

    void OnDisable()
    {
        Screw.OnScrewClicked -= HandleScrewClicked;

        _levelLoader.OnLevelLoaded -= HandleLevelLoaded;
    }

    private void HandleScrewClicked(Screw screw)
    {
        if (gameOver)
            return;

        AssignScrewAsync(screw).Forget(e => Debug.LogException(e));
    }
    
    private async UniTask AssignScrewAsync(Screw screw)
    {
        ScrewPanel currentPanel = screwPanels[currentPanelIndex];

        if (screw.screwColor == currentPanel.panelColor)
        {
            Transform assignedHole = currentPanel.AssignToHole(screw);
            if (assignedHole == null)
            {
                Debug.LogWarning("No available hole in the current panel.");
                return;
            }

            var screwAnimationTask = screw.AssignHoleAsync(assignedHole);
            ongoingScrewAnimations.Add(screwAnimationTask);
            
            if (currentPanel.IsFull())
            {
                await SchedulePanelMovement();
            }
        }
        else
        {
            await AssignToBasePanelAsync(screw);
        }
    }
    
    private async UniTask WaitForAllScrewAnimations()
    {
        if (ongoingScrewAnimations.Count > 0)
        {
            var tasks = ongoingScrewAnimations.ToArray();
            ongoingScrewAnimations.Clear();
            await UniTask.WhenAll(tasks);
        }
        else
        {
            await UniTask.CompletedTask;
        }
    }

    private async UniTask AssignToBasePanelAsync(Screw screw)
    {
        if (baseScrewPanel.IsFull())
        {
            PlayerLoses();
            return;
        }
        
        Transform assignedHole = baseScrewPanel.AssignToHole(screw);
        if (assignedHole == null)
        {
            Debug.LogWarning("No available hole in the base panel.");
            return;
        }
        
        var screwAnimationTask = screw.AssignHoleAsync(assignedHole);
        ongoingScrewAnimations.Add(screwAnimationTask);
    }

    private async UniTask MovePanelsAsync()
    {
        Tween moveTween = coloredPanels.DOMoveX(coloredPanels.position.x + 0.5f, 0.5f)
            .SetEase(Ease.Linear);

        await moveTween.AsyncWaitForCompletion();

        currentPanelIndex++;
        if (currentPanelIndex >= screwPanels.Count)
        {
            return;
        }
        
        await MoveMatchingScrewsFromBasePanelAsync();
    }
    
    private async UniTask MoveMatchingScrewsFromBasePanelAsync()
    {
        ScrewPanel currentPanel = screwPanels[currentPanelIndex];
        List<Screw> screwsToMove = baseScrewPanel.assignedScrews
            .Where(screw => screw.screwColor == currentPanel.panelColor)
            .ToList();

        foreach (var screw in screwsToMove)
        {
            baseScrewPanel.RemoveScrew(screw);

            Transform assignedHole = currentPanel.AssignToHole(screw);

            if (assignedHole != null)
            {
                var screwTask = screw.AssignHoleAsync(assignedHole);
                ongoingScrewAnimations.Add(screwTask);
            }
            else
            {
                baseScrewPanel.AssignToHole(screw);
            }
        }
    }
    
    private async UniTask SchedulePanelMovement()
    {
        if (isPanelMoving)
        {
            return;
        }
        
        isPanelMoving = true;
        try
        {
            await WaitForAllScrewAnimations();

            while (currentPanelIndex + 1 < screwPanels.Count)
            {
                await MovePanelsAsync();
                await WaitForAllScrewAnimations();

                ScrewPanel currentPanel = screwPanels[currentPanelIndex];

                if (!currentPanel.IsFull())
                {
                    break;
                }
            }

            if (currentPanelIndex + 1 >= screwPanels.Count && screwPanels[currentPanelIndex].IsFull())
            {
                Debug.Log("Last panel is filled. Panels will not move.");
                OnLevelCompleted();
            }
        }
        finally
        {
            isPanelMoving = false;
        }
    }
    
    public void SetupPanels(GameObject levelInstance)
    {
        LevelData levelData = Resources.Load("Levels/Data" + (_levelProgressionService.CurrentLevel + 1)) as LevelData;

        if (levelData == null)
        {
            return;
        }

        int i = 0;
        screwPanels.Clear();

        foreach (var panelData in levelData.panels)
        {
            var panel = _container.InstantiatePrefabForComponent<ScrewPanel>(
                Resources.Load("ColoredPanel"), 
                coloredPanels.position, 
                Quaternion.identity, 
                coloredPanels);

            panel.transform.localPosition += new Vector3(i * -0.5f, 0, 0);
            panel.panelColor = panelData.panelColor;

            screwPanels.Add(panel);

            i++;
        }
    }
    
    public void OnLevelCompleted()
    {
        _levelProgressionService.AdvanceToNextLevel();
        _sceneLoader.LoadSceneAsync("MainMenu");
    }

    private void PlayerLoses()
    {
        gameOver = true;
    }

    private void HandleLevelLoaded()
    {
        foreach (var panel in screwPanels)
        {
            if (panel != null)
            {
                Destroy(panel.gameObject);
            }
        }

        screwPanels.Clear();
        currentPanelIndex = 0;
        gameOver = false;
        SetupPanels(_levelLoader.GetCurrentLevelInstance());
    }

    private void LoadMainMenu()
    {
        _sceneLoader.LoadSceneAsync("MainMenu");
    }

}
