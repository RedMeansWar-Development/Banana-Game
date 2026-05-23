using System;
using UnityEngine;

namespace BananaGame.BananaTime;

[Serializable]
public class WorldStateSnapshot
{
    public Vector2 PlayerPosition { get; set; }
    public string EraName { get; set; }
    public int EraIndex { get; set; }
    public float Timestamp { get; set; }

    public static WorldStateSnapshot Capture(Vector2 playerPos, EraDefinition era)
    {
        return new()
        {
            PlayerPosition = playerPos,
            EraName = era.eraName,
            EraIndex = era.eraIndex,
            Timestamp = Time.time
        };
    }

}