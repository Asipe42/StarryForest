using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Booster : MonoBehaviour
{
    [SerializeField] 
    Slider slider;

    [SerializeField] 
    Image secondaryFill;

    [SerializeField] 
    Image icon;

    [SerializeField]
    ParticleSystem boosterParticle;

    public Slider BoosterSlider
    {
        get => slider;
        set => slider = value;
    }

    void Start()
    {
        InitProperty();
        SetParticle();
    }

    void InitProperty()
    {
        slider.maxValue = (BoosterController.instance.GetMaxValue() - 1f) / 2f;
        slider.minValue = 0f;
    }

    void SetParticle()
    {
        boosterParticle.Stop();
    }

    public void SetSliderValue(float value)
    {
        slider.value = value;
    }

    public void SetBoosterParticle(float ratio)
    {
        if (ratio > 0.5f)
        {
            if (!boosterParticle.isPlaying)
            {
                boosterParticle.Play();
            }
        }
        else
        {
            boosterParticle.Stop();
        }
    }

    public void PlayIconAnimation()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(icon.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.2f))
                .Append(icon.transform.DOScale(Vector3.one, 0.2f));
    }

    public void SetSecondaryFillAmount(float value)
    {
        secondaryFill.fillAmount = (value - 0.5f) / 0.5f;
    }
}
