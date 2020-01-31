using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCanvasGroupPlayer : AnimCurvePlayer
{
    public CanvasGroup targetCG;
    float currentAlpha;
    [SerializeField] float startAlpha;
    [SerializeField] float endAlpha;

    protected override void OnEnable()
    {
        base.OnEnable();
        currentAlpha = targetCG.alpha;
    }
    protected override void SetValueFromProgress(float progress)
    {
        float newValue = Mathf.LerpUnclamped(startAlpha,endAlpha, animSO.animationCurve.Evaluate(progress));
        if(targetCG == null)
        {
            Debug.LogError("Failed playing anim: target is not set",this);
            return;
        }

        targetCG.alpha = newValue;

    }

}
