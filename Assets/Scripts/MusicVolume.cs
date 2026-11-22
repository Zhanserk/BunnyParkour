using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicVolume : MonoBehaviour
{
    AudioSource source;
    float baseVolume;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        baseVolume = source.volume;
        Apply();
    }

    public void Apply()
    {
        source.volume = baseVolume * SaveSystem.Data.musicVolume;
    }
}
