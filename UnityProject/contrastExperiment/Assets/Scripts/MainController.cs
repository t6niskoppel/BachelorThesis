using System.Collections;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

/**
 * 
 * The main logic controller for the experiment
 * 
 */

public class MainController : MonoBehaviour
{

    public bool debug;
    public bool test;

    public int task;

    public bool vocabulary;
    public bool tutorial;
    public bool experiment;


    public float timeBetweenTrials = 1.5f;
    public float stimuliShowTime;

    [SerializeField]
    private bool showHands;

    [Range(0.0F, 0.5F)]
    public float delayStart;
    [Range(0.0F, 0.5F)]
    public float delayStop;

    public int tutorialTrials;
    public int experimentTrials;
    public int trialsBetweenRest;

    [Range(0.0F, 1.0F)]
    public float standardContrast;
    [Range(0.0F, 1.0F)]
    public float testContrastLow;
    [Range(0.0F, 1.0F)]
    public float testContrastHigh;
    public int distCount;

    private bool timerFinished;

    private bool topTurn;
    private bool goingUp;
    private bool targetShown;

    private float gaborLcont; //left gabor contrast
    private float gaborRcont; //right gabor contrast
    private string _orientation;//which orientation is on the left
    public string _cpd;//what cpd stimulus was used
    private string ans; //answer to the first question
    private string extraQuestion; //which question was asked
    private string ans2; //answer to extra question
    public float avgVelocity; //the average velocity over the logged points

    private Texture2D gaborL; //the textures used when preparing stimuli
    private Texture2D gaborR;

    private IEnumerable<string> tutorialQuestions;
    private IEnumerable<string> experimentQuesions;

    public KeyCode leftKey = KeyCode.LeftArrow; //key used to answer "left"
    public KeyCode rightKey = KeyCode.RightArrow; //key used to answer "right"
    public KeyCode continueKey = KeyCode.Mouse0; //key used to continue during instructions shown
    public KeyCode endKey = KeyCode.Return;

    //raw image objects on experiment plane.rawImage on the left and rawImageMirror on the right
    public RawImage rawImage;
    public RawImage rawImageMirror;

    //textures for gabors. Make sure [0] is tilted to the left(45 deg) and [1] is tilted to the right(-45 deg)
    public Texture2D[] stimuli_2cpd;
    public Texture2D[] stimuli_4cpd;


    [SerializeField]
    private WarningController warningController;
    [SerializeField]
    private UIController uiController;
    [SerializeField]
    private FixationPointController fixationPointController;
    [SerializeField]
    private LeapHandVisibilityController leapHandVisibilityController;
    [SerializeField]
    private SwipeController swipeController;
    [SerializeField]
    private HandController handController;
    [SerializeField]
    private StimuliController stimuliController;
    [SerializeField]
    private SubjectDataController subjectDataController;
    [SerializeField]
    private Text trialNumber;

    /* 
    * Events are global and managed by EventManager
    */
    void OnEnable()
    {
        EventManager.StartListening("GoingUp", GoingUp);
        EventManager.StartListening("TopTurn", TopTurn);
    }

    /*
     * Don't forget to stop listening events
     */
    void OnDisable()
    {
        EventManager.StopListening("GoingUp", GoingUp);
        EventManager.StopListening("TopTurn", TopTurn);
    }

    void Start()
    {
        InputTracking.Recenter();
        stimuliController.hideStimuli();
        StartCoroutine(Experiment());
    }

    private IEnumerator Experiment()
    {
        if (vocabulary) yield return StartCoroutine(ExperimentVocabulary());
        if (tutorial) yield return StartCoroutine(MainTutorial());
        if (experiment) yield return StartCoroutine(MainExperiment());
    }

    //The coroutine for explaning experiment vocabulary to the test subject
    private IEnumerator ExperimentVocabulary()
    {
        fixationPointController.EnableFixationPoint();

        //Centering screen
        yield return StartCoroutine(ShowInstructionsAndWaitForKeyDown("centerScreen", continueKey));

        // Show text "fixation point"
        yield return StartCoroutine(ShowInstructionsAndWaitForKeyDown("FixationPoint", continueKey));

        // Show the cue
        StartCoroutine(PlayRandomCues(6));
        yield return StartCoroutine(ShowInstructionsAndWaitForKeyDown("SignalToMoveHand", continueKey));
        yield return fixationPointController.ShowCue();
        fixationPointController.DisableFixationPoint();

        // Show stimuli objects
        stimuliController.prepareStimuli(1.0f, 1.0f, stimuli_2cpd[0], stimuli_2cpd[1]);
        yield return null; //wait for a frame
        stimuliController.showStimuli();
        yield return StartCoroutine(ShowInstructionsAndWaitForKeyDown("TargetObject", continueKey));
        stimuliController.hideStimuli();

        // Show stimuli objects
        stimuliController.prepareStimuli(1.0f, 1.0f, stimuli_4cpd[0], stimuli_4cpd[1]);
        yield return null; //wait for a frame
        stimuliController.showStimuli();
        yield return StartCoroutine(ShowInstructionsAndWaitForKeyDown("TargetObject", continueKey));
        stimuliController.hideStimuli();

        //Show question answering screen
        yield return StartCoroutine(QuestionsAboutContrast("orientationQuestion"));

    }

    //The coroutine for the experiment trials before the main experiment. Same as main experiment but without data logging.

    private IEnumerator MainTutorial()
    {
        int trialId = 0;
        stimuliController.hideStimuli();
        if (!showHands) leapHandVisibilityController.disableHands();

        yield return StartCoroutine(ShowInstructionsAndWaitForKeyDown("mainTutorialIntro", continueKey));
        yield return null;

        //Distribution of parameters is calculated

        List<string> extraQuestions = GenerateExtraQuestions(tutorialTrials);

        List<float> conts = logspace(testContrastLow, testContrastHigh, distCount, tutorialTrials);

        List<string> cpdList = cpdDist(tutorialTrials);

        List<string> sideList = sideDist(tutorialTrials);

        List<string> orientationList = orientationDist(tutorialTrials);

        while (true)
        {
            if (debug) Debug.Log("Trial: " + (trialId + 1));
            
            if (trialId == tutorialTrials) 
            {
                //Do experiments for one block, then ask the tester if the test subject is ready for the main experiment or if he should do another block
                uiController.ShowText("continueTutorial");

                while (!Input.GetKeyDown(continueKey) && !Input.GetKeyDown(endKey))
                {
                    yield return null;
                }

                yield return uiController.HideText("continueTutorial");
                if (Input.GetKey(continueKey)) //if continueKey is pressed, a new set of trials will be prepared
                {
                    trialId = 0;
                    conts = logspace(testContrastLow, testContrastHigh, distCount, tutorialTrials);
                    extraQuestions = GenerateExtraQuestions(tutorialTrials);
                    cpdList = cpdDist(tutorialTrials);
                    orientationList = orientationDist(tutorialTrials);
                    sideList = sideDist(tutorialTrials);
                    yield return null;
                }
                else if (Input.GetKey(endKey)) //if endKey is pressed, the tutorial will be over
                {
                    break;
                }

            }
            trialNumber.text = "Trial: " + (trialId + 1);

            //--------preparing stimuli--------------------

            string standardSide = sideList[trialId];
            float testCont = conts[trialId];
            if (standardSide.Equals("left"))// the standard contrast is on left
            {
                gaborLcont = standardContrast;
                gaborRcont = testCont;
            }
            else if(standardSide.Equals("right"))
            {
                gaborRcont = standardContrast;
                gaborLcont = testCont;
            }

            _cpd = cpdList[trialId];

            if (_cpd.Equals("2cpd"))
            {
                if (debug) Debug.Log("Using 2 cpd stimuli");
                gaborL = stimuli_2cpd[0];
                gaborR = stimuli_2cpd[1];
            }
            else if(_cpd.Equals("4cpd"))
            {
                if (debug) Debug.Log("Using 4 cpd stimuli");
                gaborL = stimuli_4cpd[0];
                gaborR = stimuli_4cpd[1];
            }

            _orientation = orientationList[trialId];

            if (_orientation.Equals("left"))
            {
                stimuliController.prepareStimuli(gaborLcont, gaborRcont, gaborL, gaborR);
            }
            else if(_orientation.Equals("right"))
            {
                stimuliController.prepareStimuli(gaborLcont, gaborRcont, gaborR, gaborL);
            }

            fixationPointController.EnableFixationPoint();
            handController.EmptyVelocityLog();

            yield return new WaitForSeconds(timeBetweenTrials);

            //----------show cue and wait for hand movement -------------

            yield return fixationPointController.StartCoroutine("ShowCue");

            fixationPointController.DisableFixationPoint();

            if (debug) Debug.Log("MC: Please move hand");

            yield return StartCoroutine(WaitForHandMovement(3.0F));


            // If the subject did not move hand withing 3 seconds
            // then display notice "did you forgot to move your hand?"
            if (timerFinished)
            {
                if (debug) Debug.Log("MC: Unustasid liigutada kätt?");

                swipeController.StopAllCoroutines();

                yield return StartCoroutine(warningController.ShowHandNoticeUI());
            }

            else
            {
                if (debug) Debug.Log("MC: Hand detected");

                yield return StartCoroutine(WaitForTarget());

                //If hand started moving down before end of delay or hit the hitbox while moving down

                if (topTurn)
                {
                    Debug.LogError("Move hand only once up and back down");
                    yield return StartCoroutine(warningController.ShowNoHitNoticeUI());
                    
                    yield return new WaitForSeconds(1F);
                }
                else // DELAY END, playing target animation
                {

                    if (delayStop > 0) yield return StartCoroutine(ShowStimuli()); //if it was a trial using delay

                    if (timerFinished)//if hand has moved out of the hitbox at the end of the delay
                    {
                        yield return warningController.ShowNoHitNoticeUI();
                        if (debug) Debug.LogError("Couldn't show stimuli!");
                    }
                    else // Animation played, ask questions
                    {

                        yield return StartCoroutine(QuestionsAboutContrast(extraQuestions[trialId]));
                    }
                }
            }
            yield return warningController.FadeToBlack(0.5f);
            trialId += 1;
        }

        fixationPointController.DisableFixationPoint();
    }

    /**
     *
     *-----------------------The coroutine for main experiment -------------------------- 
     * 
     */

    IEnumerator MainExperiment()
    {
        subjectDataController.WriteExperimentData(); //write the subject data and experiment parameters
        int trialId = 0;
        TrialData trial;

        stimuliController.hideStimuli();

        if (!showHands) leapHandVisibilityController.disableHands();

        yield return StartCoroutine(ShowInstructionsAndWaitForKeyDown("experimentIntro", continueKey));

        //Distribution of experiment parameters

        List<string> extraQuestions = GenerateExtraQuestions(experimentTrials);

        List<string> cpdList = cpdDist(experimentTrials);

        List<string> orientationList = orientationDist(experimentTrials);

        List<string> sideList = sideDist(tutorialTrials);

        List<float> conts = logspace(testContrastLow, testContrastHigh, distCount, experimentTrials);   //calculate the logarithmic increments of test contrasts

        //-------Main experiment loop------

        while (trialId < experimentTrials)
        {
            if (debug) Debug.Log("Trial: " + (trialId + 1));

            if (trialId % trialsBetweenRest == 0 && trialId != 0)
            {
                StartCoroutine(warningController.ShowPausNoticeUI(8f));              
                yield return new WaitForSeconds(9);
                yield return warningController.FadeToBlack(0.6f);
                yield return new WaitForSeconds(1);
            }
            trialNumber.text = "Trial: " + (trialId + 1);
            //----------setting stimuli contrasts and orientations ---------

            string standardSide = sideList[trialId];
            float testCont = conts[trialId];
            if (standardSide.Equals("left"))//standard contrast is on left
            {
                gaborLcont = standardContrast;
                gaborRcont = testCont;
            }
            else if(standardSide.Equals("right"))
            {
                gaborRcont = standardContrast;
                gaborLcont = testCont;
            }

            _cpd = cpdList[trialId]; 

            if (_cpd.Equals("2cpd"))
            {
                if (debug) Debug.Log("Using 2 cpd stimuli");
                _cpd = "2cpd";
                gaborL = stimuli_2cpd[0];
                gaborR = stimuli_2cpd[1];
            }
            else if(_cpd.Equals("4cpd"))
            {
                if (debug) Debug.Log("Using 4 cpd stimuli");
                _cpd = "4cpd";
                gaborL = stimuli_4cpd[0];
                gaborR = stimuli_4cpd[1];
            }

            _orientation = orientationList[trialId];//if 0 then gabor with left orientation is on left

            if (_orientation.Equals("left"))
            {
                stimuliController.prepareStimuli(gaborLcont, gaborRcont, gaborL, gaborR);
            }
            else if(_orientation.Equals("right"))
            {
                stimuliController.prepareStimuli(gaborLcont, gaborRcont, gaborR, gaborL);
            }

            fixationPointController.EnableFixationPoint();
            handController.EmptyVelocityLog();

            yield return new WaitForSeconds(timeBetweenTrials);

            //----------show cue and wait for hand movement -------------

            yield return fixationPointController.StartCoroutine("ShowCue");

            fixationPointController.DisableFixationPoint();


            if (debug) Debug.Log("MC: Please move hand");

            yield return StartCoroutine(WaitForHandMovement(3.0F));


            // If the subject did not move hand withing 3 seconds
            // then display notice "did you forgot to move your hand?"
            if (timerFinished)
            {
                if (debug) Debug.Log("MC: Unustasid liigutada kätt?");

                swipeController.StopAllCoroutines();

                yield return StartCoroutine(warningController.ShowHandNoticeUI());
                trial = new TrialData(trialId, gaborLcont, gaborRcont, _orientation, _cpd);
            }

            else
            {
                if (debug) Debug.Log("MC: Hand detected");

                yield return StartCoroutine(WaitForTarget());

                //If hand started moving down before end of delay or hit the hitbox while moving down

                if (topTurn) 
                {
                    yield return StartCoroutine(warningController.ShowNoHitNoticeUI());

                    trial = new TrialData(trialId, gaborLcont, gaborRcont, _orientation, _cpd, true);
                    yield return new WaitForSeconds(1F);
                }
                else // DELAY END, playing target animation
                {

                    if (delayStop > 0) yield return StartCoroutine(ShowStimuli()); //if it was a trial using delay

                    if (timerFinished)//if hand has moved out of the hitbox at the end of the delay
                    {
                        yield return warningController.ShowNoHitNoticeUI();
                        if (debug) Debug.LogError("Couldn't show stimuli!");
                        trial = new TrialData(trialId, gaborLcont, gaborRcont, _orientation, _cpd, true);
                    }
                    else // Animation played, ask questions
                    {
                        yield return StartCoroutine(QuestionsAboutContrast(extraQuestions[trialId]));
                        trial = new TrialData(trialId, gaborLcont, gaborRcont, _orientation, _cpd, true, true, avgVelocity, rawImage.transform.position,
                        Vector3.Distance(rawImage.transform.position, handController.centerEyeAnchor.transform.position), ans, extraQuestions[trialId], ans2);
                    }
                }
            }
            subjectDataController.WriteTrialData(trial);
            yield return warningController.FadeToBlack(0.3f);
            trialId += 1;
        }

        fixationPointController.DisableFixationPoint();
        //subjectDataController.WritePlayerData(); data is already written after every trial
        yield return StartCoroutine(ShowInstructionsAndWaitForKeyDown("experimentOutro", continueKey));
        Application.Quit();
    }

    /**
     * 
     *    Pick random delay period between fromTime and toTime
     *    If no toTime is provided, wait exactly fromTime seconds
     * 
     */

    private IEnumerator DelayHandTrigger(float fromTime, float toTime = 0)
    {
        float delay;
        if (toTime == 0)
        {
            delay = fromTime;
        }
        else
        {
            delay = Random.Range(fromTime, toTime);
        }

        if (debug) Debug.Log("Delay END");

        yield return new WaitForSeconds(delay);
    }


    /**
     * 
     * Waits for target
     * 
     */ 

    private IEnumerator WaitForTarget()
    {
        float start = Time.time;
        Vector3 newPos = handController.getPalmHitPoint();
        while (newPos.Equals(Vector3.zero) && Time.time - start < 2.0f)
        {
            newPos = handController.getPalmHitPoint();
            yield return null;
        }
        //if raycast hits the "raycastable" plane then either start delay and then show stimuli or show stimuli without delay
        if (delayStop > 0)
        {
            yield return StartCoroutine(DelayHandTrigger(delayStart, delayStop));
        }
        else
        {
            if (!topTurn)
            {
                swipeController.StopAllCoroutines();
                timerFinished = false;
                if (test) Debug.Log("hand hitpoint = " + newPos);
                rawImage.transform.position = new Vector3(newPos.x, newPos.y, rawImage.transform.position.z);
                rawImageMirror.transform.position = new Vector3(newPos.x * -1f, newPos.y, newPos.z);
                stimuliController.showStimuli();
                yield return new WaitForSeconds(stimuliShowTime);
                stimuliController.hideStimuli();
            }
        }
    }

    /**
     * 
     * Method that shows stimuli after the delay timer finishes.
     * 
     */

    private IEnumerator ShowStimuli()
    {
        timerFinished = false;
        Vector3 hit = handController.getPalmHitPoint();
        if (hit.Equals(Vector3.zero))
        {
            timerFinished = true;
        }
        else
        {
            if (test) Debug.Log("hand hitpoint = " + hit);
            rawImage.transform.position = new Vector3(hit.x, hit.y, rawImage.transform.position.z);
            rawImageMirror.transform.position = new Vector3(hit.x * -1f, hit.y, hit.z);
            stimuliController.showStimuli();
            yield return new WaitForSeconds(stimuliShowTime);
            stimuliController.hideStimuli();
        }

        yield return null;

    }


    /**
     * 
     * Shows questions to user and saves the answers
     * 
     */ 

    private IEnumerator QuestionsAboutContrast(string extraQuestion)
    {
        yield return new WaitForSeconds(1.5f); //wait a while so the subject finishes hand movement


        yield return uiController.StartCoroutine("ShowText", "sideQuestion");

        while (!Input.GetKeyDown(leftKey) && !Input.GetKeyDown(rightKey)) yield return null;
        if (Input.GetKeyDown(leftKey))
        {
            ans = "left";
        }
        else if (Input.GetKeyDown(rightKey))
        {
            ans = "right";
        }
        yield return uiController.StartCoroutine("HideText", "sideQuestion");

        if (extraQuestion.Equals("orientationQuestion"))
        {
            yield return uiController.StartCoroutine("ShowText", "orientationQuestion");
            extraQuestion = "orientationQuestion";
            while (!Input.GetKeyDown(leftKey) && !Input.GetKeyDown(rightKey)) yield return null;
            if (Input.GetKeyDown(leftKey))
            {
                ans2 = "left";
            }
            else if (Input.GetKeyDown(rightKey))
            {
                ans2 = "right";
            }
            yield return uiController.StartCoroutine("HideText", "orientationQuestion");
        }
        else if (extraQuestion.Equals("none"))
        {
            extraQuestion = "none";
            ans2 = "none";
        }

    }
    
    /**
     * 
     *Prepares extra questions asked:
     *   30% what orientation was more contrasty
     *   70 % no extra question 
     * 
     */

    private List<string> GenerateExtraQuestions(int totalTrials)
    {
        IEnumerable<string> q1 = Enumerable.Repeat("orientationQuestion", Mathf.RoundToInt(totalTrials * 0.3f));
        IEnumerable<string> q2 = Enumerable.Repeat("none", Mathf.RoundToInt(totalTrials * 0.7f));

        var concatQuestions = Enumerable.Concat(q1,q2);

        var shuffledTrials = concatQuestions.OrderBy(elem => System.Guid.NewGuid());
        return shuffledTrials.ToList();
    }

    /**
     * 
     * Waits until upwards hand motion is detected or until the timer finishes 
     * 
     */

    private IEnumerator WaitForHandMovement(float time)
    {
        timerFinished = false;
        goingUp = false;
        topTurn = false;

        swipeController.StartUpHandMotionCycle();

        float start = Time.time;
        while (!goingUp && Time.time < start + time) yield return null;

        if (Time.time >= start + time) timerFinished = true;
    }

    /////////////////////////////// TUTORIAL FUNCTIONS /////////////////////////////////
    private IEnumerator PlayRandomCues(int j)
    {
        yield return new WaitForSeconds(1.0F);

        for (int i = 1; i <= j; i++)
        {
            yield return new WaitForSeconds(1.0F);
            yield return StartCoroutine(fixationPointController.ShowCue());
        }
    }

    private IEnumerator ShowInstructionsAndWaitForKeyDown(string instructions, KeyCode key)
    {
        uiController.ShowText(instructions);
        uiController.ShowText("pressToContinue");
        while (!Input.GetKeyDown(key))
        {
            if (Input.GetKeyDown(leftKey) || Input.GetKeyDown(rightKey))
            {
                InputTracking.Recenter();
            }
            yield return null;
        }
        yield return StartCoroutine(uiController.HideText("pressToContinue"));
        yield return StartCoroutine(uiController.HideText(instructions));
    }


    private IEnumerator RecenterVR()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                InputTracking.Recenter();
            }
            yield return null;
        }
    }

    //Generates stimuli orientations for experiment iterations

    public List<string> orientationDist(int trials)
    {
        IEnumerable<string> q1 = Enumerable.Repeat("left", Mathf.RoundToInt(trials / 2.0f));
        IEnumerable<string> q2 = Enumerable.Repeat("right", Mathf.RoundToInt(trials / 2.0f));
        IEnumerable<string> temp = Enumerable.Concat(q1, q2);
        var shuffledDist = temp.OrderBy(elem => System.Guid.NewGuid());
        return shuffledDist.ToList();
    }

    //Which side the standard contrast will be on
    public List<string> sideDist(int trials)
    {
        IEnumerable<string> q1 = Enumerable.Repeat("left", Mathf.RoundToInt(trials / 2.0f));
        IEnumerable<string> q2 = Enumerable.Repeat("right", Mathf.RoundToInt(trials / 2.0f));
        IEnumerable<string> temp = Enumerable.Concat(q1, q2);
        var shuffledDist = temp.OrderBy(elem => System.Guid.NewGuid());
        return shuffledDist.ToList();
    }

    //Generates cpd values for experiment iterations
    public List<string> cpdDist(int trials)
    {
        IEnumerable<string> q1 = Enumerable.Repeat("2cpd", Mathf.RoundToInt(trials / 2.0f));
        IEnumerable<string> q2 = Enumerable.Repeat("4cpd", Mathf.RoundToInt(trials / 2.0f));
        IEnumerable<string> temp = Enumerable.Concat(q1, q2);
        var shuffledDist = temp.OrderBy(elem => System.Guid.NewGuid());
        return shuffledDist.ToList();
    }

    //Creates a logspace, start and endpoints included
    public List<float> logspace(float start, float end, int numberOfIncrements, int trials)
    {
        float d = numberOfIncrements - 1;
        float p = end / start;
        IEnumerable<float> increments = Enumerable.Repeat(0.0f, 0);
        for (int i = 0; i < numberOfIncrements; i++)
        {
            if (i == 0)
            {
                increments = Enumerable.Repeat(start * Mathf.Pow(p, i / d), Mathf.RoundToInt((1f / numberOfIncrements) * trials));
            }
            else
            {
                increments = Enumerable.Concat(Enumerable.Repeat(start * Mathf.Pow(p, i / d), Mathf.RoundToInt((1f / numberOfIncrements) * trials)), increments);
            }
        }
        var shuffledTrials = increments.OrderBy(elem => System.Guid.NewGuid());

        return shuffledTrials.ToList();
    }


    /////////////////////////////// Event listener f-ns /////////////////////////////////

    private void GoingUp()
    {
        goingUp = true;
    }

    private void TopTurn()
    {
        topTurn = true;
    }
}

