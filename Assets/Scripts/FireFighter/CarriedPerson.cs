using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class CarriedPerson : MonoBehaviour
{
    [SerializeField] Transform carryPosition;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip clip;
    public float delayBeforeLoop = 0.5f;
    private Coroutine loopCoroutine;

    private bool isOnBed = false;

    public bool GetIsOnBed() => isOnBed;
    public void SetIsOnBed(bool setValue) => isOnBed = setValue;
    void Start()
    {
        if (audioSource != null && clip != null)
        {
            loopCoroutine = StartCoroutine(PlayWithDelay());
        }
    }

    IEnumerator PlayWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoop);
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void BeCarried()
    {
        if (isOnBed)
        {
            HUDController.instance.ShowAnnouncement("Person is already on bed.");
            return;
        }
        if (TryGetComponent<Animator>(out var anim))
        {
            anim.SetBool("IsCarried", true);
            anim.SetBool("IsLying", false);
            anim.applyRootMotion = false;
        }

        if (TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var agent))
            agent.enabled = false;

        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = true;

        transform.SetParent(carryPosition, false);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (loopCoroutine != null)
        {
            StopCoroutine(loopCoroutine);
            loopCoroutine = null;
        }

        audioSource.Stop();
    }

    public void BeDropped(Transform dropper, LayerMask groundLayer)
    {
        transform.SetParent(null);

        Vector3 rayOrigin = dropper.position + dropper.forward * 1.5f + Vector3.up * 2f;

        Vector3 finalPosition = transform.position;
        Quaternion finalRotation = transform.rotation;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 10f, groundLayer))
        {
            finalPosition = hit.point;

            if (TryGetComponent<Collider>(out var col))
            {
                finalPosition += dropper.right * -0.5f + dropper.forward * 0.05f;
                finalPosition.y += col.bounds.extents.y - 0.35f;
            }

            float yRot = dropper.eulerAngles.y - 90f;
            finalRotation = Quaternion.Euler(-90f, yRot, 0f);
        }

        transform.SetPositionAndRotation(finalPosition, finalRotation);

        if (TryGetComponent<Animator>(out var anim))
        {
            anim.SetBool("IsLying", true);
            anim.applyRootMotion = false;
        }

        if (TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var agent))
            agent.enabled = true;

        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = true;
    }
}
