using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenButton : MonoBehaviour
{
    [Header("Button Configuration")]
    [SerializeField] private string buttonID;

    [Header("Visual Feedback")]
    [SerializeField] private Animator mainAnimation;
    [SerializeField] private Animator ventAnimation;

    private StaticScreenController _screenController;
    private Dictionary<string, System.Action> _mainButtonActions;
    private Dictionary<string, System.Action> _ventButtonActions;


    private void Start()
    {
        _screenController = FindObjectOfType<StaticScreenController>();
        SetupButtonActions();
    }

    private void SetupButtonActions()
    {
        _mainButtonActions = new Dictionary<string, System.Action>
        {
            { "CAM01", () => { _screenController.SetCameraPosition(0); ButtonPressEffect(0);}},
            { "CAM02", () => { _screenController.SetCameraPosition(1); ButtonPressEffect(1);}},
            { "CAM03", () => { _screenController.SetCameraPosition(2); ButtonPressEffect(2);}},
            { "CAM04", () => { _screenController.SetCameraPosition(3); ButtonPressEffect(3);}},
            { "CAM05", () => { _screenController.SetCameraPosition(4); ButtonPressEffect(4);}},
            { "CAM06", () => { _screenController.SetCameraPosition(5); ButtonPressEffect(5);}},
            { "CAM07", () => { _screenController.SetCameraPosition(6); ButtonPressEffect(6);}},
            { "CAM08", () => { _screenController.SetCameraPosition(7); ButtonPressEffect(7);}},
            { "CAM09", () => { _screenController.SetCameraPosition(8); ButtonPressEffect(8);}},
            { "CAM10", () => { _screenController.SetCameraPosition(9); ButtonPressEffect(9);}},
        };
        _ventButtonActions = new Dictionary<string, System.Action>
        {
            { "CAM11", () => { _screenController.SetCameraPosition(10); ButtonPressEffect(10); }},
            { "CAM12", () => { _screenController.SetCameraPosition(11); ButtonPressEffect(11); }},
            { "CAM13", () => { _screenController.SetCameraPosition(12); ButtonPressEffect(12); }},
            { "CAM14", () => { _screenController.SetCameraPosition(13); ButtonPressEffect(13); }},
            { "CAM15", () => { _screenController.SetCameraPosition(14); ButtonPressEffect(14); }},

        };
    }

    public void OnButtonPress()
    {
        
        if (_mainButtonActions.ContainsKey(buttonID))
        {
            
            _mainButtonActions[buttonID].Invoke();
            
        }
        else if (_ventButtonActions.ContainsKey(buttonID))
        {

            _ventButtonActions[buttonID].Invoke();

        }
        else
        {
            Debug.LogWarning($"No action defined for button ID: {buttonID}");
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
}
