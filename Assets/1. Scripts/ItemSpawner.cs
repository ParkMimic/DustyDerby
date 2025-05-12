using UnityEngine;
using UnityEngine.AI;

// 이 스크립트는 플레이어 주변에서 일정 시간 간격으로 아이템을 생성합니다.
// 내비메시를 활용하여 아이템이 내비게이션 가능한 위치에만 생성되도록 합니다.
// 아이템은 서로 겹치지 않도록 생성 위치를 검사합니다.

public class ItemSpawner : MonoBehaviour
{
    public GameObject items; // 생성할 아이템
    public Transform playerTransform; // 플레이어의 트랜스폼

    public float maxDistance = 5f; // 플레이어 위치에서 아이템이 배치될 최대 반경
    public float checkRadius = 1.0f; // 아이템 간 최소 거리
    public int maxAttempts = 10; // 위치 찾기 최대 시도 횟수

    public float timeBetSpawnMax = 3f; // 최대 시간 간격
    public float timeBetSpawnMin = 1f; // 최소 시간 간격
    private float timeBetSpawn; // 생성 간격
    private float lastSpawnTime; // 마지막 생성 지점

    void Start()
    {
        // 생성 간격과 마지막 생성 지점 초기화
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0f;
    }

    // 주기적으로 아이템 생성 처리 실행
    void Update()
    {
        // 현재 시점이 마지막 생성 시점에서 생성 주기 이상 지남
        // && 플레이어 캐릭터가 존재함
        if (Time.time >= lastSpawnTime + timeBetSpawn && playerTransform != null)
        {
            // 마지막 생성 시간 갱신
            lastSpawnTime = Time.time;
            // 생성 주기를 랜덤으로 변경
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            // 아이템 생성 실행
            Spawn();
        }
    }

    void Spawn()
    {
        // 최대 maxAttempts번 시도해서 겹치지 않는 위치를 찾음
        for (int i = 0; i < maxAttempts; i++)
        {
            // 플레이어 근처에서 내비메시 위의 랜덤 위치 가져오기
            Vector3 spawnPosition = GetRandomPointOnNavMesh(playerTransform.position, maxDistance);
            // 바닥에서 0.5만큼 위로 올리기
            spawnPosition += Vector3.up * 0.05f;

            // 해당 위치 주변에 Coin 스크립트를 가진 아이템이 있는지 검사
            Collider[] colliders = Physics.OverlapSphere(spawnPosition, checkRadius);
            bool isOccupied = false;

            foreach (var col in colliders)
            {
                if (col.GetComponent<Coin>() != null)
                {
                    isOccupied = true;
                    break;
                }
            }

            // 겹치지 않는다면 아이템 생성
            if (!isOccupied)
            {
                GameObject item = Instantiate(items, spawnPosition, Quaternion.identity);
                Destroy(item, 5f); // 생성된 아이템을 5초 뒤에 파괴
                return;
            }
        }
    }

    // 내비메시 위의 랜덤한 위치를 반환하는 메서드
    // center를 중심으로 distance 반경 안에서의 랜덤한 위치를 찾음
    Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        // center를 중심으로 반지름이 maxDistance인 구 안에서의 랜덤한 위치 하나를 저장
        // Random.insideUnitSphere는 반지름이 1인 구 안에서의 랜덤한 한 점을 반환하는 프로퍼티
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        // 내비메시 샘플링의 결과 정보를 저장하는 변수
        NavMeshHit hit;
        // maxDistance 반경 안에서 randomPos에 가장 가까운 내비메시 위의 한 점을 찾음
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

        // 찾은 점 반환
        return hit.position;
    }
}
