using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    public LayerMask player; // 플레이어 마스크

    private LivingEntity targetEntity;
    private NavMeshAgent navMeshAgent;

    public float timeBetAttack = 10f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점

    // 추적할 대상이 있음을 알려주는 컴포넌트
    private bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재한다면 true
            if (targetEntity != null)
            {
                return true;
            }

            // 그렇지 않다면 false를 반환
            return false;
        }
    }

    private void Awake()
    {
        // 초기화
        // 필요한 컴포넌트 가져오기
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void SetUp(EnemyData enemyData)
    {
        navMeshAgent.speed = enemyData.speed;
    }

    void Start()
    {
        timeBetAttack = 0f; // 공격 초기화
        // 게임 활성화와 동시에 AI 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }

    // 주기적으로 추적해야 할 대상의 위치를 찾아 경로 갱신
    IEnumerator UpdatePath()
    {
        // 살아있는 동안 무한 루프
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

            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    // 충돌한 트리거 확인
    public void OnTriggerEnter(Collider other)
    {
        // 충돌한 콜라이더의 태그가 플레이어라면
        if (other.tag == "Player")
        {
            // LivingEntity 를 가져옴
            LivingEntity player = other.GetComponent<LivingEntity>();
            // 마지막 공격 시점으로부터 시간이 지났다면
            if (lastAttackTime + timeBetAttack < Time.time)
            {
                // 플레이어에게 기절을 적용
                player.Stun();
                // 이후 마지막 공격 시간 갱신
                lastAttackTime = Time.time;
            }
        }
    }
}
