using BananaGame.Player;

namespace BananaGame.Interfaces;

public interface IInteractable
{
    void Interact(PlayerController player);
    string InteractPrompt { get; }
}