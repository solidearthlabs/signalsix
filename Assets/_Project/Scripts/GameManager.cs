using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        StartCoroutine(BuildNavMesh());
        StartCoroutine(LoadGolem(0));
	}
    IEnumerator LoadGolem(int i)
    {
        yield return new WaitForSeconds(5f);
        GameObject g = Instantiate(Resources.Load<GameObject>("Golem"));
        g.transform.position = new Vector3(0,40*i + 10,0);

    }
    IEnumerator BuildNavMesh()
    {
        yield return new WaitForSeconds(3f);
        GameObject.Find("NavMeshPlane").GetComponent<NavMeshSurface>().BuildNavMesh();
        yield return null;
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
