using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;

public class BackgroundEditorWindow : OdinEditorWindow
{
    [MenuItem("Util/Background")]
    static void Open()
    {
        GetWindow<BackgroundEditorWindow>();
    }

    [EnumToggleButtons]
    public EChapterType chapterType = EChapterType.Chapter1;

    [Button(size: ButtonSizes.Small, Name = "Convert")]
    public void ConvertBackground()
    {
        BackgroundController.instance.ConvertBackground(chapterType);
    }
}