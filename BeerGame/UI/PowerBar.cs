using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PowerBar : MonoBehaviour
{
    [SerializeField] private float barChangeSpeed = 1;
    [SerializeField] private float maxPowerBarValue = 10;

    [Space]
    [SerializeField] private Image powerBarMask;

    private bool isPowerBarOn = true;
    private bool powerIsIncreasing = true;
    private float currentPowerBarValue = 0;

    public static Action OnStartPowerBar;
    public static Action<float, float> OnGotPowerValue;

    private void OnEnable()
    {
        Coin.OnEndCoinRotate += StartPowerBar;
        Coin.OnMoveCoin += StopPowerBar;
        GameInfo.OnGameInfoPanelIsClosed += AssignDefaultValue;
    }

    private void OnDisable()
    {
        Coin.OnEndCoinRotate -= StartPowerBar;
        Coin.OnMoveCoin -= StopPowerBar;
        GameInfo.OnGameInfoPanelIsClosed -= AssignDefaultValue;
    }

    private IEnumerator UpdatePowerBar()
    {
        yield return new WaitForSeconds(0.1f);

        OnStartPowerBar?.Invoke();

        while (isPowerBarOn)
        {
            if (!powerIsIncreasing)
            {
                currentPowerBarValue -= barChangeSpeed;

                if (currentPowerBarValue <= 0)
                {
                    powerIsIncreasing = true;
                }
            }
            else
            {
                currentPowerBarValue += barChangeSpeed;

                if (currentPowerBarValue >= maxPowerBarValue)
                {
                    powerIsIncreasing = false;
                }
            }

            float fill = currentPowerBarValue / maxPowerBarValue;
            powerBarMask.fillAmount = fill;
            yield return new WaitForSeconds(0.02f);
        }

        yield return null;
    }

    private void StopPowerBar()
    {
        isPowerBarOn = false;
        OnGotPowerValue?.Invoke(currentPowerBarValue, maxPowerBarValue);
    }

    private void StartPowerBar()
    {
        StartCoroutine(UpdatePowerBar());
    }

    private void AssignDefaultValue()
    {
        isPowerBarOn = true;
        currentPowerBarValue = 0;
        powerBarMask.fillAmount = 1;
    }
}

