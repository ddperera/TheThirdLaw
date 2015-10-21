using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	//joystick toggle
	public bool useJoystick = false;

	//identifier
	public int playerNumber = 0;

	//indexing
	private int playerIndex = -1;

	// input abstraction
	public bool Fire1 { get { return Input.GetButtonDown("Player"+playerIndex+"-Fire1");}}
	public float HorizontalAxis { 
		get { 
			if(useJoystick)
			{
				return Input.GetAxis("Player"+playerIndex+"-HorizontalJoy");
			}
			else
			{
				return Input.GetAxis("Player"+playerIndex+"-Horizontal");
			}
		}
	}
	public float VerticalAxis { 
		get {
			if(useJoystick)
			{
				return Input.GetAxis("Player"+playerIndex+"-VerticalJoy");
			}
			else
			{
				return Input.GetAxis("Player"+playerIndex+"-Vertical");
			}
		}
	}

	//reference abstraction
	public GameObject PlayerObj {get; set;}
	public Rigidbody2D rb { get; set;}
	
	void Awake(){
		for (int i=0; i<PersistantData.indexToPlayer.Length; i++){
			if (PersistantData.indexToPlayer[i] == playerNumber){
				playerIndex = i;
				break;
			}
		}
		PlayerObj = GameObject.Find("Player"+playerNumber+"(Clone)");
		rb = PlayerObj.GetComponent<Rigidbody2D>();
	}
}
