using UnityEngine;

public class Needle : MonoBehaviour
{
    [SerializeField] HedgehogStats stats;   // Assignment 11: скорость и время жизни иголки теперь в ассете HedgehogStats

    void Start()  { Destroy(gameObject, stats.needleLifetime); }                                  // через needleLifetime с исчезнуть
    void Update() { transform.position += transform.forward * stats.needleSpeed * Time.deltaTime; } // лететь вперёд
}
