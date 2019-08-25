using UnityEngine;
using UnityEngine.EventSystems;

public class FixedJoystick : Joystick
{
    private Vector2 mJoystickPosition = Vector2.zero;
    private Camera mCam = new Camera();

    void Start()
    {
        mJoystickPosition = RectTransformUtility.WorldToScreenPoint(mCam, background.position);
    }

    public override void OnDrag(PointerEventData _eventData)
    {
        Vector2 direction = _eventData.position - mJoystickPosition;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }

    public override void OnPointerDown(PointerEventData _eventData)
    {
        OnDrag(_eventData);
    }

    public override void OnPointerUp(PointerEventData _eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}