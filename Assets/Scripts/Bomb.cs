using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private ParticleSystem bigBombParticles;
    [SerializeField] private ParticleSystem smallBombParticles;
    private Vector2Int coordinates;

    public void Init(Vector2Int coordinates)
    {
        this.coordinates = coordinates;
    }

}
