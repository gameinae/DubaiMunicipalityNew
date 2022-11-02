using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchBox : MonoBehaviour
{
    public TMP_InputField SearchWordInputField;
    public List<Building> SearchList;
    public SearchDropDownList SearchDropDownList;

    public Transform District;
    public Transform ClearAllBtn;
    public Transform Background;

    bool isGameStarted;
    bool isSearchWordEmpty;

    private void Awake()
    {
        Background.gameObject.SetActive(false);
        var clearAllBtn = ClearAllBtn.gameObject.AddComponent<Button>();
        clearAllBtn.onClick.AddListener(ActivateAllBuildingOptions);
    }
    private void Start()
    {
        SetSearchBox();

        foreach (Transform T in District)
        {
            var bData = T.GetComponent<Building>();
            if (bData)
            {
                SearchList.Add(bData);
            }
        }
        SearchDropDownList.CreateOptionsList(SearchList);
        isGameStarted = true;
    }
    private void OnEnable()
    {
        if (isGameStarted)
        {
            GameManager.Instance.GridSystem.searchBox = this;
        }
    }
    private void Update()
    {
        //if (isSearchWordEmpty && SearchWordInputField.text == "") 
        //{
        //    isSearchWordEmpty = false;
        //    ActivateAllBuildingOptions();
        //}
    }

    private void HideDropDownPanel(string arg0)
    {
        Background.gameObject.SetActive(false);
    }
    private void ShowDropDownPanel(string arg0)
    {
        if (SearchWordInputField.text != "") Background.gameObject.SetActive(true);
    }
    public void DeactivateAllBuildingBtnsExceptForInSearch()
    {
        if (SearchList.Any(b => b.buildingData.name == SearchWordInputField.text))
        {
            int searchListLenght = SearchList.Count;
            for (int i = 0; i < searchListLenght; i++)
            {
                SearchList[i].gameObject.SetActive(SearchList[i].buildingData.name == SearchWordInputField.text);
            }
        }
    }
    public void ActivateAllBuildingOptions()
    {
        Debug.Log("Activate");
        foreach (Transform T in District)
        {
            T.gameObject.SetActive(true);
        }
        SearchWordInputField.text = "";
        SearchDropDownList.ActivateAllListOptions();
    }
    public void OnItemSelected(string searchWord)
    {
        SearchWordInputField.text = searchWord;
    }
    private void OnSearchKeyValueChange(string arg0)
    {
        SearchDropDownList.ActivateAllListOptions();
        SearchDropDownList.ChangeDropDownListOptions(SearchWordInputField.text.ToLower());
        isSearchWordEmpty = true;
        if (SearchWordInputField.text != "") Background.gameObject.SetActive(true);
        else if (SearchWordInputField.text == "") Background.gameObject.SetActive(false);
    }
    private void OnSubimt(string arg0)
    {
        Background.gameObject.SetActive(false);
    }
    private void SetSearchBox()
    {
        SearchWordInputField = GetComponent<TMP_InputField>();

        SearchWordInputField.onValueChanged.AddListener(OnSearchKeyValueChange);
        SearchWordInputField.onSelect.AddListener(ShowDropDownPanel);
        SearchWordInputField.onDeselect.AddListener(HideDropDownPanel);
        SearchWordInputField.onSubmit.AddListener(OnSubimt);

        GameManager.Instance.GridSystem.searchBox = this;
    }

}