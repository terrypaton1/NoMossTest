using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AlCaTrAzzGames.Utilities;

public class AestheticManager : PersistentSingleton<AestheticManager>
{
    public GameObject[] aestheticPrefabs;

    public static GameObject getRandomAestheticPrefab(){
        return Instance.aestheticPrefabs[Random.Range(0, Instance.aestheticPrefabs.Length)];
    }
}
