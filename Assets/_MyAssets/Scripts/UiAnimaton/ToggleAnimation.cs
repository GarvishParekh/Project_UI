using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleAnimation : MonoBehaviour
{
    [SerializeField] private Toggle[] allTtoggles;
    [SerializeField] private Transform toggleSelectionIndicator;

    [Space]
    [SerializeField] private Vector3 selectedTogglePosition;
    [SerializeField] private Vector3 indicatorHoveringSize = Vector3.one * 1.1f;
    [SerializeField] private float animationSpeed = 200;

    private enum CoroutineStatus
    {
        NOT_WORKING,
        IS_WORKING
    }
    [SerializeField] private CoroutineStatus coroutineStatus;

    public void OnClick(int selectedToggle)
    {
        selectedTogglePosition = allTtoggles[selectedToggle].transform.localPosition;
        switch (coroutineStatus)
        {
            case CoroutineStatus.IS_WORKING:
                ActionHandler.UiInteracted?.Invoke(UiSfx.TOGGLE_CHANGED);
                break;
            case CoroutineStatus.NOT_WORKING:
                ActionHandler.UiInteracted?.Invoke(UiSfx.TOGGLE_PRESSED);
                LeanTween.scale(toggleSelectionIndicator.gameObject, indicatorHoveringSize, 0.25f).setEaseInOutSine();
                StartCoroutine(nameof(UpdateToggleSelection));
                break;
        }
    }

    private void OnComplete()
    {
        LeanTween.scale(toggleSelectionIndicator.gameObject, Vector3.one, 0.25f).setEaseInOutSine();
        
    }

    private IEnumerator UpdateToggleSelection()
    {
        coroutineStatus = CoroutineStatus.IS_WORKING;
        while (toggleSelectionIndicator.localPosition != selectedTogglePosition)
        {
            toggleSelectionIndicator.localPosition = Vector3.MoveTowards(toggleSelectionIndicator.localPosition, selectedTogglePosition, animationSpeed * Time.deltaTime);
            yield return null;
        }

        coroutineStatus = CoroutineStatus.NOT_WORKING;
        OnComplete();
    }
}
