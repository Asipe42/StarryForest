using UnityEngine;
using DG.Tweening;
using TMPro;

public class UI_ChapterTitle : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    public void SetChapterTitleText(EChapterType type)
    {
        var sequence = DOTween.Sequence();

        sequence.OnStart(() =>
        {
            text.text = "";
            text.color = Color.white;
            SFXHandler.instance.PlaySFX(SFXDefine.TYPING);
        });

        sequence.Append(text.DOText(ChapterDefine.CHAPTER_NAME[type], 0.45f))
                .Insert(1f, text.DOFade(0, 0.8f));
    }
}
