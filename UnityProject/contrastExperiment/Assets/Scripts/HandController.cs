using System.Collections;
using UnityEngine;
using Leap;
using Leap.Unity;

/**
 * 
 * Used for tracking hand positions and raycasting through hand
 * 
 */ 

public class HandController : MonoBehaviour {

    private LeapProvider provider;
    public MainController mainController;
    public Transform centerEyeAnchor;
    [SerializeField]
    public GameObject traceCube;
    [SerializeField]
    public GameObject hitCube;
    private ArrayList velocityLog;

    bool debug;
    private bool test;

	void Start () {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;

        debug = mainController.debug;

        velocityLog = new ArrayList();

        test = mainController.test;
    }

    

    internal Vector3 getPalmVelocity()
    {
        foreach (Hand hand in provider.CurrentFrame.Hands)
        {
            if (hand.IsLeft)
            {
                return hand.PalmVelocity.ToVector3();
            }
        }

        return default(Vector3);
    }

    internal Vector3 getPalmPosition()
    {
        foreach (Hand hand in provider.CurrentFrame.Hands)
        {
            if (hand.IsLeft)
            {
                if (test) Debug.Log("Left hand found!");
                return hand.PalmPosition.ToVector3();
            }
        }

        return default(Vector3);
    }

    public Vector3 getPalmHitPoint()
    {
        Vector3 centerEye = centerEyeAnchor.position;
        Vector3 palmPosition = getPalmPosition();
        Vector3 palmDirection = palmPosition - centerEye;
        if(test) Debug.Log("Palm direction = " + palmDirection.ToString());
        Debug.DrawRay(centerEye, palmDirection,Color.green,2f);
        RaycastHit hitOnPlane;
        if (
        Physics.Raycast(centerEye, palmDirection, out hitOnPlane, 150))
        {
            Vector3 hitPoint = hitOnPlane.point;
            if(test)Debug.Log("Raycast hit @ " + hitPoint.ToString());

            if (hitOnPlane.transform.tag == "raycastable")
            {
                GameObject cube = Instantiate(hitCube, hitOnPlane.point, Quaternion.identity);
                Destroy(cube, (mainController.timeBetweenTrials+mainController.stimuliShowTime)*2);
                velocityLog.Add(getPalmVelocity().y);
                mainController.avgVelocity = Average(velocityLog);
                if(debug) Debug.Log("Average velocity = " + Average(velocityLog));
                return hitOnPlane.point;
            }
            if (hitOnPlane.point != Vector3.zero)
            {
                velocityLog.Add(getPalmVelocity().y);
                GameObject cube = Instantiate(traceCube, hitOnPlane.point, Quaternion.identity);
                Destroy(cube, (mainController.timeBetweenTrials+mainController.stimuliShowTime)*2);
            }
        }
        return Vector3.zero;
    }

    internal Vector3 getMiddleFingerPosition()
    {
        foreach (Hand hand in provider.CurrentFrame.Hands)
        {
            if (hand.IsLeft)
            {
                foreach(Finger finger in hand.Fingers)
                {
                    if(finger.Type == Finger.FingerType.TYPE_MIDDLE)
                    {
                        return finger.TipPosition.ToVector3();
                    }
                }
            }
        }

        return default(Vector3);
    }

    public Vector3 getMiddleFingerHitPoint()
    {
        Vector3 centerEye = centerEyeAnchor.position;
        Vector3 middleFingerPosition = getMiddleFingerPosition();
        Vector3 fingerDirection = middleFingerPosition - centerEye;
        if (test) Debug.Log("Middle finger direction = " + fingerDirection.ToString());
        Debug.DrawRay(centerEye, fingerDirection, Color.green, 2f);
        RaycastHit hitOnPlane;
        if (
        Physics.Raycast(centerEye, fingerDirection, out hitOnPlane, 150))
        {
            Vector3 hitPoint = hitOnPlane.point;
            if (test) Debug.Log("Raycast hit @ " + hitPoint.ToString());

            if (hitOnPlane.transform.tag == "raycastable")
            {
                GameObject cube = Instantiate(hitCube, hitOnPlane.point, Quaternion.identity);
                Destroy(cube, (mainController.timeBetweenTrials + mainController.stimuliShowTime) * 2);
                velocityLog.Add(getPalmVelocity().y);
                mainController.avgVelocity = Average(velocityLog);
                if (debug) Debug.Log("Average velocity = " + Average(velocityLog));
                return hitOnPlane.point;
            }
            if (hitOnPlane.point != Vector3.zero)
            {
                velocityLog.Add(getPalmVelocity().y);
                GameObject cube = Instantiate(traceCube, hitOnPlane.point, Quaternion.identity);
                Destroy(cube, (mainController.timeBetweenTrials + mainController.stimuliShowTime) * 2);
            }
        }
        return Vector3.zero;
    }

    public void EmptyVelocityLog()
    {
        velocityLog = new ArrayList();
    }

    private float Average(ArrayList a)
    {
        float sum = 0f;
        foreach(float f in a) sum += f;
        return sum / a.Count;
    }

}
