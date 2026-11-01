using UnityEngine;

// Вешается на ПЛАТФОРМУ. Двигает платформу Animation Clip (через Animator),
// а этот скрипт только делает кролика "ребёнком" платформы, чтобы он ехал вместе с ней.
public class MovingPlatform : MonoBehaviour
{
    // кролик коснулся платформы
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // кролик становится ребёнком платформы → едет вместе с ней
            collision.transform.SetParent(transform);
        }
    }

    // кролик ушёл с платформы
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // у кролика больше нет родителя → снова двигается сам по себе
            collision.transform.SetParent(null);
        }
    }
}