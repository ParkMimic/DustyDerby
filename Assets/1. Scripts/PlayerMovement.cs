using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 플레이어 움직임 속도 처리
    public float rotateSpeed = 180f; // 플레이어 회전 속도 처리

    public PlayerInput playerInput; // 플레이어의 입력을 감지하는 컴포넌트

    public Rigidbody rigidbody; // 플레이어의 리지드바디 컴포넌트

    void Start()
    {
        // 사용할 컴포넌트 참조 가져오기
        playerInput = GetComponent<PlayerInput>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // 매 물리 갱신 주기마다 실행 됨
    void FixedUpdate()
    {
        // 물리 갱신 주기 때마다 회전과 움직임 실행
        // 회전 실행
        Rotate();
        // 움직임 실행
        Move();
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
        rigidbody.rotation = playerRigidbody.rotation +
    }
}
