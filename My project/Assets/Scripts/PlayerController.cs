using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab; // 발사할 총알 프리팹
    public Transform firePoint;     // 총알이 발사될 위치 (플레이어의 자식 오브젝트로 설정)
    public ParticleSystem muzzleFlash; // 총구 화염 파티클 시스템
    public float fireRate = 0.1f;   // 연사 간격 (버튼을 꾹 누르고 있을 때 사용)
    public float moveSpeed = 5f;    // 플레이어 이동 속도
    public Joystick joystick;       // UI 조이스틱 참조

    // 발사 버튼을 꾹 누르고 있는지 여부
    private bool isFiring = false;
    private float nextFireTime = 0f;

    void Update()
    {
        // 발사 버튼을 꾹 누르고 있을 때 자동 연사
        if (isFiring && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }

        // 플레이어 이동 (조이스틱 + 키보드 입력)
        Vector2 joystickInput = joystick != null ? joystick.InputDirection : Vector2.zero;

        // Unity 기본 입력축: Horizontal/Vertical → WASD + 방향키 모두 지원
        float keyboardHorizontal = Input.GetAxisRaw("Horizontal");
        float keyboardVertical   = Input.GetAxisRaw("Vertical");

        // 이동 방향 합산
        Vector3 moveDirection = Vector3.zero;

        // 조이스틱 입력
        if (joystickInput.sqrMagnitude > 0.0001f)
        {
            // 조이스틱 방향을 월드 좌표계로 변환하여 이동
            Vector3 worldMoveDirection = new Vector3(joystickInput.x, 0f, joystickInput.y);
            moveDirection += worldMoveDirection;
        }

        // 키보드 입력 (WASD / 방향키)
        if (Mathf.Abs(keyboardHorizontal) > 0.01f || Mathf.Abs(keyboardVertical) > 0.01f)
        {
            moveDirection += transform.right * keyboardHorizontal + transform.forward * keyboardVertical;
        }

        // 실제 이동 및 회전
        if (moveDirection.sqrMagnitude > 0.0001f)
        {
            Vector3 move = moveDirection.normalized * moveSpeed * Time.deltaTime;
            transform.position += move;
        }

        // 회전 방향 결정:
        // 1순위: 조이스틱 방향
        // 2순위: 키보드 이동 방향(단, 뒤로가는 입력은 회전에 반영하지 않음)
        Vector3 finalLookDir = Vector3.zero;

        // 1) 조이스틱 입력이 있으면, 그 방향으로만 회전
        if (joystickInput.sqrMagnitude > 0.0001f)
        {
            finalLookDir = new Vector3(joystickInput.x, 0f, joystickInput.y).normalized;
        }
        // 2) 조이스틱이 없을 때, 좌/우/앞으로 이동한 방향에 맞춰 회전
        else
        {
            // 뒤로가는 입력은 0으로 만들어서 회전에 반영하지 않음 (앞/좌/우만 사용)
            float forwardOnly = Mathf.Max(0f, keyboardVertical);
            if (Mathf.Abs(keyboardHorizontal) > 0.01f || forwardOnly > 0.01f)
            {
                Vector3 keyboardDir = transform.right * keyboardHorizontal + transform.forward * forwardOnly;
                finalLookDir = new Vector3(keyboardDir.x, 0f, keyboardDir.z).normalized;
            }
        }

        if (finalLookDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(finalLookDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 3f);
        }
    }

    // 발사 버튼 OnClick 에서 직접 호출할 단발 발사 함수
    public void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Debug.Log("Bullet fired!");
            
            // 총구 화염 효과 재생
            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
                Debug.Log("Muzzle flash played!");
            }
            else
            {
                Debug.LogWarning("Muzzle Flash is not assigned!");
            }
        }
        else
        {
            Debug.LogError("Bullet Prefab or Fire Point is not assigned in PlayerController!");
        }
    }

    // ===== 발사 버튼 EventTrigger용 메서드 =====

    // 버튼을 누를 때 (PointerDown)
    public void StartFiring()
    {
        isFiring = true;
        // 이미 한 발 쏜 직후라면 fireRate 이후부터 다시 쏘도록 설정
        nextFireTime = Time.time + fireRate;
    }

    // 버튼에서 손을 뗄 때 (PointerUp)
    public void StopFiring()
    {
        isFiring = false;
    }

}
