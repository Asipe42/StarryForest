using UnityEngine;

public class Popup : MonoBehaviour
{
    public static Popup instance;

    public UI_ChapterTitle chapterTitle { get; private set; }
    public UI_JudgeText judgeText { get; private set; }
    public UI_FeverText feverText { get; private set; }
    public UI_QTEGuide qteGuide { get; private set; }

    void Awake()
    {
        InitProperty();
    }

    void InitProperty()
    {
        instance = this;

        judgeText = FindObjectOfType<UI_JudgeText>();
        feverText = FindObjectOfType<UI_FeverText>();
        qteGuide = FindObjectOfType<UI_QTEGuide>();
        chapterTitle = FindObjectOfType<UI_ChapterTitle>();
    }
}
