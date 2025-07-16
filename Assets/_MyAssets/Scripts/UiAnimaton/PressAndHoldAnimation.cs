using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PressAndHoldAnimation : MonoBehaviour, IButtonAnimation
{
    CanvasGroup fillImageCanvasGroup;
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text infoTxt;

    [Tooltip ("<i>OPTIONAL")] 
    [SerializeField] private RectTransform shineObject;

    [Space]
    [SerializeField] private string defaultString = "PRESS & HOLD";
    [SerializeField] private string completeString = "TAP TO RESET";


    [Space]
    [SerializeField] private float increasingFillAmt = 0.25f;
    [SerializeField] private float decreasingFillAmt = -0.5f;

    private float fillAmt = 0;
    private float appliedFillAmt = 0;
    private enum CoroutineStatus
    {
        NOT_WORKING,
        IS_WORKING,
        IS_COMPLETED
    }

    private void Awake()
    {
        fillImageCanvasGroup = fillImage.GetComponent<CanvasGroup>();   
        ResetAnimation(true);
    }

    [SerializeField] private CoroutineStatus coroutineStatus;
    public void IsCompleted()
    {
        infoTxt.text = completeString;
        coroutineStatus = CoroutineStatus.IS_COMPLETED;
        LeanTween.scale(gameObject, Vector3.one * 1.1f, 0.25f).setEaseInBack();

        if (shineObject != null )
        {
            shineObject.localPosition = new Vector3 (-200f, 0f, 0f);
            LeanTween.moveLocal(shineObject.gameObject, new Vector3 (200,0,0), 1f).setEaseInOutSine();
        }

        ActionHandler.UiInteracted?.Invoke(UiSfx.BUTTON_PRESSED);
    }

    public void IsPressed()
    {
        appliedFillAmt = increasingFillAmt;
        switch (coroutineStatus)
        {
            case CoroutineStatus.NOT_WORKING:
                StartCoroutine(nameof(StartAnimation));
                break;
            case CoroutineStatus.IS_COMPLETED:
                ResetAnimation(false);
                break;
        }
    }

    public void IsReleased()
    {
        appliedFillAmt = decreasingFillAmt;
    }

    public void ResetAnimation(bool withoutAnimation)
    {
        if (withoutAnimation)
        {
            fillAmt = 0;
            fillImage.fillAmount = fillAmt;
            fillImageCanvasGroup.alpha = 1;

            infoTxt.text = defaultString;
            transform.localScale = Vector3.one;

            coroutineStatus = CoroutineStatus.NOT_WORKING;
        }
        else
        {
            infoTxt.text = defaultString;
            LeanTween.scale(gameObject, Vector3.one, 0.4f).setEaseInBack();
            LeanTween.alphaCanvas(fillImageCanvasGroup, 0, 0.3f).setEaseInOutSine().setOnComplete(() =>
            {
                fillAmt = 0;
                fillImage.fillAmount = fillAmt;
                fillImageCanvasGroup.alpha = 1;
            });
            coroutineStatus = CoroutineStatus.NOT_WORKING;
            ActionHandler.UiInteracted?.Invoke(UiSfx.BUTTON_RELEASED);
        }
    }

    private IEnumerator StartAnimation()
    {
        coroutineStatus = CoroutineStatus.IS_WORKING;
        while ((appliedFillAmt > 0 && fillAmt < 1f) || (appliedFillAmt < 0 && fillAmt > 0f))
        {
            fillAmt += appliedFillAmt * Time.deltaTime;
            fillImage.fillAmount = fillAmt;
            yield return null;
        }
        coroutineStatus = CoroutineStatus.NOT_WORKING;
        if (fillAmt >= 1) IsCompleted();
    }

    [ContextMenu ("Setup shine object")]
    private void CreateShineObject()
    {
        RectTransform shinPrefab = Resources.Load<GameObject>("Prefabs/ShinUI").GetComponent<RectTransform>();
        RectTransform spawnedPrefab = Instantiate(shinPrefab, Vector3.zero, Quaternion.identity, transform);
        spawnedPrefab.localPosition = new Vector3(-200, 0, 0);
        spawnedPrefab.name = "ShineUi";
        shineObject = spawnedPrefab;

        Image imageComponent = GetComponent<Image>();
        if (imageComponent != null)
        {
            Texture _texture = imageComponent.sprite.texture;
            Color _color = imageComponent.color;
            DestroyImmediate(imageComponent);
            RawImage _rawImage = gameObject.AddComponent<RawImage>();
            gameObject.AddComponent<Mask>();
            _rawImage.texture = _texture;    
            _rawImage.color = _color;   
        }
        else
        {
            RawImage _rawImage = gameObject.AddComponent<RawImage>();
        }
    }
}
