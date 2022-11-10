using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Wind : MonoBehaviour
{
    [SerializeField] private Image windDirectionImage;
    [SerializeField] private Sprite leftArrowSprite;
    [SerializeField] private Sprite rightArrowSprite;
    [SerializeField] private TMP_Text windPowerText;
    [SerializeField] private int maxWindPower;

    private int windDirectionCoefficient;

    public static Action<float, int> OnWindWasAppointed;

    private void OnEnable()
    {
        GameInfo.OnGameInfoPanelIsOpen += AssignWindDatas;
    }

    private void OnDisable()
    {
        GameInfo.OnGameInfoPanelIsOpen -= AssignWindDatas;
    }

    private void AssignWindDatas()
    {
        int randomValueWindDirection = UnityEngine.Random.Range(0, 2);
        int randomValueWindPower = UnityEngine.Random.Range(0, maxWindPower);

        windPowerText.text = randomValueWindPower.ToString();

        ColorWindImageChange(randomValueWindPower);

        if (randomValueWindDirection == 0)
        {
            windDirectionImage.sprite = leftArrowSprite;
            windDirectionCoefficient = -1;
        }
        else
        {
            windDirectionImage.sprite = rightArrowSprite;
            windDirectionCoefficient = 1;
        }

        OnWindWasAppointed?.Invoke(randomValueWindPower, windDirectionCoefficient);
    }

    private void ColorWindImageChange(int _windPower)
    {
        if (_windPower < maxWindPower / 2)
        {
            windDirectionImage.color = Color.green;
        }
        else if (_windPower == maxWindPower / 2)
        {
            windDirectionImage.color = Color.yellow;
        }
        else if (_windPower > maxWindPower / 2)
        {
            windDirectionImage.color = Color.red;
        }
    }
}

