using UnityEngine;
using UnityEngine.Events;

namespace Sophia.Instantiates
{
    using Sophia.Entitys;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Referer;
    using Sophia.Composite;

    public class ItemObjectBucket : MonoBehaviour
    {        
        public ItemObject InstantablePositioning(ItemObject instantiatedItem) {
            Vector3     position     = transform.position;
            instantiatedItem.transform.position = position;
            instantiatedItem.transform.Rotate(transform.right);
            Debug.Log(transform.forward);
            Debug.Log($"{instantiatedItem.transform}, {instantiatedItem.transform.eulerAngles}");
            return instantiatedItem;
        }
    }
}