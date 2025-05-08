using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    public LayerMask player; // �÷��̾� ����ũ

    private LivingEntity targetEntity;
    private NavMeshAgent navMeshAgent;

    public float timeBetAttack = 0.5f; // ���� ����
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
        // ���� Ȱ��ȭ�� ���ÿ� AI ���� ��ƾ ����
        StartCoroutine(UpdatePath());
    }

    void Update()
    {
        // ���� ����� ���� ���ο� ���� �ٸ� �ִϸ��̼� ���
    }

    // �ֱ������� �����ؾ� �� ����� ��ġ�� ã�� ��� ����
    IEnumerator UpdatePath()
    {
        // ����ִ� ���� ���� ����
        while (gameObject != null)
        {
            if (hasTarget)
            {
                // ���� ��� ����: ��θ� �����ϰ� AI �̵��� ��� ����
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);

                float distance = Vector3.Distance(transform.position, targetEntity.transform.position);

                if (distance < 0.3f)
                {
                    navMeshAgent.isStopped = true;
                }
            }
            else
            {
                // ���� ��� ����: AI �̵� ����
                navMeshAgent.isStopped = true;

                // 20 ������ �������� ���� ���� �׷��� �� ���� ��ġ�� ��� �ݶ��̴��� ������
                // ��, player ���̾ ���� �ݶ��̴��� ���������� ���͸�
                Collider[] colliders = Physics.OverlapSphere(transform.position, 50f, player);

                // ��� �ݶ��̴��� ��ȸ�ϸ� ����ִ� LivingEntity ã��
                for (int i = 0; i < colliders.Length; i++)
                {
                    // �ݶ��̴��κ��� LivingEntity ��������
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    // LivingEntity ������Ʈ�� �����Ѵٸ�
                    if (livingEntity != null)
                    {
                        // ���� ����� �ش� livingEntity�� ����
                        targetEntity = livingEntity;

                        // for ���� ��� ����
                        break;
                    }
                }
            }

            // 0.25�� �ֱ�� ó�� �ݺ�
            yield return new WaitForSeconds(0.25f);
        }
    }
}
