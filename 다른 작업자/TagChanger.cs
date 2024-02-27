using UnityEngine;

public class TagChanger : MonoBehaviour
{
    public string tag;
    private void Awake()
    {
        gameObject.tag = tag;

    }
}
