using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVideo : MonoBehaviour {

    public MovieTexture _movie;

	// Use this for initialization
	void Start () {
        _movie.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
