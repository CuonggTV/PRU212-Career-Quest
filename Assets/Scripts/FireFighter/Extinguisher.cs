using UnityEngine;

public class Extenguisher : MonoBehaviour
{
    [SerializeField]
    private float amountExtinguishedPerSecond = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 100f)
        && hit.collider.TryGetComponent(out Fire fire))
        {
            fire.TryExtinguish(amountExtinguishedPerSecond * Time.deltaTime);
            print(hit.collider.name);
        }
    }
}
