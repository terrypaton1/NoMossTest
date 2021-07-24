using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAestheticPoint : MonoBehaviour
{
    public float aestheticChance = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        if(Random.Range(0f, 1f) < aestheticChance){
            CreateAesthetic();
        }
    }

    void CreateAesthetic(){
        GameObject aesthetic = GameObject.Instantiate(AestheticManager.getRandomAestheticPrefab());
        aesthetic.transform.SetParent(transform);
        aesthetic.transform.localPosition = Vector3.zero;
        aesthetic.transform.localScale = Vector3.one;
    }
}
