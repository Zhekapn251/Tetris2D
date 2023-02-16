using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem rightEffect;
    [SerializeField] private ParticleSystem leftEffect;

    private FireEffectPool FireEffectPool;

    public void Init(FireEffectPool FireEffectPool)
    {
        this.FireEffectPool = FireEffectPool;
    }
    public void EnableEffects()
    {
        rightEffect.Play();
        leftEffect.Play();
        Invoke("ReturnToPool",1.5f);
    }

    private void ReturnToPool()
    {
        FireEffectPool.ReturnFireEffect(this);
    }
}
