using UnityEngine;
using TMPro; // Keep this if using TextMeshPro

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText; 

    void Update()
    {
        float timeToDisplay = GameManager.currentTime;
        
        // This adds the label "Timer: " before the number
        // Example result: "Timer: 59"
        timerText.text = "Timer: " + Mathf.CeilToInt(timeToDisplay).ToString();

        // Optional: Turn red when under 10 seconds
        if(timeToDisplay <= 10)
        {
            timerText.color = Color.red;
        }
    }
}