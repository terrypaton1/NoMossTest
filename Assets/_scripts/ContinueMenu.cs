using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContinueMenu : MonoBehaviour
{
    [SerializeField] MenuControl menuControlRef_;

    float rewardTime = (20f + 5f);
    float rewardTimer;

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Image timerImage;

    bool buttonPressed = false;

    bool isActive = false;

    public int continueScore = 0;
    public float continueSpeed = 0f;

    public void ResetEndGamePanel(int score, float speed)
    {
        continueScore = score;
        continueSpeed = speed;

        timerText.text = Mathf.CeilToInt(rewardTimer).ToString("N0");
        timerImage.fillClockwise = !timerImage.fillClockwise;

        buttonPressed = false;

        isActive = true;

        rewardTimer = rewardTime;
    }

    void LateUpdate()
    {
        if (!isActive) return;

        rewardTimer -= Time.deltaTime;
        timerText.text = Mathf.CeilToInt(rewardTimer).ToString("N0");

        timerImage.fillAmount = rewardTimer / rewardTime;

        if (rewardTimer <= 0f)
        {
            HandleNoButtonPressed();
        }
    }

    public void HandleNoButtonPressed()
    {
        if (buttonPressed) return;
        buttonPressed = true;

        menuControlRef_.SetShowMenu(true);

        isActive = false;
    }
}
