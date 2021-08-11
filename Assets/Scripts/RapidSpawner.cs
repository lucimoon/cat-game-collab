using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidSpawner : MonoBehaviour
{
    public Rigidbody prefab;
    public int spawnCount = 1;
    public float spawnInterval = 0.5f;
    public Vector3 spawnDirection = Vector3.up;
    public float spawnStrength = 10f;

    void Start()
    {
        StartCoroutine("Spawn");
    }

    public IEnumerator Spawn () {
        Rigidbody clone;

        for (int i = 0; i < spawnCount; i++) {
            clone = (Rigidbody)GameObject.Instantiate(prefab, gameObject.transform);
            clone.AddForce(spawnDirection * spawnStrength);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
