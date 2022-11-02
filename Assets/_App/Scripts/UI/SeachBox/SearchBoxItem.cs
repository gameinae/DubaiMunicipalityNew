using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchBoxItem : MonoBehaviour
{
    SearchBox searchBox;
    public string Name 
    {
        get => gameObject.name;
    }
    private void Awake()
    {
        searchBox = GameObject.FindObjectOfType<SearchBox>();
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }
    void OnClicked() 
    {
        searchBox.OnItemSelected(gameObject.name);
    }
}
