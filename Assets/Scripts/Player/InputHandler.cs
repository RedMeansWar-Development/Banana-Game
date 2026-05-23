using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BananaGame.Player;

[RequireComponent(typeof(PlayerController))]
public class InputHandler : MonoBehaviour
{
    public event Action<Vector2> OnMove;
    public event Action OnInteract;
    public event Action OnUseBanana;
    
    // Called by PlayerInput component via Send Messages mode
    private void OnMoveInput(InputValue value)      => OnMove?.Invoke(value.Get<Vector2>());
    private void OnInteractInput(InputValue value)  => OnInteract?.Invoke();
    private void OnUseBananaInput(InputValue value) => OnUseBanana?.Invoke();
}
