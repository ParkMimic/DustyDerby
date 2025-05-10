using UnityEngine;
using System.Collections;

public class ThrowingObjectHandler : MonoBehaviour
{
    public Transform holdPosition; // 머리 위 위치
    public float throwForce = 10f;

    private GameObject heldObject;
    private PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (heldObject != null && playerInput.thrown)
        {
            ThrowObject();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (heldObject == null && other.collider.tag == "Throwable")
        {
            PickUpObject(other.gameObject);
        }
    }

    void PickUpObject(GameObject obj)
    {
        heldObject = obj;
        Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        // 캐릭터와 물체 충돌 무시
        Collider objectCollider = obj.GetComponent<Collider>();
        Collider playerCollider = GetComponent<Collider>(); // 캐릭터의 Collider
        if (objectCollider != null && playerCollider != null)
        {
            Physics.IgnoreCollision(objectCollider, playerCollider, true);
        }

        // 부모로 붙이고 위치 지정
        obj.transform.SetParent(holdPosition);
        obj.transform.localPosition = Vector3.up * 1.5f;

        // 방향 정렬
        obj.transform.localRotation = Quaternion.identity;

        // 물체의 회전 고정
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
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
            Debug.Log("던짐 상태 true");
            throwable.isThrown = true;
        }

        heldObject = null;
    }
}
