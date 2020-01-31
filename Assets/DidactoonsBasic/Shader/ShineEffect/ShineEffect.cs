using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShineEffect : MonoBehaviour
{
    public bool activateOnEnable = true;
    private float currentPercentage= 0;

    private SpriteRenderer _spriteRenderer;
    private Image _image;

    [Header("Settings")]
    [Range(0f,1f)]
    public float shineWidth = 0.343f;
    [Range(-5f,5f)]
    public float slope = 1.54f;
    public bool inverseDirection = false;

    [Header("Time")]
    public float shineSpeed = 2.2f;
    public float waitBetweenShines = 1f;
    public float firstWait = 0;


    float targetPercentage;
    private Material _myMaterial;
    private int rectID;
    private int shinePercentageID;
    private int shineSlopeID;
    private int shineWidthID;
    private int showShineID;
    
    void Awake()
    {
        shinePercentageID = Shader.PropertyToID("_ShinePercentage");
        shineSlopeID = Shader.PropertyToID("_Slope");
        shineWidthID = Shader.PropertyToID("_ShineWidth");
        showShineID = Shader.PropertyToID("_ShowShine");
        rectID = Shader.PropertyToID("_Rect");

        SetReferences();
        _myMaterial = GetMaterial();
        ResetLocalUV();
    }
   
    void OnEnable()
    {
        if(activateOnEnable)
        {
            ActivateShine();
        }
    }

    [DebugButton]
    public void ActivateShine()
    {
        SetLocalUV();
        RestartShine();
        StopAllCoroutines();
        StartCoroutine(Shine());
        _myMaterial.SetFloat(showShineID,1);

    }

    public void DeactivateShine()
    {
        StopAllCoroutines();
        SetLocalUV();
        RestartShine();
        _myMaterial.SetFloat(showShineID,0);

    }

    private void RestartShine()
    {

        if(inverseDirection)
        {
            currentPercentage = 1.5f;
            targetPercentage = -0.5f;

        }else{

            currentPercentage = -0.5f;
            targetPercentage = 1.5f;
        }
        _myMaterial.SetFloat(shinePercentageID,currentPercentage);
        _myMaterial.SetFloat(shineWidthID,shineWidth);
        _myMaterial.SetFloat(shineSlopeID,slope);
    }

    IEnumerator Shine()
    {
        if(firstWait != 0){
            yield return new WaitForSeconds(firstWait);
        }

        while(true)
        {
            while( HasReachedPercentage())
            {
                if(inverseDirection)
                {
                    currentPercentage -= Time.deltaTime * shineSpeed;
                }
                else
                {
                    currentPercentage += Time.deltaTime * shineSpeed;
                }

                _myMaterial.SetFloat(shinePercentageID,currentPercentage);
                yield return null;
            }
            RestartShine();

            yield return new WaitForSeconds(waitBetweenShines);
        }
    }
    private bool HasReachedPercentage()
    {
        if(!inverseDirection) return currentPercentage < targetPercentage;
        else return currentPercentage > targetPercentage;

    }
     void SetReferences()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _image = GetComponent<Image>();
    }
    
    private Material GetMaterial()
    {
        if(_spriteRenderer != null)
        {
            return _spriteRenderer.material;
        }else if(_image != null)
        {
            return _image.material;
        }else 
        {
            _spriteRenderer = GetComponent<SpriteRenderer>(); 
            _image = GetComponent<Image>();
            if(_spriteRenderer != null) return _spriteRenderer.material;
            else if(_spriteRenderer != null) return _image.material;
            return null;
        }
    }

    [DebugButton]
    public void SetLocalUV()
    {
        Sprite sprite = null;
        if(GetComponent<SpriteRenderer>() != null)
        {
            sprite = GetComponent<SpriteRenderer>().sprite; 
        }else if(GetComponent<Image>() != null)
        {
            sprite = GetComponent<Image>().sprite;
        }else{
            Debug.LogError("ShineEffect: Cannot set UV cuz there isnt a sprite renderer or image");
            return;
        }
        
        Vector4 result = new Vector4(sprite.textureRect.min.x/sprite.texture.width,
            sprite.textureRect.min.y/sprite.texture.height,
            sprite.textureRect.max.x/sprite.texture.width,
            sprite.textureRect.max.y/sprite.texture.height);
        GetMaterial().SetVector ("_Rect", result);
    }

    private void ResetLocalUV()
    {
        GetMaterial().SetVector ("_Rect", Vector4.zero);
    }


}
