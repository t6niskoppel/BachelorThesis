using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * 
 * Used to transfer data between menu scene and the experiment scene 
 * 
 */


public class MenuSettings : MonoBehaviour {

    public Toggle vocab;
    public Toggle training;
    public Toggle experiment;

    private bool _vocab;
    private bool _training;
    private bool _experiment;

    public InputField trainingTrials;
    public InputField ExperimentTrials;

    private int _trainingTrials;
    private int _experimentTrials;

    public InputField stimuliShowTime;
    public InputField timeBetweenTrials;
    public InputField trialsBetweenRest;

    private float _stimuliShowTime;
    private float _timeBetweenTrials;
    private int _trialsBetweenRest;

    public Slider standardCont;
    public Slider testContLow;
    public Slider testContHigh;
    public InputField count;

    private float _standardCont;
    private float _testContLow;
    private float _testContHigh;
    private int _count;

    public InputField age;
    public Dropdown sex;
    public Dropdown handedness;
    public Dropdown vision;

    private int _age;
    private string _sex;
    private string _handedness;
    private string _vision;

    private MainController mainController = null;

    private SubjectDataController subjectDataController = null;

    void OnEnable()
    {
        SceneManager.sceneLoaded += LevelLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= LevelLoaded;
    }

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
    
    //At the start of the scene assignes values if scene has a main controller and subjectDataController
	void LevelLoaded (Scene scene, LoadSceneMode mode) {

        GameObject mc = GameObject.Find("MainController");
        GameObject _sdc = GameObject.Find("SubjectDataController");

        if (mc != null ) mainController = mc.GetComponent<MainController>();
        if (_sdc != null) subjectDataController = _sdc.GetComponent<SubjectDataController>();

        if (mainController != null && subjectDataController != null)
        {
            mainController.vocabulary = _vocab;
            mainController.tutorial = _training;
            mainController.experiment = _experiment;

            mainController.tutorialTrials = _trainingTrials;
            mainController.experimentTrials = _experimentTrials;

            mainController.stimuliShowTime = _stimuliShowTime;
            mainController.timeBetweenTrials = _timeBetweenTrials;
            mainController.trialsBetweenRest = _trialsBetweenRest;

            mainController.standardContrast = _standardCont;
            mainController.testContrastLow = _testContLow;
            mainController.testContrastHigh = _testContHigh;
            mainController.distCount = _count;

            subjectDataController.age = _age;
            subjectDataController.sex = _sex;
            subjectDataController.handedness = _handedness;
            subjectDataController.vision = _vision;
        }
	}

    //saves values to private variables and then loads the next scene
    public void SaveValuesAndLoad(string sceneName)
    {
        _vocab = vocab.isOn;
        _training = training.isOn;
        _experiment = experiment.isOn;

        _trainingTrials = ParseInt(trainingTrials.text, 10);
        _experimentTrials = ParseInt(ExperimentTrials.text, 10);

        _stimuliShowTime = ParseFloat(stimuliShowTime.text, 0.5f);
        _timeBetweenTrials = ParseFloat(timeBetweenTrials.text, 1.5f);
        _trialsBetweenRest = ParseInt(trialsBetweenRest.text, 30);

        _standardCont = standardCont.value;
        _testContLow = testContLow.value;
        _testContHigh = testContHigh.value;
        _count = ParseInt(count.text, 10);

        _age = ParseInt(age.text, -1);
        _sex = sex.captionText.text;
        _handedness = handedness.captionText.text;
        _vision = vision.captionText.text;

        SceneManager.LoadScene(sceneName);
    }

    //tries to parse float from string, returns the float parsed or default value d
    float ParseFloat(string n, float d)
    {
        float value = d;
        float.TryParse(n, out value);
        return value;
    }

    //tries to parse int from string, returns the int parsed or default value d
    int ParseInt(string n, int d)
    {
        int value = d;
        int.TryParse(n, out value);
        return value;
    }
}
