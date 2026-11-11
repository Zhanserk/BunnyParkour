using UnityEngine;

// Иголка ежа: летит вперёд, исчезает через время или при попадании в зайца
public class Needle : MonoBehaviour
{
    [SerializeField] float speed = 6f;      // м/с
    [SerializeField] float lifeTime = 3f;   // с

    void Start()
    {
        Destroy(gameObject, lifeTime);      // удалить себя через 3 секунды
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;  // лечу вперёд
    }

    void OnTriggerEnter(Collider other)
    {
        // урон делает PlayerController по тегу Hazard, иголке остаётся только исчезнуть
        if (other.CompareTag("Player")) Destroy(gameObject);
    }
}