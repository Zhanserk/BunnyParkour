using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   public Transform target;

   public Vector3 offset = new Vector3(0 , 6 , -8);

   public float smooth = 5f;





    void LateUpdate()
    {
        Vector3 wanted = target.position + offset;

        transform.position  = Vector3.Lerp(transform.position , wanted , smooth * Time.deltaTime);



    }





}
