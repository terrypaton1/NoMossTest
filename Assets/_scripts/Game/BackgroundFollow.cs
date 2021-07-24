using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Transform mainCamera;

    void Update(){
        Vector3 position = transform.position;
        position.x = mainCamera.transform.position.x;

        transform.position = position;
    }
}
