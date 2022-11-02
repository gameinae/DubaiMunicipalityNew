using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationButton : MonoBehaviour
{
    public TextMeshProUGUI btnText;
    public Image btnImage;
    public Color btnTextColor;
    public Color selectedButTextColor;
    public Button btn;
    public GameObject notificationsListGameObject;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn?.onClick.AddListener(OnClick);
    }
    public void OnClick() 
    {
        NotificationNavigationButtons.instance.ResetButtonsStates();
        btnText.color = selectedButTextColor;
        btnImage.enabled = true;
        notificationsListGameObject.SetActive(true);
    }
    public void ResetButtonState() 
    {
        btnText.color = btnTextColor;
        btnImage.enabled = false;
        notificationsListGameObject.SetActive(false);
    }
}

