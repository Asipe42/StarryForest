using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_FeverText : MonoBehaviour
{
    [SerializeField]
    Image feverText;

    public void Appear()
    {
        #region SFX
        SFXHandler.instance.PlaySFX(SFXDefine.FEVER);
        #endregion

        var sequence = DOTween.Sequence();

        sequence.Append(feverText.DOFade(1f, 0.8f))
                .Append(feverText.DOFade(0f, 0.8f));
    }
}
