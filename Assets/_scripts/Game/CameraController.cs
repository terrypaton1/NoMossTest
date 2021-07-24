using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Player targetPlayer;

    public Vector2 cameraOffset;

    public MenuControl menu;

    void Update(){
        if(targetPlayer){
            MoveTowardsPosition(targetPlayer.transform.position);
        }
    }

    void MoveTowardsPosition(Vector3 v){
        Vector3 newPosition = transform.position;
        
        v.z = newPosition.z;
        v.x += cameraOffset.x;
        v.y += cameraOffset.y;
        newPosition = Vector3.Lerp(newPosition, v, 1f);

        transform.position = newPosition;
    }

    public void SetTargetPlayer(Player p){
        targetPlayer = p;
    }
}
