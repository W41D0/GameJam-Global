using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class AttendeeBehaviour : MonoBehaviour
{
    public bool isAssasin;
    bool isAlive = true;

    public static int numOfAssasinsAlive = 1;
    public static int numOfAttendeesAlive = 1;

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

    int defaultLayer;
    int ignoreLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        defaultLayer = gameObject.layer;
        ignoreLayer = LayerMask.NameToLayer("Ignore Raycast");
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
        if(isAlive)
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
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAssasin && collision.CompareTag("Attendee") && collision.GetComponent<AttendeeBehaviour>().getIsAssasin() == true)
        {
            gameObject.layer = ignoreLayer;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!isAssasin && collision.CompareTag("Attendee") && collision.GetComponent<AttendeeBehaviour>().getIsAssasin() == true)
        {
            gameObject.layer = defaultLayer;
        }
    }

    void OnMouseDown()
    {
        if(isAlive)
        {
            Debug.Log("Im a " + isAssasin + " assasin");
            isAlive = false;
            attendee.linearVelocity = Vector2.zero;
            GetComponentInChildren<PhysicsBobber2D>().TriggerDeath();
            
            if(isAssasin)
            {
                numOfAssasinsAlive -= 1;
            }
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
