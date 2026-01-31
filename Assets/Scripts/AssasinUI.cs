using UnityEngine;
using TMPro; // Required for TextMeshPro

public class AssassinUI : MonoBehaviour
{
    public TextMeshProUGUI countText; 

    void Update()
    {
        // Reads the static variable from your Attendee script
        int currentCount = AttendeeBehaviour.numOfAssasinsAlive;
        
        // Updates text to look like "Assassins: 5"
        countText.text = "Assassins: " + currentCount.ToString();
    }
}