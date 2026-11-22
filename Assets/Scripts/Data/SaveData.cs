using System;

[Serializable]
public class SaveData
{
    public int version = 1;
    public int unlockedLevel = 1;
    public float[] bestTimes = new float[3];
    public float musicVolume = 0.8f;
}
