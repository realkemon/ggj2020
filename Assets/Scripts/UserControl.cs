using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(CharacterControl))]
public class UserControl : MonoBehaviour
{
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
        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

        // calculate camera relative direction to move:
        m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        m_Move = v * m_CamForward + h * m_Cam.right;

        // pass all parameters to the character control script
        m_Character.Move(m_Move);
    }
}