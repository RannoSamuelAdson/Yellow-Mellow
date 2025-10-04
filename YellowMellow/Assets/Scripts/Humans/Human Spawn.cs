using UnityEngine;

public class HumanSpawn : MonoBehaviour
{
    public Transform humanGoalpoint;             // Destination
    public Human humanPrefab;                    // Prefab to spawn
    public Human wealthyHumanPrefab;
    public QTEManager qteManager;

    public float humanSpawnFrequencyBase = 3f;   // Base spawn interval (seconds)
    public float humanSpeedBase = 3f;            // Base speed

    public float humanSpawnRandomness = 0.5f;    // % variation (0.5 = ±50%)
    public float humanSpeedRandomness = 0.2f;    // % variation (0.5 = ±50%)

    private float nextSpawnTime;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnHuman();
            ScheduleNextSpawn();
        }
    }

    private void SpawnHuman()
    {
        var wealthyChance = Random.Range(0f, 1f) < 0.25f;
        Human newHuman = Instantiate(wealthyChance ? wealthyHumanPrefab : humanPrefab, transform.position, Quaternion.identity);
        if (wealthyChance)
        {
            (newHuman as WealthyHuman).qteManager = qteManager;
        }
        newHuman.goalpoint = humanGoalpoint;

        // Randomize speed based on base ± randomness
        float speedFactor = Random.Range(1f - humanSpeedRandomness, 1f + humanSpeedRandomness);
        newHuman.Speed = humanSpeedBase * speedFactor;
    }

    private void ScheduleNextSpawn()
    {
        float spawnFactor = Random.Range(1f - humanSpawnRandomness, 1f + humanSpawnRandomness);
        nextSpawnTime = Time.time + humanSpawnFrequencyBase * spawnFactor;
    }
}
