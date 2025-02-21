using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Progress : MonoBehaviour
{
    [SerializeField] 
    Slider progressSlider;

    ProgressController progress;

    void Start()
    {
        StartCoroutine(InitProperty());
    }

    IEnumerator InitProperty()
    {
        progress = FindObjectOfType<ProgressController>();

        yield return new WaitUntil(() => progress.progressMaxValues != null);

        progressSlider.minValue = 0;
        progressSlider.maxValue = progress.progressMaxValues[(int)GameManager.instance.gameData.chapterType + 1];
    }

    public void SetProgressSliderValue(float value)
    {
        progressSlider.value = value;
    }

    public void SetMaxValue()
    {
        progressSlider.maxValue = progress.progressMaxValues[(int)GameManager.instance.gameData.chapterType + 1];
    }
}
