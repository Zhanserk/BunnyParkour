using System.Collections;
using UnityEngine;

// Музыка: один на всю игру, не уничтожается при смене сцен.
// Два AudioSource: один затихает, другой нарастает (кроссфейд).
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }   // доступ из любого скрипта

    [SerializeField] AudioSource sourceA;      // первый "проигрыватель" (группа Tracks)
    [SerializeField] AudioSource sourceB;      // второй "проигрыватель" (группа Tracks)
    [SerializeField] float fadeTime = 1f;      // с, длительность кроссфейда

    AudioSource current;    // кто сейчас играет
    AudioSource other;      // кто свободен
    Coroutine fade;         // идущий сейчас кроссфейд

    // Unity вызывает сам до первой сцены: создаём AudioRoot из папки Resources
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Create()
    {
        if (Instance != null) return;                                  // уже есть
        GameObject root = Instantiate(Resources.Load<GameObject>("AudioRoot"));
        root.name = "AudioRoot";
        DontDestroyOnLoad(root);                                       // живёт между сценами
    }

    void Awake()
    {
        Instance = this;
        current = sourceA;
        other = sourceB;
    }

    // Включить трек. Тот же самый трек заново не запускается.
    public void Play(AudioClip clip)
    {
        if (clip == null) return;
        if (current.clip == clip && current.isPlaying) return;   // уже играет этот трек

        if (fade != null) StopCoroutine(fade);                   // прерываем старый переход

        AudioSource from = current;
        AudioSource to = other;
        to.clip = clip;
        to.volume = 0f;
        to.Play();
        current = to;
        other = from;
        fade = StartCoroutine(CrossFade(from, to));
    }

    IEnumerator CrossFade(AudioSource from, AudioSource to)
    {
        float startVolume = from.volume;
        for (float t = 0f; t < fadeTime; t += Time.unscaledDeltaTime)   // unscaled: работает и на паузе
        {
            float k = t / fadeTime;               // от 0 до 1
            to.volume = k;                        // новый нарастает
            from.volume = startVolume * (1f - k); // старый затихает
            yield return null;
        }
        to.volume = 1f;
        from.volume = 0f;
        from.Stop();
        fade = null;
    }
}
