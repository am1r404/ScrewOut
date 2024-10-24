using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Screw : MonoBehaviour
{
    [Header("Unscrewing Settings")]
    public ScrewColor screwColor;            
    public float rotationSpeed = 100f;       // Degrees per second
    public float moveUpSpeed = 1f;           // Units per second
    public float totalRotation = 720f;       // Total degrees to rotate
    public float totalMoveUp = 0.2f;         // Total units to move up

    [Header("Movement to Hole Settings")]
    public float moveTowardsHoleSpeed = 2f;  // Seconds to move to the hole

    // Event to notify when the screw is clicked
    public static event Action<Screw> OnScrewClicked;

    private bool isUnscrewing = false;
    private Transform targetHole;

    void OnMouseDown()
    {
        if (!isUnscrewing)
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
        SetInitialRotation();
        await MoveToAssignedHoleAsync();
    }

    private async UniTask MoveToAssignedHoleAsync()
    {
        if (targetHole == null)
        {
            return;
        }

        Sequence moveToHoleSequence = DOTween.Sequence();

        moveToHoleSequence.Append(transform.DOMove(targetHole.position, moveTowardsHoleSpeed)
                                         .SetEase(Ease.Linear));

        await moveToHoleSequence.AsyncWaitForCompletion();

        transform.parent = targetHole;
    }
}