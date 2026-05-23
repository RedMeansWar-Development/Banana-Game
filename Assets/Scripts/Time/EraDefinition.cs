using UnityEngine;

namespace BananaGame.BananaTime
{
    public class EraDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string eraName;
        public int eraIndex;

        [Header("Visuals")]
        public Color ambientColor = Color.white;
        public Material skyboxMaterial;

        [Header("Audio")]
        public AudioClip musicClip;
        public AudioClip ambientClip;

        [Header("World")]
        public GameObject tilemapPrefab;
        public GameObject[] enemyPrefabs;
    }
}