using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class MoveController : MonoBehaviour
{
    public Transform character;
    public Transform head;
    public float m_Speed = 10.0f;
    public XRInputValueReader<Vector2> m_StickInput = new XRInputValueReader<Vector2>("Thumbstick");
    
    void Update()
    {
        if(m_StickInput != null)
        {
            Vector2 localMove = m_StickInput.ReadValue();

            // Get head forward direction in the horizontal plane
            Vector3 headForward = new Vector3(head.forward.x, 0, head.forward.z).normalized;

            // Get right vector for strafing
            Vector3 headRight = Vector3.Cross(Vector3.up, headForward);

            // Combine input direction with head orientation
            Vector3 moveDirection = headForward * localMove.y + headRight * localMove.x;

            // Apply movement
            character.Translate(moveDirection * m_Speed * Time.deltaTime, Space.World);
        }     
    }  
}
