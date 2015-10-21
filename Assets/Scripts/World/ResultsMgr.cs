using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ResultsMgr : MonoBehaviour {
	
	//the view of the MVC for the character selection screen

	public GameObject continueText;
	
	public Sprite[] shipPreviewSprites;

	public GameObject[] selectImages;
	public GameObject[] placeTexts;
	public GameObject[] scoreTexts;
	public GameObject[] bgpList;

	List<KeyValuePair<int, int>> scoresList = new List<KeyValuePair<int, int>>{};
	
	private bool displayContinueText = false;

	// Use this for initialization
	void Start () {
		foreach (KeyValuePair<int, int> kvp in PersistantData.mostRecentScores) {
			scoresList.Add(kvp);
		}

		scoresList.Sort(CompareScores);

		if (shipPreviewSprites.Length != 6){
			Debug.LogError("Improper preview sprites length. Include the 6 ships in the proper order.");
		}

		int prevPlace = 0;
		int prevScore = -1;

		// For loop assumes scoresList is sorted to work correctly
		for (int i=0; i<scoresList.Count; i++){

			bgpList[i].SetActive(true);

			KeyValuePair<int, int> score = scoresList[i];
			int place = -1;

			if (score.Value == prevScore) {
				place = prevPlace;
			} else {
				place = ++prevPlace;
			}

			prevScore = score.Value;
			prevPlace = place;


			// Display place
			placeTexts[i].SetActive(true);
			placeTexts[i].GetComponent<Text>().text = place.ToString();

			// Display Ship
			int shipIndex = score.Key;
			selectImages[i].SetActive(true);
			selectImages[i].GetComponent<Image>().sprite = shipPreviewSprites[shipIndex];

			// Display score
			scoreTexts[i].SetActive(true);
			scoreTexts[i].GetComponent<Text>().text = "Score: " + score.Value.ToString();
		}

		StartCoroutine(BlinkContinueText());
	}

	void Update() {
		if ((Input.GetButtonDown ("Player0-Fire1")) || 
		(Input.GetButtonDown ("Player1-Fire1")) || 
		(Input.GetButtonDown ("Player2-Fire1")) ||
		(Input.GetButtonDown ("Player3-Fire1"))) {
			Application.LoadLevel(0);
		}
	}

	void OnGUI() {
		if (displayContinueText) {
			continueText.SetActive(true);
		} else {
			continueText.SetActive(false);
		}
	}

	private IEnumerator BlinkContinueText() {
		while(true) {
			displayContinueText = true;
			yield return new WaitForSeconds(1f);
			displayContinueText = false;
			yield return new WaitForSeconds(1f);
		}
	}

	// Used to sort scores in decreasing order
	private static int CompareScores(KeyValuePair<int, int> score1, KeyValuePair<int, int> score2) {
		return score2.Value - score1.Value;
	}
	
	public void Join(int joiningIndex, int startCharIndex){
		placeTexts[joiningIndex].SetActive(false);
		selectImages[joiningIndex].GetComponent<Image>().sprite = shipPreviewSprites[startCharIndex];
	}

	public void Quit(int quittingIndex){
		placeTexts[quittingIndex].SetActive(true);
	}
	
	public void Unlock(int index){
		scoreTexts[index].SetActive(false);
	}
	
	public void Lock(int index){
		scoreTexts[index].SetActive(true);
	}
}

