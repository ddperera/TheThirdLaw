using UnityEngine;
using System.Collections;

public class ObjectShake : MonoBehaviour {
	
	private Vector3 startingPoint;
	private bool isShaking = false;
	public float shakeTime = .05f;
	public float shakeAmount = 1f;
	public int totalPoints = 3;

	// Use this for initialization
	void Start () {
		startingPoint = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.P)){
			StartShake();
		}
	}

	public void StartShake()
	{
		if(!isShaking)
		{
			StartCoroutine("Shake");
		}
	}

	private IEnumerator Shake()
	{
		isShaking = true;
		Vector3[] shakePoints = ShakePoints ();
		Vector3 lerpVec = new Vector3();
		for(int pointNum = 0; pointNum<totalPoints; pointNum++)
		{
			for(float t = 0f; t<1f; t+=Time.deltaTime/shakeTime)
			{
				lerpVec = Vector3.Lerp(startingPoint, shakePoints[pointNum], t);
				gameObject.transform.position = lerpVec;
				yield return null;
			}
			for(float t = 0f; t<1f; t+=Time.deltaTime/shakeTime)
			{
				lerpVec = Vector3.Lerp(shakePoints[pointNum], startingPoint, t);
				gameObject.transform.position = lerpVec;
				yield return null;
			}
			gameObject.transform.position = startingPoint;
		}
		isShaking = false;
	}

	private Vector3[] ShakePoints()
	{
		float[] randomAngles = new float[totalPoints];
		for(int i=0; i<totalPoints; i++)
		{
			randomAngles[i] = Random.Range(0, 2*Mathf.PI);
		}
		Vector3[] points = new Vector3[totalPoints];
		for(int i=0; i<totalPoints; i++)
		{
			points[i] = new Vector3(Mathf.Cos(randomAngles[i]), Mathf.Sin (randomAngles[i]), startingPoint.z);
			points[i] *= shakeAmount / (Mathf.Pow(2,i));
		}

		return points;
	}
}
