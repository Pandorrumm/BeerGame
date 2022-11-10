using UnityEngine;
using System;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [Space]
    [SerializeField] private GameSettings gameSettings;

    [Space]
    [SerializeField] private int firstPlayerIndex;
    [SerializeField] private int secondPlayerIndex;

    private int scoreFirstPlayer;
    private int scoreSecondPlayer;
    private int currentPlayerIndex;

    public static Action OnContinuationOfGame;
    public static Action<int, int> OnPlayersReceivedPoints;
    public static Action<int> OnPlayerWin;

    private void OnEnable()
    {
        Coin.OnPlayerGotPoint += GetScore;
        GameController.OnPlayerSelected += GetIndexCurrentPlayer;
        GameController.OnPlayersHaveMadeTheirMove += CheckingPointsToWin;

        OnPlayersReceivedPoints?.Invoke(scoreFirstPlayer, scoreSecondPlayer);
    }

    private void OnDisable()
    {
        Coin.OnPlayerGotPoint -= GetScore;
        GameController.OnPlayerSelected -= GetIndexCurrentPlayer;
        GameController.OnPlayersHaveMadeTheirMove -= CheckingPointsToWin;
    }

    private void GetIndexCurrentPlayer(int _index)
    {
        currentPlayerIndex = _index;
    }

    private void GetScore(int _value)
    {
        if (currentPlayerIndex == firstPlayerIndex)
        {
            scoreFirstPlayer += _value;
        }
        else if (currentPlayerIndex == secondPlayerIndex)
        {
            scoreSecondPlayer += _value;
        }

        OnContinuationOfGame?.Invoke();
        OnPlayersReceivedPoints?.Invoke(scoreFirstPlayer, scoreSecondPlayer);
    }

    private void CheckingPointsToWin()
    {
        if (scoreFirstPlayer == gameSettings.NumberOfPointsToWin && scoreSecondPlayer == gameSettings.NumberOfPointsToWin)
        {
            OnPlayerWin?.Invoke(-1);
        }
        else if (scoreFirstPlayer == gameSettings.NumberOfPointsToWin)
        {
            OnPlayerWin?.Invoke(firstPlayerIndex);
        }
        else if (scoreSecondPlayer == gameSettings.NumberOfPointsToWin)
        {
            OnPlayerWin?.Invoke(secondPlayerIndex);
        }
    }
}

