using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;

public class BoosterEditorWindow : OdinEditorWindow
{
    [MenuItem("Util/Booster Edtior")]
    static void Open()
    {
        GetWindow<BoosterEditorWindow>().Show();
    }

    [PropertyRange(1, 2)]
    public float boosterGaugeValue = 1;
    
    [Button(size: ButtonSizes.Small, Name = "Apply")]
    public void SetBoosterGauge() 
    { 
        BoosterController.instance.BoosterValue = boosterGaugeValue;
    }
}