using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;

public class FeverEditorWindow : OdinEditorWindow
{
    [MenuItem("Util/Fever")]
    static void Open()
    {
        GetWindow<FeverEditorWindow>();
    }

    [Button(size: ButtonSizes.Small, Name = "Start")]
    public void StartFever()
    {
        FeverController.instance.OnFever = true;
    }
}
