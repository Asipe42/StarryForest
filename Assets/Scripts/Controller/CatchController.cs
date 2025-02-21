using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(ObservableEventTrigger))]
public class CatchController : MonoBehaviour
{
    public static CatchController instance;

    ObservableEventTrigger eventTrigger;

    ReactiveProperty<bool> onCatch;
    public bool OnCatch
    {
        get => onCatch.Value;
        set => onCatch.Value = value;
    }

    void Awake()
    {
        instance = this;

        onCatch = new ReactiveProperty<bool>(false);

        eventTrigger = GetComponent<ObservableEventTrigger>();
        // eventTrigger.OnPointerDownAsObservable().Subscribe(_ => PlayCatch());
    }

    /*
    void PlayCatch()
    {
        if (QTEController.instance.OnQTE)
            return;

        if (!onCatch.Value)
            return;

        #region Start Slow Time
        TimeController.instance.PlaySlowTime();
        #endregion
    }
    */
}
