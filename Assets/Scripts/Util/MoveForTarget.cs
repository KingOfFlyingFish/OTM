using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForTarget : MonoBehaviour
{
    [SerializeField] private Transform  mTargetTrans = null;
    [SerializeField] private float      mCameraTraceSpeed = 3f;

    private Vector3 offsetPosition;

    #region Unity MonoBehaviour Methods
    void Start()
    {
        offsetPosition = transform.position - mTargetTrans.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        MoveForMyTarget();
    }
    #endregion

    private void MoveForMyTarget()
    {
        Vector3 nextPosition = mTargetTrans.position + offsetPosition;

        float distance = Vector3.Distance(nextPosition, transform.position);

        if (distance > 0.1f)
        {
            float cameraWorkSpeed = distance * Time.deltaTime * mCameraTraceSpeed;
            Vector3 newPos = Vector3.Lerp(transform.position, nextPosition, cameraWorkSpeed);

            transform.position = newPos;
        }
    }
}
