using UnityEngine;
using System.Collections;

public class AnimCurvePlayer : MonoBehaviour
{
   [Header("Anim")]
    public AnimationSO animSO;
    public bool loop = false;
    protected Coroutine highlightCoroutine;
    [HideInInspector]
    public bool animPlaying = false;

    public bool playOnEnable = false;
    public bool canBeOverride = true;

    protected virtual void OnEnable()
    {
        if(playOnEnable)
        {
            PlayAnim();
        }
    }
    public bool IsAnimPlaying()
    {
        return animPlaying;
    }
    void OnDisable()
    {
        animPlaying = false;
        StopAllCoroutines();
    }
    public void PlayAnim(AnimationSO forcedAnim = null)
    {
        if(animSO == null && forcedAnim == null)
        {
            Debug.LogError("Failed playing anim: anim is null");
            return;

        }
        if(!canBeOverride && animPlaying)
        {
            Debug.Log("Anim is already playing "+ gameObject.name);
            return;
        }

        if(forcedAnim != null) animSO = forcedAnim;

        highlightCoroutine = StartCoroutine(C_Anim());
       
    }
  
    public void StopAnim()
    {
        animPlaying = false;
        StopAllCoroutines();
        SetFinalState();
    }

    protected void SetInitialState(AnimationSO animation = null)
    {

    } 
    protected void SetFinalState()
    {
        SetValueFromProgress(1f);
    } 
    protected IEnumerator C_Anim()
    {
        float time = 0f;
        animPlaying = true;
        while (time <= 1f )
        {
            SetValueFromProgress(time);
            time += Time.deltaTime * animSO.animSpeed;
            if(time > 1f && loop) time = 0f;
            yield return null;
        }
        SetFinalState();
        animPlaying = false;
    }
    protected virtual void SetValueFromProgress(float time)
    {

    }

}
