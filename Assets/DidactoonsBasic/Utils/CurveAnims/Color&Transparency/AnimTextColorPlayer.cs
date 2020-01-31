using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimTextColorPlayer : AnimCurvePlayer
{
   
    [Header("Text color")]
    [SerializeField] Text targetText;
    [SerializeField] TMPro.TextMeshProUGUI targetTMPText;
    Color currentColor;
    [SerializeField] bool onlyTransparency = false;
    [SerializeField] Color startValue;
    [SerializeField] Color endValue;

    protected override void SetValueFromProgress(float progress)
    {
        if(targetText == null && targetTMPText == null)
        {
            Debug.LogError("Failed playing anim: target is not set",this);
            return;
        }
        Color newValue = Color.LerpUnclamped(startValue,endValue, animSO.animationCurve.Evaluate(progress));
        
        if(targetText != null)
        {
            if(onlyTransparency)
            {
                newValue.r = targetText.color.r;
                newValue.g = targetText.color.g;
                newValue.b = targetText.color.b;
            }
            targetText.color = newValue;
        }else{
            if(onlyTransparency)
            {
                newValue.r = targetTMPText.color.r;
                newValue.g = targetTMPText.color.g;
                newValue.b = targetTMPText.color.b;
            }
            targetTMPText.color = newValue;
        }
    }
}
