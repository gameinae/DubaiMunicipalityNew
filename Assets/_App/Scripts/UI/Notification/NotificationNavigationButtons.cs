using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationNavigationButtons : MonoBehaviour
{
    public List<NotificationButton> notificationButtons = new List<NotificationButton>();
    public static NotificationNavigationButtons instance;

    private void Awake()
    {
        instance = this;
    }
    public void ResetButtonsStates()
    {
        foreach (var button in notificationButtons) 
        {
            button.ResetButtonState();
        }
    }
}
