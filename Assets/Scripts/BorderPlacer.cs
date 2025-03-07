using UnityEngine;
using UnityEditor;

public class BorderPlacer : EditorWindow
{
    public GameObject prefab;
    public Material[] materials; // Array of materials to randomize
    private float cubeSizeX = 12f;
    private float cubeSizeZ = 12f;
    private float yPosition = 10f;

    private float minX = -204, maxX = 204;
    private float minZ = -96, maxZ = 96;

    [MenuItem("Tools/Border Placer")]
    public static void ShowWindow()
    {
        GetWindow<BorderPlacer>("Border Placer");
    }

    void OnGUI()
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
        yPosition = EditorGUILayout.FloatField("Y Position", yPosition);

        // Allow user to assign multiple materials
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty materialsProp = serializedObject.FindProperty("materials");
        EditorGUILayout.PropertyField(materialsProp, true);
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Place Border") && prefab != null)
        {
            PlaceBorder();
        }
    }

    void PlaceBorder()
    {
        if (prefab == null)
        {
            Debug.LogWarning("No prefab selected!");
            return;
        }

        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Place Border");

        // Find or create the "Border" parent object
        GameObject borderParent = GameObject.Find("Border");
        if (borderParent == null)
        {
            borderParent = new GameObject("Border");
            Undo.RegisterCreatedObjectUndo(borderParent, "Created Border Parent");
        }

        for (float x = minX; x <= maxX; x += cubeSizeX)
        {
            PlacePrefab(new Vector3(x, yPosition, minZ), borderParent);
            PlacePrefab(new Vector3(x, yPosition, maxZ), borderParent);
        }

        for (float z = minZ; z <= maxZ; z += cubeSizeZ)
        {
            PlacePrefab(new Vector3(minX, yPosition, z), borderParent);
            PlacePrefab(new Vector3(maxX, yPosition, z), borderParent);
        }

        Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
    }

    void PlacePrefab(Vector3 position, GameObject parent)
    {
        GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        newObj.transform.position = position;
        newObj.transform.SetParent(parent.transform);

        // Assign a random material if any are provided
        if (materials.Length > 0)
        {
            Material randomMat = materials[Random.Range(0, materials.Length)];
            Renderer renderer = newObj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = randomMat;
            }
        }

        Undo.RegisterCreatedObjectUndo(newObj, "Placed BorderCube");
    }
}
