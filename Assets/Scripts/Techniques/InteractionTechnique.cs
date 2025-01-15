using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


public class InteractionTechnique : MonoBehaviour
{
    public UnityEvent<GameObject> objectSelectedEvent;
    protected GameObject currentSelectedObject = null;

    public TextMeshProUGUI currentObjectText;

    protected void CheckForSelection()
    {
        if (currentSelectedObject!=null)
        {
            SendObjectSelectedEvent(currentSelectedObject);
            currentSelectedObject = null;
        }
    }

    protected void SendObjectSelectedEvent(GameObject selectedObject)
    {
        objectSelectedEvent.Invoke(selectedObject);
    }
}
