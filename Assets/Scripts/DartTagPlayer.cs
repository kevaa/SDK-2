using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class DartTagPlayer : MonoBehaviour
{
    public bool isIt { get; protected set; }

    [SerializeField] GameObject dartPrefab;
    List<GameObject> dartsGO;
    List<Dart> darts;
    public float score { get; private set; } = 0f;

    public int dartCount { get; private set; }
    public int dartTotal { get; private set; } = 3;
    [SerializeField] float cooldownTime = 5f;
    [SerializeField] float timeBetweenShots = .5f;

    float scoreMultiplier = 100f;

    float attackAnimTime = .3f;
    [SerializeField] Transform dartSpawnTransform;

    bool shootCoroutineActive;
    protected Animator animator;

    public event Action OnHitOtherPlayer = delegate { };
    bool gameEnded = false;
    [SerializeField] ParticleSystem itEffect;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        dartCount = dartTotal;
        darts = new List<Dart>();
        dartsGO = new List<GameObject>();
        for (int i = 0; i < dartTotal; i++)
        {
            GameObject dartGo = Instantiate(dartPrefab, dartSpawnTransform.position, dartSpawnTransform.rotation);
            Dart dart = dartGo.GetComponent<Dart>();
            dart.OnHitOtherPlayer += HitOtherDartPlayer;
            darts.Add(dart);
            dartGo.SetActive(false);
            dartsGO.Add(dartGo);
        }
        GameManager.Instance.OnGameEnd += GameEnded;
        if (isIt)
        {
            itEffect.Play();
        }
    }

    void GameEnded()
    {
        gameEnded = true;
    }
    protected virtual void Update()
    {
        if (!gameEnded && !isIt)
        {
            score += scoreMultiplier * Time.deltaTime;
        }

    }
    protected void ShootDart()
    {
        if (isIt && !shootCoroutineActive && dartCount > 0)
        {
            StartCoroutine(ShootDartCoroutine());
        }
    }
    protected IEnumerator ShootDartCoroutine()
    {
        shootCoroutineActive = true;
        animator.SetTrigger("IsAttacking");
        yield return new WaitForSeconds(attackAnimTime);
        var dartInd = dartCount - 1;
        darts[dartInd].ShooterIt = isIt;
        dartsGO[dartInd].transform.position = dartSpawnTransform.position;
        dartsGO[dartInd].transform.rotation = dartSpawnTransform.rotation;
        dartsGO[dartInd].SetActive(true);
        dartCount--;

        if (dartCount == 0)
        {
            yield return new WaitForSeconds(cooldownTime);
            dartCount = dartTotal;
        }
        else
        {
            yield return new WaitForSeconds(timeBetweenShots);
        }
        shootCoroutineActive = false;
    }
    void HitOtherDartPlayer()
    {
        if (isIt)
        {
            isIt = false;
            itEffect.Stop();
        }
        OnHitOtherPlayer();
    }

    public void GetShot(bool shooterIt)
    {
        if (shooterIt)
        {
            isIt = true;
            itEffect.Play();
        }
    }

}