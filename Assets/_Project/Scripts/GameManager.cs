using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public GameObject player;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

	
	void Start ()
    {
        GetComponent<levelGenerator>().Initialize();
	}

    public void NextFloor()
    {
        StartCoroutine(floorTransition());
    }
    IEnumerator floorTransition()
    {
        VRFadeToBlack.Darken();
        yield return new WaitForSeconds(1f);
        player.transform.position = new Vector3(0, 40 * GetComponent<levelGenerator>().currentFloor, 0);
        VRFadeToBlack.Lighten();
        yield return null;
    }
	
}
