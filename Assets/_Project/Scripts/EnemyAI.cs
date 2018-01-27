using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[DefaultExecutionOrder(100)]
public class EnemyAI : MonoBehaviour
{
    private float moveCheckTime = 0.2f;
    private float thresholdWalkingStop = 0.3f;
    private float thresholdWalkingStart = 0.1f;
    private float stopDistance = 0.1f;
    private Transform player;
    private NavMeshAgent navAgent;
    private Animator animator;
    public float speed;
    private bool walking;
    private Vector3 pos, lastPos;

    // Use this for initialization
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        pos = transform.position;
        lastPos = pos;
        StartCoroutine(CheckMonsterMovement());
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
                player = go.transform;
        }
        if (player != null)
            navAgent.destination = player.position;
    }

    IEnumerator CheckMonsterMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveCheckTime);
            pos = transform.position;
            speed = Vector3.Distance(lastPos, pos);
            lastPos = pos;
            if (!walking && speed > thresholdWalkingStart)
            {
                walking = !walking;
                animator.SetBool("moving", walking);
            }
            else if (walking && speed < thresholdWalkingStop) {
                walking = !walking;
                animator.SetBool("moving", walking);
                navAgent.isStopped = true;
            }
            else if (navAgent.isStopped)
            {
                float dist = Vector3.Distance(player.transform.position, pos);
                if (dist > stopDistance)
                    navAgent.isStopped = false;
            }

        }
    }
}

