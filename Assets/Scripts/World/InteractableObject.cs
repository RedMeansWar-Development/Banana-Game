using BananaGame.BananaTime;
using BananaGame.Managers;
using BananaGame.Player;
using BananaTimeTravel.BananaTime;
using UnityEngine;
using UnityEngine.Events;

namespace BananaGame.World
{
    public class InteractableObject
    {
        [Header("Interact")]
        [SerializeField] private string interactPrompt = "Interact";
        public UnityEvent<PlayerController> onInteract;

        [Header("Era Filtering (optional)")]
        [Tooltip("If set, only interactable in this era. Leave null for always.")]
        public EraDefinition requiredEra;

        public string InteractPrompt => interactPrompt;

        public void Interact(PlayerController player)
        {
            if (requiredEra != null && TimeController.Instance.CurrentEra != requiredEra)
            {
                UIManager.Instance?.ShowMessage("This doesn't work in this era.");
                return;
            }

            onInteract?.Invoke(player);
        }
    }
}

