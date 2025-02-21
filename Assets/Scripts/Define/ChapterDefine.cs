using System.Collections.Generic;

public struct ChapterDefine
{
    public static Dictionary<EChapterType, string> CHAPTER_NAME = new Dictionary<EChapterType, string>()
    {
        { EChapterType.Chapter1, "¹ö¼¸ ½£" },
        { EChapterType.Chapter2, "¹Îµé·¹ ½£" },
        { EChapterType.Chapter3, "»êµþ±â ½£" },
        { EChapterType.Chapter4, "³ëÀ» ½£" },
        { EChapterType.Chapter5, "º°ºû ½£" },
        { EChapterType.End, "¹Ù´å°¡" },
    };
}
