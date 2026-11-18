using UnityEngine;

// Данные героя (зайца). Все числа баланса лежат здесь, а не в коде.
// Чтобы создать ещё один набор: правая кнопка в Project -> Create -> Bunny -> Player Stats.
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Bunny/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public float moveSpeed = 6f;        // м/с, скорость бега
    public float jumpSpeed = 8f;        // м/с, вертикальная скорость в момент прыжка
    public int maxHp = 3;               // шт., максимум жизней
    public int hurtDamage = 1;          // шт., сколько жизней отнимает один удар
    public int healAmount = 1;          // шт., сколько жизней даёт сердечко
    public int blinkCount = 10;         // раз, сколько раз заяц мигает после удара
    public float blinkInterval = 0.2f;  // с, пауза между миганиями (10 x 0.2 = 2 с неуязвимости)
    public float fallDeathY = -3f;      // м, ниже этой высоты заяц падает и умирает
}
