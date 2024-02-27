using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MoveController : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // 왼손, 오른손을 선택할 수 있습니다.
    public SteamVR_Action_Vector2 touchpadAction; // 터치패드 입력을 받을 액션을 선택합니다.
    public float speed = 3.0f; // 이동 속도 조절을 위한 변수

    [SerializeField] private Camera playerCamera;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float maxSpeed;

    private void Update()
    {
        // 터치패드의 입력 값을 가져옵니다.
        Vector2 touchpadValue = touchpadAction.GetAxis(handType);

        // 터치패드를 누르면 이동합니다.
        if (touchpadValue != Vector2.zero)
        {
            // 터치패드의 방향을 카메라 기준으로 변환합니다.
            Vector3 moveDirection = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * new Vector3(touchpadValue.x, 0, touchpadValue.y);

            // 이동합니다.
            MovePlayer(moveDirection);
        }
        else                                 // 터치 패드를 누르지 않고 있는 경우에는
            rb.velocity = Vector3.zero;     // 속도를 제거한다.
    }
    void MovePlayer(Vector3 moveDirection)
    {
        Debug.Log("움직이라는 명령을 받음");
        Vector3 force = moveDirection.normalized * speed;
        Debug.Log(new Vector3(force.x, 0, force.z) + "만큼 이동하려고 시도함");
        rb.AddForce(new Vector3(force.x, 0, force.z), ForceMode.VelocityChange);

        Debug.Log("이동값을 주고 나니" + rb.velocity + "만큼의 속도를 가지고 있다.");
        if (rb.velocity.magnitude > maxSpeed)
        {
            Debug.Log("계산값이 최고속도를 넘었기에 수치를 최고속도로 줄임");
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
