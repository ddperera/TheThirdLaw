using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	private Vector3 vel;
	private Transform pictureTrans;
	private float myRollAngle;

	void FixedUpdate()
	{
		transform.Translate(Time.fixedDeltaTime * vel, Space.World);
		pictureTrans.Rotate(Vector3.forward, myRollAngle);
	}

	public void InitializeMotion(Vector2 motion)
	{
		vel = new Vector3(motion.x, motion.y);
		pictureTrans = GetComponentInChildren<SpriteRenderer>().transform;
		pictureTrans.Rotate(Vector3.forward, Random.Range(0f, 360f));

		myRollAngle = Random.Range (0,2) == 0 ? Random.Range(-1f, -.5f) : Random.Range (.5f, 1f);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag("Player"))
		{
			coll.GetComponent<Health>().OnImpact();
		}
	}

}
