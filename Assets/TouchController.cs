using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTouch(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Waiting:
                Debug.Log("Waiting");
                break;
            case InputActionPhase.Disabled:
                Debug.Log("Disabled");
                break;
            case InputActionPhase.Started:
                Debug.Log("Started");
                break;
            case InputActionPhase.Performed:
                Debug.Log("Performed");
                break;
            case InputActionPhase.Canceled:
                Debug.Log("Canceled");
                break;
        }
    }

}
