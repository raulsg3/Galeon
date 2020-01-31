using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimImageColorPlayer : AnimCurvePlayer
{
    
    [Header("Transform")]
    public Image targetImage;
    
    Color currentColor;
    [SerializeField] bool onlyTransparency = false;
    [SerializeField] Color startValue;
    [SerializeField] Color endValue;

    protected override void SetValueFromProgress(float progress)
    {
        if(targetImage == null)
        {
            Debug.LogError("Failed playing anim: target is not set",this);
            return;
        }
        Color newValue = Color.LerpUnclamped(startValue,endValue, animSO.animationCurve.Evaluate(progress));
        
        if(onlyTransparency)
        {
            newValue.r = targetImage.color.r;
            newValue.g = targetImage.color.g;
            newValue.b = targetImage.color.b;
        }
        targetImage.color = newValue;
    }
}
