using UnityEngine;

// Данные лисы. Все числа баланса лежат здесь, а не в коде.
[CreateAssetMenu(fileName = "FoxStats", menuName = "Bunny/Fox Stats")]
public class FoxStats : ScriptableObject
{
    public float viewRange = 7f;        // м, как далеко лиса видит зайца
    public float fov = 90f;             // градусы, угол обзора (на всю ширину)
    public float loseTime = 2f;         // с, сколько лиса не видит зайца, прежде чем вернуться на маршрут
    public float attackRange = 1.2f;    // м, дистанция, с которой лиса начинает кусать
    public float windUp = 0.5f;         // с, лиса рычит перед укусом (предупреждение)
    public float restTime = 0.8f;       // с, отдых после укуса
    public float arriveDistance = 0.3f; // м, считаем, что лиса дошла до флажка маршрута
    public float biteExtraRange = 0.3f; // м, запас: укус достанет, если заяц отошёл не дальше attackRange + это
    public float eyeHeight = 1f;        // м, высота глаз над землёй (откуда лиса смотрит)
}
