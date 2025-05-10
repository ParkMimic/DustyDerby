using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Slime : LivingEntity
{
    public LayerMask player; // 플레이어 마스크
    public float maxChaseDistance = 1f; // 최대 추적 거리

    private LivingEntity targetEntity;
    private NavMeshAgent navMeshAgent;

    public float timeBetAttack = 0.5f; // 공격 간격
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
        // 게임 활성화와 동시에 AI 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }

    void Update()
    {
        // 추적 대상의 존재 여부에 따라 다른 애니메이션 재생
    }

    // 주기적으로 추적해야 할 대상의 위치를 찾아 경로 갱신
    IEnumerator UpdatePath()
    {
        while (gameObject != null)
        {
            // 기절 상태일 때는 아예 이동을 멈추고 루프를 지속적으로 돌면서 대기
            if (isStun)
            {
                navMeshAgent.isStopped = true;
                yield return new WaitForSeconds(0.25f); // 잠시 대기
                continue; // 기절 중이면 이동 명령을 아예 실행하지 않음
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

    // 기절을 호출하는 오버라이드
    public override void Stun()
    {
        if (isStun) return; // 이미 기절 상태라면 실행 안 함.

        isStun = true;
        Debug.Log("슬라임 기절 시작");

        navMeshAgent.isStopped = true; // 기절 상태에서는 이동 못 함.

        StartCoroutine(ResumeAfterStun());
    }

    private IEnumerator ResumeAfterStun()
    {
        yield return new WaitForSeconds(2f); // 2초 동안 기절 상태 유지
        isStun = false; // 기절 해제
        navMeshAgent.isStopped = false; // 기절 해제 후 이동 재개
        Debug.Log("슬라임 기절 끝");
    }
}
