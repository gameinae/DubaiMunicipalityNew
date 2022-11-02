using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float farSpeed;
    [SerializeField]
    private int closeSpeed;
    [SerializeField]
    private int rotationSpeed;
    [SerializeField]
    private Transform m_OriginalTranform;

    public bool isZoomIn;
    public bool isFreeRoam = true;

    private float m_MovementSpeed;
    private float m_RotationSpeed;
    private bool allowFreeRoam = true;

    private Transform currentTarget;
    
    private delegate void GoToPoint(Transform point);
    private GoToPoint m_GoToPoint;
    public bool isZoom;
    public Transform currentZoomOutTarget;
    public float zoomSpeed = 5;
    public float minZoom = 0;
    public float maxZoom = 10;
    public float zoomSpeedBooster = 0;
    private void Start()
    {
        m_MovementSpeed = farSpeed;
        m_RotationSpeed = rotationSpeed;

        m_GoToPoint += MoveCameraToTarget;
        m_GoToPoint += RotateCameraTowardsTarget;
        zoomSpeedBoost();
    }
    private void LateUpdate()
    {
        if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) && isFreeRoam)
        {
            float h = m_MovementSpeed * Time.deltaTime * Input.GetAxis("Mouse X");
            float v = m_MovementSpeed * Time.deltaTime * Input.GetAxis("Mouse Y");
            h = Mathf.Clamp(h, -0.05f, 0.05f);
            v = Mathf.Clamp(v, -0.05f, 0.05f);

            transform.Rotate(-v, h, 0);
        }
    }

    public void SetCameraBack()
    {
        m_MovementSpeed = farSpeed;
        m_RotationSpeed = rotationSpeed * 10;
        currentTarget = m_OriginalTranform;
        MoveCameraToCertainPoint(m_OriginalTranform , false);
    }
    public void MoveCameraToCertainPoint(Transform target , bool isAllowFreeRoam = true , float speed = 0)
    {
      
        if (speed == 0) 
        {
            m_MovementSpeed = closeSpeed;
            
        }
        else
        {
            m_MovementSpeed = speed;
        }
        isZoomIn = true;
        allowFreeRoam = isAllowFreeRoam;
        isFreeRoam = false;
        currentTarget = target;
        m_GoToPoint(target);
    }
    private void MoveCameraToTarget(Transform target)
    {
        StartCoroutine(CameraZoomIn(target));
    }
    private IEnumerator CameraZoomIn(Transform target)
    {
        while (isZoomIn)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, m_MovementSpeed * Time.deltaTime);
            isZoomIn = Vector3.Distance(transform.position, target.position) > 0.1f;

            yield return new WaitForEndOfFrame();
        }

    }
    private void RotateCameraTowardsTarget(Transform target)
    {
        StartCoroutine(CameraRotate(target));
    }
    private IEnumerator CameraRotate(Transform target)
    {
        while ((Vector3.Angle(transform.forward.normalized, target.forward.normalized)) != 0)
        {
            if (!isZoomIn)
            {
                m_RotationSpeed = rotationSpeed * 10;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, m_RotationSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
            Debug.Log((Vector3.Angle(transform.position, target.position)));
        }
        m_RotationSpeed = rotationSpeed;
        isFreeRoam = allowFreeRoam;

        //GameManager.Instance.BuildingInfo.SetActive(true);
        //GameManager.Instance.BuildingInfo.GetComponent<BuilldingInfo>().BuildingName.text = buildingData.name;
        //// GameManager.Instance.BuildingInfo.GetComponent<BuilldingInfo>().DistrictName.text = buildingData.district.D1;
        //GameManager.Instance.BuildingInfo.GetComponent<BuilldingInfo>().Latitude.text = buildingData.eCoordinate.ToString();
        //GameManager.Instance.BuildingInfo.GetComponent<BuilldingInfo>().Longitude.text = buildingData.nCoordinate.ToString();
    }
    /*private void SkipCameraMotion()
    {
        StopAllCoroutines();
        transform.position = currentTarget.position;
        transform.LookAt(currentTarget.position);
    }*/


    public void ZoomInAtCursorPosition()
    {
        GameManager.Instance.UIManager.screenCanvas.gameObject.SetActive(false);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit point;
        Physics.Raycast(ray, out point, 1000);
        Vector3 Scrolldirection = ray.GetPoint(100);

        float step = zoomSpeed * zoomSpeedBooster;
        // Allows zooming in and out via the mouse wheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Scrolldirection.y > minZoom)
        {
            transform.position = Vector3.MoveTowards(transform.position, Scrolldirection, step);
        }
    }
    public void ZoomOutCursorPosition()
    {
        var beforeMovingTransform = transform.position;
        float step = -500;
        transform.position = Vector3.MoveTowards(transform.position, currentZoomOutTarget.position, Input.GetAxis("Mouse ScrollWheel") * step);
        var afterMovingTransform = transform.position;
        if (beforeMovingTransform == afterMovingTransform)
        {
            GameManager.Instance.UIManager.screenCanvas.gameObject.SetActive(true);
        }
    }
    void zoomSpeedBoost()
    {
        zoomSpeedBooster = (float)Math.Pow(zoomSpeed, 6);
    }

}
