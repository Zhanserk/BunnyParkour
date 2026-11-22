using UnityEngine;

[CreateAssetMenu(fileName = "HedgehogStats", menuName = "Bunny/Hedgehog Stats")]
public class HedgehogStats : ScriptableObject
{
    public float range = 10f;           
    public float aimTime = 0.8f;
    public float cooldown = 0.3f;       
    public float needleSpeed = 6f;
    public float needleLifetime = 3f;   
}
