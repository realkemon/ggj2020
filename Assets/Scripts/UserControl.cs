using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(CharacterControl))]
public class UserControl : MonoBehaviour
{
    public int playerId;

    private CharacterControl m_Character;
    private Transform m_Cam;
    private Vector3 m_CamForward;
    private Vector3 m_Move;

    private void Start()
    {
        if (Camera.main != null)
            m_Cam = Camera.main.transform;
        else
            Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);

        m_Character = GetComponent<CharacterControl>();
    }

    private void FixedUpdate()
    {
        float h = CrossPlatformInputManager.GetAxis("HorizontalPlayer" + playerId);
        float v = CrossPlatformInputManager.GetAxis("VerticalPlayer" + playerId);

        m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        m_Move = v * m_CamForward + h * m_Cam.right;

        m_Character.Move(m_Move);
    }
}