using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointerExt : MonoBehaviour
{
    private const float _maxDistance = 10;
    private GameObject _gazedAtObject = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        // Casts ray towards camera's forward direction, to detect if a GameObject is being gaze
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance, 1 << 5))
        {
            // GameObject detected in front of the camera.
            if (_gazedAtObject != hit.transform.gameObject)
            {
                // New GameObject.
                _gazedAtObject?.SendMessage("OnUIPointerExit");
                // ? 의 의미는 앞 변수가 null이 아닐때 호출
                _gazedAtObject = hit.transform.gameObject;
                _gazedAtObject.SendMessage("OnUIPointerEnter");
            }
        }
        else
        {
            _gazedAtObject?.SendMessage("OnUIPointerExit");
            _gazedAtObject = null;
        }
        // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            _gazedAtObject?.SendMessage("OnPointerClick");
        }
    }
}
