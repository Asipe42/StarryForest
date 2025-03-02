using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public static TimelineController instance;

    PlayableDirector playable;

    void Awake()
    {
        Initialize();
    }

    #region Initial Setting
    void Initialize()
    {
        instance = this;

        playable = GetComponent<PlayableDirector>();
    }
    #endregion

    /// <summary>
    /// Timeline�� �����մϴ�.
    /// </summary>
    public void PauseTimeline()
    {
        playable.Pause();
    }

    /// <summary>
    /// Timeline�� �簳�մϴ�.
    /// </summary>
    public void ContinueTimeline()
    {
        playable.Play();
    }
}
