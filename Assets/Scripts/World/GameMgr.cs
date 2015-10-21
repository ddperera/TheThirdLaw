using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameMgr : MonoBehaviour {

	private List<GameObject> activePlayers = new List<GameObject>();

	// Use this for initialization
	void Start () {
		SpawnPlayers();
		StartCoroutine(StartGameEffects());
	}

	private void SpawnPlayers(){
		int p1 = -1; int p2 = -1; int p3 = -1; int p4 = -1;
		for (int i=0; i<PersistantData.playersToSpawn.Count; i++){
			//blech this is ugly, sorry
			if (PersistantData.playersToSpawn[i] == -1){
				continue;
			}
			switch (i){
			case 0:
				p1 = PersistantData.playersToSpawn[i];
				PersistantData.indexToPlayer[0] = p1;
				break;
			case 1:
				p2 = PersistantData.playersToSpawn[i];
				PersistantData.indexToPlayer[1] = p2;
				break;
			case 2:
				p3 = PersistantData.playersToSpawn[i];
				PersistantData.indexToPlayer[2] = p3;
				break;
			case 3:
				p4 = PersistantData.playersToSpawn[i];
				PersistantData.indexToPlayer[3] = p4;
				break;
			}

			GameObject player = (GameObject) Instantiate(Resources.Load("Player"+PersistantData.playersToSpawn[i]),
			                                             GameObject.Find("SpawnPoint"+i).transform.position, GameObject.Find("SpawnPoint"+i).transform.rotation);

			activePlayers.Add(player);
		}
		GameObject.FindWithTag("Scoreboard").GetComponent<ScoreboardMgr>().Initialize(p1, p2, p3, p4);
	}

	public void EndGame(){
		PersistantData.mostRecentScores = GameObject.FindWithTag("Scoreboard").GetComponent<ScoreboardMgr>().GetScores();
		foreach (GameObject player in activePlayers){
			player.GetComponent<DirectionForceMove>().enabled = false;
		}
		StartCoroutine(EndGameEffects());
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)){
			EndGame();
		}
	}
	
	private IEnumerator StartGameEffects(){
		Time.timeScale = 0f;
		foreach (GameObject player in activePlayers){
			player.GetComponent<DirectionForceMove>().enabled = false;
			player.GetComponent<FireProjectile>().enabled = false;
		}
		Text startTimer = GameObject.FindWithTag("StartTimerText").GetComponent<Text>();
		for (float t=5f; t>0f; t-=Time.unscaledDeltaTime){
			startTimer.text = "" + Mathf.CeilToInt(t);
			yield return null;
		}
		Time.timeScale = 1f;
		startTimer.text = "GO!";
		yield return new WaitForSeconds(.5f);
		foreach (GameObject player in activePlayers){
			player.GetComponent<DirectionForceMove>().enabled = true;
			player.GetComponent<FireProjectile>().enabled = true;
		}
		startTimer.gameObject.SetActive(false);
	}

	private IEnumerator EndGameEffects(){
		for (float t=0; t<2f; t+=Time.unscaledDeltaTime){
			Time.timeScale = Mathf.Lerp(1f, .125f, t/2f);
			yield return null;
		}
		Image screen = GameObject.FindWithTag("BlackScreen").GetComponent<Image>();
		Color c = screen.color;
		for (float t=0; t<1f; t+=Time.unscaledDeltaTime){
			c.a = Mathf.Lerp(0f, 1f, t);
			screen.color = c;
			yield return null;
		}
		Time.timeScale = 1f;
		yield return new WaitForSeconds(1f);
		Application.LoadLevel(3);	//results screen
		//c.a = 0f;
		//screen.color = c;
	}
}
