using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotifactionHolder : MonoBehaviour
{
    public List<Sprite> notificationDangerLevels;
    public List<NotificationElement> notificationElements;
    public GameObject notificationElement;
    public bool isFogLevel;

    private void Awake()
    {
        int index = notificationDangerLevels.Count - 1;
        foreach (Transform T in transform)
        {
            index = index < 0 ? notificationDangerLevels.Count - 1 : index == 0 ? notificationDangerLevels.Count - 1 : --index;
            NotificationElement ne = T.GetComponent<NotificationElement>();
            if (ne && isFogLevel)
            {
                float dangerLevel = Random.Range(0, 99);
                ne.MessageBody.text = "Fog Level : " + dangerLevel + "%";

                ne.notificationDangerLevelIndicatorImage.sprite = dangerLevel>80 ? notificationDangerLevels[0] : dangerLevel <= 80 && dangerLevel >60  ? notificationDangerLevels[1] : dangerLevel <=60 ? notificationDangerLevels[2] : notificationDangerLevels[2];
                notificationElements.Add(ne);
            }
            else if(ne)
            {
                ne.notificationDangerLevelIndicatorImage.sprite = notificationDangerLevels[index];
            }
        }
    }
    public void CreateNotification()
    {
        //GameObject notification = Instantiate(notificationElement,transform);
    }
}
