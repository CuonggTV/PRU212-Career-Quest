using UnityEngine;

public class PersonInteractable : InteractableObject
{
    [SerializeField] FireFighter player;
    public override void Interact()
    {
        if (!player.GetIsEmptyHand()) return;
        if (TryGetComponent<CarriedPerson>(out var carriedScript))
        {
            if (carriedScript.GetIsOnBed())
            {
                HUDController.instance.ShowAnnouncement("Person is already on bed.");
                return;
            }
            player.SetCarriedPerson(GetComponent<Collider>().gameObject);
            player.SetIsEmptyHand(false);

            carriedScript.BeCarried();
            player.SetActiveAnimationLayer(3);
        }
        else Debug.Log("PersonInteractable: Ray is not hit");

    }
}
