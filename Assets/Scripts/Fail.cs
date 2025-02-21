using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Fail : MonoBehaviour
{
    [SerializeField] Transform slidePosition;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] TextMeshProUGUI subText;
    [SerializeField] Image dalDeadImage;
    [SerializeField] Image byulImage;
    [SerializeField] AudioClip failClip;
    [SerializeField] AudioSource audioSource;

    void Awake()
    {
        Initialize();
    }

    void OnEnable()
    {
        StartCoroutine(PlayFailDirecting());
    }

    #region Initial Setting
    void Initialize()
    {
        mainText.color = new Color(1f, 1f, 1f, 0f);
        subText.color = new Color(1f, 1f, 1f, 0f);
        dalDeadImage.color = new Color(1f, 1f, 1f, 0f);
        byulImage.color = new Color(1f, 1f, 1f, 0f);
    }
    #endregion


    bool WaitAnyKey()
    {
        return Input.GetMouseButtonDown(0);
    }

    public IEnumerator PlayFailDirecting()
    {
        BGMHandler.instance.StopBGM(1.5f);

        var sequence = DOTween.Sequence();

        //yield return new WaitForSeconds(0.5f);

        audioSource.clip = failClip;
        audioSource.Play();

        sequence.Append(mainText.DOFade(1f, 1f).SetEase(Ease.InSine))
                .Append(dalDeadImage.DOFade(1f, 1f).SetEase(Ease.InSine))
                .Insert(3.5f, dalDeadImage.transform.DOMove(slidePosition.position, 1f).SetEase(Ease.OutQuad))
                .Append(byulImage.DOFade(1f, 2f).SetEase(Ease.InSine))
                .Append(subText.DOFade(1f, 0.5f).SetEase(Ease.InSine));

        yield return new WaitUntil(() => WaitAnyKey());

        Application.Quit();
    }
}
