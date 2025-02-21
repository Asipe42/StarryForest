using System.Collections.Generic;
using UnityEngine;
using Effect;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    const string EffectPath = "Effect";

    public static Dictionary<string, GameObject> EffectDictionary;

    public JudgeEffect judgeEffect { get; private set; }

    void Awake()
    {
        InitProperty();
        LoadData();
    }

    void InitProperty()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion

        judgeEffect = FindObjectOfType<JudgeEffect>();
    }

    void LoadData()
    {
        GameObject[] Effects = Resources.LoadAll<GameObject>(EffectPath);

        EffectDictionary = new Dictionary<string, GameObject>();

        foreach (var effect in Effects)
        {
            EffectDictionary.Add(effect.name, effect.gameObject);
        }
    }
}
