using System.Collections.Generic;
using UnityEngine;

public class ChooseAssasin : MonoBehaviour
{
    public List<GameObject> attendeeList = new List<GameObject>(); 
    
    public void addAttendeeToList(GameObject attendee)
    {
        attendeeList.Add(attendee);
    }

    public void chooseAllAssasins(int numOfAssasins)
    {
        
    }
}
