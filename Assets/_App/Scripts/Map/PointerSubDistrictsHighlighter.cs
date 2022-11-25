using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerSubDistrictsHighlighter : MonoBehaviour
{
    private Camera m_camera;
    private Material subDistrictMat;


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
            SetSubDistrictMaterial(newMaterial);
        }
    }

    void SetSubDistrictMaterial(Material value)
    {
        if (value && value == subDistrictMat)
        {
            return;
        }
        else if (value)
        {

            var color0 = value.color;
            color0.a = 0;
            subDistrictMat = subDistrictMat == null ? value : subDistrictMat;

            //subDistrictMat = value;

            subDistrictMat.color = color0;

            subDistrictMat = value;
            var color = value.color;
            color.a = 1;

            subDistrictMat.color = color;

        }
    }
    public void DisableEmissionMaterial()
    {
        //subDistrictMat?.DisableKeyword("_EMISSION");
    }
}
