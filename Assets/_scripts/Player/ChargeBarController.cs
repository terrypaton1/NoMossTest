using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBarController : MonoBehaviour
{

    Player activePlayer;

    Vector3 lastDirection;

    public LineRenderer chargeBar;
    float chargeBarMin = 0.2f;
    float chargeBarMax = 2.5f;

    public bool isCharging = false;

    // Start is called before the first frame update
    void Start()
    {
        SetChargeBar(-1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(activePlayer){
            transform.position = activePlayer.transform.position;
        }   
    }

    public void SetActivePlayer(Player player){
        activePlayer = player;
    }

    public void SetChargeBar(float f){
        if(f < 0){
            chargeBar.gameObject.SetActive(false);
        }else{
            chargeBar.gameObject.SetActive(true);
            float currChargeMaxPos = ((chargeBarMax - chargeBarMin) * f) + chargeBarMin;
            chargeBar.SetPosition(1, new Vector3(currChargeMaxPos * lastDirection.x, currChargeMaxPos * lastDirection.y, 0f));
        }
    }

    public void SetChargeBarRotation(Vector3 rotationDirection){
        lastDirection = rotationDirection;
    }
}
