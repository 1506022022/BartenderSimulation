using UnityEngine;

public class ItemDataComponent : MonoBehaviour
{
    //[SerializeField] public ItemData ItemDataOrigin;
    //[SerializeField]
    //private ItemData m_ItemData;
    //public ItemData GetItemData
    //{
    //    get
    //    {
    //        if (m_ItemData == null)
    //        {
    //            if (ItemDataOrigin != null) 
    //            { 
    //                m_ItemData = Instantiate(ItemDataOrigin);
    //                //print(gameObject.name+m_ItemData.GetInstanceID());
    //            }
    //        }
    //        return m_ItemData;
    //    }
    //}

    //public void Init()
    //{
    //    (m_ItemData as RecipeData)?.data.Clear();
    //}
    //public IItemDataReader GetReader()
    //{
    //    if (GetItemData == null) return null;
    //    else if (GetItemData.GetType() == typeof(ItemData)) { return new Itemdata(); }
    //    else if (GetItemData.GetType() == typeof(RecipeData)) { return new RecipeReader(); }
    //    else if (GetItemData.GetType() == typeof(GlassItemData)) { return new GlassItemReader(); }
    //    else if (GetItemData.GetType() == typeof(EquipmentItemData)) { return new Itemdata(); }
    //    else if (GetItemData.GetType() == typeof(CountableItemData)) { return new GlassItemReader(); }
    //    else return null;

    //}

}

//public interface IItemDataReader 
//{
//    public List<string> Read(ItemData data);
//}
//public class Itemdata : IItemDataReader
//{
//    public List<string> Read(ItemData data)
//    {
//        var read = new List<string>
//        {
//            data.Discription
//        };
//        return read;
//    }
//}

//public class RecipeReader : IItemDataReader
//{
//    public List<string> Read(ItemData data)
//    {
//        var m_data = data as RecipeData;
//        if (m_data == null) return null;

//        var read = new List<string>();
//        foreach(var i in m_data.data)
//        {
//            read.Add(i.itemData.Name + " " + i.Capacity+"\n");
//        }
//        if (read.Count != 0)
//        {
//            read[read.Count - 1]=read[read.Count - 1].Replace("\n", "");
//        }
//        return read;
//    }
//    public class GlassItemReader : IItemDataReader
//    {
//        public List<string> Read(ItemData data)
//        {
//            var m_data = data as GlassItemData;
//            if (m_data == null) return null;

//            var read = new List<string>
//        {
//            m_data.Name+"\n",
//            m_data.Capacity.ToString()+" ml"
//        };
//            return read;
//        }
//    }

//}
