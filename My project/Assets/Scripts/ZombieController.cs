using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public float moveSpeed = 3f; // 좀비의 이동 속도
    private GameObject player; // 플레이어 오브젝트를 참조

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Player not found in the scene! Please tag your player GameObject as 'Player'.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            transform.Translate(directionToPlayer * moveSpeed * Time.deltaTime, Space.World);
            transform.LookAt(player.transform);
        }
    }
}
