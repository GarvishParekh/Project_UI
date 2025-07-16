using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class SfxManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private SfxData sfxData;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        ActionHandler.UiInteracted += OnUiInteracted;
    }

    private void OnDisable()
    {
        ActionHandler.UiInteracted -= OnUiInteracted;
    }

    private void OnUiInteracted(UiSfx uiSfx)
    {
        switch (uiSfx)
        {
            case UiSfx.BUTTON_PRESSED:
                audioSource.PlayOneShot(sfxData.buttonClickClip);
                break;
            case UiSfx.BUTTON_RELEASED:
                audioSource.PlayOneShot(sfxData.buttonReselaseClip);
                break;
            case UiSfx.TOGGLE_PRESSED:
                audioSource.PlayOneShot(sfxData.togglePressedClip);
                break;
            case UiSfx.TOGGLE_CHANGED:
                audioSource.PlayOneShot(sfxData.toggleChangedClip);
                break;
        }
    }
}
