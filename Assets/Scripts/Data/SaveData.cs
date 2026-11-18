using System;

// То, что хранится в save.json. JsonUtility умеет сохранять только public-поля и [Serializable]-классы.
[Serializable]
public class SaveData
{
    public int version = 1;                   // версия формата (если формат изменится, по ней поймём)
    public int unlockedLevel = 1;             // номер последнего открытого уровня (1..3)
    public float[] bestTimes = new float[3];  // с, лучшее время каждого уровня; 0 = рекорда ещё нет
    public float musicVolume = 0.8f;          // 0..1, громкость музыки
}
