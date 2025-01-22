using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    [SerializeField]
    private MeshRenderer arrowMesh;

    [SerializeField]
    private TaskManager taskManager;

    [SerializeField]
    private Material closeObjectMaterial;

    [SerializeField]
    private Material midObjectMaterial;

    [SerializeField]
    private Material farObjectMaterial;

    [SerializeField]
    private float closeDistance = 2.0f;

    [SerializeField]
    private float farDistance = 10.0f;

	void Update () {
        GameObject currentObjectToSelect = taskManager.GetCurrentObjectToSelect();
        if (currentObjectToSelect != null)
        {
            this.transform.LookAt(currentObjectToSelect.transform.position);
            float distance = Vector3.Distance(this.transform.position, currentObjectToSelect.transform.position);
            if(distance <= closeDistance)
            {
                arrowMesh.material = closeObjectMaterial;
            }
            else if(distance > closeDistance && distance < farDistance)
            {
                arrowMesh.material = midObjectMaterial;
            }
            else
            {
                arrowMesh.material = farObjectMaterial;
            }
        }
	}
}
