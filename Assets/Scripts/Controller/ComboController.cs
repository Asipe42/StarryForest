using UnityEngine;
using UniRx;

public class ComboController : MonoBehaviour
{
    public static ComboController instance;

    ReactiveProperty<int> comboCount;

    UI_ComboText comboText;

    void Awake()
    {
        InitProperty();
    }

    void InitProperty()
    {
        instance = this;

        comboText = GetComponent<UI_ComboText>();

        comboCount = new ReactiveProperty<int>(0);

        comboCount.Subscribe(value => comboText.UpdateComboText(value));  
    }

    public void AddComboCount()
    {
        comboCount.Value++;
    }

    public void ResetComboCount()
    {
        comboCount.Value = 0;
    }

    public int GetComboCount()
    {
        int count = comboCount.Value;

        return count;
    }
}
