using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    const string BGM_PATH = "Audio/BGM";
    const string SFX_PATH = "Audio/SFX";

    public static Dictionary<string, AudioClip> SFXClipsDictionary;
    public static Dictionary<string, AudioClip> BGMClipsDictionary;

    void Awake()
    {
        InitProperty();
        LoadData();
    }

    void InitProperty()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion
    }

    void LoadData()
    {
        AudioClip[] BGMClips = Resources.LoadAll<AudioClip>(BGM_PATH);
        AudioClip[] SFXClips = Resources.LoadAll<AudioClip>(SFX_PATH);

        BGMClipsDictionary = new Dictionary<string, AudioClip>();
        SFXClipsDictionary = new Dictionary<string, AudioClip>();

        foreach (var clip in BGMClips)
        {
            BGMClipsDictionary.Add(clip.name.ToUpper(), clip);
        }

        foreach (var clip in SFXClips)
        {
            SFXClipsDictionary.Add(clip.name.ToUpper(), clip);
        }
    }

    public void ChangeBGM_Game(EGameState state)
    {
        if (state == EGameState.InGame)
            return;

        // BGMHandler.instance.PlayBGM(BGMDefine.GAME_BGM[state], BGMDefine.GAME_BGM_INFO[state]);
    }

    public void ChangeBGM_InGame(EInGameState state)
    {
        if (state == EInGameState.None || state == EInGameState.GameOver)
            return;

        if (state == EInGameState.Default)
        {
            var chapterType = GameManager.instance.gameData.chapterType;

            BGMHandler.instance.PlayBGM(BGMDefine.CHPATER_BGM[chapterType], BGMDefine.CHAPTER_BGM_INFO[chapterType]);
        }
        else
        {
            BGMHandler.instance.PlayBGM(BGMDefine.INGAME_BGM[state], BGMDefine.INGAME_BGM_INFO[state]);
        }
    }

    public void ChangeBGM_Chapter(EChapterType type)
    {
        BGMHandler.instance.PlayBGM(BGMDefine.CHPATER_BGM[type], BGMDefine.CHAPTER_BGM_INFO[type]);
    }
}
