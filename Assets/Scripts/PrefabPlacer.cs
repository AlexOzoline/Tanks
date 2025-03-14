using UnityEngine;
using UnityEditor;

public class BulkPrefabPlacer : EditorWindow
{
    public GameObject prefab;
    public Material[] materials;
    private string coordinatesInput = ""; // Input for bulk coordinates

    [MenuItem("Tools/Bulk Prefab Placer")]
    public static void ShowWindow()
    {
        GetWindow<BulkPrefabPlacer>("Bulk Prefab Placer");
    }

    void OnGUI()
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        // Allow multiple materials
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty materialsProp = serializedObject.FindProperty("materials");
        EditorGUILayout.PropertyField(materialsProp, true);
        serializedObject.ApplyModifiedProperties();

        // Coordinate input box
        GUILayout.Label("Coordinates (x,y,z) - One per line");
        coordinatesInput = EditorGUILayout.TextArea(coordinatesInput, GUILayout.Height(100));

        if (GUILayout.Button("Place Prefabs") && prefab != null)
        {
            PlacePrefabs();
        }
    }

    void PlacePrefabs()
    {
        if (prefab == null)
        {
            Debug.LogWarning("No prefab selected!");
            return;
        }

        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Place Prefabs");

        // Create/find the parent object
        GameObject parent = GameObject.Find("PlacedObjects") ?? new GameObject("PlacedObjects");
        Undo.RegisterCreatedObjectUndo(parent, "Created Parent Object");

        string[] lines = coordinatesInput.Split('\n');

        foreach (string line in lines)
        {
            if (TryParseCoordinates(line, out Vector3 position))
            {
                PlacePrefab(position, parent);
            }
            else
            {
                Debug.LogWarning($"Invalid coordinates: {line}");
            }
        }

        Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
    }

    void PlacePrefab(Vector3 position, GameObject parent)
    {
        GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        newObj.transform.position = position;
        newObj.transform.SetParent(parent.transform);

        // Random material assignment
        if (materials.Length > 0)
        {
            Material randomMat = materials[Random.Range(0, materials.Length)];
            Renderer renderer = newObj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = randomMat;
            }
        }

        Undo.RegisterCreatedObjectUndo(newObj, "Placed Prefab");
    }

    bool TryParseCoordinates(string input, out Vector3 position)
    {
        position = Vector3.zero;
        string[] parts = input.Trim().Split(',');
        if (parts.Length == 3 &&
            float.TryParse(parts[0], out float x) &&
            float.TryParse(parts[1], out float y) &&
            float.TryParse(parts[2], out float z))
        {
            position = new Vector3(x, y, z);
            return true;
        }
        return false;
    }
}
