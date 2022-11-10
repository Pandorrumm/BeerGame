using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DrunkennessBar : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [Space]
    [SerializeField] private GameSettings gameSettings;

    [Space]
    [SerializeField] private Image drunkennessBarMask;

    [Space]
    [SerializeField] private float barChangeSpeed;

    private List<float> drunkennessPlayers = new List<float>();
    private List<float> numberOfMisses = new List<float>();

    private float currentNumberOfMisses;
    private float currentDrunkennessValue = 0;
    private int currentPlayerIndex;
    private bool isDrunkennessBarOn = false;

    public static Action OnDrunkennessBarFilledIn;

    private void OnEnable()
    {
        GameController.OnPlayerSelected += AssignDrunkennessCurrentPlayer;
        Coin.OnPlayerDidNotThrowCoinIntoGlass += DrunkedPlayer;
    }

    private void OnDisable()
    {
        GameController.OnPlayerSelected -= AssignDrunkennessCurrentPlayer;
        Coin.OnPlayerDidNotThrowCoinIntoGlass -= DrunkedPlayer;
    }

    private void Start()
    {
        drunkennessBarMask.fillAmount = 0;

        GetStartDrunkennessPlayers();
    }

    private IEnumerator UpdateDrunkennessBar()
    {
        while (isDrunkennessBarOn)
        {
            currentDrunkennessValue += barChangeSpeed;

            if (currentDrunkennessValue >= currentNumberOfMisses / gameSettings.MaxDrunkennessValue)
            {
                drunkennessPlayers[currentPlayerIndex] = currentDrunkennessValue;
                isDrunkennessBarOn = false;
            }

            drunkennessBarMask.fillAmount = currentDrunkennessValue;
            yield return new WaitForSeconds(0.02f);
        }

        yield return null;
    }

    private void DrunkedPlayer()
    {
        CountingMisses();

        isDrunkennessBarOn = true;
        StartCoroutine(UpdateDrunkennessBar());
    }

    private void CountingMisses()
    {
        currentNumberOfMisses = numberOfMisses[currentPlayerIndex];
        currentNumberOfMisses++;

        if (currentNumberOfMisses > gameSettings.MaxDrunkennessValue)
        {
            currentNumberOfMisses--;

            OnDrunkennessBarFilledIn?.Invoke();
        }

        numberOfMisses[currentPlayerIndex] = currentNumberOfMisses;
    }

    private void GetStartDrunkennessPlayers()
    {
        for (int i = 0; i < gameController.GetNumberOfPlayers(); i++)
        {
            drunkennessPlayers.Add(0);
            numberOfMisses.Add(0);
        }
    }

    private void AssignDrunkennessCurrentPlayer(int _index)
    {
        currentPlayerIndex = _index;

        drunkennessBarMask.fillAmount = drunkennessPlayers[currentPlayerIndex];
        currentDrunkennessValue = drunkennessBarMask.fillAmount;
    }
}

