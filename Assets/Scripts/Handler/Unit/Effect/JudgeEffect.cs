using UnityEngine;

namespace Effect
{
    public class JudgeEffect : EffectController
    {
        void Awake()
        {
            this.InitProperty();
        }

        protected override void InitProperty()
        {
            base.name = EffectDefine.JUDGE;
            base.maxSize = 5;

            base.InitProperty();
        }

        protected override EffectPool SpawnEffect()
        {
            EffectPool effect = pool.Get();
            effect.transform.SetParent(gameObject.transform, false);

            return effect;
        }

        public void PlayJudgeEffect(Vector3 pos, EJudgementType type)
        {
            EffectPool effect = SpawnEffect();

            SetPosition(effect, pos);
            SetType(effect, type);

            effect.PlayEffect();
        }

        void SetPosition(EffectPool effect, Vector3 pos)
        {
            effect.transform.position = pos;
        }

        void SetType(EffectPool effect, EJudgementType type)
        {
            switch (type)
            {
                case EJudgementType.Perfect:
                    break;
                case EJudgementType.Greate:
                    break;
                case EJudgementType.Good:
                    break;
                case EJudgementType.Fail:
                    break;
            }
        }
    }
}