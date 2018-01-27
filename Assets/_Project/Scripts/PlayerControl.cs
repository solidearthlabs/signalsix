/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerControl : MonoBehaviour {
	
	private SteamVR_TrackedObject trackObj
	
	//取得控制器的哪個按鈕被壓下、放開、觸碰
	private SteamVR_Controller.Device controller
	{
		get
		{
			return SteamVR_Controller.Input((int)trackObj.index);
		}
	}

	// Use this for initialization
	void Start () {
		trackObj = GetComponent<SteamVR_TrackedObject>();//取得控制器腳本物件
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
*/

  using UnityEngine;
  using System.Collections;
  using System.Collections.Generic;
  using Valve.VR;
  
  public class PlayerControl : MonoBehaviour
  {
      public GameObject player;
  
      //SteamVR_Controller.Device device;
      SteamVR_TrackedObject trackObj;
	  
	  private SteamVR_Controller.Device controller
	{
		get
		{
			return SteamVR_Controller.Input((int)trackObj.index);
		}
	}
  
      Vector2 touchpad;
  
      private float sensitivityX = 1.5F;
      private Vector3 playerPos;
  
      void Start()
      {
          trackObj = GetComponent<SteamVR_TrackedObject>();
      }
  
      // Update is called once per frame
      void Update()
      {
		  if (controller.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
			// Debug.Log("press");
		}
		  
          //If finger is on touchpad
          if (controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
          {
              //Read the touchpad values
              touchpad = controller.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
  
              // Handle movement via touchpad
              if (touchpad.y > 0.2f || touchpad.y < -0.2f) {
                  // Move Forward
                  player.transform.position -= player.transform.forward * Time.deltaTime * (touchpad.y * 5f);
  
                  // Adjust height to terrain height at player positin
                  playerPos = player.transform.position;
                  playerPos.y = Terrain.activeTerrain.SampleHeight (player.transform.position);
                  player.transform.position = playerPos;
              }
  
              // handle rotation via touchpad
              if (touchpad.x > 0.3f || touchpad.x < -0.3f) {
                  player.transform.Rotate (0, touchpad.x * sensitivityX, 0);
              }
  
              //Debug.Log ("Touchpad X = " + touchpad.x + " : Touchpad Y = " + touchpad.y);
          }
      }
  }
