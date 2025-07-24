using UnityEngine;

public class BedInteractable : InteractableObject
{
    [SerializeField] FireFighter player;
    public bool isPersonOnBed = false;
    public void SetIsPersonOnBed(bool result) => isPersonOnBed = result;
    public bool GetIsPersonOnBed() => isPersonOnBed;
    public override void Interact()
    {
        var carriedPerson = player.GetCarriedPerson();
        if (carriedPerson == null)
        {
            HUDController.instance.ShowAnnouncement("There is no carried person.");
            return;
        }
        if (isPersonOnBed)
        {
            HUDController.instance.ShowAnnouncement("Bed already has a person.");
            return;
        }
        if (carriedPerson.TryGetComponent<CarriedPerson>(out var carried))
        {
            if (carried.GetIsOnBed())
            {
                return;
            }
            carried.SetIsOnBed(true);
        }

        if (!TryGetComponent<Collider>(out var bedCollider))
        {
            Debug.LogWarning("BedInteractable missing collider.");
            return;
        }

        // Calculate bed top position
        Vector3 bedTop = bedCollider.bounds.center;
        bedTop.y = bedCollider.bounds.max.y;

        bedTop += bedCollider.transform.right * -0.3f;
        Quaternion rotation = Quaternion.Euler(-90f, bedCollider.transform.eulerAngles.y, -90f);

        carriedPerson.transform.SetPositionAndRotation(bedTop, rotation);

        if (carriedPerson.TryGetComponent<Animator>(out var anim))
        {
            anim.SetBool("IsLying", true);
            anim.applyRootMotion = false;
        }

        // Disable physics and navigation
        if (carriedPerson.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
        }

        carriedPerson.transform.SetParent(null);

        // Clear firefighter state
        player.SetCarriedPerson(null);
        player.SetIsEmptyHand(true);
        player.SetActiveAnimationLayer(0);

        // Set misson complete
        HUDController.instance.SetToggleMissionOn(carriedPerson.name);
        SetIsPersonOnBed(true);
    }


}
