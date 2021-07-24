using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarePlatform : MonoBehaviour
{

    public Vector2 rotationBounds;
    public Color[] possibleColours;
    public Vector2 yPositionOffset;

    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        RandomiseRotation();
        RandomiseColour();
        RandomisePosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RandomisePosition(){
        Vector3 position = transform.localPosition;
        position.y += Random.Range(yPositionOffset.x, yPositionOffset.y);
        transform.localPosition = position;
    }

    void RandomiseRotation(){
        transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(rotationBounds.x, rotationBounds.y));
    }

    void RandomiseColour(){
        sprite.color = possibleColours[Random.Range(0, possibleColours.Length)];
    }
}
