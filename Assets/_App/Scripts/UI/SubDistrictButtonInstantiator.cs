using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SubDistrictButtonInstantiator : MonoBehaviour
{
    public List<Collider> subDistricts;
    public GameObject subDistrictButton;
    public List<Transform> cameraPositions;
    public GameObject cameraParent;
    public float travelSpeed;
    private void Awake()
    {
        if (this.enabled)
        {
            subDistricts = this.GetComponentsInChildren<Collider>().ToList();
            cameraPositions = cameraParent.GetComponentsInChildren<Transform>().ToList();

            for (int i = 0; i < subDistricts.Count; i++)
            {
                Instantiate(subDistrictButton, subDistricts[i].transform);
            }
        }
    }
    private void Start()
    {
        for (int i = 0; i < subDistricts.Count; i++)
        {
            subDistricts[i].GetComponentInChildren<Button>().onClick.AddListener(() => GoToSubDistrictPosition(cameraPositions[i]));
        }

    }
    public void GoToSubDistrictPosition(Transform subDistrictCameraPosition)
    {
        Debug.Log("subDistrict btn pressed");
        GameManager.Instance.CameraController.MoveCameraToCertainPoint(subDistrictCameraPosition, false, travelSpeed);
    }
}
