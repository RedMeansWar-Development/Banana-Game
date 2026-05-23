using UnityEngine;
using BananaGame.Interfaces;
using BananaGame.Player;
using BananaGame.BananaTime;
using BananaGame.Managers;
using BananaTimeTravel.BananaTime;

namespace BananaGame.Items;

// lets face it a lot of this stuff might not be used but I don't care enough right now to find out what is and isn't used. 
// I just want to get the banana time travel working.
public class BananaItem : BaseItem, IInteractable
{
    [Header("Time Travel")]
    public EraDefinition targetEra;
    public int maxCharges = 3;

    [Header("Feedback")]
    public AudioClip useSound;
    public GameObject travelVFXPrefab;

    public string InteractPrompt => "Pick up banana";

    public void Interact(PlayerController player)
    {
        player.Inventory.Add(this);
        gameObject.SetActive(false); // remove from world; now in inventory
    }

    /// <summary>
    /// Called when the player selects this item in inventory and presses Use.
    /// </summary>
    public override void Use(PlayerController player)
    {
        if (_charges <= 0)
        {
            Debug.Log("BananaItem: no charges remaining.");
            UIManager.Instance?.ShowMessage("The banana is all out of time juice.");
            return;
        }

        if (targetEra == null)
        {
            Debug.LogWarning("BananaItem: no targetEra assigned in Inspector.");
            return;
        }

        TriggerTimeTravel(player);
    }

    private int _charges;

    private void Awake() => _charges = maxCharges;

    private void TriggerTimeTravel(PlayerController player)
    {
        _charges--;

        SpawnVFX(player);
        PlaySound(player);

        UIManager.Instance?.UpdateBananaCharges(_charges, maxCharges);

        // Hand control to TimeController — it snapshots, swaps era, fires events
        TimeController.Instance.TravelTo(targetEra, player.transform.position);

        if (_charges <= 0)
        {
            player.Inventory.Remove(this);
            Destroy(gameObject);
        }
    }

    private void SpawnVFX(PlayerController player)
    {
        if (travelVFXPrefab != null)
            Instantiate(travelVFXPrefab, player.transform.position, Quaternion.identity);
    }

    private void PlaySound(PlayerController player)
    {
        if (useSound != null)
            AudioSource.PlayClipAtPoint(useSound, player.transform.position);
    }

    public int Charges => _charges;
    public int MaxCharges => maxCharges;
} 