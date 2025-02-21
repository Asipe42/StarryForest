using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class EffectPool: MonoBehaviour
{
    IObjectPool<EffectPool> managedPool;

    ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void SetManagedPool(IObjectPool<EffectPool> pool)
    {
        managedPool = pool;
    }

    public void DestroyJudgeText()
    {
        managedPool.Release(this);
    }

    public void PlayEffect()
    {
        if (ps == null)
            ps = GetComponent<ParticleSystem>();

        ps.Play();

        StartCoroutine(AutoDisable());
    }

    IEnumerator AutoDisable()
    {
        yield return new WaitUntil(() => ps.isStopped);

        gameObject.SetActive(false);
    }
}
