using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFadeToBlack : MonoBehaviour {
    
    public static void Darken()
    {
        SteamVR_Fade.Start(Color.black, 1f);
    }
    public static void Lighten()
    {
        SteamVR_Fade.Start(Color.clear, 1f);
    }
}
