using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab; // 발사할 총알 프리팹
    public Transform firePoint;     // 총알이 발사될 위치 (플레이어의 자식 오브젝트로 설정)
    public float fireRate = 0.1f;   // 발사 간격 (따발총처럼 빠르게)
    public float moveSpeed = 5f;    // 플레이어 이동 속도
    public Joystick joystick; // UI 조이스틱 참조
    private float nextFireTime = 0f;

    void Update()
    {
        // 따발총 발사 (마우스 클릭)
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }

        // 플레이어 이동 (조이스틱 입력)
        if (joystick != null)
        {
            float horizontalInput = joystick.InputDirection.x;
            float verticalInput = joystick.InputDirection.y;

            Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;
            Vector3 newPosition = transform.position + move.normalized * moveSpeed * Time.deltaTime;
            transform.position = newPosition;

            // 조이스틱 입력 방향에 따라 플레이어 회전
            if (horizontalInput != 0 || verticalInput != 0)
            {
                Vector3 directionToLook = new Vector3(joystick.InputDirection.x, 0, joystick.InputDirection.y).normalized;
                if (directionToLook != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(directionToLook, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f); // 회전 속도 조절
                }
            }
        }
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
