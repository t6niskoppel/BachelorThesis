using UnityEngine;
using Leap.Unity;

public class LeapHandVisibilityController : MonoBehaviour
{
    [SerializeField] private LeapHandController handController;

    public void enableHands()
    {
        handController.GetComponent<HandPool>().EnableGroup("Hand_Models");
    }
    public void disableHands()
    {
        handController.GetComponent<HandPool>().DisableGroup("Hand_Models");
    }
    
    
}