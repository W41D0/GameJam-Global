using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; // REQUIRED for the Text

public class GameManager : MonoBehaviour
{
    // --- AUDIO SETTINGS ---
    [Header("Sound Effects")]
    public AudioSource sfxSource;   
    public AudioClip loseClip;      
    public AudioClip winClip;       

    // --- SCORE SETTINGS ---
    public static int score = 0; 
    public TextMeshProUGUI scoreText; 

    // --- TIMER SETTINGS ---
    public static float levelTimeLimit = -1f; 
    public static float currentTime; 
    
    [Header("Time Difficulty Settings")]
    public float defaultTime = 60f;   
    public float timeDecrease = 5f;   
    public float minTimeLimit = 10f;  

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

    // --- SAFETY VARIABLES ---
    bool isSceneLoading = false;
    private bool canLose = false; // Prevents instant loss when level loads

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
        // 1. Initialize Time
        if (levelTimeLimit == -1f) levelTimeLimit = defaultTime;
        currentTime = levelTimeLimit;
        timerIsRunning = true;
        
        // 2. Update Score Display
        UpdateScoreText();

        // 3. START GRACE PERIOD (Fixes the instant loss/audio bug)
        StartCoroutine(EnableLossCheck());

        Debug.Log($"Level Start: Score={score}, Time={levelTimeLimit}");
    }

    // Waits 0.5s before checking for deaths to let the Spawner finish
    IEnumerator EnableLossCheck()
    {
        yield return new WaitForSeconds(0.5f);
        canLose = true;
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
                HandleLoss();
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

        // LOSE CONDITION (Modified for safety)
        // We check 'canLose' to make sure the level has fully loaded first
        if (!isSceneLoading && canLose && AttendeeBehaviour.numOfAttendeesAlive < currentAttendees)
        {
            timerIsRunning = false;
            HandleLoss();
            StartCoroutine(LoadSceneAfterDelay(2));
        }
    }

   void HandleWin()
    {
        FreezeAndRevealAgents();

        // SCORE INCREASE
        score++; 
        UpdateScoreText(); 

        if (sfxSource != null && winClip != null)
        {
            sfxSource.PlayOneShot(winClip);
        }

        currentAssassins = Mathf.Min(currentAssassins + assassinIncrease, maxAssassins);
        currentAttendees = Mathf.Min(currentAttendees + attendeeIncrease, maxAttendees);
        levelTimeLimit = Mathf.Max(levelTimeLimit - timeDecrease, minTimeLimit);

        Debug.Log("Win! Score is now: " + score);
    }

    void HandleLoss()
    {
        // SCORE RESET
        score = 0;

        if (BackgroundMusic.instance != null)
        {
            Destroy(BackgroundMusic.instance.gameObject);
        }

        if (sfxSource != null && loseClip != null)
        {
            sfxSource.PlayOneShot(loseClip);
        }

        FreezeAndRevealAgents();
        currentAssassins = defaultAssassinsMemory;
        currentAttendees = defaultAttendeesMemory;
        levelTimeLimit = defaultTime;

        Debug.Log("Lost! Score reset to 0.");
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Succesful Missions: " + score;
        }
    }

    IEnumerator LoadSceneAfterDelay(int sceneIndex)
    {
        isSceneLoading = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    void FreezeAndRevealAgents()
    {
        AttendeeBehaviour[] allAgents = FindObjectsByType<AttendeeBehaviour>(FindObjectsSortMode.None);

        foreach (AttendeeBehaviour agent in allAgents)
        {
            agent.setCanMove(false);
            
            SpriteRenderer[] allSprites = agent.GetComponentsInChildren<SpriteRenderer>(true);

            if (agent.getIsAssasin()) 
            {
                // ASSASSIN: Bright White
                foreach (SpriteRenderer sprite in allSprites)
                {
                    sprite.color = Color.white; 
                }
            }
            else
            {
                // INNOCENT: Grey
                foreach (SpriteRenderer sprite in allSprites)
                {
                    sprite.color = Color.gray; 
                }
            }
        }
    }
}