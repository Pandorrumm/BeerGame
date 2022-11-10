using UnityEngine;
using DG.Tweening;

public class UpdateCoinScale : MonoBehaviour
{
    [SerializeField] private float maxScale;

    private void OnEnable()
    {
        Coin.OnChangeCoinScale += UpdateScale;
    }

    private void OnDisable()
    {
        Coin.OnChangeCoinScale -= UpdateScale;
    }

    private void UpdateScale(float _duration)
    {
        Vector3 initialScale = transform.localScale;

        transform.DOScale(initialScale * maxScale, _duration)
                .OnComplete(() =>
                {
                    transform.DOScale(initialScale, _duration);
                });
    }
}

