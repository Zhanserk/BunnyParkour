using UnityEngine;

// Сторож сохранений. Один на всю игру: создаётся сам перед загрузкой первой сцены
// и не уничтожается при смене сцен (DontDestroyOnLoad). Вешать на объект руками не нужно.
public class SaveManager : MonoBehaviour
{
    static SaveManager _instance;   // чтобы не создать второй

    // Unity вызывает этот метод сам, до загрузки первой сцены
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Create()
    {
        if (_instance != null) return;                       // уже есть -> второй не нужен
        GameObject go = new GameObject("SaveManager");
        _instance = go.AddComponent<SaveManager>();
        DontDestroyOnLoad(go);                               // живёт между сценами
    }

    void Awake()
    {
        SaveSystem.Load();   // читаем save.json (и пишем путь к нему в Console)
    }

    // На телефоне игру сворачивают, а потом система может её закрыть без предупреждения.
    // Поэтому при сворачивании сохраняем.
    void OnApplicationPause(bool paused)
    {
        if (paused) SaveSystem.Save();
    }

    // Обычный выход (и Stop в редакторе Unity)
    void OnApplicationQuit()
    {
        SaveSystem.Save();
    }
}
