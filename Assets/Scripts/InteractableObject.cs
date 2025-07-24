using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableObject : MonoBehaviour
{
    Outline outline;
    public string message;
    public void EnableOutline()
    {
        outline.enabled = true;
    }
    public void DisableOutline()
    {
        outline.enabled = false;
    }

    public abstract void Interact();

    void Start()
    {
        outline = GetComponent<Outline>();
        DisableOutline();
    }
}
