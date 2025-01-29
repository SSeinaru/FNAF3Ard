using UnityEngine;
using System.IO.Ports;

public class ArduScreenButton : MonoBehaviour
{
    [Header("Button Configuration")]
    [SerializeField] private string buttonID;

    [Header("Visual Feedback")]
    [SerializeField] private Animator staticAnimation;

    private ArduScreenController _screenController;

    private void Start()
    {
        _screenController = FindObjectOfType<ArduScreenController>();
    }

    public void OnButtonPress()
    {
        ButtonPressEffect();
        SendButtonPressToArduino();
    }

    private void ButtonPressEffect()
    {
        staticAnimation.SetTrigger("CamTrans");
    }

    private void SendButtonPressToArduino()
    {
        _screenController.SendButtonCommand(buttonID);
    }
}