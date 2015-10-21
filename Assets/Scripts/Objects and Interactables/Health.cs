using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float respawnTime, invulnerableTime;
	public ParticleSystem preDeathEmitter;
	public ParticleSystem deathEmitter;
	public AudioClip deathClip;

	[HideInInspector]
	public bool isDead = false;
	[HideInInspector]
	public bool isInvulnerable = false;

	public void OnImpact(){
		if (!isInvulnerable && !isDead){
			StartCoroutine(Hit());
		}
	}

	public void AnimatePreDeath(){
		StartCoroutine(ShipPreDeathColor());
		preDeathEmitter.Play();
	}

	private IEnumerator ShipPreDeathColor(){
		SpriteRenderer r = GetComponentInChildren<SpriteRenderer>();
		Color initialColor = r.color;
		Color c = initialColor;
		c.a = .25f;
		for (float t=0f; t<=1f; t+=Time.unscaledDeltaTime){
			r.color = Color.Lerp(initialColor, c, t);
			yield return null;
		}
		r.color = initialColor;
	}

	private IEnumerator Hit(){
		isDead = true;
		isInvulnerable = true;
		deathEmitter.Play(true);
		GetComponent<DirectionForceMove>().thrustEmitter.enableEmission = false;
		GetComponent<DirectionForceMove>().thrustEmitter.GetComponent<TrailRenderer>().enabled = false;
		AudioSource.PlayClipAtPoint(deathClip, transform.position);
		foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()){
			Color initialColor = sr.color;
			Color c = initialColor;
			c.a = .25f;
			sr.color = c;
		}
		foreach (CircleCollider2D coll in GetComponentsInChildren<CircleCollider2D>()){
			coll.enabled = false;
		}
		yield return new WaitForSeconds(respawnTime);
		StartCoroutine(Respawn());

	}

	private IEnumerator Respawn(){
		isDead = false;
		GetComponent<DirectionForceMove>().thrustEmitter.enableEmission = true;
		GetComponent<DirectionForceMove>().thrustEmitter.GetComponent<TrailRenderer>().enabled = true;
		foreach (CircleCollider2D coll in GetComponentsInChildren<CircleCollider2D>(true)){
			coll.enabled = true;
		}
		foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()){
			Color initialColor = sr.color;
			Color c = initialColor;
			c.a = 1f;
			sr.color = c;
		}
		for (int i=0; i<6; i++){
			foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>(true)){
				sr.enabled = true;
			}
			yield return new WaitForSeconds(invulnerableTime/12f);
			foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()){
				sr.enabled = false;
			}
			if (i!= 5){
				yield return new WaitForSeconds(invulnerableTime/12f);
			}
		}
		foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>(true)){
			sr.enabled = true;
		}
		isInvulnerable = false;
	}
}
