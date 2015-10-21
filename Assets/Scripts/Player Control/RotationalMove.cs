using UnityEngine;
using System.Collections;

public class RotationalMove : MonoBehaviour {

	//multiplayer logic
	private PlayerManager playerManager;

	//movement vars
	public float thrustForce;
	public float maxSpeed;
	public float turnRate;

	//cache refs
	public ParticleSystem thrustEmitter;
	private Health health;
	private FireProjectile fireProj;


	//cache vars
	private Vector2 force;
	private Vector3 inputDir;
	
	
	// Use this for initialization
	void Start () {
		playerManager = GetComponent<PlayerManager>();
		health = GetComponent<Health>();
		fireProj = GetComponent<FireProjectile>();

		inputDir = new Vector3();
	}
	
	// Update is called once per frame
	void Update () {
		if (health.isDead){
			return;
		}

		inputDir.x = playerManager.HorizontalAxis;
		inputDir.y = playerManager.VerticalAxis;
		transform.Rotate(0,0,-inputDir.x*turnRate*Time.deltaTime);

		thrustEmitter.enableEmission = inputDir.sqrMagnitude > .1f;
		thrustEmitter.GetComponent<TrailRenderer>().enabled = !health.isDead;
		
	}

	void FixedUpdate(){
		if (health.isDead){
			playerManager.rb.velocity = Vector3.zero;
			return;
		}
		
		//compute force based on player input
		Vector3 force3d = transform.TransformDirection(new Vector3(0f, playerManager.VerticalAxis, 0f));
		force.x = force3d.x;
		force.y = force3d.y;

		force *= fireProj.canMove ? thrustForce : 0f;	//stop thrusting while charging shot
		
		playerManager.rb.AddForce(force, ForceMode2D.Force);
		//max speed check
		if (playerManager.rb.velocity.sqrMagnitude > maxSpeed*maxSpeed){
			Vector2 temp = new Vector2(playerManager.rb.velocity.x, playerManager.rb.velocity.y);
			playerManager.rb.velocity = temp.normalized*maxSpeed;
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (!this.enabled) return;

		float bounciness = .75f;
		BounceOff b = coll.collider.GetComponent<BounceOff>();
		if (b != null){
			bounciness = b.bounciness;
		}
		
		Vector2 normal = coll.contacts[0].normal;
		playerManager.rb.AddForce(normal * coll.relativeVelocity.magnitude * bounciness, ForceMode2D.Impulse);
	}
}
