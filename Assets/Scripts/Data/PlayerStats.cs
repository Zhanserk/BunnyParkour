using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Bunny/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public float moveSpeed = 6f;
    public float jumpSpeed = 8f;
    public int maxHp = 3;
    public int hurtDamage = 1;
    public int healAmount = 1;
    public int blinkCount = 10;
    public float blinkInterval = 0.2f;
    public float fallDeathY = -3f;
}
