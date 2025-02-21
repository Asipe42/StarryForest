using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

public class QTEEditorWindow : OdinEditorWindow
{
    [MenuItem("Util/QTE Editor")]
    static void Open()
    {
        GetWindow<QTEEditorWindow>().Show();
    }

    [Button(size: ButtonSizes.Small, Name = "Start QTE")]
    public void SetBoosterGauge()
    {
        QTEController.instance.StartQTE();
    }
}
