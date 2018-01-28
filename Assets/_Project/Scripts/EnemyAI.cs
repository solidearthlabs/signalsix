using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

[DefaultExecutionOrder(100)]
public class EnemyAI : MonoBehaviour
{
    private float moveCheckTime = 0.2f;
    private float thresholdWalkingStop = 0.3f;
    private float thresholdWalkingStart = 0.1f;
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
        StartCoroutine(UpdateNav());
        StartCoroutine(CheckMonsterMovement());
    }

    IEnumerator UpdateNav()
    {
        while(player==null) { 
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
                player = go.transform;
            yield return new WaitForSeconds(1);
        }
        while (true)
        {
            navAgent.destination = player.position;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator CheckMonsterMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveCheckTime);
            pos = transform.position;
            speed = Vector3.Distance(lastPos, pos);
            lastPos = pos;
            if ((!walking && speed > thresholdWalkingStart) || (walking && speed < thresholdWalkingStop))
            {
                walking = !walking;
                animator.SetBool("moving", walking);
            }
        }
    }
}

