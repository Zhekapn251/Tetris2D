using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffectPool : MonoBehaviour
{
    [SerializeField] private FireEffect prefabFireEffect;
    private List<FireEffect> _pool;

    private void Start()
    {
        _pool = new List<FireEffect>();
        for (int i = 0; i < 10; i++)
        {
            FireEffect fireEffect = Instantiate(prefabFireEffect, transform);
            fireEffect.Init(this);
            _pool.Add(fireEffect);
        }
    }

    public void ReturnFireEffect(FireEffect fireEffect)
    {
        if (!_pool.Contains(fireEffect))
            _pool.Add(fireEffect);
    }

    public FireEffect GetEffect()
    {
        if (_pool.Count > 0)
        {
            FireEffect fireEffect = _pool[0];
            _pool.RemoveAt(0);
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