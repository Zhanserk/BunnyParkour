using System;
using System.IO;
using UnityEngine;

// Сохранение прогресса в JSON-файл. Статический класс: его не нужно вешать на объект,
// достаточно написать SaveSystem.Data или SaveSystem.Save() из любого скрипта.
public static class SaveSystem
{
    const int LevelCount = 3;       // сколько уровней в игре
    const float DefaultVolume = 0.8f;   // громкость музыки по умолчанию

    static SaveData _data;          // то, что сейчас лежит в памяти

    // Путь к файлу. У каждой платформы (Windows, iPhone, Android) своя папка, Unity подбирает её сама.
    static string FilePath => Application.persistentDataPath + "/save.json";

    // Данные в памяти. Если их ещё не загружали, загружаем сейчас.
    public static SaveData Data
    {
        get
        {
            if (_data == null) Load();
            return _data;
        }
    }

    // Читает save.json. Что бы ни случилось с файлом, игра не падает: берём значения по умолчанию.
    public static void Load()
    {
        string path = FilePath;
        Debug.Log("Save file path: " + path);   // чтобы можно было найти файл

        SaveData loaded = null;
        if (!File.Exists(path))
        {
            Debug.Log("No save file yet, using default values.");   // первый запуск
        }
        else
        {
            try
            {
                string json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                    Debug.LogWarning("Save file is empty, using default values.");   // пустой файл
                else
                    loaded = JsonUtility.FromJson<SaveData>(json);   // для сломанного текста бросит исключение
            }
            catch (Exception e)
            {
                Debug.LogWarning("Save file is broken, using default values. " + e.Message);   // сломанный файл
                loaded = null;
            }
        }

        _data = loaded ?? new SaveData();   // нет данных -> значения по умолчанию
        Fix(_data);                         // чиним то, что могло быть неверным внутри файла
    }

    // Пишет данные в save.json (красиво, с отступами, чтобы можно было открыть в блокноте).
    public static void Save()
    {
        try
        {
            string json = JsonUtility.ToJson(Data, true);
            File.WriteAllText(FilePath, json);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Could not write save file: " + e.Message);   // не падаем, просто сообщаем
        }
    }

    // Сброс прогресса: открыт только 1 уровень, рекордов нет. Громкость музыки не трогаем (это настройка, не прогресс).
    public static void ResetProgress()
    {
        Data.unlockedLevel = 1;
        Data.bestTimes = new float[LevelCount];
        Save();
    }

    // Проверка и починка данных после чтения файла.
    static void Fix(SaveData d)
    {
        // массив рекордов должен быть ровно из LevelCount чисел
        if (d.bestTimes == null || d.bestTimes.Length != LevelCount)
        {
            Debug.LogWarning("bestTimes has a wrong size, fixing it.");
            float[] fixedTimes = new float[LevelCount];
            if (d.bestTimes != null)
                for (int i = 0; i < Math.Min(d.bestTimes.Length, LevelCount); i++)
                    fixedTimes[i] = d.bestTimes[i];   // что можно, переносим
            d.bestTimes = fixedTimes;
        }

        // время не может быть отрицательным или NaN
        for (int i = 0; i < d.bestTimes.Length; i++)
            if (float.IsNaN(d.bestTimes[i]) || d.bestTimes[i] < 0f) d.bestTimes[i] = 0f;

        d.unlockedLevel = Mathf.Clamp(d.unlockedLevel, 1, LevelCount);   // открыт от 1 до 3 уровня

        if (float.IsNaN(d.musicVolume)) d.musicVolume = DefaultVolume;
        d.musicVolume = Mathf.Clamp01(d.musicVolume);                    // громкость от 0 до 1
    }
}
