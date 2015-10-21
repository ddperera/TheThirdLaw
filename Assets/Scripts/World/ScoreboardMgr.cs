using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScoreboardMgr : MonoBehaviour {

	public int winningPointValue = -1;	//set to -1 to have no victory by point value
	public Text[] scoresTexts; 

	private Dictionary<int, int> scoresDict = new Dictionary<int, int>();
	private List<int> keyBuf = new List<int>();

	private GameMgr gameMgr;

	void Start(){
		gameMgr = GameObject.FindWithTag("GameMgr").GetComponent<GameMgr>();
		foreach (Text t in scoresTexts){
			t.gameObject.SetActive(false);
		}
		winningPointValue = 5 + 5*(scoresTexts.Length-2);
	}

	public void Initialize(int playerNum1, int playerNum2, int playerNum3 = -1, int playerNum4 = -1){
		if (playerNum1 >= 0){
			scoresDict.Add(playerNum1, 0);
			keyBuf.Add(playerNum1);
			scoresTexts[0].gameObject.SetActive(true);
			scoresTexts[0].color = PersistantData.colors[playerNum1];
		}

		if (playerNum2 >= 0){
			scoresDict.Add(playerNum2, 0);
			keyBuf.Add(playerNum2);
			scoresTexts[1].gameObject.SetActive(true);
			scoresTexts[1].color = PersistantData.colors[playerNum2];
		}

		if (playerNum3 >= 0){
			scoresDict.Add(playerNum3, 0);
			keyBuf.Add(playerNum3);
			scoresTexts[2].gameObject.SetActive(true);
			scoresTexts[2].color = PersistantData.colors[playerNum3];
		}

		if (playerNum4 >= 0){
			scoresDict.Add(playerNum4, 0);
			keyBuf.Add(playerNum4);
			scoresTexts[3].gameObject.SetActive(true);
			scoresTexts[3].color = PersistantData.colors[playerNum4];
		}

		foreach (int key in keyBuf){
			scoresDict[key] = 0;
		}
	}

	void Update(){
		for (int i=0; i<4; i++){
			if (PersistantData.indexToPlayer[i] == -1){
				continue;
			}
			Text txt = scoresTexts[i];
			txt.text = ""+scoresDict[PersistantData.indexToPlayer[i]];
		}
	}

	public void AddPoint(int playerNumber){
		scoresDict[playerNumber]++;
		if (winningPointValue > 0 && scoresDict[playerNumber] >= winningPointValue){
			gameMgr.EndGame();
		}
	}

	public Dictionary<int, int> GetScores(){
		return scoresDict;
	}
}
