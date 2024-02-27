using UnityEngine;

public class GameSetting : MonoBehaviour
{
    const float gravity = 9.80665f;
    public float timeScale;
    // Start is called before the first frame update
    void Update()
    {
        //Physics.gravity = Vector3.down * gravity;
        Time.timeScale = timeScale;
    }

}
