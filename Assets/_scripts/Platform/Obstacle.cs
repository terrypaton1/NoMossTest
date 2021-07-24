using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public Transform bottom;
    public Transform top;

    public Vector2 gapBounds;
    public Vector2 shiftBounds;

    // Start is called before the first frame update
    void Start()
    {
        SetDistance();
    }

    void SetDistance(){
        float shift = Random.Range(shiftBounds.x, shiftBounds.y);
        float gap = Random.Range(gapBounds.x, gapBounds.y);

        bottom.localPosition = new Vector3(0f, gap / -2f, 0f);
        top.localPosition = new Vector3(0f, gap / 2f, 0f);

        transform.localPosition = new Vector3(transform.localPosition.x, shift, 0f);
    }
}
