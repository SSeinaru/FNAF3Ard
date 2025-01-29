using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;
using System;

public class ArduScreenController : MonoBehaviour
{
    [Header("Screen Settings")]
    [SerializeField] private Transform screenCamera;
    [SerializeField] private Transform ventCamera;

    [SerializeField] private Animator mainAnimation;
    [SerializeField] private Animator ventAnimation;

    [Header("Camera Positions")]
    [SerializeField] private List<Transform> cameraPositions = new List<Transform>();

    [Header("Arduino Communication")]
    [SerializeField] private string portName = "COM3"; // Adjust to your Arduino Micro port
    [SerializeField] private int baudRate = 115200;    // Match Arduino's baud rate

    // LED State Tracking
    [Header("LED States")]
    [SerializeField] private List<bool> ledStates = new List<bool>();

    private Camera _mainCamera;
    private SerialPort _serialPort;
    private Dictionary<string, System.Action> _mainButtonActions;
    private Dictionary<string, System.Action> _ventButtonActions;




    void Start()
    {
        _mainCamera = Camera.main;
        SetupButtonActions();

        // Initialize LED states (17 buttons)
        ledStates = new List<bool>(new bool[22]);

        // Initialize Serial Communication
        try
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.Open();
            _serialPort.ReadTimeout = 50;
        }
        catch (System.Exception e)
        {
            //Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    private void SetupButtonActions()
    {
        _mainButtonActions = new Dictionary<string, System.Action>
        {
            {"CAM01", () => { SetCameraPosition(0); ToggleLED(0); ButtonPressEffect(0); }},
            {"CAM02", () => { SetCameraPosition(1); ToggleLED(1); ButtonPressEffect(1); }},
            {"CAM03", () => { SetCameraPosition(2); ToggleLED(2); ButtonPressEffect(2); }},
            {"CAM04", () => { SetCameraPosition(3); ToggleLED(3); ButtonPressEffect(3); }},
            {"CAM05", () => { SetCameraPosition(4); ToggleLED(4); ButtonPressEffect(4); }},
            {"CAM06", () => { SetCameraPosition(5); ToggleLED(5); ButtonPressEffect(5); }},
            {"CAM07", () => { SetCameraPosition(6); ToggleLED(6); ButtonPressEffect(6); }},
            {"CAM08", () => { SetCameraPosition(7); ToggleLED(7); ButtonPressEffect(7); }},
            {"CAM09", () => { SetCameraPosition(8); ToggleLED(8); ButtonPressEffect(8); }},
            {"CAM10", () => { SetCameraPosition(9); ToggleLED(9); ButtonPressEffect(9); }}
        };

        _ventButtonActions = new Dictionary<string, System.Action>
        {
            {"CAM11", () => { SetCameraPosition(10); ToggleLED(10); ButtonPressEffect(10); }},
            {"CAM12", () => { SetCameraPosition(11); ToggleLED(11); ButtonPressEffect(11); }},
            {"CAM13", () => { SetCameraPosition(12); ToggleLED(12); ButtonPressEffect(12); }},
            {"CAM14", () => { SetCameraPosition(13); ToggleLED(13); ButtonPressEffect(13); }},
            {"CAM15", () => { SetCameraPosition(14); ToggleLED(14); ButtonPressEffect(14); }},
            {"Lure", () => { ButtonPressEffect(15); }},
            {"LockDown", () => { ButtonPressEffect(16); }}
        };
    }

    void Update()
    {
        CheckArduinoInput();
    }

    void CheckArduinoInput()
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            try
            {
                if (_serialPort.BytesToRead > 0)
                {
                    string message = _serialPort.ReadLine().Trim();
                    ProcessArduinoCommand(message);
                }
            }
            catch (System.Exception e)
            {
                //ebug.LogError("Serial read error: " + e.Message);
            }
        }
    }

    void ProcessArduinoCommand(string command)
    {
        Debug.Log($"Received command: {command}");

        if (_mainButtonActions.TryGetValue(command, out var action) ||
            _ventButtonActions.TryGetValue(command, out action))
        {
            action();
        }
        else
        {
            //Debug.LogWarning($"Unrecognized camera command: {command}");
        }
    }

    void ToggleLED(int index)
    {
        if (index >= 0 && index < ledStates.Count)
        {
            // Toggle LED state
            ledStates[index] = !ledStates[index];

            // Send LED state to Arduino
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.WriteLine($"LED:{index}");
            }
        }
    }

    public void SetCameraPosition(int index)
    {
        if (index >= 0 && index <= 9)
        {
            if (screenCamera.position != cameraPositions[index].position)
            {
                screenCamera.position = cameraPositions[index].position;
                screenCamera.rotation = cameraPositions[index].rotation;
                Debug.Log($"Main Camera Position: {cameraPositions[index].position}");
            }
        }
        else if (index >= 10 && index < cameraPositions.Count)
        {
            if (ventCamera.position != cameraPositions[index].position)
            {
                ventCamera.position = cameraPositions[index].position;
                ventCamera.rotation = cameraPositions[index].rotation;
                Debug.Log($"Vent Camera Position: {cameraPositions[index].position}");
            }
        }
        else
        {
            Debug.LogError($"Invalid camera index: {index}");
        }

        // Send confirmation back to Arduino
        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.WriteLine($"CONFIRM:{index}");
        }
    }

    void OnApplicationQuit()
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.Close();
        }
    }

    public void SendButtonCommand(string buttonID)
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.WriteLine(buttonID);
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