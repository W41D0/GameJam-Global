using UnityEngine;

public class AttendeesSpawner : MonoBehaviour
{
    [SerializeField] GameObject attendee;
    [SerializeField] int numOfAttendees;
    [SerializeField] int numOfAssasins;
    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChooseOutfit.usedOutfitHashes.Clear();
        SpawnAssasinScript(numOfAssasins);
        SpawnScript(numOfAttendees);
    }

    void SpawnScript(int numOfAtendees)
    {
        for(int i = 0; i < numOfAtendees; i++)
        {
            float posX = Random.Range(pos1.position.x, pos2.position.x);
            float posY = Random.Range(pos1.position.y, pos2.position.y);
            Vector2 spawnPosition = new Vector2(posX, posY);

            Instantiate(attendee, spawnPosition, Quaternion.Euler(0, 0, 0));
        }
    }

    void SpawnAssasinScript(int numOfAssasins)
    {
        for(int i = 0; i < numOfAssasins; i++       )
        {
            float posX = Random.Range(pos1.position.x, pos2.position.x);
            float posY = Random.Range(pos1.position.y, pos2.position.y);
            Vector2 spawnPosition = new Vector2(posX, posY);

            GameObject assasin = Instantiate(attendee, spawnPosition, Quaternion.Euler(0, 0, 0));
            assasin.GetComponent<AttendeeBehaviour>().setIsAssasin(true);
        }
    }
}
