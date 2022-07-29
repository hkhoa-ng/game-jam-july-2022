using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject[] roomGameObjects;
    public Room[] rooms = new Room[16];

    [SerializeField] private int startIndex;
    public int currentIndex;
    [SerializeField] private int endIndex;
    [SerializeField] private int mainPathLength;

    private int minPathLength = 8;
    private int maxPathLength = 10;

    // 0: Left, 1: Right, 2: Up, 3: Down 
    private int direction;

    public GameObject player;
    Color healthyColor = new Color(44f/255f, 57f/255f, 42f/255f);
    Color junkColor = new Color(55f/255f, 42f/255f, 57f/255f);

    // Track the current world (true: Vegetable World, false: Snack World)
    private bool isHealthy;

    // Enemies Prefabs
    public GameObject[] healthyEnemiesPrefabs;
    public GameObject[] junkEnemiesPrefabs;
    public int minEnemies = 4;
    public int maxEnemies = 8;
    public int numOfEnemies;
    public int numOfRemainingEnemies;
    public float secondsBetweenSpawn = 2;
    private bool hasSpawned;

    // Boss Prefab
    public GameObject bossPrefab;

    // Spawn Manager variables
    public GameObject alertPrefab;
    private GameObject enemyToSpawn;
    private Vector3 positionToSpawnEnemy;
    public float alertTime = 0.5f;

    // Power-up Prefabs
    int waveTillPowerUp;
    public GameObject[] gunsAndPowerUpPrefabs;
    public GameObject[] powerupPrefabs;


    // Start is called before the first frame update
    void Start()
    {
        waveTillPowerUp = 3;
        isHealthy = false;

        startIndex = Random.Range(0, 16);
        currentIndex = startIndex;
        mainPathLength = Random.Range(minPathLength, maxPathLength + 1);


        for (int i = 0; i < 16; i++)
        {
            rooms[i] = roomGameObjects[i].GetComponent<Room>();
        }

        int maxTry = 4;
        // Initialise main path
        for (int i = 0; i < mainPathLength; i++)
        {
            direction = Random.Range(0, 4);
            if (direction == 0 && rooms[currentIndex].portalLeft != null && !rooms[currentIndex - 1].isInitialised)
            {
                maxTry = 4;
                rooms[currentIndex].portalLeft.GetComponent<PortalTransition>().isOpenable = true;
                rooms[currentIndex].isInitialised = true;
                currentIndex -= 1;
            }
            else if (direction == 1 && rooms[currentIndex].portalRight != null && !rooms[currentIndex + 1].isInitialised)
            {
                maxTry = 4;
                rooms[currentIndex].portalRight.GetComponent<PortalTransition>().isOpenable = true;
                rooms[currentIndex].isInitialised = true;
                currentIndex += 1;
            }
            else if (direction == 2 && rooms[currentIndex].portalTop != null && !rooms[currentIndex - 4].isInitialised)
            {
                maxTry = 4;
                rooms[currentIndex].portalTop.GetComponent<PortalTransition>().isOpenable = true;
                rooms[currentIndex].isInitialised = true;
                currentIndex -= 4;
            }
            else if (direction == 3 && rooms[currentIndex].portalBottom != null && !rooms[currentIndex + 4].isInitialised)
            {
                maxTry = 4;
                rooms[currentIndex].portalBottom.GetComponent<PortalTransition>().isOpenable = true;
                rooms[currentIndex].isInitialised = true;
                currentIndex += 4;
            }
            else
            {
                if (maxTry > 0)
                {
                    maxTry--;
                    i--;
                }
            }
            if (i == mainPathLength - 1) endIndex = currentIndex;
        }
        player.transform.position = roomGameObjects[startIndex].transform.position;
        Camera.main.transform.position = roomGameObjects[startIndex].transform.position + new Vector3(0, 0, -10);
        currentIndex = startIndex;

        // Initialise the rest of the rooms
        for (int i = 0; i < 16; i++)
        {
            if (!rooms[i].isInitialised)
            {
                int rand; 
                rand = Random.Range(0,5); // Chance of open is 1/5
                if (rand == 0 && rooms[i].portalLeft != null)
                {
                    rooms[i].portalLeft.GetComponent<PortalTransition>().isOpenable = true;
                }
                rand = Random.Range(0, 5);
                if (rand == 0 && rooms[i].portalRight != null)
                {
                    rooms[i].portalRight.GetComponent<PortalTransition>().isOpenable = true;
                }
                rand = Random.Range(0, 5);
                if (rand == 0 && rooms[i].portalTop != null)
                {
                    rooms[i].portalTop.GetComponent<PortalTransition>().isOpenable = true;
                }
                rand = Random.Range(0, 5);
                if (rand == 0 && rooms[i].portalBottom != null)
                {
                    rooms[i].portalBottom.GetComponent<PortalTransition>().isOpenable = true;
                }
                rooms[i].isInitialised = true;
            }
        }

        // Trigger start Room event
        rooms[startIndex].isEntered = true;
        OpenDoors(startIndex);
        isHealthy = ((startIndex % 4) % 2) != ((startIndex / 4) % 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRoomEnter(int roomIndex)
    {
        Camera.main.transform.position = roomGameObjects[roomIndex].transform.position + new Vector3(0, 0, -10);
        isHealthy = !isHealthy;
        if (isHealthy)
        {
            Camera.main.backgroundColor = healthyColor;
        }
        else
        {
            Camera.main.backgroundColor = junkColor;
        }
        if (rooms[roomIndex].isEntered)
        {
            OpenDoors(roomIndex);
        }
        else
        {
            hasSpawned = false;
            CloseDoors(roomIndex);
            if (roomIndex != endIndex)
            {
                int random = Random.Range(0, 5); // 20% to be a treasure room
                if (random == 0 || waveTillPowerUp <= 0)
                {
                    int randomIndex = Random.Range(0, gunsAndPowerUpPrefabs.Length);
                    Instantiate(gunsAndPowerUpPrefabs[randomIndex], rooms[roomIndex].transform.position, Quaternion.identity);
                    OpenDoors(roomIndex);
                    waveTillPowerUp = 3;
                    rooms[roomIndex].isEntered = true;

                }
                else // Spawn normal wave
                {
                    numOfEnemies = Random.Range(minEnemies, maxEnemies);
                    StartCoroutine(SpawnEnemies(numOfEnemies));
                    waveTillPowerUp--;
                }
            }
            else
            {
                Vector3 bossSpawnPos = rooms[roomIndex].transform.position;
                GameObject alert = Instantiate(alertPrefab, bossSpawnPos, Quaternion.identity);
                alert.GetComponent<SpawnEnemyFromAlert>().enemyPrefab = bossPrefab;
                alert.GetComponent<SpawnEnemyFromAlert>().spawnPos = bossSpawnPos;
                alert.GetComponent<SpawnEnemyFromAlert>().spawnTimer = alertTime;
                hasSpawned = true;
                StartCoroutine(CheckBossRoom());
            }
        }
    }

    // Check if spawn position is currently occupied or not
    private bool CheckValidSpawn(Vector3 spawnPos)
    {
        return !Physics.CheckSphere(spawnPos, 2);
    }

    IEnumerator SpawnEnemies(int numOfEnemies)
    {
        float minSpawnX = roomGameObjects[currentIndex].transform.position.x - 5;
        float minSpawnY = roomGameObjects[currentIndex].transform.position.y - 5;
        float maxSpawnX = roomGameObjects[currentIndex].transform.position.x + 5;
        float maxSpawnY = roomGameObjects[currentIndex].transform.position.y + 5;
        while (numOfEnemies > 0)
        {
            int enemiesThisWave = Random.Range(1, 3);
            for (int i = 0; i < enemiesThisWave; i++)
            {
                // Generate spawn Position
                Vector3 spawnPos = new Vector3(Random.Range(minSpawnX, maxSpawnX), Random.Range(minSpawnY, maxSpawnY), 0);
                while (!CheckValidSpawn(spawnPos))
                {
                    spawnPos = new Vector3(Random.Range(minSpawnX, maxSpawnX), Random.Range(minSpawnY, maxSpawnY), 0);
                }

                // Spawn enemies
                GameObject[] enemies;
                if (isHealthy)
                {
                    enemies = healthyEnemiesPrefabs;
                }
                else
                {
                    enemies = junkEnemiesPrefabs;
                }
                int randomIndex = Random.Range(0, enemies.Length);
                // Instantiate(enemies[randomIndex], spawnPos, Quaternion.identity);
                // SpawnAnEnemy(enemies[randomIndex], spawnPos, Quaternion.identity);
                GameObject alert = Instantiate(alertPrefab, spawnPos, Quaternion.identity);
                alert.GetComponent<SpawnEnemyFromAlert>().enemyPrefab = enemies[randomIndex];
                alert.GetComponent<SpawnEnemyFromAlert>().spawnPos = spawnPos;
                alert.GetComponent<SpawnEnemyFromAlert>().spawnTimer = alertTime;
            }
            numOfEnemies -= enemiesThisWave;
            yield return new WaitForSeconds(alertTime);
            hasSpawned = true;
            yield return new WaitForSecondsRealtime(secondsBetweenSpawn);
        }
        StartCoroutine(CheckRoom());
    }

    IEnumerator CheckBossRoom()
    {
        yield return new WaitForSeconds(alertTime);
        while (true)
        {
            yield return new WaitForSeconds(1);
            numOfRemainingEnemies = GameObject.FindGameObjectsWithTag("Boss").Length + GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (numOfRemainingEnemies == 0 && hasSpawned)
            {
                OpenDoors(currentIndex);
                rooms[currentIndex].isEntered = true;
                SceneManager.LoadScene(3);
                yield break;
            }
        }
    }

    IEnumerator CheckRoom()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            numOfRemainingEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (numOfRemainingEnemies == 0 && hasSpawned)
            {
                if (Random.Range(0, 2) == 0)
                {
                    // Spawn a power up as reward
                    int randomIndex = Random.Range(0, powerupPrefabs.Length);
                    Instantiate(powerupPrefabs[randomIndex], rooms[currentIndex].transform.position, Quaternion.identity);
                }

                OpenDoors(currentIndex);
                rooms[currentIndex].isEntered = true;
                yield break;
            }
        }
    }

    private void OpenDoors(int roomIndex)
    {

        if (rooms[roomIndex].portalLeft != null && rooms[roomIndex].portalLeft.GetComponent<PortalTransition>().isOpenable)
        {
            rooms[roomIndex].portalLeft.GetComponent<PortalTransition>().isActive = true;
        }
        if (rooms[roomIndex].portalRight != null && rooms[roomIndex].portalRight.GetComponent<PortalTransition>().isOpenable)
        {
            rooms[roomIndex].portalRight.GetComponent<PortalTransition>().isActive = true;
        }
        if (rooms[roomIndex].portalBottom != null && rooms[roomIndex].portalBottom.GetComponent<PortalTransition>().isOpenable)
        {
            rooms[roomIndex].portalBottom.GetComponent<PortalTransition>().isActive = true;
        }
        if (rooms[roomIndex].portalTop != null && rooms[roomIndex].portalTop.GetComponent<PortalTransition>().isOpenable)
        {
            rooms[roomIndex].portalTop.GetComponent<PortalTransition>().isActive = true;
        }
    }

    // Close door to spawn enemies
    private void CloseDoors(int roomIndex)
    {
        if (rooms[roomIndex].portalLeft != null)
        {
            rooms[roomIndex].portalLeft.GetComponent<PortalTransition>().isActive = false;
        }
        if (rooms[roomIndex].portalRight != null)
        {
            rooms[roomIndex].portalRight.GetComponent<PortalTransition>().isActive = false;
        }
        if (rooms[roomIndex].portalBottom != null)
        {
            rooms[roomIndex].portalBottom.GetComponent<PortalTransition>().isActive = false;
        }
        if (rooms[roomIndex].portalTop != null)
        {
            rooms[roomIndex].portalTop.GetComponent<PortalTransition>().isActive = false;
        }
    }
}
