using UnityEngine;

[CreateAssetMenu(fileName = "GoatStats", menuName = "Bunny/Goat Stats")]
public class GoatStats : ScriptableObject
{
    public float range = 6f;            
    public float windUp = 0.7f;         
    public float dashSpeed = 10f;       
    public float dashTime = 0.8f;
    public float stunTime = 1.5f;
    public float hitDistance = 1.2f;
}
