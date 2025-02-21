using UnityEngine;

public enum EJudgementType
{
    Perfect,
    Greate,
    Good,
    Miss,
    Fail
}

public class JudgementCircleController : MonoBehaviour
{
    public EActionType actionType;

    [SerializeField] 
    float offset;

    [SerializeField] 
    float[] judgeRadius;

    Vector3 originPos;

    void Awake()
    {
        InitProperty();
    }

    void Update()
    {
        SetOffset();
    }

    void InitProperty()
    {
        originPos = transform.localPosition;
    }

    public EJudgementType Judge()
    {
        EJudgementType type = EJudgementType.Miss;

        Vector3 playerJudgeCirclePos = GameObject.FindGameObjectWithTag(TagDefine.JUDGE).transform.position;

        RaycastHit2D hit = Physics2D.Linecast(transform.position, playerJudgeCirclePos, LayerMask.GetMask(LayerDefine.JUDGE));

        for (int i = 0; i < judgeRadius.Length; i++)
        {
            if (hit.distance < judgeRadius[i])
            {
                type = (EJudgementType)i;
                break;
            }
        }

        return type;
    }

    void SetOffset()
    {
        float ratio = BoosterController.instance.BoosterRatio;

        SetPosition(offset * ratio);
    }

    void SetPosition(float value)
    {
        transform.localPosition = new Vector3(originPos.x + value, originPos.y, originPos.z);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagDefine.JUDGE))
        {
            if (ActionController.instance.CurrentKeyState == EActionKeyState.Inhibition)
                return;

            ActionController.instance.CurrentActionType = actionType;
            ActionController.instance.CurrentJudgeCircle = this;

            ActionController.instance.CurrentKeyState = EActionKeyState.Enable;
            ActionController.instance.ValidJudge = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagDefine.JUDGE))
        {
            ActionController.instance.CurrentActionType = EActionType.Jump;

            ActionController.instance.CurrentKeyState = EActionKeyState.Disable;
            ActionController.instance.ValidJudge = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        DrawJudgeLine();
        DrawJudgeCircle();
    }

    void DrawJudgeLine()
    {
        Vector3 playerJudgeCirclePos = UnityEngine.GameObject.FindGameObjectWithTag(TagDefine.JUDGE).transform.position;
        Gizmos.DrawLine(transform.position, playerJudgeCirclePos);
    }

    void DrawJudgeCircle()
    {
        foreach (var radius in judgeRadius)
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
