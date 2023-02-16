using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffect : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> effects;
    

    private FireEffectPool FireEffectPool;

    public void Init(FireEffectPool FireEffectPool)
    {
        this.FireEffectPool = FireEffectPool;
    }
    public void EnableEffects()
    {
        foreach (var item in effects)
        {
            item.Play();
        }
        Invoke("ReturnToPool",1.5f);
    }

    private void ReturnToPool()
    {
        FireEffectPool.ReturnFireEffect(this);
    }
}
