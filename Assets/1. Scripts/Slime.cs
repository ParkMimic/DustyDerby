using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Slime : LivingEntity
{
    public LayerMask player; // �÷��̾� ����ũ
    public float maxChaseDistance = 1f; // �ִ� ���� �Ÿ�

    public AudioClip hitClip; // �´� ����� Ŭ��
    private AudioSource audio; // ����� �ҽ� ��� ��

    private LivingEntity targetEntity; // ������ ���
    private NavMeshAgent navMeshAgent; // ����޽�

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
        timeBetAttack = 0f;
        audio = GetComponent<AudioSource>();
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

    // �ֱ������� �����ؾ� �� ����� ��ġ�� ã�� ��� ����
    IEnumerator UpdatePath()
    {
        while (gameObject != null)
        {
            // ���� ������ ���� �ƿ� �̵��� ���߰� ������ ���������� ���鼭 ���
            if (isStun)
            {
                navMeshAgent.isStopped = true;
                yield return new WaitForSeconds(0.25f); // ��� ���
                continue; // ���� ���̸� �̵� ����� �ƿ� �������� ����
            }

            if (hasTarget)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);

                float distance = Vector3.Distance(transform.position, targetEntity.transform.position);

                if (distance > maxChaseDistance)
                {
                    targetEntity = null;
                    navMeshAgent.isStopped = true;
                }

                if (distance < 0.3f)
                {
                    navMeshAgent.isStopped = true;
                    navMeshAgent.velocity = Vector3.zero;
                }
            }
            else
            {
                navMeshAgent.isStopped = true;

                Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, player);
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

            yield return new WaitForSeconds(0.25f);
        }
    }

    // ������ ȣ���ϴ� �������̵�
    public override void Stun()
    {
        if (isStun) return; // �̹� ���� ���¶�� ���� �� ��.

        isStun = true;
        navMeshAgent.isStopped = true; // ���� ���¿����� �̵� �� ��.

        audio.PlayOneShot(hitClip);

        StartCoroutine(ResumeAfterStun());
    }

    private IEnumerator ResumeAfterStun()
    {
        yield return new WaitForSeconds(2f); // 2�� ���� ���� ���� ����
        isStun = false; // ���� ����
        navMeshAgent.isStopped = false; // ���� ���� �� �̵� �簳
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            LivingEntity player = other.GetComponent<LivingEntity>();
            if (lastAttackTime + timeBetAttack < Time.time)
            {
                player.Stun();
                lastAttackTime = Time.time;
            }
        }
    }
}
