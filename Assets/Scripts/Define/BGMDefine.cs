using System.Collections.Generic;

public struct BGMInfo
{
    public float targetVolume;
    public float targetDelayTime;
    public float targetFadeTime;
    public float targetPitch;
}

public struct BGMDefine
{
    public const string CHPATER_1 = "BGM_1";
    public const string CHPATER_2 = "BGM_2";
    public const string CHPATER_3 = "BGM_3";
    public const string CHPATER_4 = "BGM_4";
    public const string CHPATER_5 = "BGM_5";
    public const string CHAPTER_END = "BGM_End";

    public static Dictionary<EChapterType, string> CHPATER_BGM = new Dictionary<EChapterType, string>
    {
        { EChapterType.Chapter1, CHPATER_1 },
        { EChapterType.Chapter2, CHPATER_2 },
        { EChapterType.Chapter3, CHPATER_3 },
        { EChapterType.Chapter4, CHPATER_4 },
        { EChapterType.Chapter5, CHPATER_5 },
    };

    public static Dictionary<EChapterType, BGMInfo> CHAPTER_BGM_INFO = new Dictionary<EChapterType, BGMInfo>
    {
        { EChapterType.Chapter1, new BGMInfo { targetVolume = 0.2f, targetDelayTime = 1f, targetFadeTime = 3f, targetPitch = 1f } },
        { EChapterType.Chapter2, new BGMInfo { targetVolume = 0.2f, targetDelayTime = 4f, targetFadeTime = 3f, targetPitch = 1f } },
        { EChapterType.Chapter3, new BGMInfo { targetVolume = 0.2f, targetDelayTime = 4f, targetFadeTime = 3f, targetPitch = 1f } },
        { EChapterType.Chapter4, new BGMInfo { targetVolume = 0.2f, targetDelayTime = 4f, targetFadeTime = 3f, targetPitch = 1f } },
        { EChapterType.Chapter5, new BGMInfo { targetVolume = 0.2f, targetDelayTime = 4f, targetFadeTime = 3f, targetPitch = 1f } },
    };

    public const string FEVER = "BGM_FEVER";
    public const string SLOW_TIME = "BGM_SLOWTIME";
    public const string SUPER_BOOSTER = "BGM_SUPERBOOSTER";

    public static Dictionary<EInGameState, string> INGAME_BGM = new Dictionary<EInGameState, string>
    {
        { EInGameState.Fever, FEVER },
        { EInGameState.SlowTime, SLOW_TIME },
        { EInGameState.SuperBooster, SUPER_BOOSTER },
    };

    public static Dictionary<EInGameState, BGMInfo> INGAME_BGM_INFO = new Dictionary<EInGameState, BGMInfo>
    {
        { EInGameState.Fever, new BGMInfo { targetVolume = 0.2f, targetDelayTime = 1.5f, targetFadeTime = 3f, targetPitch = 1f } },
        { EInGameState.SlowTime, new BGMInfo { targetVolume = 0.2f, targetDelayTime = 1, targetFadeTime = 3f, targetPitch = 1f } },
        { EInGameState.SuperBooster, new BGMInfo { targetVolume = 0.2f, targetDelayTime = 1, targetFadeTime = 3f, targetPitch = 1f } },
    };

    public const string LOBBY = "BGM_LOBBY";
    public const string END = "BGM_END";

    public static Dictionary<EGameState, string> GAME_BGM = new Dictionary<EGameState, string>
    {
        { EGameState.Lobby, LOBBY },
        { EGameState.End, END }
    };

    public static Dictionary<EGameState, BGMInfo> GAME_BGM_INFO = new Dictionary<EGameState, BGMInfo>
    {
        { EGameState.Lobby, new BGMInfo { targetVolume = 0.2f, targetDelayTime = 1, targetFadeTime = 3f, targetPitch = 1f } },
        { EGameState.End, new BGMInfo { targetVolume = 0.2f, targetDelayTime = 1, targetFadeTime = 3f, targetPitch = 1f } }
    };
}

