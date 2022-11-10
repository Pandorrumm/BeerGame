using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ArrowColor : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Coin.OnArrowActivated += AssignArrowColor;
    }

    private void OnDisable()
    {
        Coin.OnArrowActivated -= AssignArrowColor;
    }

    private void AssignArrowColor(int _index)
    {
        spriteRenderer.color = gameController.GetPlayerColorByIndex(_index);
    }
}

