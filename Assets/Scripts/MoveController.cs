using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class MoveController : MonoBehaviour
{
    public Transform character;
    public Transform head;
    public float m_Speed = 2.5f;
    public XRInputValueReader<Vector2> m_StickInput = new XRInputValueReader<Vector2>("Thumbstick");
    
    void Update()
    {
        if(m_StickInput != null)
        {
            Vector2 localMove = m_StickInput.ReadValue();

            Vector2 headForward = new Vector2(head.forward.x, head.forward.z);

            Vector3 move = new Vector3(localMove.x * headForward.y + localMove.y * headForward.x, 0, headForward.y * localMove.y - headForward.x * localMove.x);
            character.Translate(move * m_Speed * Time.deltaTime);
        }     
    }  
}
