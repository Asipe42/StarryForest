using UnityEngine;
using UniRx;
using DG.Tweening;

namespace Obstacle
{
    public class Crow : ObstacleController
    {
        [SerializeField]
        Transform[] destination;

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
            string clipName = SFXDefine.CROW;

            SFXHandler.instance.PlaySFX(clipName);
            #endregion

            #region Movement
            var sequence = DOTween.Sequence();

            sequence.Append(transform.DOMove(destination[0].position, 0.4f).OnStart(() => anim.SetTrigger("start")))
                                .AppendInterval(0.2f)
                                .Append(transform.DOMove(destination[1].position, 0.8f).SetEase(Ease.OutSine).OnStart(() => anim.SetBool("goDown", true)).OnComplete(() => anim.SetBool("fly", true)));
            #endregion
        }
    }

}

