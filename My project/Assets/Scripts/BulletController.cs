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

    // 다른 오브젝트와 충돌했을 때 호출되는 함수
    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트에서 HealthBar 컴포넌트 찾기
        HealthBar healthBar = collision.gameObject.GetComponent<HealthBar>();
        if (healthBar != null)
        {
            // 좀비에게 데미지 주기 (예: 20 데미지)
            healthBar.TakeDamage(20f);
        }

        // 총알은 충돌 후 파괴
        Destroy(gameObject);
    }
}
