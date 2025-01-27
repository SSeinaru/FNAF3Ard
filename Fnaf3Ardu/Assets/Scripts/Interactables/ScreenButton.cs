using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenButton : MonoBehaviour
{
    [Header("Button Configuration")]
    [SerializeField] private string buttonID;

    [Header("Visual Feedback")]
    [SerializeField] private Animator staticAnimation;

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
            { "CAM01", () => _screenController.SetCameraPosition(0) },
            { "CAM02", () => _screenController.SetCameraPosition(1) },
            { "CAM03", () => _screenController.SetCameraPosition(2) },
            { "CAM04", () => _screenController.SetCameraPosition(3) },
            { "CAM05", () => _screenController.SetCameraPosition(4) },
            { "CAM06", () => _screenController.SetCameraPosition(5) },
            { "CAM07", () => _screenController.SetCameraPosition(6) },
            { "CAM08", () => _screenController.SetCameraPosition(7) },
            { "CAM09", () => _screenController.SetCameraPosition(8) },
            { "CAM10", () => _screenController.SetCameraPosition(9) },
        };
        _ventButtonActions = new Dictionary<string, System.Action>
        {
            { "CAM11", () => _screenController.SetCameraPosition(10) },
            { "CAM12", () => _screenController.SetCameraPosition(11) },
            { "CAM13", () => _screenController.SetCameraPosition(12) },
            { "CAM14", () => _screenController.SetCameraPosition(13) },
            { "CAM15", () => _screenController.SetCameraPosition(14) },

        };
    }

    public void OnButtonPress()
    {
        ButtonPressEffect();
        
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

    private void ButtonPressEffect()
    {
        staticAnimation.SetTrigger("CamTrans");
    }
}
