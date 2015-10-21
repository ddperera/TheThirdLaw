using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeMgr : MonoBehaviour {

	public int secondsUntilEnd = -1;	//set to -1 to have no end by timer
	public Text timerText;

	private float curTimer;
	private bool gameEnded = false;
	private bool isRaining = false;
	private GameMgr gameMgr;

	// Use this for initialization
	void Start () {
		gameMgr = GameObject.FindWithTag("GameMgr").GetComponent<GameMgr>();
		curTimer = secondsUntilEnd;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameEnded || secondsUntilEnd < 0) return;
		curTimer -= Time.unscaledDeltaTime;
		timerText.text = "TIME: "+(int)curTimer;
		if(curTimer < 31 && curTimer > 30)
		{
			if(!isRaining)
			{
				StartCoroutine("StartAsteroids");
			}
		}
		if (curTimer <= 10){
			timerText.color = Color.red; 
		}
		if (curTimer <= 0){
			timerText.text = "GAME FINISHED!";
			gameEnded = true;
			gameMgr.EndGame();
		}
	}

	private IEnumerator StartAsteroids()
	{
		isRaining = true;
		gameObject.GetComponent<Asteroids>().StartSpawningAsteroids();
		yield return null;
	}
}
