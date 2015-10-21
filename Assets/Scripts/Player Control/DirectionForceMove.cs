using UnityEngine;
using System.Collections;

public class DirectionForceMove : MonoBehaviour {

	//controller vars
	private const float TRANSLATE_DEAD_ZONE = .5f;
	private const float ROTATE_DEAD_ZONE = .25f;

	//multiplayer logic
	private PlayerManager playerManager;

	//movement vars
	public float thrustForce;
	public float maxSpeed;
	public float lerpFractionPerUpdate;

	//cache references
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

		force = new Vector2();
		inputDir = new Vector3();
	}
	
	// Update is called once per frame
	void Update () {
		if (health.isDead){
			return;
		}
		inputDir.x = playerManager.HorizontalAxis;
		inputDir.y = playerManager.VerticalAxis;

		if (inputDir.magnitude > ROTATE_DEAD_ZONE){
			inputDir = inputDir.normalized;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, inputDir), lerpFractionPerUpdate);
		}

		thrustEmitter.enableEmission = inputDir.magnitude >= TRANSLATE_DEAD_ZONE;
	}

	void FixedUpdate(){
		if (health.isDead){
			playerManager.rb.velocity = Vector3.zero;
			return;
		}

		//compute force based on player input
		force.x = playerManager.HorizontalAxis;
		force.y = playerManager.VerticalAxis;

		if(playerManager.useJoystick && force.magnitude < TRANSLATE_DEAD_ZONE)
		{
			force = new Vector2(0,0);
		}

		if (!playerManager.useJoystick){
			if (force.x != 0f && force.y != 0f){
				force.Normalize();
			}
		}


		
		force *= fireProj.canMove ? thrustForce : 0f;	//stop thrusting while charging shot

		if(playerManager.rb != null)
		{
			playerManager.rb.AddForce(force, ForceMode2D.Force);
		}
		else
		{
			playerManager.rb = GetComponent<Rigidbody2D>();
		}
		//max speed check
		if (!fireProj.isRebounding && playerManager.rb.velocity.sqrMagnitude > maxSpeed*maxSpeed){
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
