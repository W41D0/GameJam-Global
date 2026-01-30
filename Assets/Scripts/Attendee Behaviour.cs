using UnityEngine;
using System.Collections;

public class AttendeeBehaviour : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float minIdleTime = 1f;
    [SerializeField] float maxIdleTime = 3f;
    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;

    GameObject perimeter;
    Rigidbody2D attendee;
    Vector2 randomPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        attendee = GetComponent<Rigidbody2D>();
        perimeter = GameObject.FindGameObjectWithTag("Spawner");
        StartCoroutine(ChooseRandomPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(attendee.position, randomPosition) < 0.1f)
        {
            attendee.linearVelocity = Vector2.zero;
        }
        else
        {
            attendee.linearVelocity = (randomPosition - attendee.position).normalized * speed;   
            Debug.Log("my speed is: " + attendee.linearVelocity);
        } 
    }

    IEnumerator ChooseRandomPosition()
    {
        while (true)
        {
            float posX = Random.Range(pos1.position.x, pos2.position.x);
            float posY = Random.Range(pos1.position.y, pos2.position.y);
            randomPosition = new(posX, posY);
            float timeIdled = Random.Range(minIdleTime, maxIdleTime);
            yield return new WaitForSeconds(timeIdled);
        }
    }
}
