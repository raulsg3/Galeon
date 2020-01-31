using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Didactoons/AnimationSO", fileName = "AnimationSO")]
public class AnimationSO : ScriptableObject
{
    public AnimationCurve animationCurve;
    public float animSpeed = 6f;

}
