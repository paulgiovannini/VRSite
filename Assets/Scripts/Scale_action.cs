using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale_action : MonoBehaviour
{
    public GameObject otherController;

    [Header("Scaling")]
    public int scaleFactor = 300;
    public Vector3 minScale = new Vector3(.2f, .2f, .2f);
    public Vector3 maxScale = new Vector3(2f, 2f, 2f);

    private GameObject objectToScale;
    private GameObject object_1;
    private GameObject object_2;

    private static bool scaling;
    private float originalDistance;

    private int controllerNumber;

    void Start() {
        ControllerGrabObject.onScale += checkScaleAction;
        ControllerGrabObject.onUnscale += checkScaleAction;

        StartCoroutine("updateOriginalDistance");
    }

    void Update() {
        if (!scaling) return;
        scaleObject(objectToScale);
    }

    private void checkScaleAction(GameObject o, int c) {
        controllerNumber += c;

        switch(controllerNumber) 
        {
            case 0:
            case 1:
                scaling = false;
                object_1 = o;
                break;
            case 2:
                object_2 = o;
                if (isScalable(object_1, object_2))
                {
                    scaling = true;
                    objectToScale = object_1;
                }
                break;
        }

        bool isScalable(GameObject o_1, GameObject o_2) {
            return (o_1 == o_2 && o_1.CompareTag("Scalable"));
        }
    }

    private void scaleObject(GameObject o) {
        var distance = getDistanceBetweenControllers();

        if (distance > originalDistance)
        {
            if (o.transform.localScale.x < maxScale.x)
            {
                o.transform.localScale *= 1 + (distance/originalDistance) / scaleFactor;
            }
        }
        else if (distance < originalDistance)
        {
            if (o.transform.localScale.x > minScale.x)
            {
                o.transform.localScale *= 1 - (distance/originalDistance) / scaleFactor;
            }
        }
    }

    private IEnumerator updateOriginalDistance() {
        while (true)
        {
            originalDistance = getDistanceBetweenControllers();
            yield return new WaitForSeconds(.1f);
        }
    }

    private float getDistanceBetweenControllers() {
        return Vector3.Distance(transform.position, otherController.transform.position);
    }
}
