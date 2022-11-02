public class NotificationModel 
{
    public string MessageHeader { get; set; }
    public string MessageBody { get; set; }
    public string MessageDepartment { get; set; }

    public NotificationModel(string messageHeader, string messageBody, string messageDepartment) 
    {
        MessageBody = messageBody;
        MessageDepartment = messageDepartment;  
        MessageHeader = messageHeader;
    }
}
