using UnityEngine;

public class AttendeesSpawner : MonoBehaviour
{
    [SerializeField] GameObject attendee;
    [SerializeField] float spawnTime1 = 2f;
    [SerializeField] float spawnTime2 = 5f;
    [SerializeField] int numOfAttendees;

    GameObject spawner;

    float randomSpawnTime;
    Transform pos1;
    Transform pos2;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pos1 = transform.GetChild(0);
        pos2 = transform.GetChild(1);
        spawner = GameObject.FindGameObjectWithTag("Spawner");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    


    void SpawnScript(int numOfAtendees)
    {
        for(int i = 0; i < numOfAtendees; i++)
        {
            //sets the spawnTime of the next sheep
            randomSpawnTime = Random.Range(spawnTime1, spawnTime2);

            //sets the spawn location of the sheep
            float posX = Random.Range(pos1.position.x, pos2.position.x);
            float posY = Random.Range(pos1.position.y, pos2.position.y);
            Vector2 spawnPosition = new Vector2(posX, posY);

            GameObject newSheep = Instantiate(attendee, spawnPosition, Quaternion.Euler(0, 0, 0)); //spawns the sheep
        }
    }
}
