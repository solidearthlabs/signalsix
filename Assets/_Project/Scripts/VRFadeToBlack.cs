using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFadeToBlack : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public static void Darken()
    {
        SteamVR_Fade.Start(Color.black, 1f);
    }
    public static void Lighten()
    {
        SteamVR_Fade.Start(Color.clear, 1f);
    }
}
