using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameManager : MonoBehaviour
{
    int initialAttendees;
    bool isSceneLoading = false;
    void Start()
    {
        initialAttendees = AttendeeBehaviour.numOfAttendeesAlive;
    }
    void Update()
    {
        if (!isSceneLoading && AttendeeBehaviour.numOfAssasinsAlive == 0)
        {
            StartCoroutine(LoadSceneAfterDelay(3));
        }
        if (!isSceneLoading && AttendeeBehaviour.numOfAttendeesAlive < initialAttendees)
        {
            StartCoroutine(LoadSceneAfterDelay(4));
        }
    }
    IEnumerator LoadSceneAfterDelay(int sceneIndex)
    {
        isSceneLoading = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(sceneIndex);
    }
    
}
