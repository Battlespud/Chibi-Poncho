using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparency : MonoBehaviour {

	SpriteRenderer playerSprite_sr;

	float transparency = 1f;
	const float minTransparency = .2f;
	const float mTransparency = 1f;
	const float rate = .5f;

	int switchTransparency = 0; //0 it will get more transparent, 1 it will get more opaque

	// Use this for initialization
	void Start (){
		playerSprite_sr = this.gameObject.GetComponent<SpriteRenderer>();
	}

	void FixedUpdate(){
		switch (switchTransparency){
		case 0:
			transparency -= rate * Time.fixedDeltaTime;
			break;

		case 1: 
			transparency += rate * Time.fixedDeltaTime;
			break;
		}

		if (transparency >= mTransparency) {
			transparency = mTransparency;
			switchTransparency = 0;
		}

		if (transparency <= minTransparency) {
			transparency = minTransparency;
			switchTransparency = 1;
		}
		playerSprite_sr.color = new Color (1f, 1f, 1f, transparency);
	}

	// Update is called once per frame
	void Update () {

	}



}
