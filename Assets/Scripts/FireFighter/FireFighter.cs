using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FireFighter : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;
    [SerializeField] private float rigFadeDuration = 0.3f;

    [Header("Other Objects Settings")]
    public GameObject turbojetNozzle;
    public GameObject axe;
    [SerializeField] private Transform rightHandBone;

    public ParticleSystem waterHoseParticle;
    public Transform nozzleTip;

    [Header("Rig Settings")]
    public Rig turbojetNozzleRig;


    [SerializeField] private LayerMask groundLayerMask;

    // Class value
    private bool isEmptyHand = true;
    private bool isChopping = false;
    private GameObject carriedPerson;

    #region Get/Set Zone
    public GameObject GetCarriedPerson() => carriedPerson;
    public void SetCarriedPerson(GameObject person) => carriedPerson = person;
    public bool GetIsEmptyHand() => isEmptyHand;
    public void SetIsEmptyHand(bool isEmptyHand) => this.isEmptyHand = isEmptyHand;
    #endregion

    void Start()
    {
        isEmptyHand = true;
        // Attach the water particle system to the nozzle tip
        waterHoseParticle.transform.SetParent(nozzleTip);
        waterHoseParticle.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        // Start with no weapon
        SetActiveWeapon(WeaponType.None);
    }

    void Update()
    {
        HandleInput();
        PlayWaterHose();
        HandleAxeCasting();
    }

    private void HandleInput()
    {
        if (carriedPerson) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveWeapon(WeaponType.None);
            SetActiveAnimationLayer(0);
            isEmptyHand = true;
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetActiveWeapon(WeaponType.TurbojetNozzle);
            SetActiveAnimationLayer(1);
            isEmptyHand = false;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetActiveWeapon(WeaponType.Axe);
            SetActiveAnimationLayer(2);
            isEmptyHand = false;

        }
    }

    private void PlayWaterHose()
    {
        if (!turbojetNozzle.activeSelf) return;

        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.instance.PlaySFXLoop(AudioManager.instance.waterSpray);
            waterHoseParticle.Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            AudioManager.instance.StopSFXLoop();
            waterHoseParticle.Stop();
        }
    }

    private void HandleAxeCasting()
    {
        if (!axe.activeSelf || isChopping) return;

        if (Input.GetMouseButtonDown(0))
        {
            // Perform a raycast forward from the camera or character
            animator.SetTrigger("Chop");
            StartCoroutine(ChopCooldown());
            AudioManager.instance.PlaySFX(AudioManager.instance.axeSlash);

        }
    }

    public void TriggerChopImpact()
    {
        // Perform the raycast at the right timing of the chop
        Ray ray = new(transform.position + Vector3.up * 1.2f, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            var breakable = hit.collider.GetComponentInParent<BreakableDoor>();
            if (hit.collider.CompareTag("BreakableDoor") && breakable != null)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.axeHit);
                breakable.Break();
            }
        }

        Debug.Log("Chop impact triggered.");
    }

    private IEnumerator ChopCooldown()
    {
        isChopping = true;
        yield return new WaitForSeconds(1f); // match animation length
        isChopping = false;
    }

    private enum WeaponType { None, TurbojetNozzle, Axe }

    private void SetActiveWeapon(WeaponType type)
    {
        // Deactivate all weapons
        turbojetNozzle.SetActive(false);
        axe.SetActive(false);

        // Reset all rig weights
        turbojetNozzleRig.weight = 0f;

        // Activate selected weapon and rig
        switch (type)
        {
            case WeaponType.TurbojetNozzle:
                turbojetNozzle.SetActive(true);
                turbojetNozzleRig.weight = 1f;
                break;
            case WeaponType.Axe:
                axe.SetActive(true);
                break;
        }
    }

    public void SetActiveAnimationLayer(int activeLayer)
    {
        int layerCount = animator.layerCount;

        for (int i = 1; i < layerCount; i++) // Usually base layer is 0, we start from 1
        {
            float weight = (i == activeLayer) ? 1f : 0f;
            animator.SetLayerWeight(i, weight);
        }
    }

    void DropCarriedPerson()
    {
        if (carriedPerson == null) return;

        if (carriedPerson.TryGetComponent<CarriedPerson>(out var carriedScript))
        {
            carriedScript.BeDropped(transform, groundLayerMask);
        }

        carriedPerson = null;
        isEmptyHand = true;
        SetActiveAnimationLayer(0);
    }

}
