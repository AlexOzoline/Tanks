using System.Collections.Generic;
using UnityEngine;

public class ParticlePooler : MonoBehaviour
{
    public GameObject particlePrefab; // The particle prefab (like the smoke effect)
    public int poolSize = 10;  // How many particle systems to pool initially

    private Queue<GameObject> particlePool = new Queue<GameObject>();  // The pool

    void Start()
    {
        // Initialize the pool by instantiating particle systems and deactivating them
        for (int i = 0; i < poolSize; i++)
        {
            GameObject particle = Instantiate(particlePrefab);
            particle.SetActive(false);  // Keep particles inactive initially
            particlePool.Enqueue(particle);  // Add to the pool
        }
    }

    // Get a particle system from the pool
    public GameObject GetParticle()
    {
        if (particlePool.Count > 0)
        {
            GameObject particle = particlePool.Dequeue();  // Dequeue an inactive particle
            particle.SetActive(true);  // Activate it
            return particle;
        }
        else
        {
            // If no particles are available, instantiate a new one
            GameObject newParticle = Instantiate(particlePrefab);
            return newParticle;
        }
    }

    // Return a particle system to the pool
    public void ReturnParticle(GameObject particle)
    {
        particle.SetActive(false);  // Deactivate it
        particlePool.Enqueue(particle);  // Return it to the pool
    }
}
