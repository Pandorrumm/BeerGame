using UnityEngine;

[CreateAssetMenu(menuName = "MultiGames/DropCoinIntoGlass/Settings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private int numberOfPointsToWin;
    public int NumberOfPointsToWin => numberOfPointsToWin;

    [SerializeField] private float maxDrunkennessValue;
    public float MaxDrunkennessValue => maxDrunkennessValue;
}

