using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingController : MonoBehaviour
{
    [SerializeField]
    GameObject box;

    [SerializeField] 
    AudioMixer mixer;

    [SerializeField] 
    Slider BGMSlider, SFXSlider;

    public const string MIXER_BGM = "BGMVolume";
    public const string MIXER_SFX = "SFXVolume";

    void Awake()
    {
        SetVolumes();
    }

    void SetVolumes()
    {
        BGMSlider.onValueChanged.AddListener(SetBGMVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void SetBGMVolume(float value)
    {
        mixer.SetFloat(MIXER_BGM, Mathf.Log10(value) * 20);
    }

    void SetSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }

    public void OpenSetting()
    {
        box.SetActive(true);
    }

    public void CloseSetting()
    {
        box.SetActive(false);
    }
}
