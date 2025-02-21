using System.Collections.Generic;

public class JudgementDefine
{
    #region etc
    public const float INVINCIBLE_TIME = 0.5f;
    #endregion

    #region HP
    const float HP_PERFECT = 40;
    const float HP_GREATE = 10;
    const float HP_GOOD = 0;
    const float HP_MISS = -5;
    const float HP_FAIL = -30;

    public static Dictionary<EJudgementType, float> HP_VALUES = new Dictionary<EJudgementType, float>()
    {
        { EJudgementType.Perfect, HP_PERFECT },
        { EJudgementType.Greate, HP_GREATE },
        { EJudgementType.Good, HP_GOOD },
        { EJudgementType.Miss, HP_MISS },
        { EJudgementType.Fail, HP_FAIL },
    };
    #endregion

    #region Booster
    const float BOOSTER_PERFECT = 0.08f;
    const float BOOSTER_GREATE = 0.02f;
    const float BOOSTER_GOOD = 0.01f;
    const float BOOSTER_MISS = -0.25f;
    const float BOOSTER_FAIL = -0.1f;

    public static Dictionary<EJudgementType, float> BOOSTER_VALUES = new Dictionary<EJudgementType, float>()
    {
        { EJudgementType.Perfect, BOOSTER_PERFECT },
        { EJudgementType.Greate, BOOSTER_GREATE },
        { EJudgementType.Good, BOOSTER_GOOD },
        { EJudgementType.Miss, BOOSTER_MISS },
        { EJudgementType.Fail, BOOSTER_FAIL },
    };
    #endregion

    #region Super Booster
    const float SUPER_BOOSTER_PERFECT = 0.3f;
    const float SUPER_BOOSTER_GREAT = 0.2f;
    const float SUPER_BOOSTER_GOOD = 0.1f;

    public static Dictionary<EJudgementType, float> SUPER_BOOSTER_VALUES = new Dictionary<EJudgementType, float>()
    {
        { EJudgementType.Perfect, SUPER_BOOSTER_PERFECT },
        { EJudgementType.Greate, SUPER_BOOSTER_GREAT },
        { EJudgementType.Good, SUPER_BOOSTER_GOOD },
    };
    #endregion
}
