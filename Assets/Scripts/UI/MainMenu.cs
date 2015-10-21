using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	//TODO: actually write this
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i<4; i++){
			if (Input.GetButtonDown("Player"+i+"-Fire1")){
				Application.LoadLevel(1);
				return;
			}
		}
	}
}
