using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Screw : MonoBehaviour
{
    [Header("Unscrewing Settings")]
    public ScrewColor screwColor;            
    public float rotationSpeed = 100f;
    public float moveUpSpeed = 1f;    
    public float totalRotation = 720f;
    public float totalMoveUp = 0.2f;  

    [Header("Movement to Hole Settings")]
    public float moveTowardsHoleSpeed = 2f;

    public static event Action<Screw> OnScrewClicked;

    private bool isUnscrewing = false;
    private Transform targetHole;
    
    public List<Plank> attachedPlanks = new List<Plank>();
    public bool isUnscrewed = false;

    void OnMouseDown()
    {
        if (isUnscrewing) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
        {
            OnScrewClicked?.Invoke(this);
        }
    }

    public async UniTask AssignHoleAsync(Transform hole)
    {
        if (hole == null)
        {
            return;
        }

        targetHole = hole;
        UnparentFromScrewPanel();
        await MoveToHoleAsync();
    }

    public UniTask MoveToHoleAsync()
    {
        return UnscrewAsync();
    }

    private void UnparentFromScrewPanel()
    {
        transform.parent = null;
    }

    private void SetInitialRotation()
    {
        transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }
    
    private async UniTask UnscrewAsync()
    {
        isUnscrewing = true;

        Sequence unscrewSequence = DOTween.Sequence();

        unscrewSequence.Append(transform.DORotate(new Vector3(0f, totalRotation, 0f), totalRotation / rotationSpeed, RotateMode.LocalAxisAdd)
                                       .SetEase(Ease.Linear));

        unscrewSequence.Join(transform.DOLocalMove(transform.localPosition + transform.up * totalMoveUp, totalMoveUp / moveUpSpeed)
                                     .SetEase(Ease.Linear));

        await unscrewSequence.AsyncWaitForCompletion();

        isUnscrewing = false;
        isUnscrewed = true;
        
        foreach (var plank in attachedPlanks)
        {
            plank.OnScrewUnscrewed(this);
        }
        
        SetInitialRotation();
        await MoveToAssignedHoleAsync();
    }

    private async UniTask MoveToAssignedHoleAsync()
    {
        if (targetHole == null)
        {
            return;
        }
        transform.parent = targetHole;

        Tween moveTween = transform.DOLocalMove(Vector3.zero, moveTowardsHoleSpeed)
            .SetEase(Ease.Linear);

        await moveTween.AsyncWaitForCompletion();

        
    }
}