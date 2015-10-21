using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public class MusicMgr : MonoBehaviour {

	public AudioSource intro, introLoop, layer1, layer2;
	public float maxMusicVolume;

	private int[] colorTimings;	//array of sample locations to pulse color on
	public Color[] pulseColors = {Color.red, new Color(.5f,.5f,.8f), Color.green, Color.magenta, Color.cyan, PersistantData.Orange, new Color(1f,.75f, .75f), Color.yellow};
	[HideInInspector]
	public int pulseColorIndex = 0;

	private int lastLevel = 0;

	private Bloom bloom;

	void Awake(){
		StartCoroutine(MenusMusic());
		SetUpColorTimingsList();
		StartCoroutine(PulseColorToMusic());
	}

	void OnLevelWasLoaded(int levelIndex){
		//levelIndex: 0 => main title; 1 => ship selection; 2 => gameplay; 3 => results
		switch(levelIndex){
		case 0:
			if (lastLevel == 3){
				Destroy(intro.gameObject);
				Destroy(introLoop.gameObject);
				Destroy(layer1.gameObject);
				Destroy(layer2.gameObject);
				Destroy(gameObject);
			}
			lastLevel = 0;
			break;
		case 1:
			//just continue from main menu music
			lastLevel = 1;
			break;
		case 2:
			StartCoroutine(GameplayMusic());
			bloom = GameObject.FindWithTag("GameMgr").GetComponent("Bloom") as Bloom;
			lastLevel = 2;
			break;
		case 3:
			StartCoroutine(ResultsMusic());
			lastLevel = 3;
			break;
		}
	}

	private void SetUpColorTimingsList(){
		int samplesPerBeat = (int) (layer1.clip.frequency/(154/60f));
		//gameplay music loop is 124 seconds long @ 154 bpm; 2.5666... beats per second => 320 beats? (guess)
		//4 beats per measure => 80 measures * 2 changes per measure => 160 plays per loop
		colorTimings = new int[160];
		for (int i=0; i<80; i++){
			colorTimings[2*i] = (int) ((i * 4/*beats per measure*/ + 2 /*3rd beat*/)*samplesPerBeat);
			colorTimings[(2*i)+1] = (int) ((i * 4/*beats per measure*/ + 3 /*4th beat*/)*samplesPerBeat);
		}
	}

	private IEnumerator MenusMusic(){
		intro.volume = maxMusicVolume;
		introLoop.volume = 0f;
		layer1.volume = 0f;
		layer2.volume = 0f;
		yield return new WaitForSeconds(17f);
		for (float t=0; t<1f; t+=Time.unscaledDeltaTime){
			introLoop.volume = t*maxMusicVolume;
			intro.volume = maxMusicVolume*(1f-t);
			yield return null;
		}
		introLoop.volume = maxMusicVolume;
		intro.volume = 0f;
	}

	private IEnumerator GameplayMusic(){
		intro.volume = 0f;
		introLoop.volume = 0f;
		for (float t=0f; t<1f; t+=Time.unscaledDeltaTime/5f){
			layer1.volume = t*maxMusicVolume;
			layer2.volume = t*maxMusicVolume;
			yield return null;
		}
		layer1.volume = maxMusicVolume;
		layer2.volume = maxMusicVolume;
	}

	private IEnumerator ResultsMusic(){
		for (float t=0f; t<1f; t+=Time.unscaledDeltaTime){
			layer2.volume = maxMusicVolume*(1f-t);
			yield return null;
		}
		layer2.volume = 0f;
	}

	private IEnumerator PulseColorToMusic(){
		int playNumber = 0;
		while (true){
			while (layer1.timeSamples < colorTimings[playNumber]){
				yield return null;
			}
			playNumber ++;
			playNumber %= colorTimings.Length;
			pulseColorIndex++;
			pulseColorIndex %= pulseColors.Length;
			if (bloom != null){
				StartCoroutine(BloomPulse());
			}
		}
	}

	private IEnumerator BloomPulse(){
		float originalIntensity = bloom.bloomIntensity;
		for (float t=0; t<1f; t+=Time.deltaTime/.25f){
			bloom.bloomIntensity = Mathf.Lerp(originalIntensity, originalIntensity*2f, t);
			yield return null;
		}
		for (float t=0; t<1f; t+=Time.deltaTime/.25f){
			bloom.bloomIntensity = Mathf.Lerp(originalIntensity, originalIntensity*2f, 1-t);
			yield return null;
		}
		bloom.bloomIntensity = originalIntensity;
	}
}
