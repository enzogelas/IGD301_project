using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class RaycastTechnique : InteractionTechnique
{
    [SerializeField]
    int raycastMaxDistance = 1000;

    [SerializeField]
    private GameObject ray;

    private LineRenderer lineRenderer;

    [SerializeField]
    private XRInputValueReader<float> m_TriggerInput = new XRInputValueReader<float>("Trigger");

    private Vector3 localOrigin = new Vector3(0, 0, 0);
    private Vector3 localDirection = new Vector3(0, 0, 1);

    private void Start()
    {
        lineRenderer = ray.GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        Transform rightControllerTransform = ray.transform;
        
        // Set the beginning of the line renderer to the position of the controller
        lineRenderer.SetPosition(0, localOrigin);

        // Creating a raycast and storing the first hit if existing
        RaycastHit hit;
        bool hasHit = Physics.Raycast(rightControllerTransform.position, rightControllerTransform.forward, out hit, Mathf.Infinity);

        // Checking that the user pushed the trigger
        float triggerValue = m_TriggerInput.ReadValue();
        if (triggerValue > 0.1f && hasHit)
        {
            // Sending the selected object hit by the raycast
            currentSelectedObject = hit.collider.gameObject;            
        }

        // Determining the end of the LineRenderer depending on whether we hit an object or not
        if (hasHit)
        {
            lineRenderer.SetPosition(1, hit.distance * localDirection);
        }
        else
        {
            lineRenderer.SetPosition(1, raycastMaxDistance * localDirection);
        }

        // DO NOT REMOVE
        // If currentSelectedObject is not null, this will send it to the TaskManager for handling
        base.CheckForSelection();
    }
}
