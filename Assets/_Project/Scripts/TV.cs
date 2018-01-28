using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : MonoBehaviour {

    bool canActivate = true;
    void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.tag == "Player" && canActivate)
        {
            canActivate = false;
            GameManager.Instance.NextFloor();
            Debug.Log("Transmitting");
        }
    }
}
