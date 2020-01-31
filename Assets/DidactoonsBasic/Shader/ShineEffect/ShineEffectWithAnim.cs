using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShineEffectWithAnim : MonoBehaviour
{
    public bool activateOnEnable = true;
    [SerializeField]private float currentPercentage= 0;

    private SpriteRenderer _spriteRenderer;
    private Image _image;

    [Header("Settings")]
    [Range(0f,1f)]
    public float shineWidth = 0.02f;
    [Range(-5f,5f)]
    public float slope = 0.02f;
    public bool inverseDirection = false;

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
        SetupShine();
    }

    void Update()
    {
        if(gameObject.activeInHierarchy)
        {
            _myMaterial.SetFloat(shinePercentageID,currentPercentage);
        }
    }

    [DebugButton]
    public void SetupShine()
    {
        SetLocalUV();
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
