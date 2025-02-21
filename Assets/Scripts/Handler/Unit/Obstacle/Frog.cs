using UnityEngine;
using UniRx;
using DG.Tweening;

namespace Obstacle
{
    public class Frog : ObstacleController
    {
        [SerializeField]
        Transform destination;

        [SerializeField]
        float height, duration;

        Animator anim;

        void Awake()
        {
            InitProperty();
        }

        void InitProperty()
        {
            anim = GetComponent<Animator>();
        }

        void Start()
        {
            base.onDetect = new ReactiveProperty<bool>(false);
            base.onDetect.Where(state => state == true)
                         .Subscribe(state => Appear());
        }

        void Update()
        {
            base.DetectPlayer(base.startPos.position, base.endPos.position);
        }

        void Appear()
        {
            #region Play SFX
            string clipName = SFXDefine.FROG;

            SFXHandler.instance.PlaySFX(clipName);
            #endregion

            #region Movement
            transform.DOJump(destination.position, height, 1, duration).SetEase(Ease.OutSine)
                     .SetDelay(0.55f)
                     .OnStart(() =>
                     {
                         anim.SetTrigger("jump");
                     });
            #endregion
        }

        void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.DrawWireSphere(destination.position, 0.5f);
        }
    }

}