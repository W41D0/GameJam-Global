using UnityEngine;

public class AttendeesSpawner : MonoBehaviour
{
    [SerializeField] GameObject attendee;
    [SerializeField] int numOfAttendees;
    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnScript(numOfAttendees);
    }
    
    void SpawnScript(int numOfAtendees)
    {
        for(int i = 0; i < numOfAtendees; i++)
        {
            float posX = Random.Range(pos1.position.x, pos2.position.x);
            float posY = Random.Range(pos1.position.y, pos2.position.y);
            Vector2 spawnPosition = new Vector2(posX, posY);

            Instantiate(attendee, spawnPosition, Quaternion.Euler(0, 0, 0)); //spawns the sheep
        }
    }
}
