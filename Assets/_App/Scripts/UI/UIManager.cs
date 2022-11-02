using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{ 
    public MainUICanvas mainUICanvas;

    private int currentBuilidingOption;
    private int currentDistrictOption;
    public GameObject screenCanvas;

    #region Building

    public void SetCurrentBuildingOptionInDropDownList(int currentBuildingIndex)
    {
        currentBuilidingOption = currentBuildingIndex;
    }

    public int GetCurrentBuildingOption()
    {
        return currentBuilidingOption;
    }

    #endregion

    #region District

    public int GetCurrentDistrictOption()
    {
        return currentDistrictOption;
    }

    #endregion
}
