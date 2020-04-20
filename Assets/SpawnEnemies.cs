using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{

    public Collider colliderPlane;
    public GameObject dudeToKillPrefab;
    public GameObject sucidialDude;
    public Transform targetFire;
    public float spawnSucidialDudeProbablity;
    public float spawnRadius;
    public float tick;
    public int tickModulo;
    public float tickProbability;
    public float tickIncrement;
    public int tickProgressionModulo;
    public float timeScale;

    fireSizeReducer reducer;

    // Start is called before the first frame update
    void Start()
    {
        reducer = GameObject.FindObjectOfType<fireSizeReducer>();
    }

    void spawnDude()
    {
        // spawn prefab
        Transform newDude = Instantiate(dudeToKillPrefab, transform).transform;
        newDude.position = transform.position + colliderPlane.ClosestPointOnBounds(Random.insideUnitSphere * spawnRadius) + Vector3.up;
        newDude.GetComponentInChildren<Stroll>().colliderPlane = colliderPlane;
        reducer.fuelCountInverse += 1;

        if (Random.value < spawnSucidialDudeProbablity)
        {
            newDude = Instantiate(sucidialDude, transform).transform;
            newDude.position = transform.position + colliderPlane.ClosestPointOnBounds(Random.insideUnitSphere * spawnRadius) + Vector3.up;
            newDude.GetComponentInChildren<suicide>().colliderPlane = colliderPlane;
            newDude.GetComponentInChildren<suicide>().targetFire = targetFire;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool doInc = false;
        if (Random.value < tickProbability) tick += Time.deltaTime * timeScale;

        if ((int) tick % tickModulo == 0) {
            doInc = true;
            spawnDude();
        }
        
        if ((int) tick % tickProgressionModulo == 0)
        {
            doInc = true;
            tickProbability += tickIncrement;
        }

        if (doInc) tick += 1;
    }
}
