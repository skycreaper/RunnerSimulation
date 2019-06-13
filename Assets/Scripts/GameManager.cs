using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{

	public GameObject playerPrefab;
	public Text continueText;
	public Text scoreText;

	// Para el texto
	private float blinkTime = 0;
	private bool blink;

	// Para los tiempos
	private float timeElapsed = 0f;
	private float bestTime = 0f;

	// Paara reiniciar el juego
	private bool gameStarted;
	private TimeManager timeManager;
	private GameObject player;
	private GameObject floor;
	private Spawner spawner;

	private bool beatBestTime;

	void Awake() {
		floor = GameObject.Find("Foreground");
		spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
		timeManager = GetComponent<TimeManager>();
	}

    // Start is called before the first frame update
    void Start()
    {
        var floorHeight = floor.transform.localScale.y;

        var pos = floor.transform.position;

        pos.x = 0;
        pos.y = -((Screen.height / PixelPerfectCamera.pixelsToUnits) / 2) + (floorHeight / 2);
        floor.transform.position = pos;

        spawner.active = false;

        Time.timeScale = 0;

        continueText.text = "Presiona una tecla para comenzar!";
        bestTime = PlayerPrefs.GetFloat("BestTime");
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameStarted && Time.timeScale == 0) {
        	if(Input.anyKeyDown) {
        		timeManager.ManipulateTime(1, 1f);
        		ResetGame();
        	}
        }

        if(!gameStarted) {
        	blinkTime++;

        	if(blinkTime % 40 == 0) {
        		blink = !blink;
        	}

        	continueText.canvasRenderer.SetAlpha(blink ? 0:1);

        	var textColor = beatBestTime ? "#FF0" : "#FFF";

        	scoreText.text = "TIME: "+FormatTime(timeElapsed) + "\n"+
        		"<color="+textColor+">BEST: "+FormatTime(bestTime)+"</color>";
        } else {
        	timeElapsed += Time.deltaTime;
        	scoreText.text = "TIME: "+FormatTime(timeElapsed);
        }
    }

    void OnPlayerKilled() {
    	spawner.active = false;

    	var playerDestroyScript = player.GetComponent<DestroyOffScreen>();
    	playerDestroyScript.DestroyCallback -= OnPlayerKilled;

    	player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    	timeManager.ManipulateTime(0, 5.5f);

    	gameStarted = false;

    	continueText.text = "Presiona cualquier botón para volver a comenzar";

    	if(timeElapsed > bestTime) {
    		bestTime = timeElapsed;
    		PlayerPrefs.SetFloat("BestTime", bestTime);
    		beatBestTime = true;
    	}
    }

    void ResetGame() {
    	spawner.active = true;

    	player = GameObjectUtily.Instantiate(playerPrefab, new Vector3(0, (Screen.height/PixelPerfectCamera.pixelsToUnits)/2 + 100, 0));

    	var playerDestroyScript = player.GetComponent<DestroyOffScreen>();
    	playerDestroyScript.DestroyCallback += OnPlayerKilled;

    	gameStarted = true;

    	continueText.canvasRenderer.SetAlpha(0);

    	timeElapsed = 0;

    	beatBestTime = false;
    }


    string FormatTime(float value) {
    	TimeSpan t = TimeSpan.FromSeconds(value);

    	return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }
}
