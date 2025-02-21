using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using TMPro;
using Sirenix.OdinInspector;

public class JudgeTextPool : SerializedMonoBehaviour
{
    public static JudgeTextPool instance;

    [SerializeField] 
    GameObject judgeTextPrefab;

    [SerializeField]
    Dictionary<EJudgementType, TMP_ColorGradient> colorGradientDic;

    IObjectPool<UI_JudgeText> pool;

    void Awake()
    {
        InitProperty();
    }

    void InitProperty()
    {
        instance = this;

        pool = new ObjectPool<UI_JudgeText>(CreateJudgeText, OnGetJudgeText, OnReleaseJudgeText, OnDestroyJudgeText, maxSize: 10);
    }

    public void SpawnJudgeText(EJudgementType type)
    {
        UI_JudgeText judgeText = pool.Get();
        judgeText.transform.SetParent(gameObject.transform, false);
        judgeText.SetJudgeText(type);
        // judgeText.SetColorGradient(colorGradientDic[type]);
    }

    UI_JudgeText CreateJudgeText()
    {
        UI_JudgeText judgeText = Instantiate(judgeTextPrefab).GetComponent<UI_JudgeText>();
        judgeText.SetManagedPool(pool);
        return judgeText;
    }

    void OnGetJudgeText(UI_JudgeText judgeText)
    {
        judgeText.gameObject.SetActive(true);
    }

    void OnReleaseJudgeText(UI_JudgeText judgeText)
    {
        judgeText.gameObject.SetActive(false);
    }

    void OnDestroyJudgeText(UI_JudgeText judgeText)
    {
        Destroy(judgeText.gameObject);
    }
}
