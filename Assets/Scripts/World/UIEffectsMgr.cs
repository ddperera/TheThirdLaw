using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIEffectsMgr : MonoBehaviour {

	public GameObject plusOneText;
	public AudioClip slowTimeHitClip;

	private bool isSlowing = false;
	private bool isBeingTagged = false;

	public void PlayImpactEffects(Health health, GameObject proj, ScoreboardMgr scoreboard, int playerOwner){
		Destroy(proj);
		if (!isBeingTagged){
			health.AnimatePreDeath();
			StartCoroutine(SlowTime(health, proj));
			StartCoroutine(PlusOnePointEffect(health.gameObject, scoreboard, playerOwner));
		}
	}

	private IEnumerator SlowTime(Health health, GameObject proj){
		isBeingTagged = true;
		Rigidbody2D rb = health.gameObject.GetComponent<Rigidbody2D>();
		if (!isSlowing){
			AudioSource.PlayClipAtPoint(slowTimeHitClip, proj.transform.position, 1f);
			isSlowing = true;
			for (float t=0; t<=1f; t+=Time.unscaledDeltaTime){
				rb.velocity = Vector3.zero;
				Time.timeScale = Mathf.Lerp(1f, .2f, t);
				yield return null;
			}
			Time.timeScale = 1f;
			isSlowing = false;
		} else {
			yield return new WaitForSeconds(1f);
		}
		health.OnImpact();
		Camera.main.GetComponent<ObjectShake>().StartShake();
		isSlowing = false;
		isBeingTagged = false;
	}

	private IEnumerator PlusOnePointEffect(GameObject hitPlayer, ScoreboardMgr scoreboard, int playerOwner){
		yield return new WaitForSeconds(.75f);
		GameObject textObj = (GameObject) Instantiate(plusOneText, hitPlayer.transform.position+Vector3.up*1f, Quaternion.LookRotation(Vector3.forward, Vector3.up));
		textObj.GetComponent<SpriteRenderer>().color = PersistantData.colors[playerOwner];
		Vector3 initialLoc = textObj.transform.position + Vector3.back*5f;
		Vector3 targetLoc = initialLoc + Vector3.up*2f + Vector3.back*5f;
		for (float t=0; t<=1f; t+=Time.unscaledDeltaTime){
			textObj.transform.position = Vector3.Lerp(initialLoc, targetLoc, t);
			yield return null;
		}
		Destroy(textObj);
		scoreboard.AddPoint(playerOwner);
	}

}
