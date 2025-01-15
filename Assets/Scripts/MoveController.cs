using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class MoveController : MonoBehaviour
{
    public Transform character;
    public float m_Speed = 2.5f;
    public XRInputValueReader<Vector2> m_StickInput = new XRInputValueReader<Vector2>("Thumbstick");
    
    void Update()
    {
        if(m_StickInput != null)
        {
            Vector2 stickVal = m_StickInput.ReadValue();
            Vector3 move = new Vector3(stickVal.x, 0, stickVal.y);
            character.Translate(move * m_Speed * Time.deltaTime);
        }     
    }  
}
