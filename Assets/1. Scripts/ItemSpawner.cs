using UnityEngine;
using UnityEngine.AI;

// �� ��ũ��Ʈ�� �÷��̾� �ֺ����� ���� �ð� �������� �������� �����մϴ�.
// ����޽ø� Ȱ���Ͽ� �������� ������̼� ������ ��ġ���� �����ǵ��� �մϴ�.
// �������� ���� ��ġ�� �ʵ��� ���� ��ġ�� �˻��մϴ�.

public class ItemSpawner : MonoBehaviour
{
    public GameObject items; // ������ ������
    public Transform playerTransform; // �÷��̾��� Ʈ������

    public float maxDistance = 5f; // �÷��̾� ��ġ���� �������� ��ġ�� �ִ� �ݰ�
    public float checkRadius = 1.0f; // ������ �� �ּ� �Ÿ�
    public int maxAttempts = 10; // ��ġ ã�� �ִ� �õ� Ƚ��

    public float timeBetSpawnMax = 3f; // �ִ� �ð� ����
    public float timeBetSpawnMin = 1f; // �ּ� �ð� ����
    private float timeBetSpawn; // ���� ����
    private float lastSpawnTime; // ������ ���� ����

    void Start()
    {
        // ���� ���ݰ� ������ ���� ���� �ʱ�ȭ
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0f;
    }

    // �ֱ������� ������ ���� ó�� ����
    void Update()
    {
        // ���� ������ ������ ���� �������� ���� �ֱ� �̻� ����
        // && �÷��̾� ĳ���Ͱ� ������
        if (Time.time >= lastSpawnTime + timeBetSpawn && playerTransform != null)
        {
            // ������ ���� �ð� ����
            lastSpawnTime = Time.time;
            // ���� �ֱ⸦ �������� ����
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            // ������ ���� ����
            Spawn();
        }
    }

    void Spawn()
    {
        // �ִ� maxAttempts�� �õ��ؼ� ��ġ�� �ʴ� ��ġ�� ã��
        for (int i = 0; i < maxAttempts; i++)
        {
            // �÷��̾� ��ó���� ����޽� ���� ���� ��ġ ��������
            Vector3 spawnPosition = GetRandomPointOnNavMesh(playerTransform.position, maxDistance);
            // �ٴڿ��� 0.5��ŭ ���� �ø���
            spawnPosition += Vector3.up * 0.05f;

            // �ش� ��ġ �ֺ��� Coin ��ũ��Ʈ�� ���� �������� �ִ��� �˻�
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

            // ��ġ�� �ʴ´ٸ� ������ ����
            if (!isOccupied)
            {
                GameObject item = Instantiate(items, spawnPosition, Quaternion.identity);
                Destroy(item, 5f); // ������ �������� 5�� �ڿ� �ı�
                return;
            }
        }
    }

    // ����޽� ���� ������ ��ġ�� ��ȯ�ϴ� �޼���
    // center�� �߽����� distance �ݰ� �ȿ����� ������ ��ġ�� ã��
    Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        // center�� �߽����� �������� maxDistance�� �� �ȿ����� ������ ��ġ �ϳ��� ����
        // Random.insideUnitSphere�� �������� 1�� �� �ȿ����� ������ �� ���� ��ȯ�ϴ� ������Ƽ
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        // ����޽� ���ø��� ��� ������ �����ϴ� ����
        NavMeshHit hit;
        // maxDistance �ݰ� �ȿ��� randomPos�� ���� ����� ����޽� ���� �� ���� ã��
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

        // ã�� �� ��ȯ
        return hit.position;
    }
}
