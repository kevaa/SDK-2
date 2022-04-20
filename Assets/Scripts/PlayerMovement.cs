using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    Vector3 movement;
    Quaternion mRotation = Quaternion.identity;
    Rigidbody rb;
    AudioSource mAudioSource;
    bool gameEnded = false;

    Animator mAnimator;
    [SerializeField] float moveSpeed = .5f;

    bool isWalking;
    public event Action OnPushed = delegate { };

    private void Awake()
    {
        mAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        mAudioSource = GetComponent<AudioSource>();

    }
    private void Start()
    {
        GameManager.Instance.OnGameEnd += OnGameEnd;
    }

    void OnGameEnd()
    {
        gameEnded = true;
        mAudioSource.Stop();
    }

    void FixedUpdate()
    {
        var horizontal = 0f;
        var vertical = 0f;
        if (!gameEnded)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }

        movement.Set(horizontal, 0f, vertical);
        movement.Normalize();
        isWalking = IsWalking(horizontal, vertical);
        mAnimator.SetBool("IsWalking", isWalking);
        if (isWalking)
        {
            if (!mAudioSource.isPlaying)
            {
                mAudioSource.Play();
            }
        }
        else
        {
            mAudioSource.Stop();
        }
        rb.MovePosition(rb.position + transform.TransformDirection(movement * moveSpeed * Time.fixedDeltaTime));
    }

    private bool IsWalking(float horizontal, float vertical)
    {
        var hasVerticalUnput = !Mathf.Approximately(vertical, 0f);
        var hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        return hasHorizontalInput || hasVerticalUnput;
    }
}
