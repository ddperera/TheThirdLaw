using UnityEngine;
using System.Collections;

public class LevelSpawnMgr : MonoBehaviour {

	public GameObject[] levelRoots;
	[HideInInspector]
	public GameObject levelRoot;

	// Use this for initialization
	void Start () {
		levelRoot = levelRoots[Random.Range(0,levelRoots.Length)];
		Instantiate(levelRoot);
	}
	

}
