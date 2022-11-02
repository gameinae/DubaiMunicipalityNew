using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UseCase : MonoBehaviour
{
    Button button;
    TextMeshProUGUI text;
    Color textColor = new Color(.8f, .8f, .8f, 255);

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnClick() 
    {
        ActivateOption();
    }
    public void ActivateOption() 
    {
        text.color = Color.white;
        foreach (Transform T in transform)
        {
            T.gameObject.SetActive(true);
        }
    }
    public void DeactivateOption() 
    {
        text.color = textColor;
        foreach (Transform T in transform)
        {
            T.gameObject.SetActive(false);
        }
    }
}
