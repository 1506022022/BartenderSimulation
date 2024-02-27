using UnityEngine;

public class MentHandler : MonoBehaviour
{
    [SerializeField] FollowingText _instance;

    public void Reposition()
    {
        _instance.Reposition();
    }
}
