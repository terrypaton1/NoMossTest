using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{

    float latestGeneratedPlatform = -11f;
    float latestGeneratedObstacle = -11f;
    float generationAheadDistance = 30f;
    Player activePlayer;

    public float platformEveryX;
    public float obstacleEveryX;

    public float platformXOffset;
    public float obstacleXOffset;

    public GameObject platformPrefab;
    public GameObject obstaclePrefab;

    List<GameObject> platforms;
    List<GameObject> obstacles;

    void Start(){
        platforms = new List<GameObject>();
        obstacles = new List<GameObject>();
    }

    // Update is called once per frame
    public void update()
    {
        CheckGeneratePlatform();
        CheckGenerateObstacle();
    }

    void CheckGeneratePlatform(){
        if(activePlayer.transform.position.x + generationAheadDistance > latestGeneratedPlatform + platformEveryX){
            GeneratePlatform();
        }
    }

    void GeneratePlatform(){
        float platformPosition;
        if(latestGeneratedPlatform < -10f){
            platformPosition = platformXOffset;
        }else{
            platformPosition = platformEveryX;
            platformPosition += latestGeneratedPlatform;
        }

        latestGeneratedPlatform = platformPosition;

        GameObject newPlatform = GameObject.Instantiate(platformPrefab);
        newPlatform.transform.position = new Vector3(latestGeneratedPlatform, newPlatform.transform.position.y, 0f);
        platforms.Add(newPlatform);
    }

    void CheckGenerateObstacle(){
        if(activePlayer.transform.position.x + generationAheadDistance > latestGeneratedObstacle + obstacleEveryX){
            GenerateObstacle();
        }
    }

    void GenerateObstacle(){
        float obstaclePosition;
        if(latestGeneratedObstacle < -10f){
            obstaclePosition = obstacleXOffset;
        }else{
            obstaclePosition = obstacleEveryX;
            obstaclePosition += latestGeneratedObstacle;
        }

        latestGeneratedObstacle = obstaclePosition;

        GameObject newObstacle = GameObject.Instantiate(obstaclePrefab);
        newObstacle.transform.position = new Vector3(latestGeneratedObstacle, newObstacle.transform.position.y, 0f);
        obstacles.Add(newObstacle);
    }

    public void SetActivePlayer(Player player){
        activePlayer = player;
    }

    public void ClearAll(){
        latestGeneratedObstacle = -11f;
        latestGeneratedPlatform = -11f;

        foreach(GameObject g in platforms){
            Destroy(g);
        }

        foreach(GameObject g in obstacles){
            Destroy(g);
        }

        platforms = new List<GameObject>();
        obstacles = new List<GameObject>();
    }
}
