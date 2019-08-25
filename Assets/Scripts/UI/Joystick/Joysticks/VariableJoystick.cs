using UnityEngine;
using UnityEngine.EventSystems;

public class VariableJoystick : Joystick
{
    [Header("Variable Joystick Options")]
    [SerializeField] private bool mIsFixed = true;
    [SerializeField] private Vector2 mFixedScreenPosition = Vector2.zero;

    private Vector2 mJoystickCenter = Vector2.zero;

    void Start()
    {
        if (mIsFixed)
            OnFixed();
        else
            OnFloat();
    }

    public void ChangeFixed(bool _joystickFixed)
    {
        if (_joystickFixed)
            OnFixed();
        else
            OnFloat();
        mIsFixed = _joystickFixed;
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
        if (!mIsFixed)
        {
            background.gameObject.SetActive(true);
            background.position = _eventData.position;
            handle.anchoredPosition = Vector2.zero;
            mJoystickCenter = _eventData.position;
        }
    }

    public override void OnPointerUp(PointerEventData _eventData)
    {
        if (!mIsFixed)
        {
            background.gameObject.SetActive(false);
        }
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    void OnFixed()
    {
        mJoystickCenter = mFixedScreenPosition;
        background.gameObject.SetActive(true);
        handle.anchoredPosition = Vector2.zero;
        background.anchoredPosition = mFixedScreenPosition;
    }

    void OnFloat()
    {
        handle.anchoredPosition = Vector2.zero;
        background.gameObject.SetActive(false);
    }
}