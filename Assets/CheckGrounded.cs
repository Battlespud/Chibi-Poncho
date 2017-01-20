using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrounded : MonoBehaviour {


	PlayerController playerController;

	// Use this for initialization
	void Start () {
		playerController = gameObject.GetComponentInParent<PlayerController> ();
		Physics.IgnoreLayerCollision (9, 10);
	}

	void FixedUpdate(){
		
	}

	void OnCollisionStay(Collision col){
		if (col.collider.gameObject.CompareTag ("Terrain")) {
			playerController.grounded = true;
		}
			}

	void OnCollisionExit(){
		playerController.grounded = false;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
