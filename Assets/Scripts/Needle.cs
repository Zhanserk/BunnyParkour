using UnityEngine;

public class Needle : MonoBehaviour
{
    [SerializeField] HedgehogStats stats;

    void Start()  { Destroy(gameObject, stats.needleLifetime); }
    void Update() { transform.position += transform.forward * stats.needleSpeed * Time.deltaTime; }
}
