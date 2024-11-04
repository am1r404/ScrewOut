using System;
using System.Collections.Generic;
using UnityEngine;

public class Plank : MonoBehaviour
{
    public List<Screw> screws = new();
    private bool isFalling;

    void Start()
    {
        if (screws.Count != 0)
        {
            foreach (var screw in screws)
            {
                if (!screw.attachedPlanks.Contains(this))
                {
                    screw.attachedPlanks.Add(this);
                }
            }
        }
    }

    private void Update()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    public void OnScrewUnscrewed(Screw screw)
    {
        if (AllScrewsUnscrewed() && !isFalling)
        {
            isFalling = true;
            StartFalling();
        }
    }

    private bool AllScrewsUnscrewed()
    {
        foreach (var screw in screws)
        {
            if (!screw.isUnscrewed)
                return false;
        }
        return true;
    }

    private void StartFalling()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 1f;
        transform.parent = null;

        Debug.Log($"Plank '{gameObject.name}' is falling!");
    }
}