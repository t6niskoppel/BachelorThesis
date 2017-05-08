using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

/**
 * 
 * This class is used for getting notifications about the hand movement and it's stages. 
 * 
 */

public class SwipeController : MonoBehaviour
{

    [SerializeField]
    MainController mainController;
    [SerializeField]
    WarningController WarningController;
    [SerializeField]
    HandController handController;
    [SerializeField]
    float minDeltaPosition; // NB! MUST BE THE SAME AS THE TIME IT TAKES VERTICAL MOTION TO RUN A CYCLE
    [SerializeField]
    float minLength;
    [SerializeField]
    float minVelocity;
    [SerializeField]
    bool debugVelocity;

    [HideInInspector]
    public float movementBeginTime;
    [HideInInspector]
    public float movementTopTurnTime;
    [HideInInspector]
    public float movementEndTime;
    [HideInInspector]
    public bool running;

    private List<float> positions = new List<float>();
    private List<float> times = new List<float>();
    private bool minLenghtExceeded;
    private bool minVelocityExceeded;
    private bool testMode;
    private bool debug;

    void Start()
    {
        testMode = mainController.test;
        debug = mainController.debug;
    }

    void Update()
    {
        if (debugVelocity)
        {
            Vector3 p = handController.getPalmVelocity();

            if (p != default(Vector3)) Debug.Log(p);
        }
        if (testMode && !running)
        {
            Debug.Log("Starting Motion Begin");
            StartUpHandMotionCycle();
        }
    }

    public void StartUpHandMotionCycle()
    {
        running = true;
        StartCoroutine(DetectVerticalMotion());
    }

    public void StartLeftRightHandMotionCycle()
    {
        running = true;
        StartCoroutine(DetectHorizontalMotion());
    }

    private IEnumerator DetectVerticalMotion()
    {
        while (handController.getPalmVelocity().y < 0.4) yield return null;

        if (debug)  Debug.Log("GOING UP");

        EventManager.TriggerEvent("GoingUp");

        movementBeginTime = Time.time;

        MinLengthSpeed("up");
        yield return StartCoroutine(TopTurn());
    }

    private IEnumerator TopTurn()
    {
        if (debug) Debug.Log("SC: WAITING FOR TOP TURN");
        while (handController.getPalmVelocity().y >= -0.2)  yield return null;

        if (testMode) StartCoroutine(measureDeltaPosition());

        
        if (debug)  Debug.Log("SC: TOP TURN");
        
        
        
        if (handController.getPalmPosition().y < 0.2)
        {
            EventManager.TriggerEvent("TooLow");
            //StopAllCoroutines();
        }

        EventManager.TriggerEvent("TopTurn");

        movementTopTurnTime = Time.time;

        StartCoroutine(MinLengthSpeedTimed("down"));
        yield return StartCoroutine(VerticalMotionEnd());
    }


    private IEnumerator VerticalMotionEnd()
    {
        while (handController.getPalmVelocity().y < -0.1)
        {
            yield return null;
        }
        
        if (debug) Debug.Log("SC: STOPPED");

        EventManager.TriggerEvent("Stopped");

        movementEndTime = Time.time;
        running = false;

        if (testMode)
        {
            StopCoroutine(measureDeltaPosition());
            Debug.Log("HAND mean pos: " + positions.Average().ToString("F4") + " time: " + times.Average().ToString("F4"));
            
            Debug.Log("Turn to Stop time: " + (Time.time - movementTopTurnTime));

            StartUpHandMotionCycle();
        }
    }


    private IEnumerator DetectHorizontalMotion()
    {
        while (handController.getPalmVelocity().x < 0.1)
        {
            yield return null;
        }

        if (debug)
            Debug.Log("GOING RIGHT");

        EventManager.TriggerEvent("GoingRight");

        movementBeginTime = Time.time;

        MinLengthSpeed("right");
        yield return StartCoroutine(RightTurn());
    }

    private IEnumerator RightTurn()
    {
        while (handController.getPalmVelocity().x > 0)
        {
            yield return null;
        }

        if (debug)
            Debug.Log("RIGHT TURN");

        EventManager.TriggerEvent("RightTurn");

        movementTopTurnTime = Time.time;

        yield return StartCoroutine(MinLengthSpeedTimed("left"));
        yield return StartCoroutine(HorizontalMotionEnd());
    }

    private IEnumerator HorizontalMotionEnd()
    {
        while (handController.getPalmVelocity().x < -0.2)
        {
            yield return null;
        }

        if (debug) Debug.Log("SC: STOPPED");

        EventManager.TriggerEvent("Stopped");

        movementEndTime = Time.time;
        running = false;

        if (testMode)
        {
            StartLeftRightHandMotionCycle();
        }
    }

    private IEnumerator measureDeltaPosition()
    {
        Vector3 startPosition = handController.getPalmPosition();
        Vector3 curPos;
        float deltaPosition;

        float a = Time.time;

        while (true)
        {
            curPos = handController.getPalmPosition();
            deltaPosition = Math.Abs(curPos.y - startPosition.y);

            if (deltaPosition >= minDeltaPosition)
            {
                positions.Add(deltaPosition);
                times.Add(Time.time - a);

                a = Time.time;
                startPosition = handController.getPalmPosition();
            }

            yield return null;
        }

    }

    private IEnumerator MinLengthSpeedTimed(string direction)
    {
        StartCoroutine(SwipeMinLength());
        StartCoroutine(SwipeMinVelocity(direction));

        float start = Time.time;
        float t = 0.8F;

        while (!(minLenghtExceeded && minVelocityExceeded) && (t > Time.time - start))
        {
            yield return null;
        }

        if (t < Time.time - start)
        {
            if (debug)
            {
                if (!minVelocityExceeded)
                    Debug.Log("Min velocity length not exceeded!");
                else
                    Debug.Log("Min swipe length not exceeded!");
            }

            EventManager.TriggerEvent("TooSlow");
        }
    }

    private void MinLengthSpeed(string direction)
    {
        StartCoroutine(SwipeMinLength());
        StartCoroutine(SwipeMinVelocity(direction));
        
    }

    private IEnumerator SwipeMinLength()
    {
        if (mainController.task != 2)
        {
            float startPosition = handController.getPalmPosition().y;

            minLenghtExceeded = false;
            while (Math.Abs(handController.getPalmPosition().y - startPosition) * 1000 < minLength)
            {
                yield return null;
            }
            minLenghtExceeded = true;
        }
        else
        {
            float startPosition = handController.getPalmPosition().x;

            minLenghtExceeded = false;
            while (Math.Abs(handController.getPalmPosition().x - startPosition) * 1000 < minLength)
            {
                yield return null;
            }
            minLenghtExceeded = true;
        }

        if (debug) Debug.Log("SC: Length exceeded!");

    }

    private IEnumerator SwipeMinVelocity(string direction)
    {
        minVelocityExceeded = false;

        if (direction == "up")
        {
            while (handController.getPalmVelocity().y > 0 && Math.Abs(handController.getPalmVelocity().y) * 1000 < minVelocity)
            {
                yield return null;
            }
        }
        else if (direction == "down")
        {
            while (handController.getPalmVelocity().y <= 0 && Math.Abs(handController.getPalmVelocity().y) * 1000 < minVelocity)
            {
                yield return null;
            }
        }
        else if (direction == "right")
        {
            while (handController.getPalmVelocity().x > 0 && Math.Abs(handController.getPalmVelocity().x) * 1000 < minVelocity / 2)
            {
                yield return null;
            }
        }
        else if (direction == "left")
        {
            while (handController.getPalmVelocity().x <= 0 && Math.Abs(handController.getPalmVelocity().x) * 1000 < minVelocity / 2)
            {
                yield return null;
            }
        }
        else
        {
            while (Math.Abs(handController.getPalmVelocity().y) * 1000 < minVelocity)
            {
                yield return null;
            }
        }

        minVelocityExceeded = true;

        if (debug) Debug.Log("SC: Velocity exceeded!");

    }
}