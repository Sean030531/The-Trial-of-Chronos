using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    public GameObject coinPrefab; // Coin Prefab
    public float generateInterval = 1f; // Time between each generate

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateLoop());
    }

    private IEnumerator GenerateLoop()
    {
        // Repeat forever
        while (true)
        {
            // Generate from the position of the generator
            Vector3 generatePos = transform.position;

            // Generate coin
            Instantiate(coinPrefab, generatePos, Quaternion.identity);

            // Wait before generating the next one
            yield return new WaitForSeconds(generateInterval);
        }
    }
}