using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class StickToPlayer : MonoBehaviour
{
    public GameObject playerBody;
    public Vector3 forViewFitness = new Vector3(0f,0f,0f);
    public float high; 

    // Use this for initialization
    void Start()
    {
        transform.position = playerBody.transform.position + forViewFitness;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerBody.transform.position + forViewFitness;//+ new Vector3(0f, high f, 0f);
    }
}
