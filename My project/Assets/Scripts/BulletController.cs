using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 20f; // 총알의 속도
    public float bulletLifetime = 3f; // 총알이 사라지는 시간

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Rigidbody not found on Bullet. Bullet will not move physically.");
        }

        Destroy(gameObject, bulletLifetime);
    }
}
