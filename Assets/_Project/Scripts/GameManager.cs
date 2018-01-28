using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public GameObject player;

    public int currentFloor = 0;

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
        GetComponent<levelGenerator>().Initialize(0);// (Style)currentFloor);
        StartCoroutine(BuildNavMesh());
        StartCoroutine(LoadGolem(0));
	}
    IEnumerator LoadGolem(int i)
    {
        yield return new WaitForSeconds(5f);
        GameObject g = Instantiate(Resources.Load<GameObject>("Golem"));
        g.transform.position = new Vector3(0,0,0);

    }
    IEnumerator BuildNavMesh()
    {
        yield return new WaitForSeconds(3f);
        GameObject.Find("NavMeshPlane").GetComponent<NavMeshSurface>().BuildNavMesh();
        yield return null;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.H))
            NextFloor();
    }

    public void NextFloor()
    {
        currentFloor++;
        StartCoroutine(floorTransition());
    }
    IEnumerator floorTransition()
    {
        VRFadeToBlack.Darken();
        yield return new WaitForSeconds(.5f);
        AsyncOperation async = SceneManager.LoadSceneAsync("6_conglomerate_test-VRTK");
        while (!async.isDone)
        {
            yield return null;
        }
        
        GetComponent<levelGenerator>().Initialize((Style)currentFloor);//(Style)currentFloor);
        StartCoroutine(BuildNavMesh());
        StartCoroutine(LoadGolem(0));
        VRFadeToBlack.Lighten();
        yield return null;
    }
	
}
