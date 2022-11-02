using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LeaderBoardUI : MonoBehaviour
{
    [SerializeField] float ThresholdValue1, ThresholdValue2;
    [SerializeField] Sprite[] ColourUI ;
    [SerializeField] GameObject ContentHolder;
    [SerializeField] TMP_Dropdown _Dropdown;
    float[] TempValues;
    // Start is called before the first frame update
    void Start()
    {
        TempValues = new float[8];

    }

    public void _DropDownValue() {
        Debug.Log(_Dropdown.value);
        AssignValues(_Dropdown.value);
        for (int i = 0; i < ContentHolder.transform.childCount; i++) {
            ContentHolder.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().fillAmount = TempValues[i];
            ContentHolder.transform.GetChild(i).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (TempValues[i] * 100).ToString() + "%";
        }
       
        AssignColours();
    }
    void AssignColours() {
        for (int i = 0; i < ContentHolder.transform.childCount; i++) {
            if (TempValues[i]>= ThresholdValue1) {
                ContentHolder.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = ColourUI[0];

            }else if(TempValues[i] >= ThresholdValue2) {
                ContentHolder.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = ColourUI[1];

            } else {
                ContentHolder.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = ColourUI[2];

            }
        }
    }
    void AssignValues(int i) {
       
        if (i==0) {
             TempValues[0] = .95f;
             TempValues[1] = .85f;
             TempValues[2] = .55f;
             TempValues[3] = .25f;
             TempValues[4] = .15f;
             TempValues[5] = .45f;
             TempValues[6] = .25f;
             TempValues[7] = .25f;
        } else {
            TempValues[0] = .85f;
            TempValues[1] = .75f;
            TempValues[2] = .45f;
            TempValues[3] = .15f;
            TempValues[4] = .25f;
            TempValues[5] = .95f;
            TempValues[6] = .45f;
            TempValues[7] = .35f;
        }
        
    }
}
