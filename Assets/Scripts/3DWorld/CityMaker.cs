using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityMaker : MonoBehaviour, ICityGen
{
    [System.Serializable]
    public struct PrefabPart
    {
        public enum PartTypes
        {
            Cross,
            T_Cross,
            Straight,
            Curve,
        }

        public GameObject prefab;
        public PartTypes type;
    }

    [Header("Prefabs")]
    public PrefabPart[] streetPrefabs;
    protected int totalStreetWeights;
    protected List<CityTile> tileInstances = new List<CityTile>();
    [Header("Size")]
    public Vector2Int citySizeInTiles;
    protected const int tileSize = 20;

    void Start()
    {
        StartCoroutine(Generate());
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Vector3 size = (citySizeInTiles * tileSize).ToVector3XZ();
        Gizmos.DrawWireCube(size * 0.5f, size);
    }

    public IEnumerator Generate()
    {
        throw new System.NotImplementedException();
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        foreach (var instance in tileInstances)
        {
            if (instance != null)
            {
                Destroy(instance);
            }
        }
        tileInstances.Clear();
        totalStreetWeights = 0;
    }
}
