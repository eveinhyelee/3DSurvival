using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject thirdCam;   
      

    public void ZoomInOutView(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && mainCam.activeSelf == true)
        {
            mainCam.SetActive(false);
            thirdCam.SetActive(true);
        }
        else if (context.phase == InputActionPhase.Started && thirdCam.activeSelf == true)
        {
            thirdCam.SetActive(false);
            mainCam.SetActive(true);
        }
    }
}
