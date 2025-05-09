using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.EventSystems;

// �÷��̾ ĳ���͸� �����ϱ� ���� ����� �Է� ����
// ������ �Է°��� �ٸ� ������Ʈ�� ����� �� �ֵ��� ����
public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical"; // �յ� �������� ���� �Է���
    public string rotateAxisName = "Horizontal"; // ȸ���� ���� �Է���
    public string throwButton = "Fire1";

    // �� �Ҵ��� ���ο����� ����
    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool thrown { get; private set; }

    // �� ������ ������� �Է��� ����
    void Update()
    {
        if (GameManager.instance == null && GameManager.instance.isStun)
        {
            move = 0;
            rotate = 0;
            return;
        }

        // move�� ���� �Է� ����
        move = Input.GetAxis(moveAxisName);
        rotate = Input.GetAxis(rotateAxisName);
        thrown = Input.GetButton(throwButton);
    }
}