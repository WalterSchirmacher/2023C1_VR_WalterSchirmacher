using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class Haptic
{
    [Range(0, 1)]
    public float intensity;
    public float duration;

    public void TriggerHaptics(BaseInteractionEventArgs eventArgs)
    {
        if(eventArgs.interactableObject is XRBaseControllerInteractor controllerInteractor)
        {
            TriggerHaptics(controllerInteractor.xrController);
        }
    }

    public void TriggerHaptics(XRBaseController controller)
    {
        if (intensity > 0)
        {
            controller.SendHapticImpulse(intensity, duration);
        }
    }

}

public class HapticInteractable : MonoBehaviour
{
    public bool useHaptics = false;
    public Haptic hapticOnActivated;
    public Haptic hapticHoverEntered;
    public Haptic hapticHoverExited;
    public Haptic hapticSelectEntered;
    public Haptic hapticSelectExited;


    private void Start()
    {
        if(useHaptics)
        {
            XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();

            interactable.activated.AddListener(hapticOnActivated.TriggerHaptics);
            interactable.hoverEntered.AddListener(hapticHoverEntered.TriggerHaptics);
            interactable.hoverExited.AddListener(hapticHoverExited.TriggerHaptics);
            interactable.selectEntered.AddListener(hapticSelectEntered.TriggerHaptics);
            interactable.selectExited.AddListener(hapticSelectExited.TriggerHaptics);
        }

    }
}