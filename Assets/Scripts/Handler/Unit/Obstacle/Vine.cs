using UnityEngine;
using UniRx;
using DG.Tweening;

namespace Obstacle
{
    public class Vine : ObstacleController
    {
        [SerializeField] float duration;
        [SerializeField] Vector3 targetPos;

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
            string clipName = SFXDefine.BUSH;

            SFXHandler.instance.PlaySFX(clipName);
            #endregion

            #region Movement
            transform.DOLocalMoveY(targetPos.y, duration).SetEase(Ease.OutQuad);
            #endregion
        }
    }

}
