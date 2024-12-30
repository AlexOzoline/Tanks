using UnityEngine;

public class GenerateTankWithSizing : MonoBehaviour
{
    public Vector3 tankPosition = Vector3.zero; // Position of the tank
    public Material baseMaterial;              // Material for the tank base
    public Material turretMaterial;            // Material for the turret
    public Material barrelMaterial;            // Material for the barrel

    public Vector3 baseSize = new Vector3(1, 0.5f, 1);    // Size of the tank base
    public Vector3 turretSize = new Vector3(0.5f, 0.2f, 0.5f); // Size of the turret
    public Vector3 barrelSize = new Vector3(0.2f, 0.2f, 1);    // Size of the barrel

    public float barrelOffset = 0.75f; // Distance the barrel extends from the turret

    void Start()
    {
        CreateTank(tankPosition, baseSize, turretSize, barrelSize, barrelOffset);
    }

    void CreateTank(Vector3 position, Vector3 baseScale, Vector3 turretScale, Vector3 barrelScale, float barrelExtension)
    {
        // Create the Tank GameObject
        GameObject tank = new GameObject("Tank");

        // Create Tank Base
        GameObject tankBase = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tankBase.name = "TankBase";
        tankBase.transform.parent = tank.transform;
        tankBase.transform.localPosition = Vector3.zero;
        tankBase.transform.localScale = baseScale;
        if (baseMaterial != null)
        {
            tankBase.GetComponent<Renderer>().material = baseMaterial;
        }

        // Create Turret
        GameObject turret = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        turret.name = "Turret";
        turret.transform.parent = tank.transform;
        turret.transform.localPosition = new Vector3(0, baseScale.y / 2 + turretScale.y / 2, 0);
        turret.transform.localScale = turretScale;
        if (turretMaterial != null)
        {
            turret.GetComponent<Renderer>().material = turretMaterial;
        }

        // Create Barrel
        GameObject barrel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        barrel.name = "Barrel";
        barrel.transform.parent = turret.transform;
        barrel.transform.localPosition = new Vector3(0, 0, barrelExtension);
        barrel.transform.localScale = barrelScale;
        barrel.transform.localRotation = Quaternion.Euler(90, 0, 0); // Rotate to face forward
        if (barrelMaterial != null)
        {
            barrel.GetComponent<Renderer>().material = barrelMaterial;
        }

        // Set tank position
        tank.transform.position = position;

        Debug.Log("Tank created at position: " + position);
    }
}
