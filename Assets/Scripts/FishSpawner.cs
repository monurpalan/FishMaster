using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private Fish fishPrefab;
    [SerializeField] private Fish.FishType[] fishTypes;

    void Awake()
    {
        foreach (var fishType in fishTypes)
        {
            // Her balık tipi için belirtilen sayıda balık oluştur
            int count = Mathf.RoundToInt(fishType.fishCount);
            for (int i = 0; i < count; i++)
            {
                Fish fish = Instantiate(fishPrefab);
                fish.Type = fishType;
                fish.ResetFish();
            }
        }
    }
}