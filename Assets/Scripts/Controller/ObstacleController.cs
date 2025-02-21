using UnityEngine;
using UniRx;
using DG.Tweening;

public class ObstacleController : MonoBehaviour
{    
    [SerializeField] 
    protected float damage;

    [SerializeField]
    protected Transform startPos, endPos;

    [SerializeField] 
    protected RaycastHit2D hit;

    protected ReactiveProperty<bool> onDetect;

    protected bool onAppear;

    protected virtual void DetectPlayer(Vector3 startPos, Vector3 endPos)
    {
        if (onDetect.Value)
            return;

        hit = Physics2D.Linecast(startPos, endPos, LayerMask.GetMask(LayerDefine.PLAYER));

        if (hit.collider != null)
            onDetect.Value = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagDefine.PLAYER))
        {
            if (SuperBoosterController.instance.OnSuperBooster)
                Shoot();
            else
            {
                if (collision.gameObject.TryGetComponent(out PlayerController pc))
                {
                    pc.Hit(damage, SFXDefine.OBSTACLE_HIT[gameObject.name]);
                }
            }
        }
    }

    protected void OnDrawGizmos()
    {
        if (startPos == null || endPos == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos.position, endPos.position);
    }

    public void Shoot()
    {
        var sequence = DOTween.Sequence();

        int index = Random.Range(0, 2);
        if (index == 1)
        {
            SFXHandler.instance.PlaySFX(SFXDefine.SHOOT_1);
        }
        else
        {
            SFXHandler.instance.PlaySFX(SFXDefine.SHOOT_2);

        }

        sequence.Append(transform.DOMove(new Vector3(30, 2, 1), 1.5f).SetEase(Ease.OutQuad))
                .Join(transform.DORotate(new Vector3(0, 0, 350f), 1.5f,RotateMode.FastBeyond360))
                .Join(transform.DOPunchScale(new Vector3(0.1f, 0.1f, 1f), 0.5f, vibrato:10));

        sequence.OnComplete(() => Destroy(this.gameObject));
    }
}
