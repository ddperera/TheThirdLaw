using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectionMenu : MonoBehaviour {

	//the model and controller of the MVC for the character selection screen

	public bool useJoystick = false;

	public CharacterSelectionMgr selectionMgr;

	float timeSinceStart = 0f;
	bool starting = false;
	private int[] curSelections = {-1,-1,-1,-1};
	private bool[] inputLagLocks = {false, false, false, false};
	private bool[] inputLockedIn = {false, false, false, false};

	void Start(){
		PersistantData.playersToSpawn.Clear();
	}

	void Update(){
		if (starting) return;
		for (int i=0; i<4; i++){
			float h = useJoystick ? Input.GetAxisRaw("Player"+i+"-HorizontalJoy") : Input.GetAxisRaw("Player"+i+"-Horizontal");
			if (useJoystick && Mathf.Abs(h) < .5f){
				h = 0;
			}

			if (h!=0 && !inputLagLocks[i] && !inputLockedIn[i] && curSelections[i] != -1){
					StartCoroutine(ChangeSelection(i, h<0f));
			}
			if (Input.GetButtonDown ("Player"+i+"-Fire1")){
				if (curSelections[i] == -1){
					curSelections[i] = GetNextAvailableIndex();
					selectionMgr.Join(i, curSelections[i]);
				} else if (curSelections[i] == 6){
					curSelections[i] = -1;
					selectionMgr.Quit(i);
					inputLockedIn[i] = false;
				} else {
					if (inputLockedIn[i]){
						inputLockedIn[i] = false;
						selectionMgr.Unlock(i);
						for (int j=0; j<4; j++){
							if (curSelections[j] == curSelections[i]){
								selectionMgr.Unblock(i);
							}
						}
					} else if (!CharInUse(curSelections[i])){
						inputLockedIn[i] = true;
						selectionMgr.Lock(i);
						for (int j=0; j<4; j++){
							if (curSelections[j] == curSelections[i] && i != j){
								selectionMgr.Block(j);
							}
						}
					}
				}
			}
		}
		if (CheckForPlay() && timeSinceStart > 5f){
			StartCoroutine(BeginPlay());
		}
		timeSinceStart += Time.deltaTime;
	}

	private bool CheckForPlay(){
		int count = 0;
		for (int i=0; i<4; i++){
			if (curSelections[i] != -1 && inputLockedIn[i]){
				count += 1;
			} else if (curSelections[i] != -1 && !inputLockedIn[i]){
				return false;
			}
		}
		return count >= 2;
	}

	private IEnumerator ChangeSelection(int index, bool left){
		inputLagLocks[index] = true;

		int selection = curSelections[index]; 
		selection += left ? -1 : 1;
		if (selection < 0) selection = 6;
		selection %= 7;
		bool canSelect = !CharInUse(selection);

		selectionMgr.Swipe(index, curSelections[index], selection, left, canSelect);
		curSelections[index] = selection;

		yield return new WaitForSeconds(.35f);
		inputLagLocks[index] = false;
	}

	private IEnumerator BeginPlay(){
		starting = true;
		//set up logic for what players to spawn in gameplay screen
		foreach (int ship in curSelections){
			PersistantData.playersToSpawn.Add(ship);
		}
		//fade out and load
		Image blackScreen = GameObject.FindWithTag("BlackScreen").GetComponent<Image>();
		Color c = blackScreen.color;
		for (float t=0; t<1f; t+=Time.deltaTime){
			c.a = Mathf.Lerp(0f,1f,t);
			blackScreen.color = c;
			yield return null;
		}
		//c.a = 0f;
		//blackScreen.color = c;
		Application.LoadLevel(2);
	}

	private bool CharInUse(int c){
		for (int i=0; i<4; i++){
			if (curSelections[i] == c && inputLockedIn[i]){
				return true;
			}
		}
		return false;
	}

	private int GetNextAvailableIndex(){
		for (int i=0; i<6; i++){
			if (curSelections[0] != i && curSelections[1] != i && curSelections[2] != i && curSelections[3] != i){
				return i;
			}
		}
		//should never reach here
		return -1;
	}
}
