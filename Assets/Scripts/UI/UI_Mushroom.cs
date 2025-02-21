using UnityEngine;
using TMPro;

public class UI_Mushroom : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI mushroomText;

    public void SetMushroomText(int value)
    {
        mushroomText.text = "X " + value;
    }
}
