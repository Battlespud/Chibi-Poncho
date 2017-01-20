using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public int lives = 3; //how many lives do we get

	bool isInitialized = false; //is our character properly setup?

	public const string characterName = "Dongcho"; //characters name, make it cute
	public string playerName; //Player's name for savegame or whatever

	const float JumpForce = 200;

	public SpriteRenderer player_sr; //our sprite, useed to flip and stuff
	public Rigidbody player_rb;
	public GameObject player_go;
	public GameObject player_feet;


	//Sprites
	public Sprite standing;
	public Sprite moving;
	public Sprite movingBack;

	Collider feet_col;

	float speed = 18;
	float speedLimit = 1;
	public bool grounded = false;

	// Use this for initialization
	void Start () {
		getSR (); //same fam, same
		Physics.IgnoreLayerCollision (9, 10); //ignore collisions between feet and body
		isInitialized = true;
	}


	void getSR(){
		player_sr = this.gameObject.GetComponent<SpriteRenderer> ();
		player_rb = this.gameObject.GetComponent<Rigidbody> ();
		player_go = this.gameObject;
		player_feet = player_go.transform.Find ("feets").gameObject;
		feet_col = player_feet.GetComponent<Collider> ();
	}

	void FixedUpdate() {
		Clamp ();
	}



	void Clamp(){
		if (Mathf.Abs(player_rb.velocity.x) > speedLimit) {
			if (player_rb.velocity.x > 0) {
				player_rb.AddForce (new Vector3 (5 * (player_rb.velocity.x / (-1 * player_rb.velocity.x)) * (Mathf.Abs (player_rb.velocity.x) - speedLimit), 0, 0));
			} else {
				player_rb.AddForce (new Vector3 (-5* (player_rb.velocity.x / (-1 *player_rb.velocity.x)) * (Mathf.Abs(player_rb.velocity.x) - speedLimit), 0, 0));
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftArrow)) {
			Move (Vector2.left);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			Move (Vector2.right);
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			Jump();
		}
		CheckSprite ();
	}

	void CheckSprite(){
		//first check if moving or not
		bool isMoving = false;
		if (player_rb.velocity.x != 0) {
			player_sr.sprite = moving;
			isMoving = true;
		} else {
			player_sr.sprite = standing;
		}
		if (isMoving) {
			if (player_rb.velocity.x < 0 && !player_sr.flipX || player_rb.velocity.x > 0 && player_sr.flipX) {
				player_sr.sprite = movingBack;
			}

		}
	}

	void Move(Vector2 dir){
		int x = (int)dir.x;
		Vector2 modifier = new Vector2(0,0);
		switch (x) {
		case(-1):
			{
				modifier = new Vector2 (-1, 0f);
				break;
			}
		case(1):
			{
				modifier = new Vector2 (1, 0f);
				break;
			}
		default:
			Debug.Log ("Movement went wrong " + dir); //moving in a nonexistent direction or not moving at all
			break;
		}
		if (grounded && !Input.GetKey(KeyCode.LeftShift)) {  //turn to face where we're moving
			if (modifier.x < 0) {
				player_sr.flipX = true;
			} else {
				player_sr.flipX = false;
			}
		}
		if (!grounded) {
			modifier *= .3f;
		}
		player_rb.AddForce (modifier*speed);
	}

	void Jump(){
		//	player_cc.Move (Vector3.up * JumpHeight);
		if (grounded) {
			player_rb.AddForce (Vector3.up * JumpForce);
		}
	}



}
