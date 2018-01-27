using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent navAgent;
    // Use this for initialization
    void Start()
    {
        
        navAgent = GetComponent<NavMeshAgent>();
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


}

