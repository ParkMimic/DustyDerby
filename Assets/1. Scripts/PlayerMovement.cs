using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : LivingEntity
{
    public float moveSpeed = 1f; // 플레이어 움직임 속도 처리
    public float rotateSpeed = 180f; // 플레이어 회전 속도 처리

    private PlayerInput playerInput; // 플레이어의 입력을 감지하는 컴포넌트

    private new Rigidbody rigidbody; // 플레이어의 리지드바디 컴포넌트

    public Transform holdPosition; // 머리 위 위치
    public float throwForce = 10f; // 물건을 던지는 힘

    private GameObject heldObject; // 던질 게임 오브젝트 담기


    void Start()
    {
        // 사용할 컴포넌트 참조 가져오기
        playerInput = GetComponent<PlayerInput>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // 매 물리 갱신 주기마다 실행 됨
    void FixedUpdate()
    {
        if (!isStun)
        {
            // 물리 갱신 주기 때마다 회전과 움직임 실행
            // 회전 실행
            Rotate();
            // 움직임 실행
            Move();

            if (heldObject != null && playerInput.thrown)
            {
                ThrowObject();
            }
        }
    }

    // 입력값에 따라 플레이어 움직임
    private void Move()
    {
        // 상대적으로 이동할 거리 계산
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        // 리지드바디를 이용해 플레이어 이동
        rigidbody.MovePosition(rigidbody.position + moveDistance);
    }

    // 입력값에 따라 플레이어 회전
    private void Rotate()
    {
        // 상대적으로 회전할 거리 계산
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        // 리지드바디를 이용해 플레이어 회전
        rigidbody.rotation = rigidbody.rotation * Quaternion.Euler(0, turn, 0f);
    }

    void PickUpObject(GameObject obj)
    {
        heldObject = obj;
        Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;

        // 캐릭터와 물체 충돌 무시
        Collider objectCollider = obj.GetComponent<Collider>();
        Collider playerCollider = GetComponent<Collider>(); // 캐릭터의 Collider
        if (objectCollider != null && playerCollider != null)
        {
            Physics.IgnoreCollision(objectCollider, playerCollider, true);
        }

        // 부모로 붙이고 위치 지정
        obj.transform.SetParent(holdPosition);
        obj.transform.localPosition = Vector3.up * 1.2f;

        // 방향 정렬
        obj.transform.localRotation = Quaternion.identity;

        // 물체의 회전 고정
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (heldObject == null && other.collider.tag == "Throwable")
        {
            PickUpObject(other.gameObject);
        }
    }

    void ThrowObject()
    {
        Rigidbody rigidbody = heldObject.GetComponent<Rigidbody>();

        // 물체를 손에서 분리
        heldObject.transform.SetParent(null);
        rigidbody.isKinematic = false;

        // 플레이어가 바라보는 방향으로 힘을 줌
        Vector3 throwDirection = transform.forward + Vector3.up * 0.2f; // 약간 위로
        rigidbody.AddForce(throwDirection.normalized * throwForce, ForceMode.VelocityChange);

        // 기절 가능하게 표시
        ThrowableObject throwable = heldObject.GetComponent<ThrowableObject>();
        if (throwable != null)
        {
            throwable.isThrown = true; // 추가됨
        }

        // 충돌 다시 활성화
        Collider objectCollider = heldObject.GetComponent<Collider>();
        Collider playerCollider = GetComponent<Collider>();
        if (objectCollider != null && playerCollider != null)
        {
            Physics.IgnoreCollision(objectCollider, playerCollider, false);
        }

        // 회전 고정 해제
        rigidbody.constraints = RigidbodyConstraints.None;

        if (throwable != null)
        {
            throwable.isThrown = true;
        }

        heldObject = null;
    }
}
