using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public class Coin : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IPointerClickHandler
{
    [SerializeField] private GameObject glass;

    [Space]
    [SerializeField] private GameObject arrow;

    [Space]
    [SerializeField] private float winningDistance;

    [Space]
    [SerializeField] private float durationOfMovement;

    private float currentPowerBarValue;
    private float maxPowerBarValue;
    private Animator coinAnimator;
    private float distanceToGlass;
    private Vector3 arrowStartPosition;
    private bool isCoinTurns = false;
    private bool isPointerMove = false;
    private bool isPointerClick = false;
    private bool isPointerDown = true;
    private Vector3 fingerPosition;
    private Vector2 directionOfThrow;
    private BoxCollider2D boxCollider2D;
    private Vector3 startCoinPosition;
    private float windPower;
    private int windDirectionCoefficient;
    private float newY;
    private int currentPlayerIndex;

    public static Action<int> OnPlayerGotPoint;
    public static Action OnEndCoinRotate;
    public static Action OnMoveCoin;
    public static Action OnPlayerDidNotThrowCoinIntoGlass;
    public static Action OnPlayerDidThrowCoinIntoGlass;
    public static Action<int> OnArrowActivated;
    public static Action<float> OnChangeCoinScale;

    private void OnEnable()
    {
        Wind.OnWindWasAppointed += ChangingAngleWithWind;
        GameInfo.OnGameInfoPanelIsClosed += FlipCoinAgain;
        GameController.OnPlayerSelected += GetPlayerIndex;
        PowerBar.OnStartPowerBar += AllowClickToThrow;
        PowerBar.OnGotPowerValue += MoveCoin;
    }

    private void OnDisable()
    {
        Wind.OnWindWasAppointed -= ChangingAngleWithWind;
        GameInfo.OnGameInfoPanelIsClosed -= FlipCoinAgain;
        GameController.OnPlayerSelected -= GetPlayerIndex;
        PowerBar.OnStartPowerBar -= AllowClickToThrow;
        PowerBar.OnGotPowerValue -= MoveCoin;
    }

    private void ChangingAngleWithWind(float _windPower, int _windDirectionCoefficient)
    {
        windPower = _windPower;
        windDirectionCoefficient = _windDirectionCoefficient;
    }

    private void Start()
    {
        startCoinPosition = transform.position;
        coinAnimator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isCoinTurns)
        {
            directionOfThrow = new Vector2(arrowStartPosition.x, arrowStartPosition.y) - new Vector2(fingerPosition.x, fingerPosition.y);
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, directionOfThrow);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 200f * Time.deltaTime);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPointerDown)
        {
            arrow.SetActive(true);

            OnArrowActivated?.Invoke(currentPlayerIndex);

            arrowStartPosition = arrow.transform.position;

            isPointerMove = true;
            isPointerDown = false;
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (isPointerMove)
        {
            fingerPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            isCoinTurns = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isCoinTurns = false;
        isPointerMove = false;
        arrow.SetActive(false);

        OnEndCoinRotate?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isPointerClick)
        {
            boxCollider2D.enabled = false;
            OnMoveCoin?.Invoke();
        }
    }

    private void AllowClickToThrow()
    {
        isPointerClick = true;
    }

    private void MoveCoin(float _powerBarValue, float _maxPowerBarValue)
    {
        float duration = durationOfMovement;

        currentPowerBarValue = _powerBarValue;
        maxPowerBarValue = _maxPowerBarValue;

        if (currentPowerBarValue < maxPowerBarValue / 2)
        {
            duration = durationOfMovement / 2;
        }

        coinAnimator.CrossFade("Coin_Rotation", 0.2f);

        isPointerClick = false;

        if (directionOfThrow == Vector2.zero)
        {
            newY = transform.position.y + currentPowerBarValue;
            transform.DOMove(new Vector3(transform.position.x + (windDirectionCoefficient * windPower / 2), newY, 0), duration).OnComplete(CheckingPositionRelativeToGlass);
        }
        else
        {
            directionOfThrow.Normalize();

            Vector3 target = new Vector3(directionOfThrow.x, directionOfThrow.y, 0) * currentPowerBarValue + transform.position;

            transform.DOMove(new Vector3(target.x + (windDirectionCoefficient * windPower / 2), target.y, 0), duration).OnComplete(CheckingPositionRelativeToGlass);
        }

        OnChangeCoinScale(durationOfMovement / 3);
    }

    private void CheckingPositionRelativeToGlass()
    {
        coinAnimator.CrossFade("Coin_Idle", 0.2f);

        distanceToGlass = Vector3.Distance(transform.position, glass.transform.position);

        if (distanceToGlass > winningDistance)
        {
            OnPlayerDidNotThrowCoinIntoGlass?.Invoke();

            DOTween.Sequence()
                 .AppendInterval(1.5f)
                 .AppendCallback(() => OnPlayerGotPoint?.Invoke(0));
        }
        else
        {
            OnPlayerDidThrowCoinIntoGlass?.Invoke();

            DOTween.Sequence()
                 .AppendInterval(1f)
                 .AppendCallback(() => OnPlayerGotPoint?.Invoke(1));
        }
    }

    private void GetPlayerIndex(int _index)
    {
        currentPlayerIndex = _index;
    }

    private void FlipCoinAgain()
    {
        isCoinTurns = false;
        isPointerMove = false;
        isPointerClick = false;
        isPointerDown = true;

        boxCollider2D.enabled = true;

        directionOfThrow = Vector2.zero;

        transform.position = startCoinPosition;
        transform.rotation = Quaternion.identity;
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}

