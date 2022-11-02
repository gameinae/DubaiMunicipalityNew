using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardElement : MonoBehaviour
{
    //cached
    public TextMeshProUGUI Name;
    public TextMeshProUGUI HealthPercentage;
    public TextMeshProUGUI Index;
    public Image Healthbar;

    public void SetElementValues(string name, float healthPercentage, int index, Sprite healthbar = null)
    {
        if (name.Length - 1 > 15)
        {
            int count = name.Length - 15;
            name = name.Remove(15, count);
            Name.text = name + "...";
        }
        else
        {
            Name.text = name;
        }
        HealthPercentage.text = healthPercentage.ToString();
        Index.text = index.ToString();
        Healthbar.fillAmount = healthPercentage / 100;

        if (healthbar)
        {
           Healthbar.sprite = healthbar;
        }
    }
}
