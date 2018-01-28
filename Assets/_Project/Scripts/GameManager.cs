using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public GameObject player;

    public int currentFloor = 0;
    public string deathScene = "7_360_vid";
    //public AudioSource audioSource;
    public string deathAudioClip = "death1";
    private NavMeshSurface navMeshSurface;

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
        NavMeshSurface[] navMeshSurfaces = (NavMeshSurface[])FindObjectsOfType(typeof(NavMeshSurface));
        if (navMeshSurfaces.Length != 1)
            Debug.LogError(string.Format("Expecting exactly 1 navmeshsurface in the scene (found {0} NavMeshSurface components)",navMeshSurfaces.Length));
        navMeshSurface = navMeshSurfaces[0];

        GetComponent<levelGenerator>().Initialize(0);// (Style)currentFloor);
        StartCoroutine(BuildNavMesh());
        StartCoroutine(LoadGolem(0));
        if (player == null)
            Debug.LogError("Player == null");

        //if (audioSource==null)
        //    audioSource = player.GetComponent<AudioSource>();

        //if (audioSource == null)
        //    Debug.LogWarning("No audiosource found on player!");
        if (deathAudioClip== null)
            Debug.LogWarning("Death Audio Clip isn't defined!");

    }
    IEnumerator LoadGolem(int i)
    {
        yield return new WaitForSeconds(5f);
        GameObject g = Instantiate(Resources.Load<GameObject>("Golem"));
        g.transform.position = new Vector3(3,0,0);

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

    public void Death()
    {
        StartCoroutine(TransitionScene(deathScene,true));
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(TransitionScene(scene));
    }

    IEnumerator TransitionScene(string scene,bool isDeath=false)
    {
        VRFadeToBlack.Darken();
        yield return new WaitForSeconds(.5f);
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        if (isDeath && deathAudioClip!=null)
        {
            AudioManager.Instance.PlayClip(deathAudioClip);
        }
        while (async!=null && !async.isDone)
        {
            yield return null;
        }
        VRFadeToBlack.Lighten();
        yield return null;
    }


}
