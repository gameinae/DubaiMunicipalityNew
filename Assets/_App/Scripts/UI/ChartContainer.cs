using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartContainer : MonoBehaviour
{
    public List<Transform> children = new List<Transform>();

    private void OnEnable()
    {
        foreach (Transform child in children) 
        {
            child.gameObject.SetActive(true);
        }
    }
    private void OnDisable()
    {
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(false);
        }
    }
}
