using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private ParticleSystem _bomb;

    private PoolOfBullets _poolOfBullets;

    public void Init(PoolOfBullets poolOfBullets)
    {
        this._poolOfBullets = poolOfBullets;
    }
    

    private void ReturnToPool()
    {
        this.gameObject.SetActive(false);
        _poolOfBullets.ReturnBullet(this);
    }
    
    public void FireBullet(Vector3Int start, Vector3Int distance)
    {
        Vector3 startPosition = (Vector3)start;
        transform.position = new Vector3(startPosition.x + 0.5f, startPosition.y-1f, 0);
        Vector3 distancePosition = new Vector3(distance.x + 0.5f, distance.y+0.5f, 0);
        gameObject.SetActive(true);
        _particleSystem.gameObject.SetActive(true);
        _bomb.gameObject.SetActive(true);
        DOTween.Sequence()
            .Append(transform.DOMove(distancePosition, 0.2f))
            .AppendCallback(() => _particleSystem.gameObject.SetActive(false))
            .AppendCallback(()=>_bomb.Play());
        Invoke("ReturnToPool",2f);
    }
}
