using BananaGame.Interfaces;
using BananaGame.Items;
using BananaGame.Managers;
using UnityEngine;

namespace BananaGame.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(InputHandler))]
    [RequireComponent(typeof(InventorySystem))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 5f;

        public InventorySystem Inventory { get; private set; }

        private Rigidbody2D _rb;
        private InputHandler _input;
        private Vector2 _moveInput;
        private IInteractable _nearbyInteractable;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _input = GetComponent<InputHandler>();
            Inventory = GetComponent<InventorySystem>();

            _input.OnMove += dir => _moveInput = dir;
            _input.OnInteract += OnInteract;
            _input.OnUseBanana += OnUseBanana;
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _moveInput * moveSpeed;
        }

        private void OnInteract()
        {
            _nearbyInteractable?.Interact(this);
        }

        private void OnUseBanana()
        {
            var banana = Inventory.GetItem<BananaItem>();
            banana?.Use(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                _nearbyInteractable = interactable;
                UIManager.Instance?.ShowInteractPrompt(interactable.InteractPrompt);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable _))
            {
                _nearbyInteractable = null;
                UIManager.Instance?.HideInteractPrompt();
            }
        }
    }
}