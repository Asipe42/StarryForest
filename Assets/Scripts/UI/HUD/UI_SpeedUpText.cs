using UnityEngine;
using TMPro;
using DG.Tweening;

public class UI_SpeedUpText : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI speedUpText;

    float previousBoosterRatio = 0f;

    void Awake()
    {
        speedUpText.gameObject.SetActive(false);
    }

    public void ShowSpeedUpText(float value)
    {
        if (SuperBoosterController.instance.OnSuperBooster)
            return;

        if (!CheckBoosterRatio(value))
            return;

        var sequence = DOTween.Sequence();

        sequence.OnStart(() =>
        {
            speedUpText.gameObject.SetActive(true);
            speedUpText.DOFade(1f, 0f);
            SFXHandler.instance.PlaySFX(SFXDefine.SPEED_UP);
        });

        sequence.Append(speedUpText.transform.DOLocalMove(new Vector3(100f, -400f, 0f), 0.3f).From().SetEase(Ease.OutQuad))
                .Join(speedUpText.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 1f), 0.3f, vibrato: 3).SetDelay(0.2f).SetEase(Ease.OutQuad))
                .Join(speedUpText.DOFade(0f, 0.3f).SetDelay(0.5f).SetEase(Ease.OutQuad));

        sequence.OnComplete(() => speedUpText.gameObject.SetActive(false));
    }

    bool CheckBoosterRatio(float value)
    {
        if (previousBoosterRatio < 0.5f)
        {
            if (value > 0.5f)
            {
                #region Vibrate
                Vibrator.Vibrate(50);
                #endregion

                previousBoosterRatio = value;
                SetText("Speed Up");
                return true;
            }
        }

        if (value >= 1f)
        {
            if (previousBoosterRatio < 1f)
            {
                #region Vibrate
                Vibrator.Vibrate(50);
                #endregion

                previousBoosterRatio = value;
                SetText("Max Speed");
                return true;
            }
        }

        previousBoosterRatio = value;
        return false;
    }

    void SetText(string message)
    {
        speedUpText.text = message;
    }
}
