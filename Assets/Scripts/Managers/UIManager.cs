using System.Collections;
using BananaGame.BananaTime;
using BananaTimeTravel.BananaTime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BananaGame.Managers;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    public TextMeshProUGUI eraLabel;
    public TextMeshProUGUI bananaChargesLabel;
    public TextMeshProUGUI interactPromptLabel;
    public TextMeshProUGUI messageLabel;

    [Header("Timeline Bar")]
    public Slider timelineSlider;

    private Coroutine _messageCoroutine;

    private void Awake()
    {
        if (Instance is not null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        if (TimeController.Instance is not null)
            TimeController.Instance.OnEraChanged += OnEraChanged;

        HideInteractPrompt();
        if (messageLabel) messageLabel.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (TimeController.Instance is not null)
            TimeController.Instance.OnEraChanged -= OnEraChanged;
    }

    private void OnEraChanged(EraDefinition era)
    {
        if (eraLabel) eraLabel.text = era.eraName;
        if (timelineSlider) timelineSlider.value = era.eraIndex;
    }

    public void UpdateBananaCharges(int current, int max)
    {
        if (bananaChargesLabel)
            bananaChargesLabel.text = $"Banana: {current}/{max}";
    }

    public void ShowInteractPrompt(string prompt)
    {
        if (interactPromptLabel)
        {
            interactPromptLabel.text = prompt;
            interactPromptLabel.gameObject.SetActive(true);
        }
    }

    public void HideInteractPrompt()
    {
        if (interactPromptLabel)
            interactPromptLabel.gameObject.SetActive(false);
    }

    public void ShowMessage(string message, float duration = 2.5f)
    {
        if (messageLabel == null) return;
        if (_messageCoroutine != null) StopCoroutine(_messageCoroutine);
        _messageCoroutine = StartCoroutine(ShowMessageRoutine(message, duration));
    }

    private IEnumerator ShowMessageRoutine(string message, float duration)
    {
        messageLabel.text = message;
        messageLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        messageLabel.gameObject.SetActive(false);
    }
}
