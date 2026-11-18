using UnityEngine;

// Данные ежа и его иголки. Все числа баланса лежат здесь, а не в коде.
[CreateAssetMenu(fileName = "HedgehogStats", menuName = "Bunny/Hedgehog Stats")]
public class HedgehogStats : ScriptableObject
{
    public float range = 10f;           // м, с какого расстояния ёж стреляет
    public float aimTime = 0.8f;        // с, ёж целится (лазер) перед выстрелом
    public float cooldown = 1.5f;       // с, отдых после выстрела
    public float needleSpeed = 6f;      // м/с, скорость полёта иголки
    public float needleLifetime = 3f;   // с, через сколько иголка исчезает
}
