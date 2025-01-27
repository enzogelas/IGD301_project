using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using TMPro;
using UnityEditor;
using System.Linq;
using Unity.Mathematics;
using UnityEngine.EventSystems;

// Your implemented technique inherits the InteractionTechnique class

public class ObjectInfo
{
    public Vector3 OriginalPosition { get; set; }
    public Color OriginalColor { get; set; }

    public ObjectInfo(Vector3 position, Color color)
    {
        OriginalPosition = position;
        OriginalColor = color;
    }
}

public class MyTechnique : InteractionTechnique
{
    [SerializeField]
    int raycastMaxDistance = 1000;

    [SerializeField]
    private GameObject ray;

    [SerializeField]
    private Transform characterTransform;

    [SerializeField]
    private RectTransform characterPositionIndicator;

    [SerializeField]
    private RectTransform nextItemIndicator;

    private LineRenderer lineRenderer;

    [SerializeField]
    private XRInputValueReader<float> m_TriggerInput = new XRInputValueReader<float>("Trigger");

    private Vector3 localOrigin = new Vector3(0, 0, 0);
    private Vector3 localDirection = new Vector3(0, 0, 1);

    // create a dictionary to store the objects that are intersecting with the ray and their original position and color
    private Dictionary<GameObject, (Vector3 originalPosition, Color originalColor)> intersectingObjects = new Dictionary<GameObject, (Vector3, Color)>();

    // selected object
    private GameObject flowSelectedObject = null;

    // flowDistance
    private float flowDistance = 4.0f;

    private void Start()
    {
        lineRenderer = ray.GetComponent<LineRenderer>();

    }

    public void OnNewItemPosition(Vector3 position)
    {
        nextItemIndicator.anchoredPosition = new Vector2(position.x / 140, position.z / Mathf.Abs(position.z) * 0.46f);
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
        if (triggerValue > 0.9f)
        {
            if (intersectingObjects.Count == 0)
            {
                // collect all the colliders that intersect with a capsule
                Collider[] hitColliders = Physics.OverlapCapsule(rightControllerTransform.position, rightControllerTransform.position + rightControllerTransform.forward * 100, 0.7f);

                // order by distance
                hitColliders = hitColliders.OrderBy(x => Vector3.Distance(x.transform.position, rightControllerTransform.position)).ToArray();

                // remove far away objects if there are too many
                if (hitColliders.Length > 8)
                {
                    hitColliders = hitColliders.Take(8).ToArray();
                }
                

                // for all the colliders that intersect with the sphere
                foreach (var hitCollider in hitColliders)
                {
                    // if the collider is not the sphere itself
                    // print name, make it red

                    // skip UI layer
                    if (hitCollider.gameObject.layer == 5)
                    {
                        continue;
                    }
                    // skip colliders that are not box colliders
                    if (hitCollider.GetType() != typeof(BoxCollider))
                    {
                        continue;
                    }
                    //Debug.Log("Clicked " + hitCollider.name);
                    intersectingObjects.Add(hitCollider.gameObject, (hitCollider.transform.position, hitCollider.GetComponent<Renderer>().material.color));
                }

                // calculate angle between each object
                float angle = 360f / intersectingObjects.Count;
                int count = 0;
                flowDistance = intersectingObjects.Count / 2.0f;
                Vector3 center = rightControllerTransform.position + rightControllerTransform.forward * (2.0f + flowDistance);
                foreach (var intersectingObject in intersectingObjects)
                {
                    // move the object in front of the camera
                    Vector3 tempPos = center + rightControllerTransform.right * flowDistance * Mathf.Cos(count * angle * Mathf.Deg2Rad) + rightControllerTransform.up * flowDistance * Mathf.Sin(count * angle * Mathf.Deg2Rad);
                    intersectingObject.Key.transform.position = tempPos;
                    // rotate the object around the ray
                    count++;
                }
            }
            else
            {
                // monitor who is the closest object to the ray
                float minDistance = Mathf.Infinity;
                GameObject closestObject = null;
                foreach (var intersectingObject in intersectingObjects)
                {
                    // calculate the distance between the object and the ray
                    float distance = Vector3.Distance(intersectingObject.Key.transform.position, rightControllerTransform.position + rightControllerTransform.forward * (2.0f + flowDistance));
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestObject = intersectingObject.Key;
                    }
                }
                // highlight the closest object
                if (closestObject != null && minDistance <  math.max(flowDistance * 0.7f, 1.5f))
                {
                    // reset the color of the previous closest object
                    if (flowSelectedObject != null && closestObject != flowSelectedObject)
                    {
                        flowSelectedObject.GetComponent<Renderer>().material.color = intersectingObjects[flowSelectedObject].originalColor;
                    }
                    flowSelectedObject = closestObject;
                    flowSelectedObject.GetComponent<Renderer>().material.color = Color.red;
                }
                else if (flowSelectedObject != null)
                {
                    flowSelectedObject.GetComponent<Renderer>().material.color = intersectingObjects[flowSelectedObject].originalColor;
                    flowSelectedObject = null;
                }
            }
        }
        else
        {
            // move the object back to its original position
            foreach (var intersectingObject in intersectingObjects)
            {
                intersectingObject.Key.transform.position = intersectingObject.Value.originalPosition;
                intersectingObject.Key.GetComponent<Renderer>().material.color = intersectingObject.Value.originalColor;
            }
            intersectingObjects.Clear();
            // selection
            if (flowSelectedObject != null && currentSelectedObject != flowSelectedObject)
            {
                currentSelectedObject = flowSelectedObject;
                flowSelectedObject = null;
            }
        }


        //characterPositionIndicator.anchoredPosition = new Vector2(characterTransform.position.x/140, characterTransform.position.z/(6/0.4f));

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
