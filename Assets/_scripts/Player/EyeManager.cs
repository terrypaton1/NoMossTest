using UnityEngine;
using System.Collections;

public class EyeManager : MonoBehaviour {
	EyeAnimationScript[] eyes;

	public bool doEyes;

	//These variables manage eye rotation
	private float previousParentRotation = 0f;
	private float maxRotChangePerFrame = 3f;
	private int prevIndex;
	private float rotCounter;
	private float collectiveAcceptedRotationTimeRequired = 0.15f;

	private bool facingRight = true;

	private int prevFinalIndex;

	//Pseudocode for eyes
	/*
		If we haven't rotated more than 3 degrees for at least 10 frames in a row
			Find our appropriate eye
			If it's the same as it was last frame
				Move our appropriate eye to open
			Otherwise
				Move the previous eye towards closed
		Otherwise, move our eye to closed
	 */ 

	void Start(){
		InitializeEyes ();
	}

	void InitializeEyes(){
		eyes = GetComponentsInChildren<EyeAnimationScript>();
	}
	
	void Update(){
		//Update the eye animation based on rotation
		float currentParentRotation = transform.parent.localRotation.eulerAngles.z;

		//Make sure our current and previous rotation aren't apart by more than 360
		if (currentParentRotation < 10f) {
			if (previousParentRotation > 350f) {
				previousParentRotation = 0f - (360f - previousParentRotation);
			}
		}
		if (currentParentRotation > 350f) {
			if (previousParentRotation < 10f) {
				previousParentRotation = 360f + previousParentRotation;
			}
		}

		//Get the difference between our current rotation and past rotation
		float rotationDifference = Mathf.Abs (previousParentRotation - currentParentRotation);

		if (!doEyes) {
			//If the eyes are disabled, they should all be closed / closing
			SetActive (4);
			rotCounter = 0f;
		} else if(rotationDifference < maxRotChangePerFrame){
			//We are not rotating (too much) at the moment
			rotCounter += Time.deltaTime;

			if(rotCounter > collectiveAcceptedRotationTimeRequired){
				//We haven't been rotating for too long

				int index = zRotIndex;
				if(!facingRight){
					if(index == 1){
						index = 3;
					}else if(index == 3){
						index = 1;
					}
				}

				if(prevIndex != zRotIndex){
					//If we already had a different eye open, close it (so we never have two eyes open)
					SetActive (4);
					rotCounter = 0f;
				}else{
					//Otherwise, open this eye
					SetActive (index);
				}

				prevFinalIndex = index;

				prevIndex = zRotIndex;
			}else{
				//We haven't hi
				SetActive (4);
			}
		}else{
			//We are rotating this frame
			//Set our rotation counter back to 0
			SetActive (4);
			rotCounter = 0f;
		}

		//Update our previous rotation
		previousParentRotation = currentParentRotation;
	}

	int zRotIndex = 0;
	void FixedUpdate(){
		//Figure out which eye should be open
		float zRotation = transform.parent.localRotation.eulerAngles.z;

		/*
		 * If we're between -45 and 45 degrees, our first eye should be open
		 * If we're between 45 and 135 degrees, our second eye should be open
		 * If we're between 135 & 225 degrees, our third eye should be open
		 * Otherwise, our fourth eye should be open
		 */
		if(zRotation > 45f){
			if(zRotation > 135f){
				if(zRotation > 225f){
					if(zRotation < 315f){		
						zRotIndex = 3;
					}else{
						zRotIndex = 0;
					}
				}else{
					zRotIndex = 2;
				}
			}else{
				zRotIndex = 1;
			}
		}else{
			zRotIndex = 0;
		}
	}

	public void SetFacing(bool b){
		if (eyes == null) {
			InitializeEyes ();
		}

		if(facingRight != b){
			int index = zRotIndex;

			if (b) {
				//We're going FROM facing left TO facing right
				if (index == 1) {
					index = 3;
				} else if (index == 3) {
					index = 1;
				}
			}

			for (int i = 0; i < 4; i++) {
				eyes [i].QuickClose ();
			}
			if (doEyes) {
				eyes [index].QuickOpen ();
			}
		}

		facingRight = b;
	}
	
	public void SetActive(int targ){
		//Debug.Log("Set active " + targ);
		for(int i = 0; i < eyes.Length; i++){
			if(i == targ){
				eyes[i].open = true;
			}else{
				eyes[i].open = false;
			}
		}
	}

	public int GetFinalIndex(){
		return prevFinalIndex;
	}

	public void SetSortOrder(int i){
		InitializeEyes();
		foreach(EyeAnimationScript eye in eyes){
			eye.SetSortOrder(i);
		}
	}
}
