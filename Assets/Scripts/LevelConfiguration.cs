using UnityEngine;

public class LevelConfiguration : MonoBehaviour
{
    [SerializeField] private LevelData levelData;

    public LevelData LevelData => levelData;
}