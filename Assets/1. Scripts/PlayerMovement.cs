using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f; // �÷��̾� ������ �ӵ� ó��
    public float rotateSpeed = 180f; // �÷��̾� ȸ�� �ӵ� ó��

    private PlayerInput playerInput; // �÷��̾��� �Է��� �����ϴ� ������Ʈ

    private Rigidbody rigidbody; // �÷��̾��� ������ٵ� ������Ʈ

    void Start()
    {
        // ����� ������Ʈ ���� ��������
        playerInput = GetComponent<PlayerInput>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // �� ���� ���� �ֱ⸶�� ���� ��
    void FixedUpdate()
    {
        // ���� ���� �ֱ� ������ ȸ���� ������ ����
        // ȸ�� ����
        Rotate();
        // ������ ����
        Move();
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
}
