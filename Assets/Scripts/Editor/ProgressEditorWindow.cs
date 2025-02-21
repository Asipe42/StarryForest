using System.Collections;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

public class ProgressEditorWindow : OdinEditorWindow
{
    [MenuItem("Util/Progress Edtior")]
    static void Open()
    {
        GetWindow<ProgressEditorWindow>().Show();
    }

    [PropertyRange(0, 1)]
    public float progressRatio = 1;

    [Button(size: ButtonSizes.Small, Name = "Apply")]
    public void SetProgressValue()
    {
        int index = (int)GameManager.instance.gameData.chapterType + 1;
        var temp = ProgressController.instance.progressMaxValues[index];

        ProgressController.instance.ProgressValue.Value = temp * progressRatio;
    }
}
