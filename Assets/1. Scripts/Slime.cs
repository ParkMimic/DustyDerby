using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour
{
    public LayerMask player; // 플레이어 마스크
    public float maxChaseDistance = 1f; // 최대 추적 거리

    private LivingEntity targetEntity;
    private NavMeshAgent navMeshAgent;

    public float timeBetAttack = 0.5f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점
    public bool isStunned = false;

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
        // 살아있는 동안 무한 루프
        while (gameObject != null)
        {
            if (hasTarget)
            {
                // 추적 대상 존재: 경로를 갱신하고 AI 이동을 계속 진행
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);

                float distance = Vector3.Distance(transform.position, targetEntity.transform.position);

                if (distance > maxChaseDistance)
                {
                    // 플레이어가 너무 멀어지면 추적 중지!
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
                // 추적 대상 없음: AI 이동 중지
                navMeshAgent.isStopped = true;

                // 20 유닛의 반지름을 가진 구를 그렸을 때 구와 겹치는 모든 콜라이더를 가져옴
                // 단, player 레이어를 가진 콜라이더만 가져오도록 필터링
                Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, player);

                // 모든 콜라이더를 순회하며 살아있는 LivingEntity 찾기
                for (int i = 0; i < colliders.Length; i++)
                {
                    // 콜라이더로부터 LivingEntity 가져오기
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    // LivingEntity 컴포넌트가 존재한다면
                    if (livingEntity != null)
                    {
                        // 추적 대상을 해당 livingEntity로 변경
                        targetEntity = livingEntity;

                        // for 루프 즉시 정지
                        break;
                    }
                }
            }
            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void Stun()
    {
        if (isStunned == false)
        {
            StartCoroutine(StunWait());
        }
    }

    IEnumerator StunWait()
    {
        // 기절로 이동 멈춤
        isStunned = true;
        navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(2f);
        isStunned = false;
    }
}
