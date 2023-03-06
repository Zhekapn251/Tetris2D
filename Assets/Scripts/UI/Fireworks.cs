using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Fireworks : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private WinGame winGame;
    
    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    private int _numberOfParticles;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        var main = _particleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    private void Update()
    {
        var amount = Math.Abs(_numberOfParticles - _particleSystem.particleCount);
        if (_particleSystem.particleCount > _numberOfParticles) 
        { 
            StartCoroutine(PlaySound(sounds[UnityEngine.Random.Range(0, sounds.Length)], amount));
        }

        _numberOfParticles = _particleSystem.particleCount;
    }

    public void FireworksEnable()
    {
        gameObject.SetActive(true);
    }

    public void FireWorksDisable()
    {
        gameObject.SetActive(false);
    }

    void OnParticleSystemStopped()
    {
        winGame.ShowWinGame();
    }
    private IEnumerator PlaySound(AudioClip clip, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var soundDelay = 0.05f;
            var sound = clip;
            StartCoroutine(PlaySound(sound, soundDelay));
            
            yield return new WaitForSeconds(0.05f);
        }
    }
    private IEnumerator PlaySound(AudioClip sound, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        _audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        _audioSource.PlayOneShot(sound);
    }
}
