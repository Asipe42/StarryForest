using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class UI_JudgeText : MonoBehaviour
{
    [SerializeField] 
    TextMeshProUGUI judgeText;

    IObjectPool<UI_JudgeText> managedPool;

    public void SetJudgeText(EJudgementType type)
    {
        judgeText.text = type.ToString();
    }

    public void SetManagedPool(IObjectPool<UI_JudgeText> pool)
    {
        managedPool = pool;
    }

    public void DestroyJudgeText()
    {
        managedPool.Release(this);
    }

    public void SetColorGradient(TMP_ColorGradient cg)
    {
        judgeText.colorGradientPreset = cg;
    }
}