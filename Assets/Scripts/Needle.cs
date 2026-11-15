using UnityEngine;

public class Needle : MonoBehaviour
{
    [SerializeField] float speed = 6f;
    [SerializeField] float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject , lifetime);

    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
