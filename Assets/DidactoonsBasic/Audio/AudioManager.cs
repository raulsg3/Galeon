using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

	[Header("Settings")]
	public float maxMusicVolume=1f;
	public float maxEffectsVolume=1f;
	
	[Header("Generic Sounds")]
	public AudioEffectSO buttonClickForward;
	public AudioEffectSO buttonClickBack;

	public enum PossibleMusics{
		START_MENU,DIBUJO,DRAWING_SELECTOR,DOT_SELECTOR, FANFARE_DIBUJO,FANFARE_REGALO
	}

	[Header("References")]

    public AudioSource[] effectsSources;
	public static AudioSource[] s_effectsSources;
	private static int numEffectSources = 0;
	private static int nextEffectSource = 0;

	public static Dictionary <PossibleMusics,AudioSource> musicAudioSources;
	public static Dictionary <string,AudioClip[]> audioClips;

	private static bool _pausa = false;
	private static PossibleMusics _lastMusicName;

	private static AudioSource current=null;
	private static float fadeSpeed=2f;
	// private static bool isMusicOn=true;

	private static AudioManager s_musicManager = null;
	public static AudioManager Instance
	{
		get { return s_musicManager; }
	}

	void Awake()
	{
		// singleton
		if(s_musicManager==null)
		{
			// si no existía uno ya inicializa la referencia
			s_musicManager = this;	

			DontDestroyOnLoad (gameObject);
			// si algún diccionario no está creado lo crea
			if(AudioManager.audioClips==null)
			{
				AudioManager.audioClips = new Dictionary<string,AudioClip[]> ();
			}
			if(AudioManager.musicAudioSources==null)
			{
				AudioManager.musicAudioSources = new Dictionary<PossibleMusics,AudioSource> ();
			}
			
			// inicializar las fuentes de efectos
			numEffectSources = effectsSources.Length;
			s_effectsSources = effectsSources;
			for(int i=0;i<numEffectSources;i++)
			{
				effectsSources [i].volume = maxEffectsVolume;
			}
		}
		else
		{
			// si ya existía uno, destruye este
			Destroy (gameObject);
		}
	}

	public static void PlayMusic(PossibleMusics newMusicName,float newMusicTime=0f)
	{
		if(Instance == null)
		{
			Debug.LogWarning("No hay instancia de AudioManager");
			return;
		}
		// almacena la música solicitada por si hubiera que volver del silencio
		_lastMusicName = newMusicName;

		if(IsMusicOptionEnable())
		{
			AudioSource newMusic;
			if(musicAudioSources.ContainsKey(newMusicName))
			{
				// si existe la nueva música, obtiene una referencia
				newMusic = musicAudioSources [newMusicName];
				if(newMusic!=current)
				{   
                    Instance.StopCoroutine("FadeIn");
                    Instance.StopCoroutine("FadeOut");
                    // establece el tiempo de inicio de la nueva música (normalmente 0 a no ser que se vuelva de un mix)
                    newMusic.time=newMusicTime;
					// si no es la que está ya sonando hace un cross fade
					if(current)
					{
     	                Instance.StartCoroutine (Instance.FadeOut (current));
					}
                    
					Instance.StartCoroutine (Instance.FadeIn (newMusic));	
					// Debug.Log ("AudioManager: Now playing: " + newMusicName);
				} else {
					Debug.Log ("AudioManager: Same music, not restarting: " + newMusicName);
				}
			} else {
				Debug.Log ("AudioManager: Music unknown: " + newMusicName);
			}
		} else {
			Debug.Log ("AudioManager: Music is off, not playing: "+newMusicName);
		}
	}
	
	public static void MixMusic(PossibleMusics newMusicName)
	{
		if(Instance == null)
		{
			Debug.LogWarning("No hay instancia de AudioManager");
			return;
		}
        // si volviera del silencio volvería a la música anterior, así que
        // no se almacena nada

        if(IsMusicOptionEnable())
        {
            AudioSource newMusic;
            if(musicAudioSources.ContainsKey(newMusicName))
            {
                // si existe la nueva música, obtiene una referencia
                newMusic = musicAudioSources [newMusicName];
                if(newMusic!=current)
                {
					// if(current)
					// {
                    // 	// se llama a la corrutina para que vuelva a la música cuando acabe
     	            //     Instance.StartCoroutine(Instance.EndMix(newMusic.clip.length, current.time, _lastMusicName));
					// }
                    // empieza a sonar la música del mix
					if(current != null)
                    	PlayMusic(newMusicName,current.time);
					else
                    	PlayMusic(newMusicName);

                    // Debug.Log ("Now mixing: " + newMusicName);
                }
                else {
                    Debug.Log ("Same music, not mixing: " + newMusicName);
                }
            }
            else {
                Debug.Log ("Music unknown for mix: " + newMusicName);
            }
        }
        else {
            Debug.Log ("Music is off, not playing mix: "+newMusicName);
        }
	}
	private IEnumerator FadeOut(AudioSource fadeOutMusic, bool SetCurrentNull = false)
	{
		do
		{
			yield return null;
			fadeOutMusic.volume=Mathf.MoveTowards(fadeOutMusic.volume,0f,fadeSpeed*Time.deltaTime);
		}while(fadeOutMusic.volume>0.01f);
		fadeOutMusic.volume = 0f;
		fadeOutMusic.Stop ();
		if(SetCurrentNull) current = null;
	}
	private IEnumerator FadeIn(AudioSource fadeInMusic)
	{
		float limitVolume = maxMusicVolume - 0.01f;
		current = fadeInMusic;
		current.volume = 0f;
		current.Play ();
		do
		{
			yield return null;
			current.volume=Mathf.MoveTowards(current.volume,maxMusicVolume,fadeSpeed*Time.deltaTime);
			
		}while(current.volume<limitVolume);
		current.volume = maxMusicVolume;
	}

	public static void FadeOutCurrentMusic()
	{
		if(Instance == null) return;

		if(current != null)
		{
			Instance.StopAllCoroutines();
			Instance.StartCoroutine(Instance.FadeOut(current,true));
		}else{
			Debug.LogError("There is not a current music set ");
		}

	} 

	public void StopAll()
	{ 
		if(current) 
		{
			StopAllCoroutines();
			current.Stop();
			current = null;
		}
	}
    private IEnumerator EndMix(float duration,float previousMusicTime,PossibleMusics previousMusicName)
    {
        Debug.Log("Waiting: "+duration);
        yield return new WaitForSeconds(duration);
        Debug.Log("Back from mix");
        PlayMusic(previousMusicName,previousMusicTime);
    }

	public bool Music
	{
		set
		{
			SetMusicOptionEnable(value);
			// se guarda el nuevo estado y se para la musica o inicia, segun el caso
			if(value)
			{
				// suena la última música solicitada
				PlayMusic(_lastMusicName);
			}
			else
			{
				// para todas las músicas
				StopAll();
			}
		}
		get
		{
			return IsMusicOptionEnable();
		}
	}

	public bool SoundEffects
	{
		set
		{
			SetSoundEffectsOptionEnable(value);
		}
		get
		{
			return IsSoundEffectsOptionEnable();
		}
	}

	public bool Pausa
	{
		set
		{
			if(IsMusicOptionEnable())
			{
				_pausa=value;
				if(_pausa)
				{
					current.Pause ();
				}
				else
				{
					current.UnPause();
				}
			}	
		}
		get
		{
			return _pausa;
		}
	}
    // public static void PlayEffect(string effectName)
    // {
	// 	if(numEffectSources>0 && audioClips.ContainsKey(effectName) && isMusicOn)
    //     {
    //         int randomIndex = Random.Range(0, audioClips[effectName].Length);
	// 		s_effectsSources[nextEffectSource].PlayOneShot(audioClips[effectName][randomIndex]);
	// 		nextEffectSource = (nextEffectSource + 1) % numEffectSources;
    //     }
	// 	else if(numEffectSources<=0)
	// 	{
	// 		Debug.Log ("No hay AudioSources definidas para efectos");
	// 	}
	// 	else if(isMusicOn)
	// 	{
	// 		Debug.Log ("No se reconoce el efecto con el nombre: " + effectName);
	// 	}
    // }

	public static void PlayEffect(AudioEffectSO effectSO)
    {
		if(effectSO == null)
		{
			Debug.LogWarning("AudioManager: effectSO is null");
			return;
		}
		if(!IsSoundEffectsOptionEnable())  return;

		AudioClip clip = effectSO.GetAudioClip();
		if(clip == null)
		{
			Debug.LogWarning("AudioManager: audio clip is null");
			return;
		}
		if(numEffectSources>0)
        {
			s_effectsSources[nextEffectSource].pitch = effectSO.pitch;
			s_effectsSources[nextEffectSource].PlayOneShot(clip,effectSO.volume);
			nextEffectSource = (nextEffectSource + 1) % numEffectSources;
        }
		else if(numEffectSources<=0)
		{
			Debug.Log ("No hay AudioSources definidas para efectos");
		}
    }
	
	public static void PlayButtonForwardEffect()
	{
		if(Instance == null)
		{
			// Debug.LogWarning("No hay instancia de AudioManager");
			return;
		}
		PlayEffect(Instance.buttonClickForward);
	}
	public static void PlayButtonBackEffect()
	{
		if(Instance == null)
		{
			// Debug.LogWarning("No hay instancia de AudioManager");
			return;
		}
		PlayEffect(Instance.buttonClickBack);
	}

	
	private static bool IsMusicOptionEnable()
	{
		return true;
		// return DataManager.Music;
	}
	private static void SetMusicOptionEnable(bool status)
	{
		// DataManager.Music = status;
	}


	private static bool IsSoundEffectsOptionEnable()
	{
		return true;
		// return DataManager.SoundEffects;
	}
	private static void SetSoundEffectsOptionEnable(bool status)
	{
		// DataManager.SoundEffects = status;
	}
}
