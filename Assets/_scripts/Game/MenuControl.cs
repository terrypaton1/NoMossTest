using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuControl : MonoBehaviour
{
    public FaderControl menuFader;
    public FaderControl gameFader;

    public FaderControl continueFader;
    public FaderControl backFader;
    public ContinueMenu continueMenu;

    public TextMeshProUGUI highScore;

    public Button musicButton;
    public Button soundButton;
    public Button gameContinueButton;

    public MusicManager musicManager;

    //Which part of the menu is being shown
    // 0 - main menu
    // 1 - credits
    public int menuOptionShown = 0;
    public CanvasGroup menuCanvas;
    public CanvasGroup creditsCanvas;

    public float menuFadePerSecond = 3f;

    public ScoreArea scoreArea;

    public bool musicEnabled{
        get{
            return PlayerPrefs.GetInt("musicEnabled", 1) == 1;
        }
        set{
            PlayerPrefs.SetInt("musicEnabled", (value == true ? 1 : 0));
        }
    }

    public bool soundEnabled{
        get{
            return PlayerPrefs.GetInt("soundEnabled", 1) == 1;
        }
        set{
            PlayerPrefs.SetInt("soundEnabled", (value == true ? 1 : 0));
        }
    }


    public bool menuActive;

    public Button[] creditsButtons;
    private bool hasContinued;

    // Start is called before the first frame update
    void Start()
    {
        menuFader.SetTarget(1f, true);
        gameFader.SetTarget(0f, true);

        SetSound();
        SetMusic();
    }

    // Update is called once per frame
    public void update(float deltaTime)
    {
        UpdateMenuFade();
    }

    public void SetShowMenu(bool b)
    {
        if (b)
            hasContinued = false;

        //Set whether menu should be showing
        menuActive = b;

        menuFader.SetTarget(b ? 1f : 0f);
        gameFader.SetTarget(b ? 0f : 1f);
        backFader.SetTarget(b ? 1f : 0f);

        continueFader.SetTarget(0f);

        soundButton.enabled = b;
        musicButton.enabled = b;
        gameContinueButton.enabled = b;

        UpdateMenuFade(true);
    }

    public void SetShowContinueMenu(bool b)
    {
        gameFader.SetTarget(b ? 0f : 1f);
        backFader.SetTarget(b ? 1f : 0f);

        if (hasContinued)
        {
            SetShowMenu(true);
            return;
        }

        //show the continue menu
        menuActive = b;

#if UNITY_EDITOR
        continueFader.SetTarget(b ? 1f : 0f);
        continueMenu.ResetEndGamePanel(score: GameController.Instance.Score, GameController.PlayerMoveSpeed);
        hasContinued = true;
#else
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            continueFader.SetTarget(b ? 1f : 0f);
            continueMenu.ResetEndGamePanel(score: GameController.Instance.Score, GameController.PlayerMoveSpeed);
            hasContinued = true;
        }
        else
            SetShowMenu(true);
#endif
    }

    public void SetHighScore(int i){
        highScore.text = "High Score: " + i;
        if(i == 0){
            highScore.GetComponent<CanvasGroup>().alpha = 0f;
        }else{
            highScore.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

    public void ToggleMusic(){
        musicEnabled = !musicEnabled;
        SetMusic();
    }

    public void ToggleSound(){
        soundEnabled = !soundEnabled;
        SetSound();
    }

    void SetSound(){
        soundButton.GetComponent<Animator>().SetBool("enabled", soundEnabled);
        Studios.Utils.SoundManager.volume = soundEnabled ? 1f : 0f;
    }

    void SetMusic(){
        musicButton.GetComponent<Animator>().SetBool("enabled", musicEnabled);
        musicManager.SetActive(musicEnabled);
    }

    public void ContinueGame(){
        GameController.Instance.RestartGame();

        menuOptionShown = 0;
    }

    public void ShowCredits(){
        if(menuOptionShown == 1){
            menuOptionShown = 0;
            creditsCanvas.interactable = false;
        }else{
            menuOptionShown = 1;
            creditsCanvas.interactable = true;
        }
    }

    void UpdateMenuFade(bool hard = false){
        if(hard){
            menuCanvas.alpha = menuOptionShown == 0 ? 1f : 0f;
            creditsCanvas.alpha = menuOptionShown == 1 ? 1f: 0f;
        }
        else{
            menuCanvas.alpha = Mathf.MoveTowards( menuCanvas.alpha, (menuOptionShown == 0 ? 1f : 0f ), menuFadePerSecond * Time.deltaTime );
            creditsCanvas.alpha = Mathf.MoveTowards( creditsCanvas.alpha, (menuOptionShown == 1 ? 1f: 0f), menuFadePerSecond * Time.deltaTime );
        }
    }

    public void PrivacyPolicy(){
        Application.OpenURL("http://studios.nomoss.co/privacy");
    }

    public void MusicCredit(){
        Application.OpenURL("http://dig.ccmixter.org/files/djlang59/37792");
    }

    public void StudiosLink(){
        Application.OpenURL("http://studios.nomoss.co");
    }
}
