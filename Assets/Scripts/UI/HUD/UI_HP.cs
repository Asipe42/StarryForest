using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_HP : MonoBehaviour
{
    [SerializeField] 
    Slider hpSlider;

    [SerializeField] 
    Image icon, cover;

    [SerializeField]
    ParticleSystem hpParticle;

    void Start()
    {
        InitProperty();
        SetParticle();
    }

    void InitProperty()
    {
        hpSlider.maxValue = PlayerController.instance.playerData.maxHP;
        hpSlider.minValue = 0f;
    }

    void SetParticle()
    {
        hpParticle.Stop();
    }

    public void SetHpSliderValue(float hp)
    {
        hpSlider.value = hp;
    }

    public void SetHPParticle(bool state)
    {
        if (state)
        {
            if (!hpParticle.isPlaying)
            {
                hpParticle.Play();
            }
        }
        else
        {
            hpParticle.Stop();
        }
    }

    public void PlayIconAnimation()
    {
        var sequence = DOTween.Sequence();
        var sequence2 = DOTween.Sequence();


        sequence.Append(icon.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.2f))
                .Append(icon.transform.DOScale(Vector3.one, 0.2f));

        sequence2.Append(cover.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.2f))
                 .Append(cover.transform.DOScale(Vector3.one, 0.2f));
    }

    public void SetIconFillAmount(float life, float maxLife)
    {
        icon.fillAmount = (float)life / (float)maxLife;

        PlayIconAnimation();
    }
}
