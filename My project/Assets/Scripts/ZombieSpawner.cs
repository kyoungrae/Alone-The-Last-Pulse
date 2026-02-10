using UnityEngine;

/// <summary>
/// 플레이어를 중심으로 일정 반경 바깥에서 좀비를 랜덤하게 소환해 주는 스크립트.
/// </summary>
public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject zombiePrefab;      // 소환할 좀비 프리팹
    public Transform player;             // 플레이어 Transform (없으면 Start에서 Player 태그로 찾음)

    public float spawnRadius = 20f;      // 플레이어를 기준으로 소환되는 거리
    public float minSpawnInterval = 1.5f; // 최소 소환 간격
    public float maxSpawnInterval = 3.0f; // 최대 소환 간격

    public int maxZombies = 50;          // 동시에 존재할 수 있는 최대 좀비 수

    private float nextSpawnTime = 0f;
    private int currentZombieCount = 0;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("ZombieSpawner: Player not found. Please assign the player Transform or tag the player as 'Player'.");
            }
        }

        ScheduleNextSpawn();
    }

    void Update()
    {
        if (zombiePrefab == null || player == null)
        {
            return;
        }

        if (Time.time >= nextSpawnTime && currentZombieCount < maxZombies)
        {
            SpawnZombie();
            ScheduleNextSpawn();
        }
    }

    private void ScheduleNextSpawn()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void SpawnZombie()
    {
        // 플레이어를 중심으로 랜덤한 방향 선정 (XZ 평면)
        Vector2 randomDir2D = Random.insideUnitCircle.normalized;
        Vector3 spawnOffset = new Vector3(randomDir2D.x, 0f, randomDir2D.y) * spawnRadius;

        Vector3 spawnPos = player.position + spawnOffset;

        GameObject zombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);

        currentZombieCount++;

        // 좀비가 파괴될 때 카운트를 줄이도록 콜백용 컴포넌트 추가
        var tracker = zombie.AddComponent<ZombieLifeTracker>();
        tracker.Init(this);
    }

    // 좀비가 죽었을 때 호출되어 카운트를 줄이는 용도
    public void OnZombieDestroyed()
    {
        currentZombieCount = Mathf.Max(0, currentZombieCount - 1);
    }
}

/// <summary>
/// 파괴될 때 ZombieSpawner에 알려서 카운트를 감소시키는 보조 컴포넌트.
/// </summary>
public class ZombieLifeTracker : MonoBehaviour
{
    private ZombieSpawner spawner;

    public void Init(ZombieSpawner s)
    {
        spawner = s;
    }

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnZombieDestroyed();
        }
    }
}

