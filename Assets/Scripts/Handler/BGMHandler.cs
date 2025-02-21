using UnityEngine;
using DG.Tweening;

public class BGMHandler : MonoBehaviour
{
    public static BGMHandler instance;

    AudioSource[] channels;

    AudioSource currentPlaingChannel;

    void Awake()
    {
        instance = this;

        channels = new AudioSource[(int)(transform.childCount * 1.5)];

        for (int i = 0; i < transform.childCount; i++)
        {
            channels[i] = transform.GetChild(i).GetComponent<AudioSource>();
            channels[i].loop = true;
        }
    }

    AudioSource GetAvailableChannel()
    {
        foreach (var channel in channels)
        {
            if (channel.enabled && !channel.isPlaying)
            {
                return channel;
            }
        }

        return null;
    }

    public void PlayBGM(string clipName, float fadeTime = 3f)
    {
        StopBGM(fadeTime);

        #region Get Available Channel
        AudioSource channel = GetAvailableChannel();
        currentPlaingChannel = channel;
        #endregion

        channel.clip = AudioManager.BGMClipsDictionary[clipName];
        channel.volume = 1f; channel.pitch = 1f;
        channel.Stop();
        channel.Play();
    }

    public void PlayBGM(string clipName, BGMInfo info, float fadeTime = 1f)
    {
        StopBGM(fadeTime);

        #region Get Available Channel
        AudioSource channel = GetAvailableChannel();
        currentPlaingChannel = channel;
        #endregion

        #region Change Clip
        channel.clip = AudioManager.BGMClipsDictionary[clipName];
        #endregion

        #region Change Pitch
        channel.pitch = info.targetPitch;
        #endregion

        #region Change Volume
        if (info.targetFadeTime > 0)
        {
            channel.volume = 0f;
            channel.DOFade(info.targetVolume, info.targetFadeTime);
        }
        else
        {
            channel.volume = info.targetVolume;
        }
        #endregion

        channel.Stop();
        channel.PlayDelayed(info.targetDelayTime);
    }

    public void StopBGM(float targetFadeTime)
    {
        var temp = currentPlaingChannel;

        temp?.DOFade(0, targetFadeTime).OnComplete(() => temp.Stop());
    }
}
