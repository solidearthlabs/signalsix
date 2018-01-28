using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour {

    public static int score = 0;
    public static float timer;
    public Text scoreText;
    public Text timeText;
    public GameObject UICanvas;

	// Use this for initialization
	void Start () {
        scoreText.text = "Score: " + score;
        timeText.text = "Time: " + Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        timer = Time.time;
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        scoreText.GetComponent<Text>().text = "Score: " + score;
        timeText.GetComponent<Text>().text = "Time: " + niceTime;
	}
}
