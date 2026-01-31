using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // --- TIMER SETTINGS ---
    // STATIC: Remembers the current limit across scene reloads
    public static float levelTimeLimit = -1f; 
    public static float currentTime; // Timer current value
    
    [Header("Time Difficulty Settings")]
    public float defaultTime = 60f;   // Starting time (Level 1)
    public float timeDecrease = 5f;   // Seconds removed per win
    public float minTimeLimit = 10f;  // Hardest possible limit (cap)

    private bool timerIsRunning = false;

    // --- ATTENDEE/ASSASSIN SETTINGS ---
    public static int currentAssassins = -1;
    public static int currentAttendees = -1;
    private static int defaultAssassinsMemory;
    private static int defaultAttendeesMemory;

    [Header("Spawner Difficulty Settings")]
    public int assassinIncrease = 1;
    public int attendeeIncrease = 2;
    public int maxAssassins = 10;
    public int maxAttendees = 50;

    int initialAttendeesCount;
    bool isSceneLoading = false;

    // Helper to sync Spawner (Same as before)
    public static void SyncDifficulty(ref int spawnerAssassins, ref int spawnerAttendees)
    {
        if (currentAssassins == -1)
        {
            currentAssassins = spawnerAssassins;
            currentAttendees = spawnerAttendees;
            defaultAssassinsMemory = spawnerAssassins;
            defaultAttendeesMemory = spawnerAttendees;
        }
        else
        {
            spawnerAssassins = currentAssassins;
            spawnerAttendees = currentAttendees;
        }
    }

    void Start()
    {
        // 1. Initialize Time Limit if it's the very first run
        if (levelTimeLimit == -1f)
        {
            levelTimeLimit = defaultTime;
        }

        // 2. Set the current timer to the level's limit
        currentTime = levelTimeLimit;
        timerIsRunning = true;

        initialAttendeesCount = AttendeeBehaviour.numOfAttendeesAlive;
        
        // Debug check to see difficulty
        Debug.Log($"Level Start: Time={levelTimeLimit}s, Assassins={currentAssassins}, Attendees={currentAttendees}");
    }

    void Update()
    {
        // TIMER LOGIC
        if (timerIsRunning)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                timerIsRunning = false;
                HandleLoss(); // Time ran out = Lose
                StartCoroutine(LoadSceneAfterDelay(2));
            }
        }

        // WIN CONDITION
        if (timerIsRunning && !isSceneLoading && AttendeeBehaviour.numOfAssasinsAlive == 0)
        {
            timerIsRunning = false;
            HandleWin();
            StartCoroutine(LoadSceneAfterDelay(1));
        }

        // LOSE CONDITION (Civilian death)
        if (!isSceneLoading && AttendeeBehaviour.numOfAttendeesAlive < initialAttendeesCount)
        {
            timerIsRunning = false;
            HandleLoss();
            StartCoroutine(LoadSceneAfterDelay(2));
        }
    }

   void HandleWin()
    {
        FreezeAndRevealAgents(); // <--- ADD THIS

        // Increase Enemy Count
        currentAssassins = Mathf.Min(currentAssassins + assassinIncrease, maxAssassins);
        currentAttendees = Mathf.Min(currentAttendees + attendeeIncrease, maxAttendees);

        // Decrease Time
        levelTimeLimit = Mathf.Max(levelTimeLimit - timeDecrease, minTimeLimit);

        Debug.Log("Win! Difficulty increased.");
    }

    void HandleLoss()
    {
        FreezeAndRevealAgents(); // <--- ADD THIS

        // Reset Enemy Count
        currentAssassins = defaultAssassinsMemory;
        currentAttendees = defaultAttendeesMemory;

        // Reset Time
        levelTimeLimit = defaultTime;

        Debug.Log("Lost! Resetting all stats to default.");
    }

    IEnumerator LoadSceneAfterDelay(int sceneIndex)
    {
        isSceneLoading = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    void FreezeAndRevealAgents()
    {
        // 1. Find every Attendee/Assassin in the scene
        AttendeeBehaviour[] allAgents = FindObjectsByType<AttendeeBehaviour>(FindObjectsSortMode.None);

        foreach (AttendeeBehaviour agent in allAgents)
        {
            // Stop their movement
            agent.setCanMove(false);
            
            // Get all body parts (Head, Body, Clothes)
            SpriteRenderer[] allSprites = agent.GetComponentsInChildren<SpriteRenderer>(true);

            if (agent.gameObject.GetComponent<AttendeeBehaviour>().getIsAssasin()) 
            {
           
            }
            else
            {
                // EVERYONE ELSE (Alive Attendees AND Dead Bodies): Turn Darker/Grey
                foreach (SpriteRenderer sprite in allSprites)
                {
                    sprite.color = Color.gray; 
                }
            }
        }
    }
}