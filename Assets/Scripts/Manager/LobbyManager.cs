using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using DG.Tweening;

public class LobbyManager : MonoBehaviour
{ 
    public static LobbyManager instance;

    [SerializeField]
    GameObject lobby, title;

    [SerializeField]
    Image logo;

    [SerializeField]
    TextMeshProUGUI guideMessageText;

    [SerializeField]
    VolumeProfile volume;

    [SerializeField]
    DOTweenAnimation[] animations;

    public UI_Mushroom mushroom;

    bool onClick;

    void Awake()
    {
        instance = this;
    }

    public void GoToLobby()
    {
        if (onClick)
            return;

        onClick = true;

        var sequence = DOTween.Sequence();

        float duration = 1f;

        #region SFX
        SFXHandler.instance.PlaySFX(SFXDefine.TITLE);
        #endregion

        sequence.Append(logo.DOFade(0, duration))
                .Join(Camera.current.DOOrthoSize(6f, duration))
                .Join(guideMessageText.DOFade(0, duration))
                .InsertCallback(duration + 0.75f, () =>
                {
                    title.SetActive(false);
                    lobby.SetActive(true);
                    ShowLobbyUI();
                });

        StartCoroutine(SetFocalLength());
    }

    IEnumerator SetFocalLength()
    {
        DepthOfField dof;
        volume.TryGet(out dof);

        float length = (float)dof.focalLength;

        while (length > 0)
        {
            length -= 10f;

            dof.focalLength.Override(length);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ShowLobbyUI()
    {
        foreach (var anim in animations)
        {
            anim.DORestartById("Show");
        }
    }
}
