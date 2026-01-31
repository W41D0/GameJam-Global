using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; // REQUIRED for the Text

public class GameManager : MonoBehaviour
{
    public static bool isGameOver = false;
    [Header("Sound Effects")]
    public AudioSource sfxSource;   
    public AudioClip loseClip;      
    public AudioClip winClip;       

    public static int score = 0; 
    public TextMeshProUGUI scoreText; 

    public static float levelTimeLimit = -1f; 
    public static float currentTime; 
    
    [Header("Time Difficulty Settings")]
    public float defaultTime = 60f;   
    public float timeDecrease = 5f;   
    public float minTimeLimit = 10f;  

    private bool timerIsRunning = false;

    public static int currentAssassins = -1;
    public static int currentAttendees = -1;
    private static int defaultAssassinsMemory;
    private static int defaultAttendeesMemory;

    [Header("Spawner Difficulty Settings")]
    public int assassinIncrease = 1;
    public int attendeeIncrease = 2;
    public int maxAssassins = 10;
    public int maxAttendees = 50;

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
        if (levelTimeLimit == -1f) levelTimeLimit = defaultTime;
        currentTime = levelTimeLimit;
        timerIsRunning = true;
        isGameOver = false;
        
        UpdateScoreText();

        StartCoroutine(EnableLossCheck());
        if (levelTimeLimit == -1f) levelTimeLimit = defaultTime;
    }

    IEnumerator EnableLossCheck()
    {
        yield return new WaitForSeconds(0.5f);
        canLose = true;
    }

    void Update()
    {
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

        if (timerIsRunning && !isSceneLoading && AttendeeBehaviour.numOfAssasinsAlive == 0)
        {
            timerIsRunning = false;
            HandleWin();
            StartCoroutine(LoadSceneAfterDelay(1));
        }

        if (!isSceneLoading && canLose && AttendeeBehaviour.numOfAttendeesAlive < currentAttendees)
        {
            timerIsRunning = false;
            HandleLoss();
            StartCoroutine(LoadSceneAfterDelay(2));
        }
    }

   void HandleWin()
    {
        isGameOver = true;
        FreezeAndRevealAgents();
        score++; 
        UpdateScoreText(); 

        if (sfxSource != null && winClip != null)
        {
            sfxSource.PlayOneShot(winClip);
        }

        currentAssassins = Mathf.Min(currentAssassins + assassinIncrease, maxAssassins);
        currentAttendees = Mathf.Min(currentAttendees + attendeeIncrease, maxAttendees);
        levelTimeLimit = Mathf.Max(levelTimeLimit - timeDecrease, minTimeLimit);
    }

    void HandleLoss()
    {
        isGameOver = true;
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
        if (SceneTransition.instance != null)
        {
            SceneTransition.instance.LoadLevel(sceneIndex);
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    void FreezeAndRevealAgents()
    {
        AttendeeBehaviour[] allAgents = FindObjectsByType<AttendeeBehaviour>(FindObjectsSortMode.None);

        foreach (AttendeeBehaviour agent in allAgents)
        {
            agent.setCanMove(false);
            
            SpriteRenderer[] allSprites = agent.GetComponentsInChildren<SpriteRenderer>(true);

            if (!agent.getIsAssasin()) 
            {
                foreach (SpriteRenderer sprite in allSprites)
                {
                    sprite.color = Color.gray; 
                }
            }
        }
    }
}