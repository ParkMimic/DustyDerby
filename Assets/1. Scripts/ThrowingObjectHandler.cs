using UnityEngine;
using System.Collections;

public class ThrowingObjectHandler : MonoBehaviour
{
    public Transform holdPosition; // �Ӹ� �� ��ġ
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

        // ĳ���Ϳ� ��ü �浹 ����
        Collider objectCollider = obj.GetComponent<Collider>();
        Collider playerCollider = GetComponent<Collider>(); // ĳ������ Collider
        if (objectCollider != null && playerCollider != null)
        {
            Physics.IgnoreCollision(objectCollider, playerCollider, true);
        }

        // �θ�� ���̰� ��ġ ����
        obj.transform.SetParent(holdPosition);
        obj.transform.localPosition = Vector3.up * 1.5f;

        // ���� ����
        obj.transform.localRotation = Quaternion.identity;

        // ��ü�� ȸ�� ����
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    void ThrowObject()
    {
        Rigidbody rigidbody = heldObject.GetComponent<Rigidbody>();

        // ��ü�� �տ��� �и�
        heldObject.transform.SetParent(null);
        rigidbody.isKinematic = false;

        // �÷��̾ �ٶ󺸴� �������� ���� ��
        Vector3 throwDirection = transform.forward + Vector3.up * 0.2f; // �ణ ����
        rigidbody.AddForce(throwDirection.normalized * throwForce, ForceMode.VelocityChange);

        // ���� �����ϰ� ǥ��
        ThrowableObject throwable = heldObject.GetComponent<ThrowableObject>();
        if (throwable != null)
        {
            throwable.isThrown = true; // �߰���
        }

        // �浹 �ٽ� Ȱ��ȭ
        Collider objectCollider = heldObject.GetComponent<Collider>();
        Collider playerCollider = GetComponent<Collider>();
        if (objectCollider != null && playerCollider != null)
        {
            Physics.IgnoreCollision(objectCollider, playerCollider, false);
        }

        // ȸ�� ���� ����
        rigidbody.constraints = RigidbodyConstraints.None;

        if (throwable != null)
        {
            Debug.Log("���� ���� true");
            throwable.isThrown = true;
        }

        heldObject = null;
    }
}
