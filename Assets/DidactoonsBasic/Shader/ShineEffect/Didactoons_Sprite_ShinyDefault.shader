Shader "Sprites/Didactoons/DidactoonsShinyDefault"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        _Rect ("Rect Display", Vector) = (0,0,1,1)
        _Slope("Slope", Range(-5,5)) = 0
        _ShinePercentage("ShinePercentage", Range(-0.5,1.5)) = 0
        _ShineWidth("ShineWidth", Range(0,1)) = 0
        [MaterialToggle] _ShowShine("ShowShine", Float) = 0

        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
    }
    
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        
        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord  : TEXCOORD0;
            };
            
            fixed4 _Color;
            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float _AlphaSplitEnabled;
            float _ShinePercentage;
            float _ShineWidth;
            float _Slope;
            float _ShowShine;
            
            fixed4 _Rect;
            
            float ConvertValueToADifferentInterval(float OldValue, float OldMax, float OldMin, float NewMax, float NewMin)
            {
                float OldRange = (OldMax - OldMin)  ;
                float NewRange = (NewMax - NewMin)  ;
                
                float newValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
                return newValue;
            }


            fixed4 CreateShineEffect(float2 pixelPos)
            {
                //Obtener el color utilizando la textura del atlas
                fixed4 color = tex2D(_MainTex, pixelPos);
                if(_ShowShine==1)
                {
                    //Convertir las uv del atlas, en uv locales para el sprite dentro del atlas
                    float2 localuv = (pixelPos - _Rect.xy) / (_Rect.zw - _Rect.xy);

                    
                    float shineHalfWidth = _ShineWidth/2;               
                    //float slopeOffset = 1 - _ShinePercentage * 2 + (0.5 - _Slope / 2);
                    
                    float slopeOffset;
                    if(_Slope>=0)
                    {
                        slopeOffset = 1-_ShinePercentage*(_Slope+1);
                    }
                    else
                    {
                        //slopeOffset = -_ShinePercentage*(_Slope+1);
                        slopeOffset = -_ShinePercentage*(_Slope-1);
                    }

                    
                    float slopeCenter = _Slope * localuv.x + slopeOffset;

                    float highLevel = slopeCenter + shineHalfWidth; 
                    float lowLevel = slopeCenter - shineHalfWidth;

                    //Dibujar una franja que tiene como centro el slopecenter, y de anchos lowLevel y highlevel
                    if(localuv.y > lowLevel && localuv.y < highLevel)
                    {
                        float opacity = 1;
                        float maxValue = localuv.y - slopeCenter;
                        if(localuv.y > slopeCenter)
                        {
                            opacity = clamp(abs(highLevel - localuv.y )  / shineHalfWidth *1,0,1); //Esto funciona
                        }
                        else
                        {
                            opacity = clamp(abs(lowLevel - localuv.y )  / shineHalfWidth *1,0,1); //Esto funciona
                        }
                        color.rgb +=  color.a * opacity;

                    }
                }
             
                return color;
            }

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                    OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif
                
                return OUT;
            }
            
            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = CreateShineEffect(IN.texcoord) * IN.color;
                c.rgb *= c.a;
                
                return c;
            }
            ENDCG
        }
    }
}