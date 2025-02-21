using UnityEngine;
using UnityEngine.Pool;

public class EffectController : MonoBehaviour
{
    protected IObjectPool<EffectPool> pool;
    protected int maxSize = 10;
    protected string name;

    protected virtual void InitProperty()
    {
        pool = new ObjectPool<EffectPool>(CreateEffect, OnGetEffect, OnReleaseEffect, OnDestroyEffect, maxSize: this.maxSize);
    }

    EffectPool CreateEffect()
    {
        EffectPool effect = Instantiate(EffectManager.EffectDictionary[name]).GetComponent<EffectPool>();
        effect.SetManagedPool(pool);
        return effect;
    }

    void OnGetEffect(EffectPool effect)
    {
        effect.gameObject.SetActive(true);
    }

    void OnReleaseEffect(EffectPool effect)
    {
        effect.gameObject.SetActive(false);
    }

    void OnDestroyEffect(EffectPool effect)
    {
        Destroy(effect.gameObject);
    }

    protected virtual EffectPool SpawnEffect()
    {
        EffectPool effect = pool.Get();
        effect.transform.SetParent(gameObject.transform, false);

        return effect;
    }
}
