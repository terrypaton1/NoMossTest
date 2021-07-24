using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class FaderControl : MonoBehaviour
{

    float targetAlpha;
    float currentAlpha;

    float alphaShiftPerSecond = 3f;

    CanvasGroup canvas;

    void Start()
    {
        canvas = GetComponent<CanvasGroup>();
    }


    void Update()
    {
        UpdateAlpha();
    }

    void UpdateAlpha(){
        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, alphaShiftPerSecond * Time.deltaTime);
        canvas.alpha = currentAlpha;

        canvas.interactable = canvas.alpha > 0f;
        canvas.blocksRaycasts = canvas.interactable;
    }

    public void SetTarget(float f, bool hard = false){
        targetAlpha = f;
        if(hard){
            currentAlpha = f;
        }
    }
}
