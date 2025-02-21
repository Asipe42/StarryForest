using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class ProgressController : MonoBehaviour
{
    public static ProgressController instance;

    [SerializeField] 
    ProgressData[] progressValues;

    [SerializeField] 
    float point;

    [SerializeField]
    GameObject panel, image, text;

    ReactiveProperty<float> progressValue;
    public ReactiveProperty<float> ProgressValue
    {
        get => progressValue;
        set => progressValue = value;
    }

    public float ProgressRatio
    {
        get => progressValue.Value / progressMaxValues[(int)GameManager.instance.gameData.chapterType + 1];
    }

    public Dictionary<int, float> progressMaxValues;

    void Start()
    {
        InitProperty();
        StartCoroutine(IncreaseProgressValue());
    }

    void InitProperty()
    {
        instance = this;

        progressMaxValues = new Dictionary<int, float>();
        foreach (var progress in progressValues)
        {
            progressMaxValues.Add(progress.index, progress.value);
        }

        progressValue = new ReactiveProperty<float>();
        progressValue.Subscribe(value =>
        {
            HUD.instance.progress.SetProgressSliderValue(value);
            ProgressValueIsFull(value);
        });
    }

    IEnumerator IncreaseProgressValue()
    {
        while (true)
        {
            progressValue.Value += Time.deltaTime;

            yield return null;
        }
    }

    public void AddPoint()
    {
        progressValue.Value += point;
    }

    void ProgressValueIsFull(float value)
    {
        int index = (int)GameManager.instance.gameData.chapterType + 1;

        if (index >= (int)EChapterType.End)
        {
            Ending();
        }
        else
        {
            if (value >= progressMaxValues[index])
            {
                GameManager.instance.NextChapter();
            }
        }
    }

    public void ResetProgressValue()
    {
        progressValue.Value = 0;
    }

    void Ending()
    {
        PlayerController.instance.SetInvincible(true);
        BGMHandler.instance.PlayBGM(BGMDefine.END, BGMDefine.GAME_BGM_INFO[EGameState.End]);
        ScrollController.instance.StopScrolling();
        panel.SetActive(true);
        image.SetActive(true);
        text.SetActive(true);
        StartCoroutine(EndingDelay(30));
    }

    IEnumerator EndingDelay(float cooltime)
    {
        yield return new WaitForSeconds(cooltime);

        Application.Quit();
    }
}