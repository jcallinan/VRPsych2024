using System.Collections;
using UnityEngine;

public class ReefGeneratorScript : MonoBehaviour
{
    public GameObject[] coralPrefabs;
    public int numberOfCorals = 100;
    public Vector3 areaSize = new Vector3(50, 10, 50);
    public float dropHeight = 10f; // Height from which corals will be dropped

    // Optional: Variability in scale
    public float minScale = 0.5f;
    public float maxScale = 2.0f;

    void Start()
    {
        GenerateCorals();
    }

    void GenerateCorals()
    {
        for (int i = 0; i < numberOfCorals; i++)
        {
            float scale = Random.Range(minScale, maxScale);
            Vector3 position = new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                transform.position.y + 10, // Start above the transform position to let corals fall
                Random.Range(-areaSize.z / 2, areaSize.z / 2)
            );

            Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            GameObject coralPrefab = coralPrefabs[Random.Range(0, coralPrefabs.Length)];
            GameObject coral = Instantiate(coralPrefab, position, rotation);
            coral.transform.localScale = new Vector3(scale, scale, scale);

            // Add Rigidbody and configure it
            Rigidbody rb = coral.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            // Set coral to drop
            StartCoroutine(SettleCoral(coral, rb));
        }
    }

    IEnumerator SettleCoral(GameObject coral, Rigidbody rb)
    {
        // Wait for coral to potentially settle
        yield return new WaitForSeconds(2.0f); // Adjust based on your needs

        // Check if the coral has nearly stopped moving
        if (rb.velocity.magnitude < 0.01f)
        {
            // Remove the Rigidbody component
            Destroy(rb);
        }
        else
        {
            // If still moving, wait a bit more and check again
            yield return new WaitForSeconds(1.0f); // Adjust based on your needs
            Destroy(rb); // Remove Rigidbody regardless of movement to prevent endless loop
        }
    }

    IEnumerator RemovePhysicsComponentsAfterDelay(Rigidbody rb, Collider collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(rb);
        Destroy(collider);
    }
}
