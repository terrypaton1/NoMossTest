using UnityEngine;
using System.Collections;

public class EyeAnimationScript : MonoBehaviour {

	public bool open = false;
	
	private float counter;
	private float framerate = 0.03f;
	
	private int spriteIndex = 0;
	
	public Sprite[] eyeSprites;
	
	private SpriteRenderer spr;

	// Use this for initialization
	void Start () {
		InitializeSprite ();
	}

	void InitializeSprite(){
		spr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		counter += Time.deltaTime;
		if(counter > framerate){
			counter -= framerate;
			
			if(open){
				if(spriteIndex < 3){
					spriteIndex++;
				}
			}else{
				if(spriteIndex > 0){
					spriteIndex--;
				}
			}
			
			spr.sprite = eyeSprites[spriteIndex];
		}
	}

	public void QuickOpen(){
		if (spr == null) {
			InitializeSprite ();
		}

		open = true;
		spriteIndex = 3;
		spr.sprite = eyeSprites [spriteIndex];
	}

	public void QuickClose(){
		if (spr == null) {
			InitializeSprite ();
		}

		open = false;
		spriteIndex = 0;
		spr.sprite = eyeSprites [0];
	}

	public void SetSortOrder(int i){
		InitializeSprite();
		spr.sortingOrder = i;
	}
}
