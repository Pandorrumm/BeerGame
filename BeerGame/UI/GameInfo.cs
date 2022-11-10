using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GameInfo : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [SerializeField] private GameObject gameInfoPanel;
    [SerializeField] private GameObject victoryPanel;

    [Space]
    [SerializeField] private Image playerIconImage;
    [SerializeField] private Image victoryPlayerImage;

    [Space]
    [SerializeField] private TMP_Text victoryPlayerText;

    [Space]
    [SerializeField] private Button closeInfoPanelButton;

    [Space]
    [SerializeField] private string winText;
    [SerializeField] private string drawText;

    private bool isEndGame;

    public static Action OnGameInfoPanelIsClosed;
    public static Action OnGameInfoPanelIsOpen;
    public static Action OnStartOverGame;

    private void OnEnable()
    {
        GameController.OnPlayerChangeEnd += OpenGameInfoPanel;
        GameController.OnPlayerColorSelected += AssignPlayerColor;
        ScoreCounter.OnPlayerWin += EndGame;
    }

    private void OnDisable()
    {
        GameController.OnPlayerChangeEnd -= OpenGameInfoPanel;
        GameController.OnPlayerColorSelected -= AssignPlayerColor;
        ScoreCounter.OnPlayerWin -= EndGame;
    }

    private void Start()
    {
        closeInfoPanelButton.onClick.AddListener(() => StatusGameInfoPanel(false));
        StatusGameInfoPanel(true);
    }

    private void StatusGameInfoPanel(bool _enabled)
    {
        gameInfoPanel.SetActive(_enabled);

        if (!_enabled)
        {
            OnGameInfoPanelIsClosed?.Invoke();
        }
        else
        {
            OnGameInfoPanelIsOpen?.Invoke();
        }
    }

    private void OpenGameInfoPanel()
    {
        StatusGameInfoPanel(true);
    }

    private void AssignPlayerColor(Color _playerColor)
    {
        if (!isEndGame)
        {
            playerIconImage.color = _playerColor;
        }
    }

    private void EndGame(int _index)
    {
        isEndGame = true;
        victoryPanel.SetActive(true);

        if (_index >= 0)
        {
            victoryPlayerImage.color = gameController.GetPlayerColorByIndex(_index);
            victoryPlayerText.text = winText;
        }
        else
        {
            victoryPlayerImage.color = Color.clear;
            victoryPlayerText.text = drawText;
        }
    }
}

