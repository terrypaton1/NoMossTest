using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    SpriteRenderer sprite;
    int activeSortingOrder = 10;
    int inactiveSortingOrder = 5;

    public Color aliveColor;
    public Color aliveNonFocusedColor;
    float colourShiftRate = 1f;

    public bool isFirst;
    float startingTorque = -35f;

    float minMenuTorque = 35f;
    float menuTorqueIncreasePerSecond = 40f;

    public bool active;

    public bool isActivePlayer = false;

    public Rigidbody2D rigid;

    public AudioClip landClip;

    float minForceForSound = 130f;

    float previousSqrVelocity = 0f;

    public bool canBeDestroyed = false;

    float counterForLiving = 0f;
    float minTimeBeforeAlive = 0.08f;

    int platformSideTouchCount = 0;

    public void Init(){
        rigid = GetComponent<Rigidbody2D>();

        sprite = GetComponent<SpriteRenderer>();

        if(isFirst){
            rigid.AddTorque(startingTorque);
        }

        SetActivePlayer(false);
    }

    void Update(){
        if(active){
            MoveTowardsAliveColour(Time.deltaTime);

            if(GameController.Instance.menuControl.menuActive){
                //Move towards our min torque
                if(Mathf.Abs(rigid.angularVelocity) < minMenuTorque){
                    rigid.angularVelocity = rigid.angularVelocity + (Mathf.Sign(rigid.angularVelocity) * menuTorqueIncreasePerSecond * Time.deltaTime);
                }
            }
        }else if(minTimeBeforeAlive > counterForLiving){
            counterForLiving += Time.deltaTime;
        }

        previousSqrVelocity = rigid.velocity.sqrMagnitude;

        CheckDestroy();
    }

    void MoveTowardsAliveColour(float dt){
        Color c = sprite.color;

        Color currentColor;
        if(isActivePlayer){
            currentColor = aliveColor;
        }else{
            currentColor = aliveNonFocusedColor;
        }
        
        c.r = Mathf.MoveTowards(c.r, currentColor.r, dt * colourShiftRate);
        c.g = Mathf.MoveTowards(c.g, currentColor.g, dt * colourShiftRate);
        c.b = Mathf.MoveTowards(c.b, currentColor.b, dt * colourShiftRate);

        sprite.color = c;
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.CompareTag("Ground")){
            if(!active && counterForLiving > minTimeBeforeAlive){
                active = true;
            }

            if(previousSqrVelocity > minForceForSound){
                if(isActivePlayer){
                    Studios.Utils.SoundManager.PlayClip(landClip, 0.3f);
                }
            }
        }
    }

    void CheckDestroy(){
        if(transform.position.y < -70f && !isActivePlayer){
            canBeDestroyed = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.CompareTag("PointZone")){
            GameController.Instance.TriggerScoreIncrease();
            collider.gameObject.SetActive(false);
        }

        if(collider.CompareTag("PlatformSide")){
            platformSideTouchCount++;
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if(collider.CompareTag("PlatformSide")){
            platformSideTouchCount--;
        }
    }

    public void MovePlayer(){
        if(active){
            if(GameController.Instance.shouldDoTutorial && isActivePlayer && isFirst){
                //Don't move, yo!
                rigid.velocity = Vector2.zero;
                GameController.Instance.SetShowTutorial();
            }else{
                if(platformSideTouchCount == 0){
                    rigid.velocity = new Vector2(GameController.playerMoveSpeed, rigid.velocity.y);
                }
            }
        }
    }

    public void SetActivePlayer(bool b){
        isActivePlayer = b;

        if(b){
            sprite.sortingOrder = activeSortingOrder;
            GetComponentInChildren<EyeManager>().SetSortOrder(activeSortingOrder + 1);
        }else{
            sprite.sortingOrder = inactiveSortingOrder;
            GetComponentInChildren<EyeManager>().SetSortOrder(inactiveSortingOrder + 1);
        }
    }
}
