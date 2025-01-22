using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask buttonMask;
    [SerializeField] private KeyCode inputButton = KeyCode.Mouse0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, buttonMask))
        {
            if (Input.GetKeyDown(inputButton))
             {
                /*ScreenButton button = hitInfo.collider.GetComponent<ScreenButton>();
                if (button != null)
                {
                    button.OnButtonPress();
                }*/
             }
        }
            
    }
}
