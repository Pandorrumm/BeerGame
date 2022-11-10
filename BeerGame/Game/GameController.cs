using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Color> colorPlayers = new List<Color>();

    [Space]
    [SerializeField] private GameSettings gameSettings;

    private int indexCurrentPlayer = -1;

    public static Action OnPlayerChangeEnd;
    public static Action<int> OnPlayerSelected;
    public static Action<Color> OnPlayerColorSelected;
    public static Action OnPlayersHaveMadeTheirMove;

    private void OnEnable()
    {
        ScoreCounter.OnContinuationOfGame += PlayerChange;
    }

    private void OnDisable()
    {
        ScoreCounter.OnContinuationOfGame -= PlayerChange;
    }

    private void Start()
    {
        PlayerChange();
    }

    private void PlayerChange()
    {
        indexCurrentPlayer++;

        if (indexCurrentPlayer == colorPlayers.Count)
        {
            indexCurrentPlayer = 0;

            OnPlayersHaveMadeTheirMove?.Invoke();
        }

        OnPlayerSelected?.Invoke(indexCurrentPlayer);
        OnPlayerColorSelected?.Invoke(colorPlayers[indexCurrentPlayer]);
        OnPlayerChangeEnd?.Invoke();
    }

    public Color GetPlayerColorByIndex(int _index)
    {
        return colorPlayers[_index];
    }

    public int GetNumberOfPlayers()
    {
        return colorPlayers.Count;
    }
}

