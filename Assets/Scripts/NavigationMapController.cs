using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using TMPro;
public class NavigationMapController : MonoBehaviour
{
    [SerializeField]
    int raycastMaxDistance = 2;

    [SerializeField]
    private GameObject ray;

    [SerializeField]
    private Transform characterTransform;

    [SerializeField]
    private Transform cameraCharacterTransform;

    [SerializeField]
    private GameObject navigationPanel;

    private float navigationPanelWidth;
    private float navigationPanelHeight;

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
        RectTransform panelRectTransform = navigationPanel.GetComponent<RectTransform>();
        navigationPanelWidth = panelRectTransform.rect.width;
        navigationPanelHeight = panelRectTransform.rect.height;
    }

    public void OnNewItemPosition(Vector3 position)
    {
        nextItemIndicator.anchoredPosition = new Vector2(position.x * (navigationPanelWidth/140.0f), Mathf.Sign(position.z) * (navigationPanelHeight/2.0f - 0.05f));
    }

    private void LateUpdate()
    {
        Transform rightControllerTransform = ray.transform;
        
        // Set the beginning of the line renderer to the position of the controller
        lineRenderer.SetPosition(0, localOrigin);

        // Creating a raycast and storing the first hit if existing
        RaycastHit hit;
        bool hasHit = Physics.Raycast(rightControllerTransform.position, rightControllerTransform.forward, out hit, raycastMaxDistance);

        // Checking that the user pushed the trigger
        float triggerValue = m_TriggerInput.ReadValue();
        if (triggerValue > 0.9f && hasHit)
        {            // If the users pointed at the map, the character will be moved to the corresponding point
            if(hit.collider.gameObject == navigationPanel)
            {
                RectTransform panelRectTransform = navigationPanel.GetComponent<RectTransform>();
                float panelWidth = panelRectTransform.rect.width;
                float panelHeight = panelRectTransform.rect.height;

                Vector3 hitPoint = hit.point;
                Vector3 localHitPoint = navigationPanel.transform.InverseTransformPoint(hitPoint);
                Vector2 panelCoordinates = new Vector2(localHitPoint.x, localHitPoint.y);

                characterTransform.position = new Vector3(localHitPoint.x / (navigationPanelWidth/140.0f), 0, localHitPoint.y / ((navigationPanelHeight/2.0f -0.05f) / 6.0f));
                characterTransform.rotation = localHitPoint.y > 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
            }
        }

        characterPositionIndicator.anchoredPosition = new Vector2(characterTransform.position.x * (navigationPanelWidth/140.0f), characterTransform.position.z * ((navigationPanelHeight/2.0f -0.05f) / 6.0f));

        // Determining the end of the LineRenderer depending on whether we hit an object or not
        if (hasHit)
        {
            lineRenderer.SetPosition(1, hit.distance * localDirection);
        }
        else
        {
            // NOTHING because the raycasting distance in MyTechnique is better
        }

    }
}
