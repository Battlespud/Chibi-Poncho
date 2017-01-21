using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public int lives = 3; //how many lives do we get

	public const string characterName = "Dongcho"; //characters name, make it cute
	public string playerName; //Player's name for savegame or whatever

    private SpriteRenderer player_sr; //our sprite, useed to flip and stuff
    private Rigidbody player_rb;
    private GameObject player_go;

    //Sprites
    //TODO: Won't need these once an animator is used
    [Header("Sprites")]
    public Sprite standing;
	public Sprite moving;
	public Sprite movingBack;

	public GameObject face;
	public SpriteRenderer face_sr;


	private Collider feet_col;

	private const float speed = 18.0f; //18 is a good number here
	private const float speedLimit = 1.0f;  //1 is a good number here
    private const float jumpForce = 200.0f;
    private const float groundCheckDistance = 0.3f, groundCheckHalfRange = 0.125f;
    public bool grounded = false;

	// Use this for initialization
	void Start () {
		getComponents (); //same fam, same
		Physics.IgnoreLayerCollision (9, 10); //ignore collisions between feet and body 
        //TODO: consider ignoring phys layers in the Unity editor physics settings to avoid having to use numbers in scripts
	}

	void getComponents(){
		player_sr = this.gameObject.GetComponent<SpriteRenderer> ();
		player_rb = this.gameObject.GetComponent<Rigidbody> ();
		player_go = this.gameObject;
		face_sr = face.GetComponent<SpriteRenderer> ();
	}

	void FixedUpdate() {
		ClampVelocity();
        CheckGrounded();
	}

	private void ClampVelocity(){   //prevent rigidbody from accelerating uncontrollably without dealing with the fuckery of drag
		if (Mathf.Abs(player_rb.velocity.x) > speedLimit) {
			if (player_rb.velocity.x > 0) {
				player_rb.AddForce (new Vector3 (5 * (player_rb.velocity.x / (-1 * player_rb.velocity.x)) * (Mathf.Abs (player_rb.velocity.x) - speedLimit), 0, 0));
			} else {
				player_rb.AddForce (new Vector3 (-5* (player_rb.velocity.x / (-1 *player_rb.velocity.x)) * (Mathf.Abs(player_rb.velocity.x) - speedLimit), 0, 0));
			}
		}
	}

    private void CheckGrounded() //Perform a raycast downwards and see if it hits a foothold
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * 0.3f), Color.blue);
        Debug.DrawLine(transform.position + (Vector3.right * groundCheckHalfRange), transform.position + (Vector3.right * groundCheckHalfRange) + (Vector3.down * 0.3f), Color.blue);
        Debug.DrawLine(transform.position - (Vector3.right * groundCheckHalfRange), transform.position - (Vector3.right * groundCheckHalfRange) + (Vector3.down * 0.3f), Color.blue);

        RaycastHit rayHit1, rayHit2, rayHit3;
        Physics.Raycast(transform.position, Vector2.down, out rayHit1);
        Physics.Raycast(transform.position + (Vector3.right * groundCheckHalfRange), Vector2.down, out rayHit2);
        Physics.Raycast(transform.position - (Vector3.right * groundCheckHalfRange), Vector2.down, out rayHit3);

        if ((rayHit1.collider && rayHit1.distance < 0.3f && rayHit1.collider.CompareTag("Terrain"))
            || (rayHit2.collider && rayHit2.distance < 0.3f && rayHit2.collider.CompareTag("Terrain"))
            || (rayHit3.collider && rayHit3.distance < 0.3f && rayHit3.collider.CompareTag("Terrain")))
        {
            grounded = true;
            print("Distance from ground: Grounded");
        }     
        else
        {
            grounded = false;
            print("Distance from ground: " + (rayHit1.distance - 0.3f));
        }
            

        
    }

	// Update is called once per frame
	void Update () {
		CheckSprite ();
	}

	//Update the sprite based on how we're moving.  Basically manual animating. Maybe we wont need this tbh
    //This is done with animation trees in the editor. This will be scrapped eventually..
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

	public void Move(Vector2 dir){
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
        face_sr.flipX = player_sr.flipX;

        //Perform a raycast in the direction we're moving to see if we're humping a wall..
        RaycastHit rayHit;
        Physics.Raycast(transform.position, dir, out rayHit);
        if (rayHit.collider && rayHit.distance < 0.2f && rayHit.collider.CompareTag("Terrain"))
        {
            player_rb.velocity = new Vector3(0.0f, player_rb.velocity.y, 0.0f);
            return;
        }

        player_rb.AddForce(modifier * speed);
    }

	public void Jump(){
		//	player_cc.Move (Vector3.up * JumpHeight);
		if (grounded) {
			player_rb.AddForce (Vector3.up * jumpForce);
		}
	}
}
