using UnityEngine;

public class SuppressCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Destroyable")
        {
            Destroy(collision.gameObject);
        }
    }
}
