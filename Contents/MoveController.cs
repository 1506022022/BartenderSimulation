using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MoveController : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // �޼�, �������� ������ �� �ֽ��ϴ�.
    public SteamVR_Action_Vector2 touchpadAction; // ��ġ�е� �Է��� ���� �׼��� �����մϴ�.
    public float speed = 3.0f; // �̵� �ӵ� ������ ���� ����

    [SerializeField] private Camera playerCamera;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float maxSpeed;

    private void Update()
    {
        // ��ġ�е��� �Է� ���� �����ɴϴ�.
        Vector2 touchpadValue = touchpadAction.GetAxis(handType);

        // ��ġ�е带 ������ �̵��մϴ�.
        if (touchpadValue != Vector2.zero)
        {
            // ��ġ�е��� ������ ī�޶� �������� ��ȯ�մϴ�.
            Vector3 moveDirection = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * new Vector3(touchpadValue.x, 0, touchpadValue.y);

            // �̵��մϴ�.
            MovePlayer(moveDirection);
        }
        else                                 // ��ġ �е带 ������ �ʰ� �ִ� ��쿡��
            rb.velocity = Vector3.zero;     // �ӵ��� �����Ѵ�.
    }
    void MovePlayer(Vector3 moveDirection)
    {
        Debug.Log("�����̶�� ����� ����");
        Vector3 force = moveDirection.normalized * speed;
        Debug.Log(new Vector3(force.x, 0, force.z) + "��ŭ �̵��Ϸ��� �õ���");
        rb.AddForce(new Vector3(force.x, 0, force.z), ForceMode.VelocityChange);

        Debug.Log("�̵����� �ְ� ����" + rb.velocity + "��ŭ�� �ӵ��� ������ �ִ�.");
        if (rb.velocity.magnitude > maxSpeed)
        {
            Debug.Log("��갪�� �ְ�ӵ��� �Ѿ��⿡ ��ġ�� �ְ�ӵ��� ����");
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
