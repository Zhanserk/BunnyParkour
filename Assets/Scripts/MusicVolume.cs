using UnityEngine;

// Вешается на объект Music в каждой сцене. Громкость музыки = громкость из сцены x настройка игрока из save.json.
[RequireComponent(typeof(AudioSource))]
public class MusicVolume : MonoBehaviour
{
    AudioSource source;     // динамик с музыкой
    float baseVolume;       // громкость, которая стоит в сцене (у каждой сцены своя)

    void Awake()
    {
        source = GetComponent<AudioSource>();
        baseVolume = source.volume;     // запоминаем громкость из сцены
        Apply();
    }

    // Применить настройку игрока. Меню вызывает это при движении слайдера.
    public void Apply()
    {
        source.volume = baseVolume * SaveSystem.Data.musicVolume;
    }
}
