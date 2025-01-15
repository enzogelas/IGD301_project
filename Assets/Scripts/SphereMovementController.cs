using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class SphereMovementController : MonoBehaviour
{
    public XRInputValueReader<Vector2> m_StickInput = new XRInputValueReader<Vector2>("Thumbstick");
    public XRInputValueReader<float> m_TriggerInput = new XRInputValueReader<float>("Trigger");
    public float m_Speed = 1.0f;
    private Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        if(m_StickInput != null)
        {
            Vector2 stickVal = m_StickInput.ReadValue();
            Vector3 move = new Vector3(stickVal.x, 0, stickVal.y);
            m_Rigidbody.AddForce(move * m_Speed);
            if(m_TriggerInput.ReadValue() > 0.5f)
            {   
                m_Rigidbody.linearVelocity = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }
    }
}