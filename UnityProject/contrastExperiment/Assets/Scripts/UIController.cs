using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private UIFader CenterScreenUI;
    [SerializeField]
    private UIFader FixationPointUI;
    [SerializeField]
    private UIFader SignalToMoveHandUI;
    [SerializeField]
    private UIFader MovingObjectsUI;
    [SerializeField]
    private UIFader TargetObjectUI;
    [SerializeField]
    private UIFader HandVisibleUI;
    [SerializeField]
    private UIFader HandInvisibleUI;
    [SerializeField]
    private UIFader ExperimentIntroUI;
    [SerializeField]
    private UIFader ExperimentOutroUI;
    [SerializeField]
    private UIFader MainTutorialIntroUI;
    [SerializeField]
    private UIFader OrientationQuestionUI;
    [SerializeField]
    private UIFader SideQuestionUI;
    [SerializeField]
    private UIFader ContinueUI;
    [SerializeField]
    private UIFader ContinueTutorialUI;

    public void ShowText(string instructions)
    {
        switch (instructions)
        {
            case "centerScreen":
                StartCoroutine(ShowCenterScreenUI());
                break;
            case "FixationPoint":
                StartCoroutine(ShowFixationPointUI());
                break;
            case "SignalToMoveHand":
                StartCoroutine(ShowSignalToMoveHandUI());
                break;
            case "MovingObjects":
                StartCoroutine(ShowMovingObjectsUI());
                break;
            case "TargetObject":
                StartCoroutine(ShowTargetObjectUI());
                break;
            case "handVisible":
                StartCoroutine(ShowHandVisibleUI());
                break;
            case "handInvisible":
                StartCoroutine(ShowHandInvisibleUI());
                break;
            case "experimentIntro":
                StartCoroutine(ShowExperimentIntroUI());
                break;
            case "experimentOutro":
                StartCoroutine(ShowExperimentOutroUI());
                break;
            case "mainTutorialIntro":
                StartCoroutine(ShowMainTutorialIntroUI());
                break;
            case "orientationQuestion":
                StartCoroutine(ShowOrientationQuestionUI());
                break;
            case "sideQuestion":
                StartCoroutine(ShowSideQuestionUI());
                break;
            case "pressToContinue":
                StartCoroutine(ShowContinueUI());
                break;
            case "continueTutorial":
                StartCoroutine(ShowContinueTutorialUI());
                break;
            default:
                Debug.Log("Unregistred case of SHOW UI text call!");
                break;
        }
    }

    public IEnumerator HideText(string instructions)
    {
        switch (instructions)
        {
            case "centerScreen":
                yield return StartCoroutine(HideCenterScreenUI());
                break;
            case "FixationPoint":
                yield return StartCoroutine(HideFixationPointUI());
                break;
            case "SignalToMoveHand":
                yield return StartCoroutine(HideSignalToMoveHandUI());
                break;
            case "MovingObjects":
                yield return StartCoroutine(HideMovingObjectsUI());
                break;
            case "TargetObject":
                yield return StartCoroutine(HideTargetObjectUI());
                break;
            case "handVisible":
                yield return StartCoroutine(HideHandVisibleUI());
                break;
            case "handInvisible":
                yield return StartCoroutine(HideHandInvisibleUI());
                break;
            case "experimentIntro":
                yield return StartCoroutine(HideExperimentIntroUI());
                break;
            case "experimentOutro":
                yield return StartCoroutine(HideExperimentOutroUI());
                break;
            case "mainTutorialIntro":
                yield return StartCoroutine(HideMainTutorialIntroUI());
                break;
            case "orientationQuestion":
                StartCoroutine(HideOrientationQuestionUI());
                break;
            case "sideQuestion":
                StartCoroutine(HideSideQuestionUI());
                break;
            case "pressToContinue":
                StartCoroutine(HideContinueUI());
                break;
            case "continueTutorial":
                StartCoroutine(HideContinueTutorialUI());
                break;
            default:
                Debug.Log("Unregistred case of HIDE UI text call!");
                break;
        }
    }


    private IEnumerator ShowCenterScreenUI()
    {
        yield return StartCoroutine(CenterScreenUI.InteruptAndFadeIn());
    }
    private IEnumerator HideCenterScreenUI()
    {
        yield return StartCoroutine(CenterScreenUI.InteruptAndFadeOut());
    }

    private IEnumerator ShowContinueUI()
    {
        yield return StartCoroutine(ContinueUI.InteruptAndFadeIn());
    }
    private IEnumerator HideContinueUI()
    {
        yield return StartCoroutine(ContinueUI.InteruptAndFadeOut());
    }

    private IEnumerator ShowFixationPointUI()
    {
        yield return StartCoroutine(FixationPointUI.InteruptAndFadeIn());
    }
    private IEnumerator HideFixationPointUI()
    {
        yield return StartCoroutine(FixationPointUI.InteruptAndFadeOut());
    }


    private IEnumerator ShowSignalToMoveHandUI()
    {
        yield return StartCoroutine(SignalToMoveHandUI.InteruptAndFadeIn());
    }
    private IEnumerator HideSignalToMoveHandUI()
    {
        yield return StartCoroutine(SignalToMoveHandUI.InteruptAndFadeOut());
    }


    private IEnumerator ShowMovingObjectsUI()
    {
        yield return StartCoroutine(MovingObjectsUI.InteruptAndFadeIn());
    }
    private IEnumerator HideMovingObjectsUI()
    {
        yield return StartCoroutine(MovingObjectsUI.InteruptAndFadeOut());
    }


    private IEnumerator ShowTargetObjectUI()
    {
        yield return StartCoroutine(TargetObjectUI.InteruptAndFadeIn());
    }
    private IEnumerator HideTargetObjectUI()
    {
        yield return StartCoroutine(TargetObjectUI.InteruptAndFadeOut());
    }

    private IEnumerator ShowHandVisibleUI()
    {
        yield return StartCoroutine(HandVisibleUI.InteruptAndFadeIn());
    }
    private IEnumerator HideHandVisibleUI()
    {
        yield return StartCoroutine(HandVisibleUI.InteruptAndFadeOut());
    }


    private IEnumerator ShowHandInvisibleUI()
    {
        yield return StartCoroutine(HandInvisibleUI.InteruptAndFadeIn());
    }
    private IEnumerator HideHandInvisibleUI()
    {
        yield return StartCoroutine(HandInvisibleUI.InteruptAndFadeOut());
    }


    private IEnumerator ShowExperimentIntroUI()
    {
        yield return StartCoroutine(ExperimentIntroUI.InteruptAndFadeIn());
    }
    private IEnumerator HideExperimentIntroUI()
    {
        yield return StartCoroutine(ExperimentIntroUI.InteruptAndFadeOut());
    }


    private IEnumerator ShowExperimentOutroUI()
    {
        yield return StartCoroutine(ExperimentOutroUI.InteruptAndFadeIn());
    }
    private IEnumerator HideExperimentOutroUI()
    {
        yield return StartCoroutine(ExperimentOutroUI.InteruptAndFadeOut());
    }


    private IEnumerator ShowMainTutorialIntroUI()
    {
        yield return StartCoroutine(MainTutorialIntroUI.InteruptAndFadeIn());
    }
    private IEnumerator HideMainTutorialIntroUI()
    {
        yield return StartCoroutine(MainTutorialIntroUI.InteruptAndFadeOut());
    }

    private IEnumerator ShowContinueTutorialUI()
    {
        yield return StartCoroutine(ContinueTutorialUI.InteruptAndFadeIn());
    }
    private IEnumerator HideContinueTutorialUI()
    {
        yield return StartCoroutine(ContinueTutorialUI.InteruptAndFadeOut());
    }

    private IEnumerator ShowOrientationQuestionUI()
    {
        yield return StartCoroutine(OrientationQuestionUI.InteruptAndFadeIn());
    }
    private IEnumerator HideOrientationQuestionUI()
    {
        yield return StartCoroutine(OrientationQuestionUI.InteruptAndFadeOut());
    }


    private IEnumerator ShowSideQuestionUI()
    {
        yield return StartCoroutine(SideQuestionUI.InteruptAndFadeIn());
    }
    private IEnumerator HideSideQuestionUI()
    {
        yield return StartCoroutine(SideQuestionUI.InteruptAndFadeOut());
    }

}