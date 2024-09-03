using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public float touchSensitivity = 0.1f;
    public RectTransform joystickArea; // Reference to the joystick's UI area

    private float _previousTouchDeltaX;
    private float _previousTouchDeltaY;

    private void Update()
    {
        if (Input.touchCount == 1) // Single touch
        {
            var touch = Input.GetTouch(0);

            // Check if the touch is outside the joystick area
            if (!IsTouchOverUI(touch))
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    // Calculate the touch delta
                    var deltaX = touch.deltaPosition.x * touchSensitivity;
                    var deltaY = touch.deltaPosition.y * touchSensitivity;

                    // Adjust the Cinemachine FreeLook camera's X and Y axes
                    freeLookCamera.m_XAxis.Value += deltaX;
                    freeLookCamera.m_YAxis.Value -= deltaY; // Inverted for natural touch control

                    // Store the current touch position as the previous position for the next frame
                    _previousTouchDeltaX = deltaX;
                    _previousTouchDeltaY = deltaY;
                }
            }
        }
    }

    private bool IsTouchOverUI(Touch touch)
    {
        // Convert touch position to screen point
        var touchPosition = touch.position;

        // Check if the touch position overlaps with the joystick area
        return RectTransformUtility.RectangleContainsScreenPoint(joystickArea, touchPosition, null);
    }
}
