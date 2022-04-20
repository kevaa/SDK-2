using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAIController : DartTagPlayer
{
    NavMeshAgent navMeshAgent;
    float randomDestDistance = 30f;
    float escapeDist = 8f;

    Transform target;
    AudioSource audioSource;

    bool startedSeekingCoroutine;
    float viewDist = 20f;
    bool seekingTarget;
    float rotSpeed = 5f;

    float seekTimeout = 5f;
    float seekTimer = 0f;
    bool sawItPlayer;
    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

    }
    protected override void Start()
    {
        base.Start();
        animator.SetBool("IsWalking", true);
        navMeshAgent.SetDestination(GetRandomWanderPos(randomDestDistance, -1));
        GameManager.Instance.OnGameEnd += OnGameEnd;
    }

    void OnGameEnd()
    {
        audioSource.Stop();
    }
    protected override void Update()
    {
        base.Update();
        HandleWalkingAnim();
        if (isIt)
        {
            HuntingState();
        }
        else
        {
            EvadingState();
        }
    }

    void HandleWalkingAnim()
    {
        if (navMeshAgent.velocity.magnitude > 0f)
        {
            animator.SetBool("IsWalking", true);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
            audioSource.Stop();

        }
    }
    void HuntingState()
    {
        CheckVisibleTarget(targetIt: false);
        if (target == null)
        {
            // wander until target found
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                navMeshAgent.SetDestination(GetRandomWanderPos(randomDestDistance, -1));
            }
        }
        else
        {
            if (seekTimer < seekTimeout)
            {
                // continue rotating even when navmeshagent is within stopping distance
                if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                {
                    var thisToTarget = (target.transform.position - transform.position).normalized;
                    Quaternion lookRot = Quaternion.LookRotation(thisToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotSpeed);
                }
                navMeshAgent.SetDestination(target.position);
                if (TargetInCrosshair())
                {
                    ShootDart();
                }
                seekTimer += Time.deltaTime;
            }
            else
            {
                target = null;
                seekTimer = 0f;
            }
        }
    }

    void EvadingState()
    {
        if (target == null || sawItPlayer)
        {
            // wander until target found
            CheckVisibleTarget(targetIt: true);

            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                sawItPlayer = false;
                navMeshAgent.SetDestination(GetRandomWanderPos(randomDestDistance, -1));
            }
        }
        else
        {
            var targetToSelf = (transform.position - target.position).normalized;
            NavMeshHit navMeshHit;
            NavMesh.SamplePosition(transform.position + (targetToSelf * escapeDist), out navMeshHit, escapeDist, -1);
            if ((navMeshHit.position - transform.position).magnitude > 4)
            {
                navMeshAgent.SetDestination(navMeshHit.position);
            }
            else
            {
                navMeshAgent.SetDestination(GetRandomWanderPos(randomDestDistance, -1));

            }
            target = null;
            sawItPlayer = true;
        }
    }

    void CheckVisibleTarget(bool targetIt)
    {
        var initialAngle = Quaternion.AngleAxis(-45, Vector3.up);
        var stepAngle = Quaternion.AngleAxis(5, Vector3.up);
        RaycastHit hit;
        var direction = initialAngle * transform.forward;
        var pos = transform.position + (1.1f * Vector3.up);
        for (int i = 0; i < 19; i++)
        {
            if (Physics.Raycast(pos, direction, out hit, viewDist))
            {
                var dartTagPlayer = hit.collider.GetComponent<DartTagPlayer>();
                if (dartTagPlayer != null)
                {
                    if (targetIt)
                    {
                        if (dartTagPlayer.isIt)
                        {
                            target = dartTagPlayer.transform;
                            seekTimer = 0f;
                            return;
                        }
                    }
                    else
                    {
                        if (!dartTagPlayer.isIt)
                        {
                            target = dartTagPlayer.transform;
                            seekTimer = 0f;
                            return;
                        }
                    }
                }
            }
            // Debug.DrawRay(pos, direction);
            direction = stepAngle * direction;
        }
    }

    bool TargetInCrosshair()
    {
        RaycastHit hit;
        var initialAngle = Quaternion.AngleAxis(-5, Vector3.up);
        var stepAngle = Quaternion.AngleAxis(5, Vector3.up);
        var direction = initialAngle * transform.forward;
        var pos = transform.position + (1.1f * Vector3.up);
        for (int i = 0; i < 3; i++)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, viewDist))
            {
                var dartTagPlayer = hit.collider.GetComponent<DartTagPlayer>();
                if (dartTagPlayer != null)
                {
                    return true;
                }
            }
        }
        return false;

    }

    public Vector3 GetRandomWanderPos(float dist, int layermask)
    {
        var randomSurroundingPos = transform.position + (Random.insideUnitSphere * dist);
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(randomSurroundingPos, out navMeshHit, dist, layermask);

        return navMeshHit.position;
    }

}
