using UnityEngine;
using UniRx;
using DG.Tweening;

namespace Obstacle
{
    public class Sparrow : ObstacleController
    {
        [SerializeField]
        Vector2 destination;

        [SerializeField]
        float duration;

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
            string clipName = SFXDefine.BIRD;

            SFXHandler.instance.PlaySFX(clipName);
            #endregion

            transform.DOMove(destination, duration).SetEase(Ease.OutSine);
        }
    }
}

