using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Pages")]
public class Pages : ScriptableObject
{
    [SerializeField] Sprite[] _sprites;

    public void PrintPages(Book book)
    {
        book.bookPages = _sprites;
    }
}
