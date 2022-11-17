using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateOrDiactivatePanel : MonoBehaviour
{
    private DatePickerControl calendar;
    private void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(ActivateOrDeactivate);
        calendar = this.GetComponentInChildren<DatePickerControl>();
    }
    public void ActivateOrDeactivate()
    {
        if (calendar.gameObject.GetComponent<CanvasGroup>().alpha == 0)
        {
            calendar.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            calendar.gameObject.GetComponent<CanvasGroup>().interactable = true;
            calendar.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            calendar.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            calendar.gameObject.GetComponent<CanvasGroup>().interactable = false;
            calendar.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }
}
