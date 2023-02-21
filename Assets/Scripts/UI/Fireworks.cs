using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Fireworks : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    private int _nubberOfParticles;

    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private WinGame _winGame;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        var main = _particleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    private void Update()
    {
        var amount = Math.Abs(_nubberOfParticles - _particleSystem.particleCount);
        if (_particleSystem.particleCount > _nubberOfParticles) 
        { 
            StartCoroutine(PlaySound(sounds[UnityEngine.Random.Range(0, sounds.Length)], amount));
            
        }

        _nubberOfParticles = _particleSystem.particleCount;
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
        _winGame.ShowWinGame();
        //Debug.Log("System has stopped!");
    }
    private IEnumerator PlaySound(AudioClip clip, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var soundDelay = 0.05f;
            var sound = clip;
            StartCoroutine(PlaySound(sound, soundDelay));
            
            //Attempt to avoid multiple of the same audio being played at the exact same time - as it sounds wierd
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
