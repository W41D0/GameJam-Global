using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameManager : MonoBehaviour
{
    int initialAttendees;
    void Start()
    {
        initialAttendees = AttendeeBehaviour.numOfAttendeesAlive;
    }
    void Update()
    {
        if (AttendeeBehaviour.numOfAssasinsAlive == 0)
        {
            SceneManager.LoadSceneAsync(3);
        }
        if (AttendeeBehaviour.numOfAttendeesAlive < initialAttendees)
        {
            SceneManager.LoadSceneAsync(4);
        }
    }
    
}
