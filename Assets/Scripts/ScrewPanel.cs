using System.Collections.Generic;
using UnityEngine;

public class ScrewPanel : MonoBehaviour
{
    [Header("Panel Settings")]
    public ScrewColor panelColor;
    private int filledHoles = 0;    

    private List<Transform> holeTransforms = new();

    private List<Screw> assignedScrews = new();

    void Start()
    {
        foreach (Transform child in transform)
        {
            holeTransforms.Add(child);
        }
    }

    public Transform GetHole()
    {
        if (filledHoles < holeTransforms.Count)
            return holeTransforms[filledHoles];
        return null;
    }
    
    public Transform AssignToHole(Screw screw)
    {
        if (filledHoles >= holeTransforms.Count)
            return null;

        Transform assignedHole = holeTransforms[filledHoles];
        assignedScrews.Add(screw);
        filledHoles++;

        return assignedHole;
    }
    
    public Transform GetNextAvailableHole()
    {
        if (filledHoles < holeTransforms.Count)
            return holeTransforms[filledHoles];
        return null;
    }
    
    public bool IsFull()
    {
        return filledHoles >= holeTransforms.Count;
    }

    public void SetActivePanel(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
