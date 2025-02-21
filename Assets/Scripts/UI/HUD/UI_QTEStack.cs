using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_QTEStack : MonoBehaviour
{
    [SerializeField]
    GameObject qteStackGuide;

    [SerializeField]
    TextMeshProUGUI qteStackText;

    public void ShowQTEStackGuide()
    {
        var sequence = DOTween.Sequence();

        Vector3 targetPos = new Vector3(60f, -240f, 0f);
        float duration = 1.5f;

        sequence.Append(qteStackGuide.transform.DOLocalMove(targetPos, duration).SetEase(Ease.OutCirc).SetUpdate(UpdateType.Fixed, true));
    }

    public void HideQTEStackGuide()
    {
        var sequence = DOTween.Sequence();

        Vector3 targetPos = new Vector3(-230f, -240f, 0f);
        float duration = 1.5f;

        sequence.Append(qteStackGuide.transform.DOLocalMove(targetPos, duration).SetEase(Ease.OutBack).SetUpdate(UpdateType.Fixed, true));
    }

    public void SetQTEStackText(float value, float goal)
    {
        qteStackText.text = $"[{value}/{goal}]";
    }

    public void ResetQTEStackText()
    {
        qteStackText.text = "0/0";
    }
}
