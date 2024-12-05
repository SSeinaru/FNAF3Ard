using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO.Ports;
using System.Threading;

public class Kebab : MonoBehaviour
{
    private SerialPort _serialPort;
    // Start is called before the first frame update

    void Start()
    {
        _serialPort = new SerialPort();

        string[] ports = SerialPort.GetPortNames();

        foreach (string port in ports)
        {
            if (port.ToLower().Contains("arduino"))
            {
                _serialPort.PortName = port;
            }

        }
        

        _serialPort.BaudRate = 115200;
        _serialPort.Parity = Parity.None;
        _serialPort.DataBits = 8;
        _serialPort.StopBits = StopBits.One;
        _serialPort.Handshake = Handshake.None;
        _serialPort.ReadBufferSize = 1;


        _serialPort.Open();

        _serialPort.DiscardInBuffer();
        _serialPort.DiscardOutBuffer();

        //_serialPort.ByteToRead nummertje :D
        //_serialPort.ReadByte()
        //_serialPort.Write() = byte unassigned 0 - 255

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ReadData()
    {
        //for (int ByteToRead )
        //{

        //}

        yield return null;
    }
}
