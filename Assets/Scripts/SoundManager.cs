using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip loseSound;
    [Header("Pieces")]
    [SerializeField] AudioClip moveAsideSound;
    [SerializeField] AudioClip rotateSound;
    [SerializeField] AudioClip moveDownSound;
    [SerializeField] AudioClip fireSound;
    [SerializeField] AudioClip dropSound;
    [SerializeField] private AudioClip lockSound;
    
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlaySound(Sounds sound)
    {
        var clip = GetAudioClip(sound);
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetAudioClip(Sounds sound)
    {
        return sound switch
        {
            Sounds.Win => winSound,
            Sounds.Lose => loseSound,
            Sounds.MoveAside => moveAsideSound,
            Sounds.Rotate => rotateSound,
            Sounds.MoveDown => moveDownSound,
            Sounds.Fire => fireSound,
            Sounds.Drop => dropSound,
            Sounds.Lock => lockSound,
            _ => null
        };
    }
}
