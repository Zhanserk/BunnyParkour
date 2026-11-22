using UnityEngine;

[CreateAssetMenu(fileName = "FoxStats", menuName = "Bunny/Fox Stats")]
public class FoxStats : ScriptableObject
{
    public float viewRange = 7f;        
    public float fov = 90f;             
    public float loseTime = 2f;         
    public float attackRange = 1.2f;  
    public float windUp = 0.5f;       
    public float restTime = 0.8f;      
    public float arriveDistance = 0.3f; 
    public float biteExtraRange = 0.3f; 
    public float eyeHeight = 1f;      
}
