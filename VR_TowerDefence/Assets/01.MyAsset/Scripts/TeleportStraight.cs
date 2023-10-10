using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportStraight : MonoBehaviour
{
    //텔레포트를 표시할 UI
    public Transform teleportCircleUI;
    //선을 그릴 라인 렌더러
    LineRenderer Lr;
    //최초텔레포트크기
    Vector3 originScale = Vector3.one * 0.02f;
    void Awake()
    {
        //시작할 때 비활성화한다.
        teleportCircleUI.gameObject.SetActive(false);
        //라인 렌더러 컴포넌트 얻어오기
        Lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        //왼쪽 컨트롤러의 One 버튼을 누르면
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            //라인 렌더러 컴포넌트 활성화
            Lr.enabled = true;
        }
        //왼쪽 컨트롤러의 One버튼에서 손을 때면
        else if (ARAVRInput.GetUp(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            //라인 렌더러 컴포넌트 비활성화
            Lr.enabled = false;

            if (teleportCircleUI.gameObject.activeSelf)
            {
                GetComponent<CharacterController>().enabled = false;
                transform.position = teleportCircleUI.position + Vector3.up;
                GetComponent<CharacterController>().enabled = true;
            }
            teleportCircleUI.gameObject.SetActive(false);
        }

        //왼쪽 컨트롤러의 One버튼을 누르고 있을때
        else if (ARAVRInput.Get(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            //1. 왼쪽 컨트롤러를 기준으로 Ray를 만든다.
            Ray ray = new Ray(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);
            RaycastHit hitInfo;
            int layer = 1 << LayerMask.NameToLayer("Terrain");
            //2.Terrain만 Ray 충돌 검출한다.
            if (Physics.Raycast(ray, out hitInfo, 200, layer))
            {
                //3.Ray가 부딪힌 지점에 라인 그리기
                Lr.SetPosition(0, ray.origin);
                Lr.SetPosition(1, hitInfo.point);

                //4.Ray가 부딪힌 지점에 텔레포트 UI표시
                teleportCircleUI.gameObject.SetActive(true);
                teleportCircleUI.position = hitInfo.point;
                //텔레포트 UI가 위로 누워 있도록 방향을 설정한다.
                teleportCircleUI.forward = hitInfo.normal;
                //텔레포트 UI의 큭디가 거리에 따라 보정되도록 설정한다.
                teleportCircleUI.localScale = originScale * Mathf.Max(1f, hitInfo.distance);
            }
            else
            {
                Lr.SetPosition(0, ray.origin);
                Lr.SetPosition(1, ray.origin + ARAVRInput.LHandDirection * 200);
                teleportCircleUI.gameObject.SetActive(false);
            }
        }
    }
}