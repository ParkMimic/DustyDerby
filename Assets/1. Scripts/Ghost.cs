using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    public LayerMask player; // �÷��̾� ����ũ

    private LivingEntity targetEntity;
    private NavMeshAgent navMeshAgent;

    public float timeBetAttack = 10f; // ���� ����
    private float lastAttackTime; // ������ ���� ����

    // ������ ����� ������ �˷��ִ� ������Ʈ
    private bool hasTarget
    {
        get
        {
            // ������ ����� �����Ѵٸ� true
            if (targetEntity != null)
            {
                return true;
            }

            // �׷��� �ʴٸ� false�� ��ȯ
            return false;
        }
    }

    private void Awake()
    {
        // �ʱ�ȭ
        // �ʿ��� ������Ʈ ��������
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void SetUp(EnemyData enemyData)
    {
        navMeshAgent.speed = enemyData.speed;
    }

    void Start()
    {
        timeBetAttack = 0f; // ���� �ʱ�ȭ
        // ���� Ȱ��ȭ�� ���ÿ� AI ���� ��ƾ ����
        StartCoroutine(UpdatePath());
    }

    // �ֱ������� �����ؾ� �� ����� ��ġ�� ã�� ��� ����
    IEnumerator UpdatePath()
    {
        // ����ִ� ���� ���� ����
        while (gameObject != null)
        {
            if (hasTarget)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);

                float distance = Mathf.Abs(transform.position.x - targetEntity.transform.position.x);

                if (distance < 0.2f)
                {
                    navMeshAgent.isStopped = true;
                    navMeshAgent.velocity = Vector3.zero;
                    
                }
            }
            else
            {
                navMeshAgent.isStopped = true;

                Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, player);
                for (int i = 0; i < colliders.Length; i++)
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                    if (livingEntity != null)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }

            // 0.25�� �ֱ�� ó�� �ݺ�
            yield return new WaitForSeconds(0.25f);
        }
    }

    // �浹�� Ʈ���� Ȯ��
    public void OnTriggerEnter(Collider other)
    {
        // �浹�� �ݶ��̴��� �±װ� �÷��̾���
        if (other.tag == "Player")
        {
            // LivingEntity �� ������
            LivingEntity player = other.GetComponent<LivingEntity>();
            // ������ ���� �������κ��� �ð��� �����ٸ�
            if (lastAttackTime + timeBetAttack < Time.time)
            {
                // �÷��̾�� ������ ����
                player.Stun();
                // ���� ������ ���� �ð� ����
                lastAttackTime = Time.time;
            }
        }
    }
}
