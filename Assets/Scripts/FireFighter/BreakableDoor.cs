using UnityEngine;
using UnityEngine.UIElements;

public class BreakableDoor : MonoBehaviour
{
    public void Break()
    {
        // Play breaking animation or destroy
        Debug.Log("Door broken!");
        Destroy(gameObject);
    }
}