using System.Collections.Generic;

public struct ChapterDefine
{
    public static Dictionary<EChapterType, string> CHAPTER_NAME = new Dictionary<EChapterType, string>()
    {
        { EChapterType.Chapter1, "���� ��" },
        { EChapterType.Chapter2, "�ε鷹 ��" },
        { EChapterType.Chapter3, "����� ��" },
        { EChapterType.Chapter4, "���� ��" },
        { EChapterType.Chapter5, "���� ��" },
        { EChapterType.End, "�ٴ尡" },
    };
}
