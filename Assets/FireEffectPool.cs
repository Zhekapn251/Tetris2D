using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffectPool : MonoBehaviour
{
    [SerializeField] private FireEffect prefabFireEffect;
    private List<FireEffect> pool;

    private void Start()
    {
        pool = new List<FireEffect>();
        for (int i = 0; i < 10; i++)
        {
            FireEffect fireEffect = Instantiate(prefabFireEffect, transform);
            fireEffect.Init(this);
            pool.Add(fireEffect);
        }
    }

    public void ReturnFireEffect(FireEffect fireEffect)
    {
        if (!pool.Contains(fireEffect))
            pool.Add(fireEffect);
    }

    public FireEffect GetEffect()
    {
        if (pool.Count > 0)
        {
            FireEffect fireEffect = pool[0];
            pool.RemoveAt(0);
            return fireEffect;
        }
        else
        {
            FireEffect fireEffect = Instantiate(prefabFireEffect,transform);
            fireEffect.Init(this);
            return fireEffect;
        }
    }
}