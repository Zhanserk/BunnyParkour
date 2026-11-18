using UnityEngine;

// Данные одного уровня. Три ассета: Level_01, Level_02, Level_03 (папка Assets/Data).
// Один и тот же скрипт LevelManager берёт такой ассет и настраивает уровень.
[CreateAssetMenu(fileName = "Level_01", menuName = "Bunny/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelNumber = 1;                   // номер уровня (1..3)
    public string displayName = "LEVEL 1";        // надпись вверху экрана
    public string sceneName = "Level1";           // имя сцены (должно быть в Build Settings)
    public Vector3 spawnPosition = Vector3.zero;  // м (x, y, z), где появляется заяц
    public int carrotsGoal = 3;                   // шт., сколько морковок надо собрать, чтобы открыть портал
    public float parTime = 60f;                   // с, "хорошее" время прохождения (для сравнения с твоим)
}
