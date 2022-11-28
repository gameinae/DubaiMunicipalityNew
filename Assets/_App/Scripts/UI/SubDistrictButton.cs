using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubDistrictButton : MonoBehaviour
{
    public float travelSpeed;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {

        //this.GetComponent<Button>().onClick.AddListener((delegate { GoToSubDistrictPosition(cameraPositions); });
    }

    public void GoToSubDistrictPosition(Transform subDistrictCameraPosition)
    {
        Debug.Log("subDistrict btn pressed");
        GameManager.Instance.CameraController.MoveCameraToCertainPoint(subDistrictCameraPosition, false, travelSpeed);
    }
}
