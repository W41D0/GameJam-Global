using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class AttendeeBehaviour : MonoBehaviour
{
    public bool isAssasin;

    [Header("Movement")]
    [SerializeField] float speed = 5f;
    [SerializeField] float minIdleTime = 1f;
    [SerializeField] float maxIdleTime = 3f;

    GameObject spawner;
    Transform pos1;
    Transform pos2;

    GameObject perimeter;
    Rigidbody2D attendee;
    Vector2 randomPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        attendee = GetComponent<Rigidbody2D>();
        perimeter = GameObject.FindGameObjectWithTag("Spawner");
    }

    void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("Spawner");
        pos1 = GameObject.Find("Pos1").transform;
        pos2 = GameObject.Find("Pos2").transform;
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

    public bool getIsAssasin()
    {
        return isAssasin;
    }

    public void setIsAssasin(bool isAssasin)
    {
        this.isAssasin = isAssasin;
    }
}
