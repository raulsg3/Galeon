using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScalePlayer : AnimCurvePlayer
{
    [Header("Transform")]
    [SerializeField] RectTransform targetRT;
    [SerializeField] Transform targetT;
    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 endPos;

    [SerializeField] bool modifyX = false;
    [SerializeField] bool modifyY = false;
    [SerializeField] bool modifyZ = false;
  
    protected override void SetValueFromProgress(float progress)
    {
        Vector3 newValue = Vector3.LerpUnclamped(startPos,endPos, animSO.animationCurve.Evaluate(progress));
        if(targetT == null && targetRT == null)
        {
            Debug.LogError("Failed playing anim: target is not set",this);
            return;
        }

        if(targetT != null)
        {
            if(!modifyX) newValue.x = targetT.localScale.x;
            if(!modifyY) newValue.y = targetT.localScale.y;
            if(!modifyZ) newValue.z = targetT.localScale.z;
            targetT.localScale = newValue;

           
        }else
        {
            if(!modifyX) newValue.x = targetRT.localScale.x;
            if(!modifyY) newValue.y = targetRT.localScale.y;
            if(!modifyZ) newValue.z = targetRT.localScale.z;

            targetRT.localScale = newValue;
        }

    }
}
