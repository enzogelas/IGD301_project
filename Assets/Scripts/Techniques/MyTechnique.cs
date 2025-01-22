using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using TMPro;

// Your implemented technique inherits the InteractionTechnique class
public class MyTechnique : InteractionTechnique
{
    [SerializeField]
    int raycastMaxDistance = 1000;

    [SerializeField]
    private GameObject ray;

    [SerializeField]
    private Transform characterTransform;

    [SerializeField]
    private TextMeshProUGUI currentObjectText;

    [SerializeField]
    private RectTransform characterPositionIndicator;

    [SerializeField]
    private RectTransform nextItemIndicator;

    private LineRenderer lineRenderer;

    [SerializeField]
    private XRInputValueReader<float> m_TriggerInput = new XRInputValueReader<float>("Trigger");

    private Vector3 localOrigin = new Vector3(0, 0, 0);
    private Vector3 localDirection = new Vector3(0, 0, 1);

    private void Start()
    {
        lineRenderer = ray.GetComponent<LineRenderer>();
    }

    public void OnNewItemPosition(Vector3 position)
    {
        nextItemIndicator.anchoredPosition = new Vector2(position.x/140, position.z/Mathf.Abs(position.z) * 0.46f);
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
        if (triggerValue > 0.9f && hasHit)
        {
            // Sending the selected object hit by the raycast
            currentSelectedObject = hit.collider.gameObject;
            string objectName = currentSelectedObject.name;
            // I f the users pointed at the map, the character will be moved to the corresponding point
            if(objectName == "Panel")
            {
                RectTransform panelRectTransform = currentSelectedObject.GetComponent<RectTransform>();
                float panelWidth = panelRectTransform.rect.width;
                float panelHeight = panelRectTransform.rect.height;
                Debug.Log("Panel Width: " + panelWidth + ", Panel Height: " + panelHeight);
           
                Vector3 hitPoint = hit.point;
                Vector3 localHitPoint = currentSelectedObject.transform.InverseTransformPoint(hitPoint);
                Vector2 panelCoordinates = new Vector2(localHitPoint.x, localHitPoint.y);

                characterTransform.position = new Vector3(localHitPoint.x*140, 0, localHitPoint.y*6/0.4f);

                currentObjectText.text = "Panel hit at point: " +panelCoordinates.ToString();
            }
            else
            {
                currentObjectText.text = currentSelectedObject.name;
            } 
        }

        characterPositionIndicator.anchoredPosition = new Vector2(characterTransform.position.x/140, characterTransform.position.z/(6/0.4f));

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
