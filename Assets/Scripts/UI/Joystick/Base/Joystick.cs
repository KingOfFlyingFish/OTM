using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Options")]
    [Range(0f, 2f)] public float handleLimit = 1f;
    public MJOYSTICKMODE joystickMode = MJOYSTICKMODE.AllAxis;

    protected Vector2 inputVector = Vector2.zero;

    [Header("Components")]
    public RectTransform background;
    public RectTransform handle;

    public float Horizontal { get { return inputVector.x; } }
    public float Vertical { get { return inputVector.y; } }
    public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }
    public bool IsMaxSpeed { get { return Direction.magnitude > 0.99f; } }

    public float Magnitude { get { return Direction.magnitude; } }
    public virtual void OnDrag(PointerEventData eventData)
    {

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {

    }

    protected void ClampJoystick()
    {
        if (joystickMode == MJOYSTICKMODE.Horizontal)
            inputVector = new Vector2(inputVector.x, 0f);
        if (joystickMode == MJOYSTICKMODE.Vertical)
            inputVector = new Vector2(0f, inputVector.y);
    }
}

public enum MJOYSTICKMODE { AllAxis, Horizontal, Vertical}
