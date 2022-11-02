using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FogLevelAcrossCity : MonoBehaviour
{
    [SerializeField] Image Image1;
    [SerializeField] TMP_Dropdown _Dropdown;
    [SerializeField] GameObject RedBlock, GreenBlock;
    [SerializeField] TextMeshProUGUI ValueText;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void DropDownvalue() {
        if (_Dropdown.value == 0) {//per month
            _FogLevelAcrossCityMonthly();
            RedBlock.SetActive(false); GreenBlock.SetActive(true);
        } else if (_Dropdown.value == 1) {//per Week
            _FogLevelAcrossCityWeekly();
            RedBlock.SetActive(true); GreenBlock.SetActive(false);

        }
        if (Image1.fillAmount >= .75f) {
            Image1.color = new Vector4(1, 0.7f, 0, 1);
        } else {
            Image1.color = new Vector4(1, 1, 1, 1);

        }
    }
    void _FogLevelAcrossCityMonthly() {
        Image1.fillAmount = .6f;
        Image1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "60%";
        ValueText.text = "200";

    }
    void _FogLevelAcrossCityWeekly() {
        Image1.fillAmount = .8f;
        Image1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "80%";
        if (Image1.fillAmount>=.75f) {
            Image1.color = new Vector4(1,0.7f,0,1);
        }
        ValueText.text = "800";

    }

}
