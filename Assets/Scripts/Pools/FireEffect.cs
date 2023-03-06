using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffect : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> effects;
    
    private FireEffectPool _fireEffectPool;

    public void Init(FireEffectPool fireEffectPool)
    {
        this._fireEffectPool = fireEffectPool;
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
        _fireEffectPool.ReturnFireEffect(this);
    }
}
