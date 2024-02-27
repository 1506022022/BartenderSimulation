using UnityEngine;

public class Bookcase : MonoBehaviour
{
    GameObject _selectedBook;
    public void BookisSelected(GameObject book)
    {
        _selectedBook?.SetActive(true);
        _selectedBook = book;
        book.SetActive(false);
        GameObjectManager.InitGameObject();
        DisableMent();
    }
    void DisableMent()
    {
        FollowingText.Instances?.ForEach(x => x.StopMent());
    }

}
