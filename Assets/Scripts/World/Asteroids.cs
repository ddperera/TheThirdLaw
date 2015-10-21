using UnityEngine;
using System.Collections;

public class Asteroids : MonoBehaviour {

	// time in between asteroids spawning
	public GameObject Asteroid;
	public float timeInBetween;
	public float speedUpFactor;
	public float asteroidSpeed;
	private Vector2 passThrough;
	private Vector2 forceDirection;
	private Vector2 normForce;

	public void StartSpawningAsteroids(){
		StartCoroutine(Spawn(timeInBetween));
	}
	
	private IEnumerator Spawn(float nextDelay){
		while (nextDelay >= .1f){
			var randomInnerX = Random.Range(-10f,10f);
			var randomInnerY = Random.Range(-7.5f,7.5f);
			var randomOutterPoint = Random.insideUnitCircle.normalized*30f;
			forceDirection = new Vector2((randomInnerX-randomOutterPoint.x),(randomInnerY-randomOutterPoint.y));
			normForce = forceDirection.normalized;
			GameObject ast = (GameObject) Instantiate (Asteroid, new Vector3(randomOutterPoint.x, randomOutterPoint.y), 
			                                           Quaternion.identity);
			ast.GetComponent<Asteroid>().InitializeMotion(normForce*asteroidSpeed*Random.Range(.9f, 1.6f));
			Destroy(ast, 15f);

			nextDelay *= speedUpFactor;
			yield return new WaitForSeconds(nextDelay);
		}
	}
}
