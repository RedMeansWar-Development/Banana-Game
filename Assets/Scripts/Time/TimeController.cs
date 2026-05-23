using System;
using System.Collections.Generic;
using BananaGame.Managers;
using BananaGame.BananaTime;
using UnityEngine;

namespace BananaTimeTravel.BananaTime
{
    public class TimeController : MonoBehaviour
    {
        public static TimeController Instance { get; private set; }

        [Header("Eras")]
        public EraDefinition startingEra;

        public EraDefinition CurrentEra { get; private set; }

        public event Action<EraDefinition> OnEraChanged;

        private readonly Stack<WorldStateSnapshot> _history = new();
        private GameObject _currentTilemapInstance;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (startingEra != null)
                LoadEraEnvironment(startingEra, skipSnapshot: true);
        }

        /// <summary>Travel to a new era from BananaItem.Use().</summary>
        public void TravelTo(EraDefinition era, Vector2 playerPosition)
        {
            if (era is null || era == CurrentEra) return;

            // Snapshot before leaving
            var snapshot = WorldStateSnapshot.Capture(playerPosition, CurrentEra);
            _history.Push(snapshot);

            ApplyEra(era);
        }

        /// <summary>Rewind to the most recent snapshot (undo last travel).</summary>
        public bool TryRewind(out WorldStateSnapshot snapshot)
        {
            if (_history.Count == 0) { snapshot = null; return false; }
            snapshot = _history.Pop();
            // Find the matching EraDefinition from GameManager's era list
            var era = GameManager.Instance.GetEraByName(snapshot.EraName);
            if (era != null) ApplyEra(era);
            return true;
        }

        private void ApplyEra(EraDefinition era)
        {
            CurrentEra = era;
            LoadEraEnvironment(era);
            OnEraChanged?.Invoke(era);
        }

        private void LoadEraEnvironment(EraDefinition era, bool skipSnapshot = false)
        {
            // Swap tilemap
            if (_currentTilemapInstance != null)
                Destroy(_currentTilemapInstance);

            if (era.tilemapPrefab != null)
                _currentTilemapInstance = Instantiate(era.tilemapPrefab);

            // Ambient lighting
            RenderSettings.ambientLight = era.ambientColor;
            if (era.skyboxMaterial != null)
                RenderSettings.skybox = era.skyboxMaterial;

            CurrentEra = era;
        }
    }
}