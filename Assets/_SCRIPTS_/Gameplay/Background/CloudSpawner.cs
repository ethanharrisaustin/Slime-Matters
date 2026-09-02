using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] ObjectPool objectPool;
    float timeBetweenClouds = 4f;

    float cloudTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        cloudTimer += Time.deltaTime;

        if (cloudTimer > timeBetweenClouds)
        {
            cloudTimer = 0f;

            SpawnCloud();
        }
    }

    void InitSpawn()
    {
        for (int i = 0; i < 10; ++i)
        {
            SpawnCloud(Random.Range(-20f, 0f));
        }
    }

    void SpawnCloud(float xPos = 0)
    {
        Cloud newCloud = objectPool.SpawnObject<Cloud>();

        newCloud.scale = Random.Range(0.5f, 1.5f);
        newCloud.transform.localPosition = new Vector3(xPos, Random.Range(-5f, 5f));
    }
}
