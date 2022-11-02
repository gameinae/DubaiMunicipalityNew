using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class District : MonoBehaviour
{
    public Transform myCameraPosition;
    public Transform MyCameraRotation
    {
        get => myCameraPosition;
    }
    public DistrictSO districtData;
}
