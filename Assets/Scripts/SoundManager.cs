using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] AudioClip pieceMoveSound;
    [SerializeField] AudioClip pieceRotateSound;
    AudioSource au;

    void Start()
    {
        au = GetComponent<AudioSource>(); //cashing the variable
    }


    public void PlaySound(string audioClip)
    {
        if (!au.isPlaying)
        {
            if(audioClip=="rotate")
            {
                au.PlayOneShot(pieceRotateSound);
            }
            if(audioClip=="move")
            {
                au.PlayOneShot(pieceMoveSound);
            }
        }

        //au.Stop();
    }

    
}