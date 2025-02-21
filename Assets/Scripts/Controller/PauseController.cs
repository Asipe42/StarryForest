using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    GameObject box, panel;
    public static PauseController instance;

    float targetTimeScale = 0f;
    float defaultTimeScale;
    float defaultFixedDeltaTime;

    void Awake()
    {
        InitProperty();
    }

    void InitProperty()
    {
        defaultTimeScale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    void PauseTime()
    {
        Time.timeScale = targetTimeScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime * targetTimeScale;
    }

    void ContinueTime()
    {
        Time.timeScale = defaultTimeScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime;
    }

    public void Pause()
    {
        #region SFX
        SFXHandler.instance.PlaySFX(SFXDefine.UI_1);
        #endregion

        panel.SetActive(true);
        box.SetActive(true);

        PauseTime();
    }

    public void Continue()
    {
        #region SFX
        SFXHandler.instance.PlaySFX(SFXDefine.UI_1);
        #endregion

        panel.SetActive(false);
        box.SetActive(false);

        ContinueTime();
    }

    public void Exit()
    {
        #region SFX
        SFXHandler.instance.PlaySFX(SFXDefine.UI_1);
        #endregion

        Application.Quit();
    }
}
