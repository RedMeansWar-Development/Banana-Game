using BananaGame.BananaTime;
using BananaGame.Player;
using BananaTimeTravel.BananaTime;
using UnityEngine;

namespace BananaGame.World;

public class EnemyController : MonoBehaviour
{
    [Header("AI")]
    public float moveSpeed = 2f;
    public float detectionRadius = 5f;

    [Header("Era")]
    [Tooltip("If set, this enemy only exists in this era.")]
    public EraDefinition activeEra;

    private Transform _player;

    [System.Obsolete]
    private void Start()
    {
        var pc = FindFirstObjectByType<PlayerController>();
        if (pc != null) _player = pc.transform;

        if (TimeController.Instance != null)
            TimeController.Instance.OnEraChanged += OnEraChanged;

        RefreshActive();
    }

    private void OnDestroy()
    {
        if (TimeController.Instance != null)
            TimeController.Instance.OnEraChanged -= OnEraChanged;
    }

    private void Update()
    {
        if (_player is null) return;

        float dist = Vector2.Distance(transform.position, _player.position);
        if (dist < detectionRadius)
        {
            Vector2 dir = (_player.position - transform.position).normalized;
            transform.Translate(dir * moveSpeed * Time.deltaTime);
        }
    }

    private void OnEraChanged(EraDefinition newEra) => RefreshActive();

    private void RefreshActive()
    {
        if (activeEra == null) return;
        bool inCorrectEra = TimeController.Instance?.CurrentEra == activeEra;
        gameObject.SetActive(inCorrectEra);
    }
}