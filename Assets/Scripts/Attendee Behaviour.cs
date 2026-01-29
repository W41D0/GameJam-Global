using UnityEngine;
using System.Collections;

public class AttendeeBehaviour : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float minIdleTime = 1f;
    [SerializeField] float maxIdleTime = 3f;

    GameObject perimeter;
    Transform pos1;
    Transform pos2;
    Rigidbody2D attendee;
    Vector2 randomPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        attendee = GetComponent<Rigidbody2D>();
        perimeter = GameObject.FindGameObjectWithTag("Spawn");
        pos1 = perimeter.transform.GetChild(0);
        pos2 = perimeter.transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
         attendee.MovePosition(Vector2.MoveTowards(attendee.position, randomPosition, speed * 2 * Time.deltaTime));
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
