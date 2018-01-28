using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : MonoBehaviour {
    
    void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.tag == "Player")
        {
            GameManager.Instance.NextFloor();
            //Debug.Log("Transmitting");
        }
    }
}
