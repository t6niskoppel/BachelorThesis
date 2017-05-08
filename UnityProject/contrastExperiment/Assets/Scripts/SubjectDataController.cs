using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class SubjectDataController : MonoBehaviour
{
    public static SubjectDataController control;

    public List<TrialData> trials = new List<TrialData>();
    public int age = 25;
    public string sex = "M/N";
    public string handedness = "Right/Left";
    public string vision = "No/Glasses/Yes_NoGlasses";
    private int experimentTrials;
    private float standardCont;
    private float testContLow;
    private float testContHigh;
    private int testContIncrements;
    private float stimuliShowingTime;
    private float timeBetweenTrials;
    private int trialsBetweenRest;

    public MainController mc;

    private DateTime date = DateTime.Now; // This isn't shown in Editor, don't bother with getter
    public int subjectId;
    private string savePath;

    void Start()
    {
        savePath = Application.dataPath + "/ExperimentData";
        bool exists = Directory.Exists(savePath);
        if (!exists) Directory.CreateDirectory(savePath);
        Debug.Log("Subject id: " + subjectId);
    }

    public void WriteTrialData(TrialData trial)//Writes one trial to a .CSV file
    {
        Debug.Log("Writing trial data...");
        string data = "";
        if (!File.Exists(savePath + "/subject_" + subjectId + "_trial_data.csv")) //if the file doesn't exist, write a header first
        {
            data = string.Format("TRIAL_ID;LEFT_STIMULUS_CONTRAST;RIGHT_STIMULUS_CONTRAST;LEFT_STIMULUS_ORIENTATION;MOVED;TARGET_SHOWN;AVG_HAND_VEL;LEFT_STIMULUS_X;LEFT_STIMULUS_Y;LEFT_STIMULUS_Z;CPD;LEFT_STIMUL_DIST_TO_FACE;WHAT_SIDE_WAS_MORE_CONTRASTY;QUESTION_2_ASKED;QUESTION_2_ANS\n");
        }

        data += trial.trialId + ";";
        data += trial.gaborLContrast.ToString() + ";";
        data += trial.gaborRContrast.ToString() + ";";
        data += trial.leftOrientation + ";";
        data += trial.moved.ToString() + ";";
        data += trial.targetShown.ToString() + ";";
        data += trial.avgVel.ToString() + ";";
        data += trial.leftStimulusX.ToString() + ";";
        data += trial.leftStimulusY.ToString() + ";";
        data += trial.leftStimulusZ.ToString() + ";";
        data += trial.cpd + ";";
        data += trial.leftStimulusDist.ToString() + ";";
        data += trial.ans + ";";
        data += trial.questionAsked + ";";
        data += trial.ans2 + ";";
        data += "\n";


        File.AppendAllText(savePath + "/subject_" + subjectId + "_trial_data.csv", data);

        Debug.Log("Done!");
    }
    
    public void WriteExperimentData()//Writes the player data and experiment parameters into a .CSV file
    {
        Debug.Log("Writing experiment data...");
        string data = string.Format("Subject_ID;date;Age;Sex;Handedness;Vision;Experiment_trial_count;Standard_contrast;Test_contrast_low;Test_contrast_high;Test_contrast_increments;Stimuli_show_time;Time_between_trials;Trials_between_rest \n");
        experimentTrials = mc.experimentTrials;
        standardCont = mc.standardContrast;
        testContLow = mc.testContrastLow;
        testContHigh = mc.testContrastHigh;
        testContIncrements = mc.distCount;
        stimuliShowingTime = mc.stimuliShowTime;
        timeBetweenTrials = mc.timeBetweenTrials;
        trialsBetweenRest = mc.trialsBetweenRest;
        data += string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13}", subjectId, date, age, sex, handedness, vision, experimentTrials, standardCont, testContLow, testContHigh, testContIncrements, stimuliShowingTime, timeBetweenTrials, trialsBetweenRest);

        File.AppendAllText(savePath + "/subject_" + subjectId + "experiment__data.csv", data);

        Debug.Log("Done!");
    }

    // Writes all the player data and trial data at once
    public void WritePlayerData()
    {
        print("Writing results to file...");
        // Subject header and data
        string data = string.Format("Subject_ID;date;Age;Sex;Handedness;Vision;Experiment_trial_count;Standard_contrast;Test_contrast_low;Test_contrast_high;Test_contrast_increments;Stimuli_show_time;Time_between_trials;Trials_between_rest \n");
        experimentTrials = mc.experimentTrials;
        standardCont = mc.standardContrast;
        testContLow = mc.testContrastLow;
        testContHigh = mc.testContrastHigh;
        testContIncrements = mc.distCount;
        stimuliShowingTime = mc.stimuliShowTime;
        timeBetweenTrials = mc.timeBetweenTrials;
        trialsBetweenRest = mc.trialsBetweenRest;
        data += string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13}", subjectId, date, age, sex, handedness, vision, experimentTrials, standardCont, testContLow, testContHigh, testContIncrements, stimuliShowingTime, timeBetweenTrials, trialsBetweenRest);

        File.AppendAllText(savePath + "/subject_" + subjectId + "experiment__data.csv", data);

        // Trial data header and data
        data = string.Format("TRIAL_ID;LEFT_STIMULUS_CONTRAST;RIGHT_STIMULUS_CONTRAST;LEFT_STIMULUS_ORIENTATION;MOVED;TARGET_SHOWN;AVG_HAND_VEL;LEFT_STIMULUS_X;LEFT_STIMULUS_Y;LEFT_STIMULUS_Z;CPD;LEFT_STIMUL_DIST_TO_FACE;WHAT_SIDE_WAS_MORE_CONTRASTY;QUESTION_2_ASKED;QUESTION_2_ANS\n");
        foreach (TrialData trial in trials)
        {
            data += trial.trialId + ";";
            data += trial.gaborLContrast.ToString() + ";";
            data += trial.gaborRContrast.ToString() + ";";
            data += trial.leftOrientation + ";";
            data += trial.moved.ToString() + ";";
            data += trial.targetShown.ToString() + ";";
            data += trial.avgVel.ToString() + ";";
            data += trial.leftStimulusX.ToString() + ";";
            data += trial.leftStimulusY.ToString() + ";";
            data += trial.leftStimulusZ.ToString() + ";";
            data += trial.cpd + ";";
            data += trial.leftStimulusDist.ToString() + ";";
            data += trial.ans + ";";
            data += trial.questionAsked + ";";
            data += trial.ans2 + ";";
            data += "\n";
        }

        File.AppendAllText(savePath + "/subject_" + subjectId + "_trial_data.csv", data);

        print("Done!");
    }
}
//Used for transferring trial data
public class TrialData
{
    public int trialId;
    public float gaborLContrast;
    public float gaborRContrast;
    public string leftOrientation;
    public bool moved;
    public bool targetShown;
    public float avgVel;
    public float leftStimulusX;
    public float leftStimulusY;
    public float leftStimulusZ;
    public string cpd;
    public float leftStimulusDist;
    public string ans;
    public string questionAsked;
    public string ans2;

    public TrialData(
        int trialId,
        float gaborLContrast,
        float gaborRContrast,
        string leftOrientation,
        string cpd,
        bool moved = false,
        bool targetShown = false,
        float avgVel = 0f,
        Vector3 leftStimulusPos = default(Vector3),
        float leftStimulusDist = 0f,
        string ans = "none",
        string questionAsked = "none",
        string ans2 = "none"
        )
    {
        this.trialId = trialId;
        this.gaborLContrast = gaborLContrast;
        this.gaborRContrast = gaborRContrast;
        this.leftOrientation = leftOrientation;
        this.moved = moved;
        this.targetShown = targetShown;
        this.avgVel = avgVel;
        leftStimulusX = leftStimulusPos.x;
        leftStimulusY = leftStimulusPos.y;
        leftStimulusZ = leftStimulusPos.z;
        this.cpd = cpd;
        this.leftStimulusDist = leftStimulusDist;
        this.ans = ans;
        this.questionAsked = questionAsked;
        this.ans2 = ans2;
    }
}

[Serializable]
class PlayerData
{
    public DateTime date;
    public int subjectId;
    public int age;
    public string sex;
    public List<TrialData> trials;
}