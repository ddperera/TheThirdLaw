using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelColorChanger : MonoBehaviour {

	private MusicMgr music;
	public Material wallMat;

	// Use this for initialization
	void Start () {
		music = GameObject.FindWithTag("MusicMgr").GetComponent<MusicMgr>();
	}
	
	// Update is called once per frame
	void Update () {
		wallMat.color = music.pulseColors[music.pulseColorIndex];
	}
}
