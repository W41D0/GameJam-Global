using System.Collections.Generic;
using UnityEngine;

public class ChooseAssasin : MonoBehaviour
{
    public List<GameObject> attendeeList = new List<GameObject>(); 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void addAttendeeToList(GameObject attendee)
    {
        attendeeList.Add(attendee);
    }

    public void chooseAllAssasins(int numOfAssasins)
    {
        
    }
}
