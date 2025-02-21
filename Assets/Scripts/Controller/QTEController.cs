using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;
using Sirenix.OdinInspector;
using Sirenix;
using TMPro;

public enum EQTEState
{
    None,
    Start,
    Input,
    End
}

public enum EQTEInputType
{
    Up,
    Down,
    Left,
    Right
}

[RequireComponent(typeof(ObservableEventTrigger))]
public class QTEController : SerializedMonoBehaviour
{
    public static QTEController instance;

    [SerializeField]
    public List<EQTEInputType[]> myQTEPattern;
    // public Dictionary<int, EQTEInputType[]> myQTEPattern;

    [SerializeField]
    QTEData qteData;

    Image inputPanel;
    ObservableEventTrigger eventTrigger;

    Vector2 startInputPos;
    Vector2 endInputPos;

    EQTEState currentState;
    EQTEInputType startInputType;
    EQTEInputType endInputType;

    bool onQTE;
    bool onInput;
    bool onFail;

    Coroutine checkTimeCoroutine;

    [SerializeField]
    TextMeshProUGUI guideText, first, second;

    public bool OnQTE
    {
        get => onQTE;
        set => onQTE = value;
    }

    void Awake()
    {
        InitProperty();
        DisableInputReader();
    }

    void InitProperty()
    {
        instance = this;

        onInput = true;

        eventTrigger = GetComponent<ObservableEventTrigger>();
        inputPanel = GetComponent<Image>();

        eventTrigger.OnPointerDownAsObservable().Subscribe(eventData => StartInput(eventData));
        eventTrigger.OnPointerUpAsObservable().Subscribe(eventData => EndInput(eventData));
    }

    void StartInput(PointerEventData data)
    {
        startInputPos = new Vector2(data.position.x / Screen.width, data.position.y / Screen.height);
    }

    void EndInput(PointerEventData data)
    {
        endInputPos = new Vector2(data.position.x / Screen.width, data.position.y / Screen.height);

        onInput = false;
    }
    
    public void StartQTE()
    {
        onQTE = true;

        SetQTEState(EQTEState.Start);
    }

    void SetQTEState(EQTEState state)
    {
        if (currentState == state)
            return;

        currentState = state;

        CheckQTEState();
    }

    void CheckQTEState()
    {
        switch (currentState)
        {
            case EQTEState.Start:
                QTEStart();
                break;
            case EQTEState.Input:
                QTEInput();
                break;
            case EQTEState.End:
                QTEEnd();
                break;
        }
    }

    void QTEStart()
    {
        StartCoroutine(QTEStartLogic());
    }

    IEnumerator QTEStartLogic()
    {
        float waitTime = 2f;

        onFail = false;

        #region Hide HUD
        HUD.instance.HideHUD();
        #endregion

        #region Hide Button
        ActionController.instance.HideCover();
        FeverController.instance.HideCover();
        TimeController.instance.HideCover();
        #endregion

        #region Show Guide
        HUD.instance.qteCorrect.ShowQTECorrectGuide();
        #endregion

        #region Stop Scrolling
        ScrollController.instance.StopScrolling();
        #endregion

        #region Animation
        Popup.instance.qteGuide.FadeInPanel();
        Popup.instance.qteGuide.AppearClock();
        #endregion

        #region Convert Camera
        CameraController.instance.ConvertCamera(ECameraType.Main, ECameraType.QTE);
        #endregion

        yield return new WaitForSeconds(waitTime);

        SetQTEState(EQTEState.Input);
    }

    void QTEInput()
    {
        StartCoroutine(QTEInputLogic());
    }

    IEnumerator QTEInputLogic()
    {
        int count = 0;
        float term = 1f;

        EnableInputReader();

        while (!onFail && count < qteData.maxCount)
        {
            #region Reset Clock Hand
            if (count >= 1)
            {
                float duration = term * 0.8f;

                Popup.instance.qteGuide.ResetClockHand(duration);
                Popup.instance.qteGuide.InitQTEGuide();
                guideText.text = "";

                yield return new WaitForSeconds(term);
            }
            #endregion

            #region Init Position
            InitPosition();
            #endregion

            #region Check Time
            CheckTime();
            #endregion

            #region Create Pattern
            CreatePattern();
            #endregion

            #region Guide QTE
            GuideQTE();
            #endregion

            yield return new WaitUntil(() => !onInput);

            if (InputIsCorrect())
            {
                count++;

                #region Play SFX
                SFXHandler.instance.PlaySFX(SFXDefine.CORRECT);
                #endregion

                #region Vibrate
                Vibrator.Vibrate(50);
                #endregion

                #region Set QTE Correct Text
                HUD.instance.qteCorrect.SetQTECorrectText(count, qteData.maxCount);
                #endregion
            }
            else
            {
                #region Play SFX
                SFXHandler.instance.PlaySFX(SFXDefine.FAIL);
                #endregion

                onFail = true;
            }

            onInput = true;
        }

        SetQTEState(EQTEState.End);
    }

    void QTEEnd()
    {
        #region Show HUD
        HUD.instance.ShowHUD();
        #endregion

        #region Hide Guide
        HUD.instance.qteCorrect.HideQTECorrectGuide();
        #endregion

        #region Disable Input Reader
        DisableInputReader();
        #endregion

        #region Play Scrolling
        ScrollController.instance.PlayScrolling();
        #endregion

        #region Animation
        Popup.instance.qteGuide.FadeOutPanel();
        Popup.instance.qteGuide.DisappearClock();
        #endregion

        #region Convert Camera
        CameraController.instance.ConvertCamera(ECameraType.QTE, ECameraType.Main);
        #endregion

        #region Init QTE Guide UI
        Popup.instance.qteGuide.InitQTEGuide();
        guideText.text = "";
        #endregion

        #region Reset QTE Correct Text
        HUD.instance.qteCorrect.ResetQTECorrectText();
        #endregion

        #region Check Super Booster
        if (!onFail)
        {
            SuperBoosterController.instance.StartSuperBooster();
        }
        else
        {
            #region Show Button
            ActionController.instance.ShowCover();
            FeverController.instance.ShowCover();
            TimeController.instance.ShowCover();
            #endregion
        }
        #endregion
    }

    void EnableInputReader()
    {
        inputPanel.raycastTarget = true;
    }

    void DisableInputReader()
    {
        inputPanel.raycastTarget = false;
    }

    void InitPosition()
    {
        startInputPos = Vector2.zero;
        endInputPos = Vector2.zero;
    }

    void CheckTime()
    {
        if (checkTimeCoroutine != null)
            StopCoroutine(checkTimeCoroutine);

        checkTimeCoroutine = StartCoroutine(CheckTimeLogic());
    }

    IEnumerator CheckTimeLogic()
    {
        float duration = 3f;

        #region Elapse Time Animation
        Popup.instance.qteGuide.ElapseTime(duration);
        #endregion

        yield return new WaitForSeconds(duration);

        if (startInputPos == Vector2.zero || endInputPos == Vector2.zero)
            onInput = false;
    }

    void CreatePattern()
    {
        first.text = "None";
        second.text = "None";

        int randomIndex = UnityEngine.Random.Range(0, 7);

        if (randomIndex == 0)
        {
            startInputType = EQTEInputType.Left;
            endInputType = EQTEInputType.Left;
        }
        else if (randomIndex == 1)
        {
            startInputType = EQTEInputType.Right;
            endInputType = EQTEInputType.Right;
        }
        else if (randomIndex == 2)
        {
            startInputType = EQTEInputType.Left;
            endInputType = EQTEInputType.Right;
        }
        else if (randomIndex == 4)
        {
            startInputType = EQTEInputType.Right;
            endInputType = EQTEInputType.Left;
        }
        else if (randomIndex == 5)
        {
            startInputType = EQTEInputType.Up;
            endInputType = EQTEInputType.Down;
        }
        else 
        {
            startInputType = EQTEInputType.Down;
            endInputType = EQTEInputType.Up;
        }

        first.text = "start: " + startInputType;
        second.text = "end: " + endInputType;

        /*
        startInputType = myQTEPattern[randomIndex][0];
        endInputType = myQTEPattern[randomIndex][1];

        startInputType = qteData.QTEPatterns[randomIndex][0];
        endInputType = qteData.QTEPatterns[randomIndex][1];
        */
    }

    void GuideQTE()
    {
        Popup.instance.qteGuide.ShowGuide(startInputType, endInputType);

        if (startInputType == endInputType)
        {
            if (startInputType == EQTEInputType.Left)
            {
                guideText.text = "LEFT";
            }

            if (startInputType == EQTEInputType.Right)
            {
                guideText.text = "RIGHT";
            }
        }
        else
        {
            if (startInputType == EQTEInputType.Left && endInputType == EQTEInputType.Right)
            {
                guideText.text = "กๆ";
            }

            if (startInputType == EQTEInputType.Right && endInputType == EQTEInputType.Left)
            {
                guideText.text = "ก็";
            }

            if (startInputType == EQTEInputType.Up && endInputType == EQTEInputType.Down)
            {
                guideText.text = "ก้";
            }

            if (startInputType == EQTEInputType.Down && endInputType == EQTEInputType.Up)
            {
                guideText.text = "ก่";
            }
        }

        SFXHandler.instance.PlaySFX(SFXDefine.QTE);
    }

    bool InputIsCorrect()
    {
        if (startInputPos == Vector2.zero || endInputPos == Vector2.zero)
            return false;

        switch (startInputType)
        {
            case EQTEInputType.Up:
                if (startInputPos.y < 0.5f)
                    return false;
                break;
            case EQTEInputType.Down:
                if (startInputPos.y > 0.5f)
                    return false;
                break;
            case EQTEInputType.Left:
                if (startInputPos.x > 0.5f)
                    return false;
                break;
            case EQTEInputType.Right:
                if (startInputPos.x < 0.5f)
                    return false;
                break;
        }

        switch (endInputType)
        {
            case EQTEInputType.Up:
                if (endInputPos.y < 0.5f)
                    return false;
                break;
            case EQTEInputType.Down:
                if (endInputPos.y > 0.5f)
                    return false;
                break;
            case EQTEInputType.Left:
                if (endInputPos.x > 0.5f)
                    return false;
                break;
            case EQTEInputType.Right:
                if (endInputPos.x < 0.5f)
                    return false;
                break;
        }

        return true;
    }
}
