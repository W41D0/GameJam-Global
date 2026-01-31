using UnityEngine;
using TMPro; // Keep this if using TextMeshPro

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText; 

    void Update()
    {
        float timeToDisplay = GameManager.currentTime;
        timerText.text = "Timer: " + Mathf.CeilToInt(timeToDisplay).ToString();

        if(timeToDisplay <= 10)
        {
            timerText.color = Color.red;
        }
    }
}