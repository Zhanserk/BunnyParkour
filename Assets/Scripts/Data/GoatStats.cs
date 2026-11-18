using UnityEngine;

// Данные козла. Все числа баланса лежат здесь, а не в коде.
[CreateAssetMenu(fileName = "GoatStats", menuName = "Bunny/Goat Stats")]
public class GoatStats : ScriptableObject
{
    public float range = 6f;            // м, с какого расстояния козёл замечает зайца
    public float windUp = 0.7f;         // с, красная полоса (предупреждение) перед рывком
    public float dashSpeed = 10f;       // м/с, скорость рывка
    public float dashTime = 0.8f;       // с, длительность рывка (10 м/с x 0.8 с = 8 м)
    public float stunTime = 1.5f;       // с, козёл оглушён после рывка
    public float hitDistance = 1.2f;    // м, на каком расстоянии рывок задевает зайца
}
