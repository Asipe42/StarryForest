using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[System.Serializable]
public struct Speaker
{
    public Sprite portraitSprite;
    public Vector3 portraitScale;
}

[System.Serializable]
public struct DialogData
{
    public int speakerIndex;
    public string name;
    [TextArea(3, 5)]
    public string message;
}

public class DialogSystem : MonoBehaviour
{
    [SerializeField] Image portrait;
    [SerializeField] Image Panel;
    [SerializeField] Image messageBox;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] Image arrow;

    [SerializeField] int stage;
    [SerializeField] int branch;
    [SerializeField] float typingSpeed = 0.1f;
    [SerializeField] bool onStart = true;

    [SerializeField] DialogDB dialogDB;
    [SerializeField] Speaker[] speakers;
    [SerializeField] DialogData[] dialogs;

    int currentDialogIndex = -1;
    int currentSpeakerIndex = 0;

    bool isFirst = true;
    bool onTyping;

    AudioClip messageClip;

    AudioSource audioSource;

    IEnumerator typingCoroutine;

    void Awake()
    {
        Initialize();
        GetAudioClip();
    }

    #region Initial Setting
    void Initialize()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void GetAudioClip()
    {
        messageClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Message");
    }
    #endregion

    void Start()
    {
        ParsingData(stage);
    }

    public void ProgressDialog()
    {
        StartCoroutine(ProgressDialogLogic());
    }

    bool UpdateDialog()
    {
        if (isFirst == true)
        {
            SetActivateObjects(true);

            if (onStart)
            {
                SetNextDialog();
            }

            isFirst = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (onTyping)
            {
                onTyping = false;

                audioSource.Stop();
                StopCoroutine("TypingMessage");

                message.text = dialogs[currentDialogIndex].message;
                arrow.DOKill();
                arrow.DOFade(1f, 0.75f).SetLoops(-1, LoopType.Yoyo);

                return false;
            }

            if (dialogs.Length > currentDialogIndex + 1)
            {
                SetNextDialog();
            }
            else
            {
                SetActivateObjects(false);

                return true;
            }
        }

        return false;
    }

    void ParsingData(int stageIndex)
    {
        int count = 0;
        int index = 0;

        #region Title
        if (stageIndex == 1)
        {
            for (int i = 0; i < dialogDB.Title.Count; i++)
            {
                if (dialogDB.Title[i].branch == branch)
                {
                    count++;
                }
            }

            dialogs = new DialogData[count];

            for (int i = 0; i < dialogDB.Title.Count; i++)
            {
                if (dialogDB.Title[i].branch == branch)
                {
                    dialogs[index].speakerIndex = dialogDB.Title[i].speakerIndex;
                    dialogs[index].name = dialogDB.Title[i].name;
                    dialogs[index].message = dialogDB.Title[i].message;
                    index++;
                }
            }
        }
        #endregion

        #region Title
        if (stageIndex == 2)
        {
            for (int i = 0; i < dialogDB.End.Count; i++)
            {
                if (dialogDB.End[i].branch == branch)
                {
                    count++;
                }
            }

            dialogs = new DialogData[count];

            for (int i = 0; i < dialogDB.End.Count; i++)
            {
                if (dialogDB.End[i].branch == branch)
                {
                    dialogs[index].speakerIndex = dialogDB.End[i].speakerIndex;
                    dialogs[index].name = dialogDB.End[i].name;
                    dialogs[index].message = dialogDB.End[i].message;
                    index++;
                }
            }
        }
        #endregion
    }

    IEnumerator ProgressDialogLogic()
    {
        yield return new WaitUntil(() => UpdateDialog());
        TimelineController.instance.ContinueTimeline();
    }

    IEnumerator TypingMessage()
    {
        onTyping = true;

        for (int i = 0; i <= dialogs[currentDialogIndex].message.Length; i++)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = messageClip;
                audioSource.Play();
            }

            message.text = dialogs[currentDialogIndex].message.Substring(0, i);
            yield return new WaitForSeconds(typingSpeed);
        }

        onTyping = false;
        audioSource.Stop();

        arrow.DOKill();
        arrow.DOFade(1f, 0.75f).SetLoops(-1, LoopType.Yoyo);
    }

    void SetNextDialog()
    {
        arrow.DOKill();
        arrow.DOFade(0f, 0.25f);

        currentDialogIndex++;
        currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;

        if (portrait.sprite != speakers[currentSpeakerIndex].portraitSprite)
        {
            FadePortrait();
        }
        else
        {
            portrait.sprite = speakers[currentSpeakerIndex].portraitSprite;
            portrait.transform.localScale = speakers[currentSpeakerIndex].portraitScale;
        }

        name.text = dialogs[currentDialogIndex].name;
        message.text = dialogs[currentDialogIndex].message;

        StartCoroutine("TypingMessage");
    }

    void FadePortrait()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(portrait.DOFade(0f, 0.1f))
                .AppendCallback(() =>
                {
                    portrait.sprite = speakers[currentSpeakerIndex].portraitSprite;
                    portrait.transform.localScale = speakers[currentSpeakerIndex].portraitScale;
                })
                .Append(portrait.DOFade(1f, 0.1f));
    }

    void SetActivateObjects(bool visible)
    {
        if (visible)
        {
            Panel.DOFade(1f, 1f);
            messageBox.DOFade(1f, 0.75f);
            portrait.DOFade(1f, 0.75f);
            name.DOFade(1f, 0.75f);
            message.DOFade(1f, 0.75f);         
        }

        if (!visible)
        {
            Panel.DOFade(0f, 0.75f);
            messageBox.DOFade(0f, 0.5f);
            portrait.DOFade(0f, 0.5f);
            name.DOFade(0f, 0.5f);
            message.DOFade(0f, 0.5f);
        }
    }
}
