using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

[RequireComponent(typeof(LineRenderer))]
public class UI_QTEGuide : SerializedMonoBehaviour
{
    [SerializeField]
    Image panel;

    [SerializeField]
    Image clock, clockHand;

    [SerializeField]
    Image leftIcon, rightIcon;

    [SerializeField] 
    Dictionary<EQTEInputType, Vector3> linePositions;

    Sequence clockhandSequence;

    LineRenderer lineRenderer;

    IEnumerator showLineCoroutine;

    void Awake()
    {
        InitProperty();
    }

    void InitProperty()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    public void FadeInPanel()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(panel.DOFade(0.4f, 0.5f).SetEase(Ease.OutQuad));
    }

    public void FadeOutPanel()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(panel.DOFade(0.0f, 0.5f).SetEase(Ease.OutQuad));
    }

    public void AppearClock()
    {
        var sequence = DOTween.Sequence();

        Vector3 pos = new Vector3(0f, 650f, 0f);

        sequence.OnStart(() => clock.gameObject.SetActive(true));
        sequence.Append(clock.transform.DOLocalMove(pos, 0.8f).SetEase(Ease.OutBack));
    }

    public void ResetClockHand(float duration)
    {
        if (clockhandSequence != null)
            clockhandSequence.Kill();

        clockhandSequence = DOTween.Sequence();

        clockhandSequence.Append(clockHand.transform.DORotate(new Vector3(0f, 0f, 0f), duration).SetEase(Ease.OutQuad));
    }

    public void ElapseTime(float duration)
    {
        clockhandSequence = DOTween.Sequence();

        clockhandSequence.Append(clockHand.transform.DORotate(new Vector3(0f, 0f, -360f), duration, RotateMode.FastBeyond360).SetEase(Ease.Linear));
    }

    public void DisappearClock()
    {
        var sequence = DOTween.Sequence();

        Vector3 pos = new Vector3(0f, 1000f, 0f);

        sequence.Append(clock.transform.DOLocalMove(pos, 0.8f).SetEase(Ease.InBack));
        sequence.OnComplete(() => clock.gameObject.SetActive(false));
    }

    public void ShowGuide(EQTEInputType startType, EQTEInputType endType)
    {
        #region Init Property
        InitQTEGuide();
        #endregion

        if (startType == endType)
        {
            #region Show Image
            ShowImage(startType);
            #endregion
        }
        else
        {
            #region Show Line
            showLineCoroutine = ShowLine(startType, endType);
            StartCoroutine(showLineCoroutine);
            #endregion
        }
    }

    public void InitQTEGuide()
    {
        InitImage();
        InitLine();
    }

    void InitImage()
    {
        leftIcon.transform.localScale = Vector3.zero;
        rightIcon.transform.localScale = Vector3.zero;
    }

    void InitLine()
    {
        if (showLineCoroutine != null)
            StopCoroutine(showLineCoroutine);

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, Vector3.zero);
        }
    }

    void ShowImage(EQTEInputType type)
    {
        if (type == EQTEInputType.Left)
        {
            leftIcon.transform.DOScale(1f, 0.4f).From(0f).SetEase(Ease.OutBounce);
        }

        if (type == EQTEInputType.Right)
        {
            rightIcon.transform.DOScale(1f, 0.4f).From(0f).SetEase(Ease.OutBounce);
        }
    }

    IEnumerator ShowLine(EQTEInputType startType, EQTEInputType endType)
    {
        #region Init Position
        lineRenderer.SetPosition(0, linePositions[startType]);
        lineRenderer.SetPosition(1, linePositions[startType]);
        #endregion

        #region Animation
        float timer = 0f;
        float duration = 1.25f;

        while (timer <= duration)
        {
            timer += Time.deltaTime;
            
            lineRenderer.SetPosition(1, Vector3.Lerp(linePositions[startType], linePositions[endType], (timer / duration)));

            yield return new WaitForEndOfFrame();
        }
        #endregion        
    }
}
