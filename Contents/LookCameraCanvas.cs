using UnityEngine;

public class LookCameraCanvas : MonoBehaviour
{
    GameObject cam;
    private void Start()
    {
        cam = Camera.main.gameObject;

    }

    private void LateUpdate()
    {
        if (cam != null)
        {
            transform.LookAt(cam.transform);
            transform.Rotate(0, 180, 0);
        }
    }
}