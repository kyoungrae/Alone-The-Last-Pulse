using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab; // 발사할 총알 프리팹
    public Transform firePoint;     // 총알이 발사될 위치 (플레이어의 자식 오브젝트로 설정)
    public float fireRate = 0.1f;   // 발사 간격 (따발총처럼 빠르게)
    public float moveSpeed = 5f;    // 플레이어 이동 속도
    private float nextFireTime = 0f;

    void Update()
    {
        // 따발총 발사
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }

        // 플레이어 이동 (WASD 및 화살표 키)
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D 또는 좌/우 화살표
        float verticalInput = Input.GetAxis("Vertical");     // W/S 또는 상/하 화살표

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            Debug.LogError("Bullet Prefab or Fire Point is not assigned in PlayerController!");
        }
    }
}
