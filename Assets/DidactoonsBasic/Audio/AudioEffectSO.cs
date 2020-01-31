using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Didactoons/Audio/AudioEffect", fileName = "AudioEffect")]
public class AudioEffectSO : ScriptableObject
{
    public AudioClip[] audioClips;

    AudioSource _previewer;
    public float volume = 1f;
    public float pitch = 1f;

    public AudioClip GetAudioClip()
    {
        if(audioClips == null || audioClips.Length == 0)
        {
            Debug.LogWarning("Audio effect does not have audio clip: "+this.name);
            return null;
        }
        return audioClips[Random.Range(0,audioClips.Length)];
    }

#if UNITY_EDITOR
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button]
#else
     [DebugButton]
#endif
    public void PreviewSound()
    {
        AudioClip clip = GetAudioClip();
        if(clip != null)
        {
            if(_previewer == null)
            {
                _previewer = UnityEditor.EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
            }

            _previewer.volume = volume;
            _previewer.pitch = pitch;
            _previewer.clip = clip;
            _previewer.Play();
        }
    }
    void OnDisable()
    {
        if(_previewer != null)
        {
            DestroyImmediate(_previewer.gameObject);
        }
    }

#endif
}
