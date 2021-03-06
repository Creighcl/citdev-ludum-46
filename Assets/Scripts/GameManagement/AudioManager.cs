﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager>
{
    AudioSource audioSource;
    [SerializeField]
    public AudioSource backgroundMusicAS;
    [SerializeField]
    public AudioSource backgroundMusicAS2;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        EventManager.Instance.OnPlayerJumpSuccessful += HandleJump;
        EventManager.Instance.OnPlayerChangeDirection += HandleDirectionChange;
        EventManager.Instance.OnPlayerShotFailedNoEnergy += HandleShotFailedNoEnergy;
        EventManager.Instance.OnPlayerShoot += HandleShooting;
        EventManager.Instance.OnPlayerStoppedShooting += HandleStoppedShooting;
        EventManager.Instance.OnPlayerDropPlantSuccess += HandleDroppedPlant;
        EventManager.Instance.OnPlayerPickupPlantSuccess += HandlePickedUpPlant;
        EventManager.Instance.OnPlantDied += HandlePlantDeath;
        EventManager.Instance.OnBadGuyDied += HandleBadGuyDied;
        EventManager.Instance.OnPlantHealthFilled += HandlePlantHealthFilled;
        EventManager.Instance.OnButtonClick += HandleButtonClick;
        EventManager.Instance.OnPayAnimation += HandlePayAnimation;
    }

    void HandlePayAnimation() {
        PlaySound("Coin_up");
    }

    void HandleButtonClick() {
        PlaySound("Button_Press");
    }

    void HandlePlantHealthFilled() {
        PlaySound("Planty_Full");
    }

    void HandleJump() {
        PlaySound("Jumping");
    }

    // void HandleLand() {
    //     PlaySound("Landing");
    // }

    void HandleShotFailedNoEnergy() {
        PlaySound("Firing_OutOfEnergy");
    }

    void HandleShooting() {
        PlaySound("Firing_WithEnergy");
    }

    void HandleStoppedShooting() {
        // PlaySound("Firing_Stops");
    }

    void HandleDroppedPlant(Vector3 position) {
        PlaySound("Plant_Dropped");

        backgroundMusicAS.time = 0f;
        backgroundMusicAS2.volume = 0.05f;
        // backgroundMusicAS2.Pause();
        backgroundMusicAS.Play();
    }

    void HandlePickedUpPlant() {
        PlaySound("Plant_PickedUp");
        backgroundMusicAS2.volume = 0.1f;
        backgroundMusicAS.Stop();
    }

    void HandlePlantDeath() {
        PlaySound("Plant_Death");
    }

    void HandleDirectionChange(bool direction) {
        PlaySound("Player_ChangeDirection");
    }

    void HandleBadGuyDied(Vector2 pos) {
        PlaySound("Grub_Death");
    }

    void PlaySound(string name)
    {
        AudioClip audioClip = GetAudioClipByName(name);
        if (audioClip != null) {
            audioSource.PlayOneShot(audioClip);
        } else {
            Debug.Log("null audio clip: " + name);
        }
    }

    AudioClip GetAudioClipByName(string clipName)
    {
        return (AudioClip)Resources.Load("Sounds/" + clipName);
    }
}