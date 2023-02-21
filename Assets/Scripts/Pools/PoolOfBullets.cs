using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolOfBullets : MonoBehaviour
{
    public List<Bullet> pooledBullets;
    public Bullet pooledBullet;
    private int amountInPool = 3;

    private void Start()
    {
        Bullet tmp;
        pooledBullets = new List<Bullet>();
        for (int i = 0; i < amountInPool; i++)
        {
            tmp = Instantiate(pooledBullet, transform);
            tmp.Init(this);
            tmp.gameObject.SetActive(false);
            pooledBullets.Add(tmp);
        }
    }
    
    public void ReturnBullet(Bullet bullet)
    {
        if (!pooledBullets.Contains(bullet))
            pooledBullets.Add(bullet);
    }

    public Bullet GetBullet()
    {
        if (pooledBullets.Count > 0)
        {
            Bullet bullet = pooledBullets[0];
            pooledBullets.RemoveAt(0);
            return bullet;
        }
        else
        {
            Bullet bullet = Instantiate(pooledBullet,transform);
            bullet.Init(this);
            return bullet;
        }
    }
}
