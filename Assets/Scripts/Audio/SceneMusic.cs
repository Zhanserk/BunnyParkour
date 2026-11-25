using UnityEngine;

// Лежит в каждой сцене и говорит MusicManager, какой трек играть здесь.
public class SceneMusic : MonoBehaviour
{
    [SerializeField] AudioClip track;   // music_menu или music_level

    void Start()
    {
        if (MusicManager.Instance != null) MusicManager.Instance.Play(track);
    }
}
