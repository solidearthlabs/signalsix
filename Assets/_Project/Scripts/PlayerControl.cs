  using UnityEngine;
  using System.Collections;
  using System.Collections.Generic;
#if STEAMVR
using Valve.VR;
  
  public class PlayerControl : MonoBehaviour {

      public GameObject player;
      public Camera player_view;
      public Rigidbody rb;
  
      //SteamVR_Controller.Device device;
      SteamVR_TrackedObject trackObj;
	  
	    private SteamVR_Controller.Device controller {
        get {
          return SteamVR_Controller.Input((int)trackObj.index);
        }
      }
  
      Vector2 touchpad;
  
      private float sensitivityX = 1.5F;
      private Vector3 playerPos;
	  private bool isLock;
	  private Vector3 LockNormal;

      // For the player to pick up things and throw
      private GameObject collidingObject; 
      private GameObject objectInHand; 
  
      void Start() {
          trackObj = GetComponent<SteamVR_TrackedObject>();
          rb = player.GetComponent<Rigidbody>();
		  isLock = false;
      }
  
      // Update is called once per frame
      void Update(){
        if (controller == null){
            Debug.Log("Controller hasn't been initialized");
            return;
        }
		
	  if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip)){
		  if (isLock)
			  isLock = false;
		  else {
			  isLock = true;
			  
		  }
	  }
		  
          //If finger is on touchpad
        if (controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)){
            //Read the touchpad values
            touchpad = controller.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
            
            Vector3 normal = new Vector3 (player_view.transform.forward.x, 0f, player_view.transform.forward.z);
			
			float x_rot = touchpad.x;
            float y_rot = touchpad.y;
            
            // Avoid too sensitive touch detect
            if (Mathf.Abs(touchpad.x) < 0.5)
                x_rot = 0;

            if (Mathf.Abs(touchpad.y) < 0.5)
                y_rot = 0;

            if (x_rot == 0 && y_rot == 0){
                // do nothing
                // Debug.Log("stop");
            }
            else {
				if (isLock){
					// Debug.Log("is lock");
					Vector3 newDirect = LockNormal * y_rot + new Vector3(LockNormal.z, 0f, -LockNormal.x) * x_rot;
					player.transform.forward = newDirect;
					player.transform.position += (player.transform.forward / 15f);
				}
				if (!isLock){
					LockNormal = normal;
					Vector3 newDirect = LockNormal * y_rot + new Vector3(LockNormal.z, 0f, -LockNormal.x) * x_rot;
					player.transform.forward = newDirect;
					player.transform.position += (player.transform.forward / 15f);
				}
            }
				
          /*
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
              */
          }
      }

      void OnCollisionStay(Collision others){
        collidingObject = others.gameObject;

        if (others.gameObject.tag == "pick" && controller.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
          objectInHand = collidingObject;
          collidingObject = null;

          objectInHand.transform.position = this.transform.position;
          objectInHand.transform.rotation = this.transform.rotation;
        } 

        if (others.gameObject.tag == "pick" && controller.GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) {
          objectInHand.GetComponent<Rigidbody> ().AddForce (transform.forward*2f, ForceMode.Impulse);
        }
      }

  }
#endif