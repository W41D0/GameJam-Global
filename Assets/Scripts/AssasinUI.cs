using UnityEngine;
using TMPro;

public class AssassinUI : MonoBehaviour
{
    public TextMeshProUGUI countText; 

    void Update()
    {
        int currentCount = AttendeeBehaviour.numOfAssasinsAlive;
        
        countText.text = "Targets: " + currentCount.ToString();
    }
}