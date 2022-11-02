using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationElement : MonoBehaviour
{
    public TextMeshProUGUI MessageBody;
    public TextMeshProUGUI MessageHeader;
    public TextMeshProUGUI MessageDepartment;
    public Image notificationDangerLevelIndicatorImage; 

    private void Start()
    {
        //int Index = Random.Range(0,receivedNotification.Count-1);
        //NotificationModel notificationModel = GetMessageData(Index);

        //MessageBody.text = notificationModel.MessageBody;
        //MessageHeader.text = notificationModel.MessageHeader;
        //MessageDepartment.text = notificationModel.MessageDepartment;
    }
    //public NotificationModel GetMessageData(int messageIndex)
    //{
    //    NotificationModel receivedNotification = this.receivedNotification[messageIndex];
    //    return receivedNotification;
    //}
    //public void SetMessageData()
    //{
    //    foreach (var M in JsonMessage)
    //    {
    //        NotificationModel receivedNotification = JsonUtility.FromJson<NotificationModel>(M.text);
    //    }
    //}
}
