using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UI_ComboText : MonoBehaviour
{
    [SerializeField] 
    TextMeshProUGUI comboText;

    [SerializeField] 
    Image comboEffect;

    [SerializeField]
    Transform defaultPos;

    public void UpdateComboText(int value)
    {
        #region Animation        
        var sequence = DOTween.Sequence();

        sequence
                .OnStart(() => PlayComboEffect())
                .Join(comboText.DOFade(0.5f, 0.1f).OnComplete(() =>
                {
                    #region Change Combo Text Value
                    PrintComboText(value);
                    #endregion
                    comboText.DOFade(1f, 0.1f);
                }))
                .Join(comboText.transform.DOScale(1.2f, 0.15f))
                .Append(comboText.transform.DOScale(1f, 0.1f));

        sequence.OnComplete(() =>
        {
            comboText.transform.localScale = Vector3.one;
            comboText.rectTransform.position = defaultPos.position;
        });
        #endregion
    }

    public void PrintComboText(int value)
    {
        comboText.text = value.ToString();
    }

    void PlayComboEffect()
    {
        var sequence = DOTween.Sequence();

        sequence.OnStart(() =>
        {
            comboEffect.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            comboEffect.DOFade(0.4f, 0.5f);
        });

        sequence.Append(comboEffect.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutQuad))
                .Join(comboEffect.transform.DORotate(new Vector3(0f, 0f, 20f), 1f, mode: RotateMode.FastBeyond360).SetEase(Ease.OutQuad));

        sequence.OnComplete(() => comboEffect.DOFade(0f, 0.5f));

        sequence.Restart();
    }
}
