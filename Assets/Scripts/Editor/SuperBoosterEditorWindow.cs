using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;

public class SuperBoosterEditorWindow : OdinEditorWindow
{
    [MenuItem("Util/Super Booster Editor")]
    static void Open()
    {
        GetWindow<SuperBoosterEditorWindow>().Show();
    }

    [Button(size: ButtonSizes.Small, Name = "Start SuperBooster")]
    public void SetBoosterGauge()
    {
        SuperBoosterController.instance.StartSuperBooster();
    }
}
