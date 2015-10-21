using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSelectionMgr : MonoBehaviour {

	//the view of the MVC for the character selection screen

	public Sprite[] shipPreviewSprites;

	public GameObject[] selectArrows;
	public GameObject[] selectImages;
	public GameObject[] joinTexts;
	public GameObject[] readyTexts;

	// Use this for initialization
	void Start () {
		if (shipPreviewSprites.Length != 7){
			Debug.LogError("Improper preview sprites length. Include the 6 ships in the proper order and then a 'quit' sprite.");
		}
		for (int i=0; i<4; i++){
			selectArrows[i].SetActive(false);
			selectImages[i].SetActive(false);
			joinTexts[i].SetActive(true);
			readyTexts[i].SetActive(false);
		}
	}

	public void Join(int joiningIndex, int startCharIndex){
		joinTexts[joiningIndex].SetActive(false);
		selectImages[joiningIndex].SetActive(true);
		selectImages[joiningIndex].GetComponent<Image>().sprite = shipPreviewSprites[startCharIndex];
		selectArrows[joiningIndex].SetActive(true);
	}

	public void Quit(int quittingIndex){
		joinTexts[quittingIndex].SetActive(true);
		selectImages[quittingIndex].SetActive(false);
		selectArrows[quittingIndex].SetActive(false);
	}

	public void Unlock(int index){
		selectArrows[index].SetActive(true);
		readyTexts[index].SetActive(false);
	}

	public void Lock(int index){
		selectArrows[index].SetActive(false);
		readyTexts[index].SetActive(true);
	}

	public void Unblock(int index){
		selectImages[index].GetComponent<Image>().color = Color.white;
	}

	public void Block(int index){
		selectImages[index].GetComponent<Image>().color = new Color(.5f,.5f,.5f,.4f);
	}

	public void Swipe(int index, int oldCharIndex, int newCharIndex, bool swipeLeft, bool canSelect){
		selectImages[index].GetComponent<Image>().sprite = shipPreviewSprites[newCharIndex];
		if (canSelect){
			Unblock(index);
		} else {
			Block(index);
		}
	}
}
