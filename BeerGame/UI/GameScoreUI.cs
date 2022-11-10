using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameScoreUI : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [Space]
    [SerializeField] private List<Image> playersImage;

    [Space]
    [SerializeField] private TMP_Text scoreFirstPlayer;
    [SerializeField] private TMP_Text scoreSecondPlayer;

    private void OnEnable()
    {
        ScoreCounter.OnPlayersReceivedPoints += GetScore;
    }

    private void OnDisable()
    {
        ScoreCounter.OnPlayersReceivedPoints -= GetScore;
    }

    private void Start()
    {
        for (int i = 0; i < playersImage.Count; i++)
        {
            playersImage[i].color = gameController.GetPlayerColorByIndex(i);
        }
    }

    private void GetScore(int _firstPlayerScore, int _seconddPlayerScore)
    {
        scoreFirstPlayer.text = _firstPlayerScore.ToString();
        scoreSecondPlayer.text = _seconddPlayerScore.ToString();
    }
}

