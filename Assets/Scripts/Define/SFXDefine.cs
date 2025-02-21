using System.Collections.Generic;

public struct SFXInfo
{
    public float targetVolume;
    public float targetDelayTime;
    public float targetPitch;
}

public struct SFXDefine
{
    #region etc
    public const string BUSH = "SFX_BUSH";
    public const string CRASH = "SFX_CRASH";
    public const string BIRD = "SFX_BIRD";
    public const string CROW = "SFX_CROW";
    public const string FROG = "SFX_FROG";
    public const string CORRECT = "SFX_CORRECT";
    public const string FAIL = "SFX_FAIL";
    public const string POP_1 = "SFX_POP_1";
    public const string POP_2 = "SFX_POP_2";
    public const string FEVER = "SFX_FEVER";
    public const string TYPING = "SFX_TYPING";
    public const string RESULT = "SFX_RESULT";
    public const string ROTATION = "SFX_ROTATION";
    public const string STAR_4 = "SFX_STAR4";
    public const string TITLE = "SFX_TITLE";
    public const string SHOOT_1 = "SFX_SHOOT1";
    public const string SHOOT_2 = "SFX_SHOOT2";
    #endregion

    #region Judgement Clip
    public const string STAR_1 = "SFX_STAR1";
    public const string STAR_2 = "SFX_STAR2";
    public const string STAR_3 = "SFX_STAR3";
    public const string MISS = "SFX_MISS";

    public static Dictionary<EJudgementType, string> JUDGEMENT_CLIP = new Dictionary<EJudgementType, string>
    {
        { EJudgementType.Perfect, STAR_1 },
        { EJudgementType.Greate, STAR_2 },
        { EJudgementType.Good, STAR_3 },
        { EJudgementType.Miss, MISS },
    };
    #endregion

    #region Hit Clip
    public const string HIT = "SFX_HIT";
    public const string STONE_HIT = "SFX_STONE";
    public const string FALL = "SFX_FALL";
    public const string WEB = "SFX_WEB";

    public static Dictionary<string, string> OBSTACLE_HIT = new Dictionary<string, string>
    {
        { "Hole", FALL },
        { "Thorn A", HIT },
        { "Thorn B", HIT },
        { "Vine", HIT },
        { "Vine A", HIT },
        { "Vine B", HIT },
        { "Vine C", HIT },
        { "Sparrow", HIT },
        { "Frog", HIT },
        { "Crow", HIT },
        { "Web", WEB },
        { "Stone", STONE_HIT },
        { "Cat", HIT }
    };
    #endregion

    #region Action Clip
    public const string JUMP = "SFX_JUMP";
    public const string SLIDE = "SFX_SLIDE";
    public const string DOWNHILL = "SFX_DOWNHILL";

    public static Dictionary<EActionType, string> ACTION_CLIP = new Dictionary<EActionType, string>
    { 
        { EActionType.Jump, JUMP },
        { EActionType.Slide, SLIDE },
        { EActionType.Downhill, DOWNHILL }
    };
    #endregion

    #region UI Clip
    public const string APPEAR = "SFX_APPEAR";
    public const string QTE = "SFX_QTE";
    public const string SPEED_UP = "SFX_SPEEDUP";
    public const string UI_1 = "SFX_UI_1";
    #endregion
}
