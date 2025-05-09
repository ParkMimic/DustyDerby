using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.EventSystems;

// 플레이어가 캐릭터를 조작하기 위한 사용자 입력 감지
// 감지된 입력값을 다른 컴포넌트가 사용할 수 있도록 제공
public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축
    public string rotateAxisName = "Horizontal"; // 회전을 위한 입력축
    public string throwButton = "Fire1";

    // 값 할당은 내부에서만 가능
    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool thrown { get; private set; }

    // 매 프레임 사용자의 입력을 감지
    void Update()
    {
        if (GameManager.instance == null && GameManager.instance.isStun)
        {
            move = 0;
            rotate = 0;
            return;
        }

        // move에 관한 입력 감지
        move = Input.GetAxis(moveAxisName);
        rotate = Input.GetAxis(rotateAxisName);
        thrown = Input.GetButton(throwButton);
    }
}