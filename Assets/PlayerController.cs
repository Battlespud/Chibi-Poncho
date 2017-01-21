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

	float speed = 18; //18 is a good number here
	float speedLimit = 1;  //1 is a good number here
	public bool grounded = false;

    //Flags for player control
    private bool moveLeft, moveRight, jump;

	// Use this for initialization
	void Start () {
		getComponents (); //same fam, same
		Physics.IgnoreLayerCollision (9, 10); //ignore collisions between feet and body
		isInitialized = true;
	}


	void getComponents(){
		player_sr = this.gameObject.GetComponent<SpriteRenderer> ();
		player_rb = this.gameObject.GetComponent<Rigidbody> ();
		player_go = this.gameObject;
		player_feet = player_go.transform.Find ("feets").gameObject;
		feet_col = player_feet.GetComponent<Collider> ();
	}

	void FixedUpdate() {
        if (jump)
            Jump();

        if (moveLeft)
            Move(Vector2.left);
        else if (moveRight)
            Move(Vector2.right);

		Clamp ();
        resetAllInputFlags();
	}



	void Clamp(){   //prevent rigidbody from accelerating uncontrollably without dealing with the fuckery of drag
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
		if (Input.GetAxisRaw("Horizontal") < 0.0f)
            moveLeft = true;
		if (Input.GetAxisRaw("Horizontal") > 0.0f)
            moveRight = true;
		if (Input.GetButtonDown("Jump"))
            jump = true;

		CheckSprite ();
	}

    private void resetAllInputFlags()
    {
        moveLeft = false;
        moveRight = false;
        jump = false;
    }

	//Update the sprite based on how we're moving.  Basically manual animating. Maybe we wont need this tbh
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
		if (grounded && !Input.GetKey(KeyCode.LeftShift)) {  //turn to face where we're moving, use shift to not turn.
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
