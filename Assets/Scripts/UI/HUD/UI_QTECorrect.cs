using UnityEngine;
using TMPro;
using DG.Tweening;

public class UI_QTECorrect : MonoBehaviour
{
    [SerializeField]
    GameObject qteCorrectGuide;

    [SerializeField]
    TextMeshProUGUI qteCorrectText;

    public void ShowQTECorrectGuide()
    {
        var sequence = DOTween.Sequence();

        Vector3 targetPos = new Vector3(60f, -240f, 0f);
        float duration = 1.5f;

        sequence.Append(qteCorrectGuide.transform.DOLocalMove(targetPos, duration).SetEase(Ease.OutCirc).SetUpdate(UpdateType.Fixed, true));
    }

    public void HideQTECorrectGuide()
    {
        var sequence = DOTween.Sequence();

        Vector3 targetPos = new Vector3(-240f, -240f, 0f);
        float duration = 1.5f;

        sequence.Append(qteCorrectGuide.transform.DOLocalMove(targetPos, duration).SetEase(Ease.OutBack).SetUpdate(UpdateType.Fixed, true));
    }

    public void SetQTECorrectText(float value, float goal)
    {
        qteCorrectText.text = $"[{value}/{goal}]";
    }

    public void ResetQTECorrectText()
    {
        qteCorrectText.text = "0/0";
    }
}
