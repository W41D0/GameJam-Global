using UnityEngine;

public class AttendeesSpawner : MonoBehaviour
{
    [SerializeField] GameObject attendee;
    [SerializeField] int numOfAttendees;
    [SerializeField] int numOfAssasins;
    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;

    GameObject paper;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.SyncDifficulty(ref numOfAssasins, ref numOfAttendees);
        paper = GameObject.Find("Paper");
        ChooseOutfit.usedOutfitHashes.Clear();
        AttendeeBehaviour.numOfAssasinsAlive = numOfAssasins;
        AttendeeBehaviour.numOfAttendeesAlive = numOfAttendees;
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

            GameObject currentAttendee = Instantiate(attendee, spawnPosition, Quaternion.Euler(0, 0, 0));
            string attendeeHash = currentAttendee.GetComponent<ChooseOutfit>().getUniqueHash();
            currentAttendee.GetComponent<AttendeeBehaviour>().setUniqueHash(attendeeHash);
        }
    }

    void SpawnAssasinScript(int numOfAssasins)
    {
        for(int i = 0; i < numOfAssasins; i++)
        {
            float posX = Random.Range(pos1.position.x, pos2.position.x);
            float posY = Random.Range(pos1.position.y, pos2.position.y);
            Vector2 spawnPosition = new Vector2(posX, posY);

            GameObject assasin = Instantiate(attendee, spawnPosition, Quaternion.Euler(0, 0, 0));
            assasin.GetComponent<AttendeeBehaviour>().setIsAssasin(true);
            string assasinHash = assasin.GetComponent<ChooseOutfit>().getUniqueHash();
            assasin.GetComponent<AttendeeBehaviour>().setUniqueHash(assasinHash);
            paper.GetComponent<AssasinAtributesChecklist>().addAssasinToHashList(assasinHash);
        }
    }
}
