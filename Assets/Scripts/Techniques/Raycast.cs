using UnityEngine;

using TMPro;
public class LineRaycast : MonoBehaviour
{
    private LineRenderer lineRenderer; // Line Renderer for the ray
    public float rayLength = 1000;     // Max ray length
    private RaycastHit hit;
    private Vector3 rayOrigin;
    private Vector3 rayDirection;

    private Vector3 localOrigin;
    private Vector3 localDirection;
    
    public TextMeshProUGUI hitObjectText; // Text to display the name of the object hit

    void Start()
    {
        // Get the Line Renderer component
        lineRenderer = GetComponent<LineRenderer>();
        localOrigin = new Vector3(0, 0, 0);
        localDirection = new Vector3(0, 0, 1);
    }

    void Update()
    {
        rayOrigin = transform.position;
        rayDirection = transform.forward;
        // Perform the raycast
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayLength))
        {
            // If we hit something, draw the line to the hit point
            lineRenderer.SetPosition(0, localOrigin);
            lineRenderer.SetPosition(1, localDirection * hit.distance);

            // Example: Debug log the name of the object hit
            hitObjectText.text = hit.collider.gameObject.name;
        }
        else
        {
            // If we don't hit anything, draw the line to the max length
            lineRenderer.SetPosition(0, localOrigin);
            lineRenderer.SetPosition(1, localDirection * rayLength);
        }
    }
}