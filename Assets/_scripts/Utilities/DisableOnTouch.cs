using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnTouch : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0)
            this.gameObject.SetActive(false);
    }
}
