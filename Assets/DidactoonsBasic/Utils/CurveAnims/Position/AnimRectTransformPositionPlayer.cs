using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimRectTransformPositionPlayer : AnimCurvePlayer
{
    [Header("Transform")]
    [SerializeField] RectTransform targetRT;
    [SerializeField] Vector2 startPos;
    [SerializeField] Vector2 endPos;

    [SerializeField] bool modifyX = false;
    [SerializeField] bool modifyY = false;
    protected override void SetValueFromProgress(float progress)
    {
        Vector3 newValue = Vector3.LerpUnclamped(startPos,endPos, animSO.animationCurve.Evaluate(progress));
        if(targetRT == null)
        {
            Debug.LogError("Failed playing anim: target is not set",this);
            return;
        }

        if(!modifyX) newValue.x = targetRT.anchoredPosition.x;
        if(!modifyY) newValue.y = targetRT.anchoredPosition.y;
        
        targetRT.anchoredPosition = newValue;

    }

}
