using UnityEngine;
using System.Collections;

public class WarningController : MonoBehaviour
{

    [SerializeField]
    private FixationPointController fixationPointController;

    [SerializeField]
    private UIFader handNoticeUI;
    [SerializeField]
    private UIFader higherNoticeUI;
    [SerializeField]
    private UIFader fasterNoticeUI;
    [SerializeField]
    private UIFader slowerNoticeUI;
    [SerializeField]
    private UIFader pausNoticeUI;
    [SerializeField]
    private UIFader noHitUI;
    [SerializeField]
    private UIFader fadeToBlack;
    [Range(0, 2F)]
    [SerializeField]
    private float showTime;

    public IEnumerator ShowHandNoticeUI()
    {
        fixationPointController.DisableFixationPoint();

        yield return StartCoroutine(handNoticeUI.InteruptAndFadeIn());
        yield return new WaitForSeconds(showTime);
        StartCoroutine(HideHandNoticeUI());

        fixationPointController.EnableFixationPoint();
    }

    public IEnumerator FadeToBlack(float time)
    {
        yield return fadeToBlack.InteruptAndFadeIn();
        yield return new WaitForSeconds(time);
        yield return fadeToBlack.InteruptAndFadeOut();
    }

    public IEnumerator HideHandNoticeUI()
    {
        yield return StartCoroutine(handNoticeUI.InteruptAndFadeOut());
    }

    public IEnumerator ShowNoHitNoticeUI()
    {
        fixationPointController.DisableFixationPoint();

        yield return StartCoroutine(noHitUI.InteruptAndFadeIn());
        yield return new WaitForSeconds(showTime);
        StartCoroutine(HideNoHitNoticeUI());

        fixationPointController.EnableFixationPoint();
    }
    public IEnumerator HideNoHitNoticeUI()
    {
        yield return StartCoroutine(noHitUI.InteruptAndFadeOut());
    }


    public IEnumerator ShowFasterNoticeUI()
    {
        fixationPointController.DisableFixationPoint();

        yield return StartCoroutine(fasterNoticeUI.InteruptAndFadeIn());
        yield return new WaitForSeconds(showTime);
        StartCoroutine(HideFasterNoticeUI());

        fixationPointController.EnableFixationPoint();
    }

    public IEnumerator HideFasterNoticeUI()
    {
        yield return StartCoroutine(fasterNoticeUI.InteruptAndFadeOut());
    }

    public IEnumerator ShowSlowerNoticeUI()
    {
        fixationPointController.DisableFixationPoint();

        yield return StartCoroutine(slowerNoticeUI.InteruptAndFadeIn());
        yield return new WaitForSeconds(showTime);
        StartCoroutine(HideSlowerNoticeUI());

        fixationPointController.EnableFixationPoint();
    }

    public IEnumerator HideSlowerNoticeUI()
    {
        yield return StartCoroutine(slowerNoticeUI.InteruptAndFadeOut());
    }

    public IEnumerator ShowHigherNoticeUI()
    {
        fixationPointController.DisableFixationPoint();

        yield return StartCoroutine(higherNoticeUI.InteruptAndFadeIn());
        yield return new WaitForSeconds(showTime);
        StartCoroutine(HideHigherNoticeUI());

        fixationPointController.EnableFixationPoint();
    }

    public IEnumerator HideHigherNoticeUI()
    {
        yield return StartCoroutine(higherNoticeUI.InteruptAndFadeOut());
    }

    public IEnumerator ShowPausNoticeUI(float s)
    {
        fixationPointController.DisableFixationPoint();

        yield return StartCoroutine(pausNoticeUI.InteruptAndFadeIn());

        if (s == 0)
            s = showTime;

        yield return new WaitForSeconds(s);
        StartCoroutine(pausNoticeUI.InteruptAndFadeOut());

        fixationPointController.EnableFixationPoint();
    }
}