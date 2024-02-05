using UnityEngine;

public class CoralGenerator : MonoBehaviour
{
    public GameObject[] coralPrefabs; // Array of coral prefabs to instantiate
    public int numberOfCorals = 100; // Number of coral instances to create
    public Vector3 areaSize = new Vector3(50, 10, 50); // Size of the area to distribute corals
    public float waterLevel = 0f; // The y-level representing the water surface

    // Optional: Variability in scale
    public float minScale = 0.5f;
    public float maxScale = 2.0f;
    public float someVerticalOffset = 0.0f;
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
                0, // This will be adjusted based on terrain height
                Random.Range(-areaSize.z / 2, areaSize.z / 2)
            ) + transform.position;

            // Adjust y based on terrain height or a predefined floor level
            position.y = GetTerrainHeightAt(position.x, position.z) + scale * someVerticalOffset; // Adjust for object size if necessary

            Quaternion rotation = Quaternion.Euler(
                Random.Range(0, 360),
                Random.Range(0, 360),
                Random.Range(0, 360)
            );

            GameObject coralPrefab = coralPrefabs[Random.Range(0, coralPrefabs.Length)];
            GameObject coral = Instantiate(coralPrefab, position, rotation, transform);
            coral.transform.localScale = new Vector3(scale, scale, scale);

            // Since we're not using Rigidbody, no need for further physics adjustments
        }
    }
    float GetTerrainHeightAt(float x, float z)
    {
        // Convert global position to terrain's local position
        Vector3 terrainPosition = transform.position;
        Vector3 objectPosition = new Vector3(x, 0, z);

        // Assuming there's only one terrain in the scene. If there are multiple, you might need to find the correct one based on position.
        Terrain terrain = Terrain.activeTerrain;
        if (terrain != null)
        {
            // Calculate position relative to terrain
            Vector3 terrainLocalPos = objectPosition - terrain.transform.position;
            // Normalize the position based on terrain size
            Vector2 normalizedPos = new Vector2(
                terrainLocalPos.x / terrain.terrainData.size.x,
                terrainLocalPos.z / terrain.terrainData.size.z
            );

            // Get the height at the normalized position
            float y = terrain.terrainData.GetHeight(
                Mathf.RoundToInt(normalizedPos.x * terrain.terrainData.heightmapResolution),
                Mathf.RoundToInt(normalizedPos.y * terrain.terrainData.heightmapResolution)
            );

            // Return the global height
            return terrain.transform.position.y + y;
        }

        // Default to 0 or another appropriate value if no terrain is found
        return 0;
    }






    // Optionally, draw the spawning area in the editor for visualization
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, areaSize);
    }
}
