using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaticScreenController : MonoBehaviour
{
    [Header("Screen Settings")]
    [SerializeField] private Transform screenCamera;
    [SerializeField] private Transform ventCamera;

    [SerializeField] private Animator mainAnimation;
    [SerializeField] private Animator ventAnimation;

    [Header("Camera Positions")]
    [SerializeField] private List<Transform> cameraPositions = new List<Transform>();

    [Header("Button Layout")]
    [SerializeField] private LayerMask buttonLayer;
    [SerializeField] private float maxRaycastDistance = 3f;

    private Dictionary<string, System.Action> _mainButtonActions;
    private Dictionary<string, System.Action> _ventButtonActions;

    private Camera _mainCamera;
    internal object _serialPort;

    void Start()
    {
        _mainCamera = Camera.main;
        SetupButtonActions();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleButtonPress();
        }
    }
    private void SetupButtonActions()
    {
        _mainButtonActions = new Dictionary<string, System.Action>
        {
            {"CAM01", () => { SetCameraPosition(0); ButtonPressEffect(0); }},
            {"CAM02", () => { SetCameraPosition(1); ButtonPressEffect(1); }},
            {"CAM03", () => { SetCameraPosition(2); ButtonPressEffect(2); }},
            {"CAM04", () => { SetCameraPosition(3); ButtonPressEffect(3); }},
            {"CAM05", () => { SetCameraPosition(4); ButtonPressEffect(4); }},
            {"CAM06", () => { SetCameraPosition(5); ButtonPressEffect(5); }},
            {"CAM07", () => { SetCameraPosition(6); ButtonPressEffect(6); }},
            {"CAM08", () => { SetCameraPosition(7); ButtonPressEffect(7); }},
            {"CAM09", () => { SetCameraPosition(8); ButtonPressEffect(8); }},
            {"CAM10", () => { SetCameraPosition(9); ButtonPressEffect(9); }}
        };

        _ventButtonActions = new Dictionary<string, System.Action>
        {
            {"CAM11", () => { SetCameraPosition(10); ButtonPressEffect(10); }},
            {"CAM12", () => { SetCameraPosition(11); ButtonPressEffect(11); }},
            {"CAM13", () => { SetCameraPosition(12); ButtonPressEffect(12); }},
            {"CAM14", () => { SetCameraPosition(13); ButtonPressEffect(13); }},
            {"CAM15", () => { SetCameraPosition(14); ButtonPressEffect(14); }},
            {"Lure", () => { ButtonPressEffect(15); }},
            {"LockDown", () => { ButtonPressEffect(16); }}
        };
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

    private void ButtonPressEffect(int index)
    {
        if (index <= 9)
            mainAnimation.SetTrigger("CamTrans");

        else
            ventAnimation.SetTrigger("VentCam");
    }
}
