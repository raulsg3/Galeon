using UnityEngine;

[CreateAssetMenu(menuName = "Didactoons/Audio/AudioMusic", fileName = "AudioMusic")]
public class AudioMusicSO : ScriptableObject
{
    public AudioClip audioClip;

    public AudioManager.PossibleMusics possibleMusic;

    public float volume = 1f;
    public float pitch = 1f;

}
