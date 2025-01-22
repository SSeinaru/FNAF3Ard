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
    private Dictionary<string, System.Action> _buttonActions;


    private void Start()
    {
        _screenController = FindObjectOfType<StaticScreenController>();
        SetupButtonActions();
    }

    private void SetupButtonActions()
    {
        _buttonActions = new Dictionary<string, System.Action>
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
            // Add more button actions here
        };
    }

    public void OnButtonPress()
    {
        ButtonPressEffect();

        
        Debug.Log(staticAnimation.GetBool("CamChange"));
        if (_buttonActions.ContainsKey(buttonID))
        {
            
            _buttonActions[buttonID].Invoke();
            
        }
        else
        {
            Debug.LogWarning($"No action defined for button ID: {buttonID}");
        }
    }

    IEnumerator ButtonPressEffect()
    {
        staticAnimation.SetBool("CamChange", true);
        Debug.Log(staticAnimation.GetBool("CamChange"));

        yield return new WaitForEndOfFrame();

        staticAnimation.SetBool("CamChange", false);

        yield return null;
    }
}
