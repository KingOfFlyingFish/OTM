using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float      mMoveSpeed = 4f;
    [SerializeField] private Joystick   mJoystick = null;
    [SerializeField] private Animator   mPlayerAnimation = null;
    [SerializeField] private Rigidbody  mPlayerRigidbody = null;

    /// <summary>
    /// 한 프레임당 추가되는 가속도
    /// </summary>
    private float mAccelerationSpeed = 1.0f;

    /// <summary>
    /// 캐릭터가 최대로 빠르게 갈 수 있는 최대 속도
    /// </summary>
    private float mMaxSpeed = 1.5f;

    /// <summary>
    /// 캐릭터의 달릴 시 최저 속도
    /// </summary>
    private float mMinSpeed = 1.0f;

    /// <summary>
    /// 캐릭터의 현재 속도
    /// </summary>
    private float mCurrentSpeed = 0f;

    /// <summary>
    /// 충돌 시 캐릭터가 잠시 느려기 위한 변수
    /// </summary>
    private float mDelaySpeed = 1f;

    private void FixedUpdate()
    {      
        Move();
    }

    private void Move()
    {
        RecoverDelaySpeed();
        ChangeRunAniSpeed();
        MovePlayerCharacter();
    }

    private void RecoverDelaySpeed()
    {
        if (mDelaySpeed < 1f)
        {
            mDelaySpeed += Time.fixedDeltaTime * 1f;
        }
        else
        {
            mDelaySpeed = 1f;
        }
    }

    private void MovePlayerCharacter()
    {
        Vector3 moveVector = (Vector3.right * mJoystick.Horizontal + Vector3.forward * mJoystick.Vertical);

        if (moveVector != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveVector);

            mPlayerRigidbody.MovePosition(mPlayerRigidbody.position + GetAddNextPosition(moveVector));
        }
        else
        {
            mCurrentSpeed = 0;
            mPlayerAnimation.speed = 1f;
        }
    }

    private void ChangeRunAniSpeed()
    {
        float magnitude = GetJoystickMoveSpeed();

        //애니메이션이 실제 이동속도와 비슷하게 되도록 offSet 설정.
        float animationSpeedOffset = 2.0f;

        mPlayerAnimation.speed = magnitude * animationSpeedOffset;
        mPlayerAnimation.SetFloat("speed", magnitude);
    }

    private Vector3 GetAddNextPosition(Vector3 _moveVector)
    {
        return _moveVector * GetMoveSpeed();
    }

    private float GetMoveSpeed()
    {
        if (mJoystick.IsMaxSpeed)
        {
            float nextSpeed = mCurrentSpeed + (Time.fixedDeltaTime * mAccelerationSpeed);
            mCurrentSpeed = nextSpeed > mMaxSpeed ? mMaxSpeed : nextSpeed;
        }
        else
        {
            float nextSpeed = mCurrentSpeed - (Time.fixedDeltaTime * mAccelerationSpeed);
            mCurrentSpeed = nextSpeed < mMinSpeed ? mMinSpeed : nextSpeed;
        }

        return mMoveSpeed * Time.fixedDeltaTime * mCurrentSpeed * mDelaySpeed;
    }

    private float GetJoystickMoveSpeed()
    {
        return mJoystick.Direction.magnitude;
    }

    private void OnCollisionEnter(Collision _collision)
    {
        //EnemyLayer = 8
        //만약 적 충돌체가 아니라면 리턴
        if(_collision.gameObject.layer != 8)
        {
            return;
        }
        else
        {
            Rigidbody body = _collision.collider.attachedRigidbody;

            if (body == null || body.isKinematic)
            {
                Debug.Log("body is null or Kinematic");
                return;
            }

            //밀려날 위치 계산
            ContactPoint contact = _collision.contacts[0];
            Vector3 forceDir = (_collision.transform.position - contact.point);
            forceDir.y = 0.1f;

            //현재 플레이어의 속도가 빠를수록 적을 강하게 뒤로 밀쳐냄
            body.AddForce(forceDir * GetKnockbackPower(), ForceMode.VelocityChange);

            //속도 상태를 초기화.
            mDelaySpeed = 0;
        }        
    }

    private float GetKnockbackPower()
    {
        float knockBackPower = GetMoveSpeed() * GetJoystickMoveSpeed();

        //만약 너무 값이 적어 적이 거의 넉백 안하는걸 방지하기 위한 최소값 설정
        knockBackPower = knockBackPower > 0.04f ? knockBackPower : 0.04f;

        return knockBackPower * 100f;
    }
}
