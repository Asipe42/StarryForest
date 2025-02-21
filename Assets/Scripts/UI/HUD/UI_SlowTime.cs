using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_SlowTime : MonoBehaviour
{
    [SerializeField]
    Image mask, cover, icon;

    public void ShowIcon()
    {
        var sequence = DOTween.Sequence();

        sequence.OnStart(() =>
        {
            mask.transform.DOScale(1f, 0f);
            mask.color = new Color(1f, 1f, 1f, 0f);

            cover.transform.DOScale(0f, 0f);
        });

        sequence.Append(mask.DOFade(1f, 1f))
                .Append(cover.transform.DOScale(1f, 1f).SetEase(Ease.OutBack)
                                       .OnStart(() => SFXHandler.instance.PlaySFX(SFXDefine.APPEAR)));
    }

    public void HideIcon()
    {
        var sequence = DOTween.Sequence();

        sequence.OnStart(() =>
        {
            mask.transform.DOScale(1f, 0f);
        });

        sequence.Append(mask.transform.DOScale(0f, 1f).SetEase(Ease.OutQuad)
                                      .OnStart(() => SFXHandler.instance.PlaySFX(SFXDefine.APPEAR)));
    }

    public void SetFillAmount(float currentTime, float goal)
    {
        icon.fillAmount = currentTime / goal;
    }
}
