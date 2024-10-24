using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("Screw Panels")]
    public List<ScrewPanel> screwPanels;         // Assign via Inspector in the order of activation
    public ScrewPanel baseScrewPanel;            // Assign via Inspector (last panel)

    [Header("Game Settings")]
    public int totalPanels = 4;

    private int currentPanelIndex = 0;
    private bool gameOver = false;

    [SerializeField] private Transform coloredPanels;

    void OnEnable()
    {
        Screw.OnScrewClicked += HandleScrewClicked;
    }

    void OnDisable()
    {
        Screw.OnScrewClicked -= HandleScrewClicked;
    }

    private async void HandleScrewClicked(Screw screw)
    {
        if (gameOver)
            return;

        ScrewPanel currentPanel = screwPanels[currentPanelIndex];

        if (screw.screwColor == currentPanel.panelColor)
        {
            Transform assignedHole = currentPanel.AssignToHole(screw);
            if (assignedHole == null)
            {
                Debug.LogWarning("No available hole in the current panel.");
                return;
            }

            await screw.AssignHoleAsync(assignedHole);

            if (currentPanel.IsFull())
            {
                await MovePanelsAsync();
            }
        }
        else
        {
            await AssignToBasePanelAsync(screw);
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
        
        await screw.AssignHoleAsync(assignedHole);
        
        if (baseScrewPanel.IsFull())
        {
            PlayerLoses();
        }
        
        await UniTask.CompletedTask;
    }

    private async UniTask MovePanelsAsync()
    {
        Tween moveTween = coloredPanels.DOMoveX(coloredPanels.position.x + 0.5f, 0.5f)
            .SetEase(Ease.Linear);

        await moveTween.AsyncWaitForCompletion();

        currentPanelIndex++;
        
        if (currentPanelIndex >= screwPanels.Count)
        {
            // Implement logic for when all panels are activated
            Debug.Log("All panels activated!");
            // For example, you might reset the game or trigger a victory condition
        }
    }

    private void PlayerLoses()
    {
        gameOver = true;
        Debug.Log("Game Over! Base ScrewPanel is fully filled.");
    }
}
