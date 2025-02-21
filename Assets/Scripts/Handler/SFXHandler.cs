using UnityEngine;

public class SFXHandler : MonoBehaviour
{
    public static SFXHandler instance;

    AudioSource[] channels;

    void Awake()
    {
        instance = this;

        channels = new AudioSource[(int)(transform.childCount * 1.5)];

        for (int i = 0; i < transform.childCount; i++)
        {
            channels[i] = transform.GetChild(i).GetComponent<AudioSource>();
        }
    }

    public AudioSource GetAvailableChannel()
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

    public void PlaySFX(string clipName)
    {
        AudioSource channel = GetAvailableChannel();

        channel.clip = AudioManager.SFXClipsDictionary[clipName];
        channel.Play();
    }

    public void PlaySFX(string clipName, SFXInfo info)
    {
        AudioSource channel = GetAvailableChannel();

        channel.clip = AudioManager.SFXClipsDictionary[clipName];
        channel.volume = info.targetVolume;
        channel.pitch = info.targetPitch;
        channel.PlayDelayed(info.targetDelayTime);
    }
}
