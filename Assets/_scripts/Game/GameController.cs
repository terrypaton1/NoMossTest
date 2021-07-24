using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AlCaTrAzzGames.Utilities;

using PlayFab;
using PlayFab.ClientModels;

public class GameController : Singleton<GameController>
{
    public Player activePlayer;

    public List<Player> players;

    public ChargeBarController chargeBar;

    public CameraController cameraController;

    public PlatformGenerator platformGenerator;

    public GameObject playerPrefab;

    public TMPro.TextMeshProUGUI scoreText;

    public MenuControl menuControl;

    public Vector2 playerStartPosition;

    Vector3 lastInputtedPos;
    float lastInputtedCloneForce;

    float minChargeTime = 0.05f;
    float maxChargeTime = 0.6f;

    float chargeTime = 0f;

    float creationForceMin = 700f;
    float creationForceMax = 1100f; 

    float creationTorque = -60f;

    Vector3 creationPositionOffset = new Vector3(0.5f, 0.5f, 0f);

    public static float playerMoveSpeed = 4f;
    public float startingPlayerMoveSpeed = 4f;
    float moveSpeedIncreaseRate = 0.1f;

    public static float PlayerMoveSpeed { get => playerMoveSpeed; }


    public AudioClip cloneCreateClip;
    public AudioClip gameOverClip;
    public AudioSource pointSource;
    public AestheticGenerator aestheticGenerator;

    public int highScore {
        get{
            return PlayerPrefs.GetInt("HighScore", 0);
        }
        private set{
            PlayerPrefs.SetInt("HighScore", value);
        }
    }

    bool isGameOver = false;

    int score = 0;
    public int Score { get => score; }

    public static UnityEvent OnGameStart = new UnityEvent();
    public static UnityEvent OnGameEnd = new UnityEvent();

    int LEADERBOARD_PLAYERS_TO_GET = 5;

    public bool shouldDoTutorial;

    public Animator tutorialPointer;

    void Start()
    {
        if (OnGameStart == null)
            OnGameStart = new UnityEvent();

        if (OnGameEnd == null)
            OnGameEnd = new UnityEvent();

        players = new List<Player>();
        players.Add(activePlayer);
        activePlayer.Init();

        SetActivePlayer(activePlayer);
        
        scoreText.text = "0";

        menuControl.SetShowMenu(true);
        menuControl.SetHighScore(highScore);

        PlayFabManager.Instance.loginCallback += SuccessfulPlayfabLogin;
        PlayFabManager.Instance.LoginWithMobileID();

        if(highScore == 0){
            shouldDoTutorial = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(menuControl.menuActive){
            //Update Menu
            UpdateMenu();
        }else{
            //Update Game
            UpdateGame();
        }
    }

    void UpdateMenu(){
        menuControl.update(Time.deltaTime);
    }

    void UpdateGame(){
        foreach(Player player in players){
            player.MovePlayer();
        }

        if(Input.GetMouseButtonDown(0)){
            if(!chargeBar.isCharging){
                //Start charging
                chargeBar.isCharging = true;
            }
        }else if(Input.GetMouseButtonUp(0)){
            if(chargeBar.isCharging){
                //Stop charging
                chargeBar.isCharging = false;

                chargeBar.SetChargeBar(-1f);

                if(chargeTime > minChargeTime){
                    //Shoot a clone
                    CreateClone(lastInputtedCloneForce);
                }

                chargeTime = 0f;
            }
        }

        if(chargeBar.isCharging){
            if(activePlayer.active){
                chargeTime = Mathf.MoveTowards(chargeTime, maxChargeTime, Time.deltaTime);

                if(chargeTime > minChargeTime){
                    // UpdateChargeTimeBased();
                    UpdateChargePositionBased();
                }
            }
        }
        
        playerMoveSpeed += Time.deltaTime * moveSpeedIncreaseRate;

        platformGenerator.update();

        CheckGameOver();

        RemoveClones();
    }

    void UpdateChargeTimeBased(){
        chargeBar.SetChargeBar(chargeTime / maxChargeTime);
        lastInputtedCloneForce = chargeTime / maxChargeTime;

        lastInputtedPos = GetDistanceToPlayer();
        lastInputtedPos = lastInputtedPos.normalized;

        chargeBar.SetChargeBarRotation(lastInputtedPos);
    }

    void UpdateChargePositionBased(){
        Vector2 inputPos = Input.mousePosition;
        Vector2 playerPos = Camera.main.WorldToScreenPoint(activePlayer.transform.position);

        float distance = Vector2.Distance(inputPos, playerPos);
        float maxDist = Screen.width / 3f;
        if(distance > maxDist){
            distance = maxDist;
        }

        lastInputtedCloneForce = distance / maxDist; 

        chargeBar.SetChargeBar(distance / maxDist);

        Vector2 inputVector = new Vector2(inputPos.x - playerPos.x, inputPos.y - playerPos.y);
        inputVector = inputVector.normalized;

        chargeBar.SetChargeBarRotation(inputVector);

        lastInputtedPos = inputVector;
    }

    Vector3 GetDistanceToPlayer(){
        Vector3 inputWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 distance = inputWorldPos - activePlayer.transform.position;
        distance.z = 0f;
        return distance;
    }

    void RemoveClones(){
        List<Player> toRemove = new List<Player>();

        foreach(Player p in players){
            if(p.canBeDestroyed){
                toRemove.Add(p);
            }
        }

        foreach(Player p in toRemove){
            players.Remove(p);
            Destroy(p.gameObject);
        }
    }

    void CreateClone(float proportionalStrength){
        //Create a clone with the set proportional strength
        float force = ((creationForceMax - creationForceMin) * proportionalStrength) + creationForceMin;
        Vector2 creationForce = force * lastInputtedPos;

        if(shouldDoTutorial){
            creationForce *= 0.75f;
        }
        
        GameObject newPlayerObj = GameObject.Instantiate(playerPrefab);
        newPlayerObj.transform.position = activePlayer.transform.position + creationPositionOffset;
        Player newPlayer = newPlayerObj.GetComponent<Player>();
        newPlayer.Init();
        SetActivePlayer(newPlayer);

        Rigidbody2D newRigid = newPlayerObj.GetComponent<Rigidbody2D>();
        newRigid.AddTorque(creationTorque);
        newRigid.AddForce(creationForce);

        Studios.Utils.SoundManager.PlayClip(cloneCreateClip, 0.75f);

        //Debug.Log(creationForce);

        AddPlayer(newPlayer);

        if(shouldDoTutorial){
            SetHideTutorial();
        }
    }

    void CheckGameOver(){
        if(activePlayer.transform.position.y < -10f){
            GameOver();
        }
    }

    void SetActivePlayer(Player player){
        if(activePlayer != null){
            activePlayer.SetActivePlayer(false);
        }

        activePlayer = player;

        cameraController.SetTargetPlayer(activePlayer);
        chargeBar.SetActivePlayer(activePlayer);
        platformGenerator.SetActivePlayer(activePlayer);

        if(aestheticGenerator != null){
            aestheticGenerator.SetActivePlayer(activePlayer);
        }

        activePlayer.SetActivePlayer(true);
    }

    void AddPlayer(Player player){
        players.Add(player);
    }

    void RemovePlayer(Player player){
        players.Remove(player);
    }

    public void TriggerScoreIncrease(){
        SetScore(score + 1);
        pointSource.pitch = Random.Range(1f, 1.2f);
        pointSource.Play();
    }

    void SetScore(int newScore){
        score = newScore;
        scoreText.text = score.ToString("00");
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        //We've lost the game
        menuControl.SetShowContinueMenu(true);

        chargeBar.isCharging = false;
        chargeBar.SetChargeBar(-1f);

        bool newHighScore = CheckHighscore(score);
        menuControl.SetHighScore(highScore);

        Studios.Utils.SoundManager.PlayClip(gameOverClip, 0.8f);

        PlayFabManager.Instance.WriteSimpleEvent("attempt_completed", new Dictionary<string, object>(){
            {"score_achieved", score},
            {"new_high_score", newHighScore}
        });

        if (OnGameEnd != null)
            OnGameEnd.Invoke();
    }

    public void RestartGame(int score = 0, float speed = -1f)
    {
        isGameOver = false;

        SetScore(score);
        menuControl.SetShowMenu(false);

        activePlayer.transform.position = playerStartPosition;
        activePlayer.rigid.velocity = Vector2.zero;

        playerMoveSpeed = speed > 0f ? speed : startingPlayerMoveSpeed;

        pointSource.volume = Studios.Utils.SoundManager.volume;

        platformGenerator.ClearAll();

        if (OnGameStart != null)
            OnGameStart.Invoke();
    }

    public bool CheckHighscore(int currentScore){
        if(currentScore > highScore){
            UpdateHighScore(currentScore);
            return true;
        }

        return false;
    }

    void SuccessfulPlayfabLogin(){
        //Login to playfab success
        GetLeaderboard();
    }

    void UpdateHighScore(int newHighScore){
        if(!PlayFabManager.Instance.isLoggedIn){
            return;
        }

        highScore = newHighScore;

        menuControl.scoreArea.DisableLeaderboard();
        
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest{
            Statistics = new List<StatisticUpdate>{
                new StatisticUpdate{
                    StatisticName = "highscore",
                    Value = newHighScore
                }
            }
        }, result => OnHighScoreUpdated(result), CallbackError);
    }

    void OnHighScoreUpdated(UpdatePlayerStatisticsResult updatePlayerStatisticsResult){
        //High score succesfully submitted
        Debug.Log("New high score successfully submitted");

        //Get new leaderboard
        if(highScore > 0){
            GetLeaderboard();
        }
    }

    public void GetLeaderboard(){
        Debug.Log("Getting leaderboard");

        PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest{
            MaxResultsCount = LEADERBOARD_PLAYERS_TO_GET,
            StatisticName = "highscore"
        }, result => OnLeaderboardGetSuccess(result), CallbackError);
    }

    void OnLeaderboardGetSuccess(GetLeaderboardAroundPlayerResult getLeaderboardAroundPlayerResult){
        //Successful leaderboard around player get
        Debug.Log("Leaderboard data collected");

        //Update the leaderboard to display this!
        List<PlayerLeaderboardEntry> leaderboard = getLeaderboardAroundPlayerResult.Leaderboard;

        string debugString = "";

        ScoreArea.LeaderboardData.LeaderboardPlayer[] playerData = new ScoreArea.LeaderboardData.LeaderboardPlayer[leaderboard.Count];

        int ourNewScore = -1;

        for(int i = 0; i < leaderboard.Count; i++){
            int position = leaderboard[i].Position + 1;
            int score = leaderboard[i].StatValue;
            string id = leaderboard[i].PlayFabId;
            debugString += position + ": " + score + " (" + id + ")" + "\n";

            if(id == PlayFabManager.Instance.PlayFabID){
                ourNewScore = score;
            }

            playerData[i] = new ScoreArea.LeaderboardData.LeaderboardPlayer(position, score, (id == PlayFabManager.Instance.PlayFabID));
        }

        if(ourNewScore != highScore){
            Debug.Log("Data is not updated yet");
            StartCoroutine(DoGetLeaderboard());
            return;
        }

        Debug.Log(debugString);
        menuControl.scoreArea.UpdateLeaderboardData(new ScoreArea.LeaderboardData(playerData));
    }

    void CallbackError(PlayFabError error){
        Debug.LogError(error.GenerateErrorReport());
    }

    IEnumerator DoGetLeaderboard(float delay = 1f)
    {
        yield return new WaitForSecondsRealtime(delay);
        GetLeaderboard();
    }

    public void SetShowTutorial(){
        tutorialPointer.SetBool("showTutorial", true);
        tutorialPointer.transform.position = activePlayer.transform.position;
    }

    void SetHideTutorial(){
        tutorialPointer.SetBool("showTutorial", false);
        shouldDoTutorial = false;
    }
}
