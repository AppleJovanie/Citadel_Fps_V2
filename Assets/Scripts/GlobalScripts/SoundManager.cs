using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    public AudioSource shootingSoundPistol;
    public AudioSource reloadingSoundPistol;
    public AudioSource emptyMagazineSoundPistol;
    public AudioSource shotGunSound;

    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;

    public AudioSource zombieChannel;
    public AudioSource zombieChannel2; 

    public AudioSource playerChannel; 
    public AudioClip playerHurt;
    public AudioClip playerDeath;

    public AudioClip gameOverMusic;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PauseAllAudio()
    {
       // backgroundMusic.Pause();
        zombieChannel.Pause();
        zombieChannel2.Pause();
        playerChannel.Pause();
    }

    public void ResumeAllAudio()
    {
        //backgroundMusic.UnPause();
        zombieChannel.UnPause();
        zombieChannel2.UnPause();
        playerChannel.UnPause();
    }
}
