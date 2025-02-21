using UniRx;
using UnityEngine;

namespace Obstacle
{
    public class Hole : ObstacleController
    {
        void Start()
        {
            base.onDetect = new ReactiveProperty<bool>(false);
            base.onDetect.Where(state => state == true)
                         .Subscribe(state => FadeInOut());
        }

        void Update()
        {
            base.DetectPlayer(base.startPos.position, base.endPos.position);
        }

        void FadeInOut()
        {
            ScreenEffect.instance.PlayScreen(ScreenDefine.FADE);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(TagDefine.PLAYER))
            {
                if (collision.gameObject.TryGetComponent(out PlayerController pc))
                {
                    pc.Fall(base.damage, SFXDefine.OBSTACLE_HIT[gameObject.name]);
                }
            }
        }
    }
}