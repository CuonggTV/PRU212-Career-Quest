using UnityEngine;

public class WaterCollisionHandler : MonoBehaviour
{
    [SerializeField] private GameObject steamEffectPrefab;
    [SerializeField] private GameObject waterSplashEffectPrefab;

    private void OnParticleCollision(GameObject other)
    {
        Vector3 hitPos = other.transform.position; // default fallback

        // Try raycast to refine hit point (optional and approximate)
        if (Physics.Raycast(transform.position, other.transform.position - transform.position, out RaycastHit hit, 10f))
        {
            hitPos = hit.point;
        }

        if (other.TryGetComponent<Fire>(out var fire))
        {
            fire.TryExtinguish(0.2f);
            if (steamEffectPrefab != null)
                Instantiate(steamEffectPrefab, hitPos, Quaternion.identity);

            AudioManager.instance.PlaySFX(AudioManager.instance.firePutOut);
        }
        else
        {
            if (waterSplashEffectPrefab != null)
                Instantiate(waterSplashEffectPrefab, hitPos, Quaternion.identity);
        }
    }
}
