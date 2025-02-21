using UnityEngine;

public class CreateBorder : MonoBehaviour
{
    public GameObject borderCubePrefab; // Reference to the cube prefab
    public float planeWidth = 18f;      // Width of the plane
    public float planeLength = 10f;     // Length of the plane
    public float cubeHeight = 5f;     // Height to position cubes on

    void Start()
    {
        CreateBorderCubes();
    }

    void CreateBorderCubes()
    {
        // Create cubes along the edges of the plane

        // Front and Back edges (X axis)
        for (float x = -planeWidth / 2; x <= planeWidth / 2; x += 5)
        {
            Instantiate(borderCubePrefab, new Vector3(x, cubeHeight, -planeLength / 2), Quaternion.identity);
            Instantiate(borderCubePrefab, new Vector3(x, cubeHeight, planeLength / 2), Quaternion.identity);
        }

        // Left and Right edges (Z axis)
        for (float z = -planeLength / 2; z <= planeLength / 2; z += 5)
        {
            Instantiate(borderCubePrefab, new Vector3(-planeWidth / 2, cubeHeight, z), Quaternion.identity);
            Instantiate(borderCubePrefab, new Vector3(planeWidth / 2, cubeHeight, z), Quaternion.identity);
        }
    }
}
