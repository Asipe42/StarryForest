using UnityEngine;
using UniRx;

namespace Obstacle
{
    public class Stone : ObstacleController
    {
        [SerializeField]
        float fallGravityScale;

        [SerializeField]
        ParticleSystem stoneDust;

        [SerializeField]
        PolygonCollider2D stoneCollider;

        Rigidbody2D rigid;

        void Awake()
        {
            InitProperty();
        }

        void InitProperty()
        {
            rigid = GetComponent<Rigidbody2D>();
            stoneCollider = GetComponent<PolygonCollider2D>();
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
            #region Fall
            rigid.gravityScale = fallGravityScale;
            stoneCollider.isTrigger = false;
            #endregion
        }

        void SetObstacle()
        {
            rigid.gravityScale = 0f;

            stoneCollider.isTrigger = true;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                #region Play SFX
                string clipName = SFXDefine.CRASH;

                SFXHandler.instance.PlaySFX(clipName);
                #endregion

                #region Play Particle
                stoneDust.Emit(16);
                #endregion

                SetObstacle();
            }
        }
    }
}