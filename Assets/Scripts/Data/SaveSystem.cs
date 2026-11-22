using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    const int LevelCount = 3;
    const float DefaultVolume = 0.8f;

    static SaveData _data;

    static string FilePath => Application.persistentDataPath + "/save.json";

    public static SaveData Data
    {
        get
        {
            if (_data == null) Load();
            return _data;
        }
    }

    public static void Load()
    {
        string path = FilePath;
        Debug.Log("Save file path: " + path);

        SaveData loaded = null;
        if (!File.Exists(path))
        {
            Debug.Log("No save file yet, using default values.");
        }
        else
        {
            try
            {
                string json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                    Debug.LogWarning("Save file is empty, using default values.");
                else
                    loaded = JsonUtility.FromJson<SaveData>(json);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Save file is broken, using default values. " + e.Message);
                loaded = null;
            }
        }

        _data = loaded ?? new SaveData();
        Fix(_data);
    }

    public static void Save()
    {
        try
        {
            string json = JsonUtility.ToJson(Data, true);
            File.WriteAllText(FilePath, json);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Could not write save file: " + e.Message);
        }
    }

    public static void ResetProgress()
    {
        Data.unlockedLevel = 1;
        Data.bestTimes = new float[LevelCount];
        Save();
    }

    static void Fix(SaveData d)
    {
        if (d.bestTimes == null || d.bestTimes.Length != LevelCount)
        {
            Debug.LogWarning("bestTimes has a wrong size, fixing it.");
            float[] fixedTimes = new float[LevelCount];
            if (d.bestTimes != null)
                for (int i = 0; i < Math.Min(d.bestTimes.Length, LevelCount); i++)
                    fixedTimes[i] = d.bestTimes[i];
            d.bestTimes = fixedTimes;
        }

        for (int i = 0; i < d.bestTimes.Length; i++)
            if (float.IsNaN(d.bestTimes[i]) || d.bestTimes[i] < 0f) d.bestTimes[i] = 0f;

        d.unlockedLevel = Mathf.Clamp(d.unlockedLevel, 1, LevelCount);

        if (float.IsNaN(d.musicVolume)) d.musicVolume = DefaultVolume;
        d.musicVolume = Mathf.Clamp01(d.musicVolume);
    }
}
