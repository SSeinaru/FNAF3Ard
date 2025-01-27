using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaticScreenController : MonoBehaviour
{
    [Header("Screen Settings")]
    [SerializeField] private Transform screenCamera;

    [SerializeField] private Transform ventCamera;

    [Header("Camera Positions")]
    [SerializeField] private List<Transform> cameraPositions = new List<Transform>();

    [Header("Button Layout")]
    [SerializeField] private LayerMask buttonLayer;
    [SerializeField] private float maxRaycastDistance = 3f;

    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleButtonPress();
        }
    }

    void HandleButtonPress()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * maxRaycastDistance);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRaycastDistance, buttonLayer))
        {
            ScreenButton button = hit.collider.GetComponent<ScreenButton>();
            if (button != null)
            {
                button.OnButtonPress();
            }
        }
    }

    public void SetCameraPosition(int index)
    {
        if (index >= 0 && index <= 9)
        {
            screenCamera.position = cameraPositions[index].position;
            screenCamera.rotation = cameraPositions[index].rotation;
            Debug.Log(cameraPositions[index].position);
        }
        else if (index >= 10 && index <= cameraPositions.Count)
        {
            ventCamera.position = cameraPositions[index].position;
            ventCamera.rotation = cameraPositions[index].rotation;
            Debug.Log(cameraPositions[index].position);
        }
    }
}
