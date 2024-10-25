using System.Collections.Generic;
using UnityEngine;

public class ScrewPanel : MonoBehaviour
{
    [Header("Panel Settings")]
    public ScrewColor panelColor;
    public int filledHoles = 0;    

    public List<Transform> holeTransforms = new();

    public List<Screw> assignedScrews = new();

    void Start()
    {
        foreach (Transform child in transform)
        {
            holeTransforms.Add(child);
        }
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
    
    public bool IsFull()
    {
        return filledHoles >= holeTransforms.Count;
    }
    
    public void RemoveScrew(Screw screw)
    {
        assignedScrews.Remove(screw);
        filledHoles--;
    }
}
