using System.Collections.Generic;
using UnityEngine;

using Randy = System.Random;

public class Spawner : MonoBehaviour
{
    public LayerMask spawnedObjectLayer;
    public GameObject spawnedObjectPrefab;
    public bool isBeneficialSpawnPoint;
    public float baseSpawnMaxTime;
    float spawnMaxTime;
    float randomizedSpawnMaxTime;
    float spawnTimer;

    const int RandomizePercent = 25;

    List<Transform> spawnPoints;

    Randy randy;


    void Start()
    {
        spawnMaxTime = isBeneficialSpawnPoint ? baseSpawnMaxTime * PlayerStats.GameDifficulty : baseSpawnMaxTime / PlayerStats.GameDifficulty;

        spawnPoints = new List<Transform>();

        foreach (Transform child in this.transform)
        {
            RemoveEditorStuff(child);

            spawnPoints.Add(child);
        }

        if (spawnPoints.Count < 1)
        {
            Debug.LogWarning("There ain't so spawn points here, son.");
            Destroy(this.gameObject);
        }

        randy = new Randy(System.Guid.NewGuid().GetHashCode());
        randomizedSpawnMaxTime = getNextRandomizedMaxTime();
    }

    float getNextRandomizedMaxTime()
    {
        float randomizePercentFloat = randy.Next(RandomizePercent) / 100.0f;

        if (randy.Next() % 2 == 0)
        {
            randomizePercentFloat = -randomizePercentFloat;
        }

        float ret = spawnMaxTime + spawnMaxTime * randomizePercentFloat;
        return ret;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime * Time.timeScale;

        if (spawnTimer >= randomizedSpawnMaxTime)
        {
            randomizedSpawnMaxTime = getNextRandomizedMaxTime();
            spawnTimer = 0;

            // Spawn object at some spawn point, if there isn't an object there already.
            // Bail out if still couldn't spawn in spawnPoint.Count attempts.

            for (int i = 0; i < spawnPoints.Count; i++)
            {
                int index = randy.Next(spawnPoints.Count);
                Vector3 pos = spawnPoints[index].position;
                bool hit = Physics.CheckSphere(pos, 1f, spawnedObjectLayer, QueryTriggerInteraction.Collide);

                if (!hit)
                {
                    GameObject pickup = Instantiate(spawnedObjectPrefab);
                    pickup.transform.parent = spawnPoints[index];
                    pickup.transform.localPosition = Vector3.zero;
                    pickup.transform.parent = null;
                    break;
                }
            }
            
        }
    }

    void RemoveEditorStuff(Transform child)
    {
        // The mesh is for visibility within the editor, not used in game.
        MeshRenderer mr = child.GetComponent<MeshRenderer>();
        if (mr)
        {
            Destroy(mr);
        }

        MeshFilter mf = child.GetComponent<MeshFilter>();
        if (mf)
        {
            Destroy(mf);
        }

        Collider c = child.GetComponent<Collider>();
        if (c)
        {
            Destroy(c);
        }
    }
}


