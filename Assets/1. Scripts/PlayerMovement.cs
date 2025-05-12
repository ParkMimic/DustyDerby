using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : LivingEntity
{
    public float moveSpeed = 1f; // �÷��̾� ������ �ӵ� ó��
    public float rotateSpeed = 180f; // �÷��̾� ȸ�� �ӵ� ó��

    private PlayerInput playerInput; // �÷��̾��� �Է��� �����ϴ� ������Ʈ

    private new Rigidbody rigidbody; // �÷��̾��� ������ٵ� ������Ʈ

    public Transform holdPosition; // �Ӹ� �� ��ġ
    public float throwForce = 10f; // ������ ������ ��

    private GameObject heldObject; // ���� ���� ������Ʈ ���


    void Start()
    {
        // ����� ������Ʈ ���� ��������
        playerInput = GetComponent<PlayerInput>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // �� ���� ���� �ֱ⸶�� ���� ��
    void FixedUpdate()
    {
        if (!isStun)
        {
            // ���� ���� �ֱ� ������ ȸ���� ������ ����
            // ȸ�� ����
            Rotate();
            // ������ ����
            Move();

            if (heldObject != null && playerInput.thrown)
            {
                ThrowObject();
            }
        }
    }

    // �Է°��� ���� �÷��̾� ������
    private void Move()
    {
        // ��������� �̵��� �Ÿ� ���
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        // ������ٵ� �̿��� �÷��̾� �̵�
        rigidbody.MovePosition(rigidbody.position + moveDistance);
    }

    // �Է°��� ���� �÷��̾� ȸ��
    private void Rotate()
    {
        // ��������� ȸ���� �Ÿ� ���
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        // ������ٵ� �̿��� �÷��̾� ȸ��
        rigidbody.rotation = rigidbody.rotation * Quaternion.Euler(0, turn, 0f);
    }

    void PickUpObject(GameObject obj)
    {
        heldObject = obj;
        Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;

        // ĳ���Ϳ� ��ü �浹 ����
        Collider objectCollider = obj.GetComponent<Collider>();
        Collider playerCollider = GetComponent<Collider>(); // ĳ������ Collider
        if (objectCollider != null && playerCollider != null)
        {
            Physics.IgnoreCollision(objectCollider, playerCollider, true);
        }

        // �θ�� ���̰� ��ġ ����
        obj.transform.SetParent(holdPosition);
        obj.transform.localPosition = Vector3.up * 1.2f;

        // ���� ����
        obj.transform.localRotation = Quaternion.identity;

        // ��ü�� ȸ�� ����
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
            throwable.isThrown = true;
        }

        heldObject = null;
    }
}
