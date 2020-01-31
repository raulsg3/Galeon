using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTransformPositionPlayer : AnimCurvePlayer
{
    [Header("Transform")]
    [SerializeField] Transform m_transform;
    [SerializeField] bool localPosition;
    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 endPos;

    [SerializeField] bool modifyX = true;
    [SerializeField] bool modifyY = true;
    [SerializeField] bool modifyZ = true;

    protected override void SetValueFromProgress(float progress)
    {
        Vector3 newValue = Vector3.LerpUnclamped(startPos,endPos, animSO.animationCurve.Evaluate(progress));
        if (m_transform == null){
            Debug.LogError("Failed playing anim: target is not set",this);
            return;
        }

        if(localPosition)
        {
            if(!modifyX) newValue.x = m_transform.localPosition.x;
            if(!modifyY) newValue.y = m_transform.localPosition.y;
            if(!modifyZ) newValue.z = m_transform.localPosition.z;

            m_transform.localPosition = newValue;

        }else{
            if(!modifyX) newValue.x = m_transform.position.x;
            if(!modifyY) newValue.y = m_transform.position.y;
            if(!modifyZ) newValue.z = m_transform.position.z;

            m_transform.position = newValue;
        }
    }
}
