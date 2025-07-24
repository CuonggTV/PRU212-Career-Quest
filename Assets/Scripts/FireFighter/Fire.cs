using UnityEngine;
using UnityEngine.UIElements;

public class Fire : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float currentIntensity = 1.0f;

    private float[] StartIntensities = new float[0];

    private float timeLastWatered = 0;

    [SerializeField]
    private float RegenDelay = 2.5f;

    [SerializeField]
    private float RegenRate = .1f;

    [SerializeField]
    private ParticleSystem[] firePsParticleSystem = new ParticleSystem[0];

    private bool isLit = true;

    private void ChangeIntensity()
    {
        for (int i = 0; i < firePsParticleSystem.Length; i++)
        {
            var emission = firePsParticleSystem[i].emission;
            emission.rateOverTime = currentIntensity * StartIntensities[i];

            var ps = firePsParticleSystem[i];
            if (currentIntensity <= 0f && ps.isPlaying)
            {
                ps.Stop();
            }
            else if (currentIntensity > 0f && !ps.isPlaying)
                ps.Play();
        }

    }

    public bool TryExtinguish(float amount)
    {
        timeLastWatered = Time.time;
        currentIntensity = Mathf.Clamp01(currentIntensity - amount);
        ChangeIntensity();
        isLit = currentIntensity > 0f;

        if (!isLit)
        {
            foreach (var ps in firePsParticleSystem)
                ps.Stop();

            // Disable collider when fire is extinguished
            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;
        }

        return !isLit; // return true if extinguished
    }

    void Start()
    {
        StartIntensities = new float[firePsParticleSystem.Length];
        for (int i = 0; i < firePsParticleSystem.Length; i++)
        {
            StartIntensities[i] = firePsParticleSystem[i].emission.rateOverTime.constant;
        }

    }

    void Update()
    {
        if (isLit && currentIntensity <= 1.0f && Time.time - timeLastWatered >= RegenDelay)
        {
            currentIntensity += RegenRate * Time.deltaTime;
            ChangeIntensity();
        }

    }
}
