using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerDistrictHighlighter : MonoBehaviour
{
    private Camera m_camera;
    private Material districtMat;

    void Start()
    {
        m_camera = Camera.main;
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            var newMaterial = hit.collider.gameObject.GetComponent<Renderer>()?.material;
            SetDistrictMaterial(newMaterial);
        }
    }

    void SetDistrictMaterial(Material value) 
    {
        if (value && value == districtMat)
        {
            return;
        }
        else if(value)
        {
            districtMat?.DisableKeyword("_EMISSION");
            districtMat = value;
            districtMat.EnableKeyword("_EMISSION");
        }
    }
}
