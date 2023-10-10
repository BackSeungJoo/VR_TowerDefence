using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 캐릭터 컨트롤러 컴포넌트
    private CharacterController characterController = default;

    // 이동속도
    public float speed = 5f;

    // 점프 속도
    public float jumpPower = 5f;

    // { 중력과 관련된 변수
    public float gravity = -20;     // 중력 가속도의 크기
    float yVelocity = 0;            // 수직 속도
    // } 중력과 관련된 변수

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 1. 사용자의 입력을 받는다.
        float h = ARAVRInput.GetAxis("Horizontal");
        float v = ARAVRInput.GetAxis("Vertical");

        // 2. 방향을 만든다.
        Vector3 dir = new Vector3(h, 0, v);

        // 2.0 사용자가 바라보는 방향으로 입력 값 변화시키기
        dir = Camera.main.transform.TransformDirection(dir);

        // REGACY:
        //Vector3 direction = Vector3.zero;

        // { 업데이트 타임에 중력을 적용하는 로직
        // 2.1 중력을 적용한 수직 방향 추가
        yVelocity += gravity * Time.deltaTime;

        // 2.2 바닥에 있을 경우, 수직 항력을 처리하기 위해 속도를 0으로 한다.
        if(characterController.isGrounded)
        {
            yVelocity = 0;
        }

        // 2.3 사용자가 점프 버튼을 누르면 속도에 점프 크기를 할당한다.
        if(ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch))
        {
            yVelocity = jumpPower;
        }

        dir.y = yVelocity;

        // 3. 이동한다.
        characterController.Move(dir * speed * Time.deltaTime);
        // } 업데이트 타임에 중력을 적용하는 로직
    }
}
