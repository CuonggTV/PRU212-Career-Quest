using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;
    InteractableObject currentInteractable;

    // Update is called once per frame
    void Update()
    {
        CheckInteraction();
        if (Input.GetKeyDown(KeyCode.F) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void CheckInteraction()
    {
        Vector3 originUpper = transform.position + Vector3.up * 0.5f; // Upper ray (chest/head height)
        Vector3 originLower = transform.position + Vector3.up * 0.2f; // Lower ray (waist height)
        Vector3 direction = transform.forward;

        Debug.DrawRay(originUpper, direction * playerReach, Color.red, 1f);
        Debug.DrawRay(originLower, direction * playerReach, Color.green, 1f);

        RaycastHit hit;

        bool upperHit = Physics.Raycast(originUpper, direction, out hit, playerReach);
        bool lowerHit = !upperHit && Physics.Raycast(originLower, direction, out hit, playerReach);

        if (upperHit || lowerHit)
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                InteractableObject newInteractable = hit.collider.GetComponent<InteractableObject>();

                if (currentInteractable && newInteractable != currentInteractable)
                {
                    currentInteractable.DisableOutline();
                }

                if (newInteractable != null && newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
        {
            DisableCurrentInteractable();
        }
    }

    void SetNewCurrentInteractable(InteractableObject newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
        HUDController.instance.EnableInteractionText(currentInteractable.message);
    }

    void DisableCurrentInteractable()
    {
        HUDController.instance.DisableInteractionText();
        if (currentInteractable)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
