using UnityEngine;

public class Needle : MonoBehaviour
{
    [SerializeField] float speed = 6f;   // м/с

    void Start()  { Destroy(gameObject, 3f); }                                        // через 3 с исчезнуть
    void Update() { transform.position += transform.forward * speed * Time.deltaTime; } // лететь вперёд
}