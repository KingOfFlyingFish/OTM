using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    Vector2 mJoystickCenter = Vector2.zero;

    void Start()
    {
        background.gameObject.SetActive(false);
    }

    public override void OnDrag(PointerEventData _eventData)
    {
        Vector2 direction = _eventData.position - mJoystickCenter;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }

    public override void OnPointerDown(PointerEventData _eventData)
    {
        background.gameObject.SetActive(true);
        background.position = _eventData.position;
        handle.anchoredPosition = Vector2.zero;
        mJoystickCenter = _eventData.position;
    }

    public override void OnPointerUp(PointerEventData _eventData)
    {
        background.gameObject.SetActive(false);
        inputVector = Vector2.zero;
    }
}