using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CharacterControl : MonoBehaviour
{
    [SerializeField] float m_MovementSpeedMultiplier = 0.12f;
    [SerializeField] float m_MovingTurnSpeed = 360;
    [SerializeField] float m_StationaryTurnSpeed = 300;

    Rigidbody m_Rigidbody;
    Animator m_Animator;
    AudioSource audioS;
    float m_TurnAmount;
    float m_ForwardAmount;

    void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        audioS = GetComponent<AudioSource>();

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    public void Move(Vector3 move)
    {
        if (move.magnitude > 1.0f)
            move.Normalize();

        move = transform.InverseTransformDirection(move);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;

        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);

        transform.Translate(move * m_MovementSpeedMultiplier * Globals.currentPlayerSpeedMultiplier);

        m_Animator.SetFloat("Speed", (move * m_MovementSpeedMultiplier * Globals.currentPlayerSpeedMultiplier).magnitude);

        if ((move * m_MovementSpeedMultiplier * Globals.currentPlayerSpeedMultiplier).magnitude > 0.03f)
        {
            if (!audioS.isPlaying)
                audioS.Play();
        }
        else
            audioS.Stop();
    }
}