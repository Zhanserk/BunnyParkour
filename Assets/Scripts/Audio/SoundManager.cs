using UnityEngine;
using UnityEngine.Audio;

// Общие звуки, которые не должны обрываться при смене сцены (смерть, проигрыш, победа, клик),
// и громкость из save.json. Сидит на том же объекте AudioRoot, что и MusicManager.
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] AudioMixer mixer;          // GameMix
    [SerializeField] AudioSource sfxSource;     // эффекты (группа SFX)
    [SerializeField] AudioSource loseSource;    // мелодия проигрыша (группа SFX)
    [SerializeField] AudioSource uiSource;      // кнопки (группа UI)
    [SerializeField] AudioClip deathClip;
    [SerializeField] AudioClip loseClip;
    [SerializeField] AudioClip winClip;
    [SerializeField] AudioClip clickClip;
    [SerializeField] float loseDelay = 0.6f;    // с, проигрыш звучит после звука смерти

    void Awake()
    {
        Instance = this;
        mixer.updateMode = AudioMixerUpdateMode.UnscaledTime;   // переходы снапшотов работают на паузе
    }

    void Start()
    {
        ApplyVolumes();     // SetFloat работает начиная со Start, не из Awake
    }

    // Слайдер 0.0001..1 -> децибелы -80..0
    public static float ToDecibels(float value)
    {
        return Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
    }

    // Громкость музыки и эффектов из сохранения в микшер
    public void ApplyVolumes()
    {
        mixer.SetFloat("MusicVol", ToDecibels(SaveSystem.Data.musicVolume));
        mixer.SetFloat("SFXVol", ToDecibels(SaveSystem.Data.sfxVolume));
    }

    public void PlayDeath()
    {
        sfxSource.PlayOneShot(deathClip);
        loseSource.clip = loseClip;
        loseSource.PlayDelayed(loseDelay);   // сначала "умер", потом мелодия проигрыша
    }

    public void PlayWin() { sfxSource.PlayOneShot(winClip); }

    public void PlayClick() { uiSource.PlayOneShot(clickClip); }
}

// Несколько вариантов одного звука: случайный клип (не тот же два раза подряд)
// и чуть разная высота, чтобы звук не надоедал.
[System.Serializable]
public class RandomSound
{
    [SerializeField] AudioClip[] clips;
    [SerializeField] float minPitch = 0.92f;
    [SerializeField] float maxPitch = 1.08f;

    int last = -1;   // номер прошлого клипа

    public void Play(AudioSource source)
    {
        if (clips == null || clips.Length == 0) return;
        int i = Random.Range(0, clips.Length);
        if (clips.Length > 1 && i == last) i = (i + 1) % clips.Length;   // тот же подряд -> берём следующий
        last = i;
        source.pitch = Random.Range(minPitch, maxPitch);
        source.PlayOneShot(clips[i]);
    }
}
