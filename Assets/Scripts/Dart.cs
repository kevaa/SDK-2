using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]

public class Dart : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float dartAliveTime;
    [SerializeField] float dartSpeed = 3f;
    bool hitSomething;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip hitWallSound;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }
    public event Action OnHitOtherPlayer = delegate { };
    public bool ShooterIt { get; set; }
    private void OnCollisionEnter(Collision other)
    {
        if (!hitSomething)
        {
            hitSomething = true;

            var dartTagPlayer = other.gameObject.GetComponent<DartTagPlayer>();
            if (dartTagPlayer != null)
            {
                audioSource.PlayOneShot(hitSound, .5f);
                OnHitOtherPlayer();
                dartTagPlayer.GetShot(ShooterIt);
            }
            else
            {
                audioSource.PlayOneShot(hitWallSound, .5f);
            }
        }
        else
        {
            audioSource.PlayOneShot(hitWallSound, .5f);
        }

    }

    void OnEnable()
    {
        audioSource.PlayOneShot(shootSound, .5f);
        StartCoroutine(DisableAfterSeconds(dartAliveTime));
        rb.velocity = dartSpeed * transform.forward;
    }

    IEnumerator DisableAfterSeconds(float dartAliveTime)
    {
        yield return new WaitForSeconds(dartAliveTime);
        hitSomething = false;
        gameObject.SetActive(false);
    }

}
