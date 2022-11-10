using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransformObjectChanger : MonoBehaviour
{
    [System.Serializable]
    public class DataToChange
    {
        public Transform objectTransform;
        public TypeChangeableValue changeableValue;
        public float magnificationOffsetValue;
        public float duration;

        [HideInInspector]
        public List<Vector3> playersTransformDatas = new List<Vector3>();
        [HideInInspector]
        public List<bool> isItAllowedToTransformChanges = new List<bool>();
        [HideInInspector]
        public Vector3 currentTransformValue;
        [HideInInspector]
        public Vector3 startingTransformValue;
        [HideInInspector]
        public bool isItAllowedToTransformChange = false;
    }

    public enum TypeChangeableValue
    {
        POSITION,
        SCALE
    }

    [SerializeField] private GameController gameController;

    [Space]
    [SerializeField] private List<DataToChange> changeableDatas = new List<DataToChange>();

    private int currentPlayerIndex;
    private Tweener tweener;
    private Sequence sequence;

    private void OnEnable()
    {
        GameController.OnPlayerSelected += AssignTransformValueCurrentPlayer;
        Coin.OnPlayerDidNotThrowCoinIntoGlass += AddNewValue;
        GameInfo.OnGameInfoPanelIsClosed += StartChangeTransform;
        Coin.OnPlayerDidThrowCoinIntoGlass += StopChangeTransform;
        GameInfo.OnGameInfoPanelIsOpen += ResetTransformValue;
        DrunkennessBar.OnDrunkennessBarFilledIn += ProhibitChangeTransform;
    }

    private void OnDisable()
    {
        GameController.OnPlayerSelected -= AssignTransformValueCurrentPlayer;
        Coin.OnPlayerDidNotThrowCoinIntoGlass -= AddNewValue;
        GameInfo.OnGameInfoPanelIsClosed -= StartChangeTransform;
        Coin.OnPlayerDidThrowCoinIntoGlass -= StopChangeTransform;
        GameInfo.OnGameInfoPanelIsOpen -= ResetTransformValue;
        DrunkennessBar.OnDrunkennessBarFilledIn -= ProhibitChangeTransform;
    }

    private void Start()
    {
        for (int i = 0; i < changeableDatas.Count; i++)
        {
            if (changeableDatas[i].changeableValue == TypeChangeableValue.POSITION)
            {
                changeableDatas[i].startingTransformValue = changeableDatas[i].objectTransform.position;

                for (int y = 0; y < gameController.GetNumberOfPlayers(); y++)
                {
                    changeableDatas[i].playersTransformDatas.Add(Vector3.zero);
                    changeableDatas[i].isItAllowedToTransformChanges.Add(true);
                }
            }

            if (changeableDatas[i].changeableValue == TypeChangeableValue.SCALE)
            {
                changeableDatas[i].startingTransformValue = changeableDatas[i].objectTransform.localScale;

                for (int y = 0; y < gameController.GetNumberOfPlayers(); y++)
                {
                    changeableDatas[i].playersTransformDatas.Add(changeableDatas[i].startingTransformValue);
                    changeableDatas[i].isItAllowedToTransformChanges.Add(true);
                }
            }
        }
    }

    private void AssignTransformValueCurrentPlayer(int _index)
    {
        currentPlayerIndex = _index;

        for (int i = 0; i < changeableDatas.Count; i++)
        {
            changeableDatas[i].currentTransformValue = changeableDatas[i].playersTransformDatas[currentPlayerIndex];
            changeableDatas[i].isItAllowedToTransformChange = changeableDatas[i].isItAllowedToTransformChanges[currentPlayerIndex];
        }
    }

    private void AddNewValue()
    {
        StopChangeTransform();

        for (int i = 0; i < changeableDatas.Count; i++)
        {
            if (changeableDatas[i].isItAllowedToTransformChange)
            {
                Vector3 newValue = new Vector3(changeableDatas[i].currentTransformValue.x + changeableDatas[i].magnificationOffsetValue,
                                                changeableDatas[i].currentTransformValue.y + changeableDatas[i].magnificationOffsetValue,
                                                changeableDatas[i].currentTransformValue.z);

                changeableDatas[i].playersTransformDatas[currentPlayerIndex] = newValue;
            }
        }
    }

    private void StartChangeTransform()
    {
        for (int i = 0; i < changeableDatas.Count; i++)
        {
            if (changeableDatas[i].changeableValue == TypeChangeableValue.POSITION)
            {
                if (changeableDatas[i].currentTransformValue != Vector3.zero)
                {
                    tweener = changeableDatas[i].objectTransform.DOShakePosition(changeableDatas[i].duration, changeableDatas[i].currentTransformValue, 1, 90, false, false).SetLoops(-1);
                }
            }

            if (changeableDatas[i].changeableValue == TypeChangeableValue.SCALE)
            {
                if (changeableDatas[i].currentTransformValue != new Vector3(1, 1, 1))
                {
                    sequence = DOTween.Sequence();
                    sequence.Append(changeableDatas[i].objectTransform.DOScale(new Vector3(changeableDatas[i].currentTransformValue.x, 1, 1), changeableDatas[i].duration));
                    sequence.Append(changeableDatas[i].objectTransform.DOScale(changeableDatas[i].startingTransformValue, changeableDatas[i].duration));
                    sequence.Append(changeableDatas[i].objectTransform.DOScale(new Vector3(1, changeableDatas[i].currentTransformValue.y, 1), changeableDatas[i].duration));
                    sequence.Append(changeableDatas[i].objectTransform.DOScale(changeableDatas[i].startingTransformValue, changeableDatas[i].duration));
                    sequence.SetLoops(-1);
                }
            }
        }
    }

    private void StopChangeTransform()
    {
        for (int i = 0; i < changeableDatas.Count; i++)
        {
            if (changeableDatas[i].changeableValue == TypeChangeableValue.POSITION)
            {
                tweener.Kill();
            }

            if (changeableDatas[i].changeableValue == TypeChangeableValue.SCALE)
            {
                sequence.Kill();
            }
        }
    }

    private void ResetTransformValue()
    {
        for (int i = 0; i < changeableDatas.Count; i++)
        {
            if (changeableDatas[i].changeableValue == TypeChangeableValue.POSITION)
            {
                changeableDatas[i].objectTransform.position = changeableDatas[i].startingTransformValue;
            }

            if (changeableDatas[i].changeableValue == TypeChangeableValue.SCALE)
            {
                changeableDatas[i].objectTransform.localScale = changeableDatas[i].startingTransformValue;
            }
        }
    }

    private void ProhibitChangeTransform()
    {
        for (int i = 0; i < changeableDatas.Count; i++)
        {
            changeableDatas[i].isItAllowedToTransformChanges[currentPlayerIndex] = false;
        }
    }
}

