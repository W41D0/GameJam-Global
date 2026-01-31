using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // STATIC variables to remember difficulty across scenes
    public static int currentAssassins = -1; // -1 means "Not set yet"
    public static int currentAttendees = -1;

    // To remember the defaults for resetting
    private static int defaultAssassinsMemory;
    private static int defaultAttendeesMemory;

    // Difficulty settings
    public int assassinIncrease = 1;
    public int attendeeIncrease = 2;
    public int maxAssassins = 10;
    public int maxAttendees = 50;

    int initialAttendeesCount; // For the lose condition check
    bool isSceneLoading = false;

    // --- NEW: Helper method called by Spawner ---
    public static void SyncDifficulty(ref int spawnerAssassins, ref int spawnerAttendees)
    {
        // Case 1: First time playing (or after a full reset)
        if (currentAssassins == -1)
        {
            // We take the values FROM the Spawner
            currentAssassins = spawnerAssassins;
            currentAttendees = spawnerAttendees;

            // Memorize them so we can reset later
            defaultAssassinsMemory = spawnerAssassins;
            defaultAttendeesMemory = spawnerAttendees;
        }
        // Case 2: Round 2, 3, etc.
        else
        {
            // We force the Spawner to use OUR values
            spawnerAssassins = currentAssassins;
            spawnerAttendees = currentAttendees;
        }
    }

    void Start()
    {
        // Capture the count for the "Did anyone die?" check
        initialAttendeesCount = AttendeeBehaviour.numOfAttendeesAlive;
    }

    void Update()
    {
        // WIN CONDITION
        if (!isSceneLoading && AttendeeBehaviour.numOfAssasinsAlive == 0)
        {
            HandleWin();
            StartCoroutine(LoadSceneAfterDelay(1));
        }

        // LOSE CONDITION
        if (!isSceneLoading && AttendeeBehaviour.numOfAttendeesAlive < initialAttendeesCount)
        {
            HandleLoss();
            StartCoroutine(LoadSceneAfterDelay(2));
        }
    }

    void HandleWin()
    {
        // Increase difficulty
        currentAssassins = Mathf.Min(currentAssassins + assassinIncrease, maxAssassins);
        currentAttendees = Mathf.Min(currentAttendees + attendeeIncrease, maxAttendees);
        Debug.Log("Win! Increasing difficulty.");
    }

    void HandleLoss()
    {
        // Reset to the values we memorized at the very start
        currentAssassins = defaultAssassinsMemory;
        currentAttendees = defaultAttendeesMemory;
        Debug.Log("Lost! Resetting to defaults.");
    }

    IEnumerator LoadSceneAfterDelay(int sceneIndex)
    {
        isSceneLoading = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}