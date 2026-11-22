using UnityEngine;

[CreateAssetMenu(fileName = "Level_01", menuName = "Bunny/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelNumber = 1;
    public string displayName = "LEVEL 1";
    public string sceneName = "Level1";
    public Vector3 spawnPosition = Vector3.zero;
    public int carrotsGoal = 3;
    public float parTime = 60f;
}
