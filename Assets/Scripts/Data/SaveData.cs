using System.Collections.Generic;

public enum EITemtype
{
    ITEM01, ITEM02, ITEM03, ITEM04, ITEM05, ITEM06
}

public class SaveData
{
    public int mushroom;
    public Dictionary<EITemtype, bool> itemDic;
    public EChapterType lastChapter;
}
