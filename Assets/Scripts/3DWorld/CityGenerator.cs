using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICityGen
{
    IEnumerator Generate();
    void Clear();
}

public class CityGenerator : MonoBehaviour, ICityGen
{
    [System.Serializable]
    public struct PrefabChance
    {
        public GameObject prefab;
        [Range(0, 10)]
        public int weight;
    }

    [System.Serializable]
    private class EmptyTileInfo
    {
        public Vector2Int position;

        public EmptyTileInfo(Vector2Int pos)
        {
            position = pos;
        }
    }

    [System.Serializable]
    private class SimpleTileInfo : EmptyTileInfo
    {
        public CityTile.Connection.ConnectionTypes topType;
        public CityTile.Connection.ConnectionTypes rightType;
        public CityTile.Connection.ConnectionTypes bottomType;
        public CityTile.Connection.ConnectionTypes leftType;

        public SimpleTileInfo(Vector2Int pos) : base(pos)
        {
        }
    }

    [Header("Prefabs")]
    public PrefabChance[] streetPrefabs;
    protected int totalStreetWeights;
    protected List<CityTile> tileInstances = new List<CityTile>();
    [Header("Size")]
    public Vector2Int citySizeInTiles;
    protected const int tileSize = 20;
    private EmptyTileInfo[,] openConnections;

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

    //[ContextMenu("Generate")]
    public IEnumerator Generate()
    {
        Clear();

        openConnections = new EmptyTileInfo[citySizeInTiles.x, citySizeInTiles.y];

        Vector2Int startPos = citySizeInTiles / 2;
        CityTile startStreetTile = Instantiate(streetPrefabs[0].prefab, (startPos * tileSize + Vector2Int.one * tileSize / 2).ToVector3XZ(), Quaternion.identity).GetComponent<CityTile>();
        startStreetTile.transform.SetParent(transform);
        startStreetTile.gameObject.name += " START";
        AddTile(startPos, startStreetTile);
        //yield return new WaitForSeconds(0.5f);

        SimpleTileInfo freeSpot;
        while (tileInstances.Count < citySizeInTiles.x * citySizeInTiles.y)
        {
            freeSpot = GetFreeConnection();
            if (freeSpot == null)
            {
                break;
            }
            bool fit = false;
            CityTile streetTile = null;
            for (int i = 0; i < 50 && !fit; i++)
            {
                streetTile = Instantiate(GetRandomStreetPrefab(), (freeSpot.position * tileSize + Vector2Int.one * tileSize / 2).ToVector3XZ(), Quaternion.identity).GetComponent<CityTile>();
                streetTile.transform.SetParent(transform);

                for (int rot = 0; rot <= 4 && !fit; rot++)
                {
                    if (rot != 0)
                    {
                        streetTile.transform.Rotate(0f, -90f, 0f);
                        streetTile.Rotate90();
                    }
                    fit = CheckIfRoomFits(freeSpot.position, streetTile);
                }
                //yield return new WaitForSeconds(0.5f);
            }
            if (fit)
            {
                AddTile(freeSpot.position, streetTile);
            }
            else
            {
                if (streetTile != null)
                {
                    Destroy(streetTile.gameObject);
                }
            }
            //yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        openConnections = null;
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

    protected GameObject GetRandomStreetPrefab()
    {
        if (totalStreetWeights == 0)
        {
            foreach (PrefabChance prefabChance in streetPrefabs)
            {
                totalStreetWeights += prefabChance.weight;
            }
        }

        int tmp = totalStreetWeights;
        int random = Random.Range(0, tmp);
        tmp = 0;

        foreach (PrefabChance prefabChance in streetPrefabs)
        {
            tmp += prefabChance.weight;
            if (random <= tmp)
            {
                return prefabChance.prefab;
            }
        }
        throw new System.Exception("No prefab found");
    }

    private void AddTile(Vector2Int tilePos, CityTile streetTile)
    {
        Debug.Log(streetTile.name + " " + tilePos + " ", streetTile.gameObject);
        tileInstances.Add(streetTile);

        int yRot = Mathf.RoundToInt(streetTile.transform.rotation.eulerAngles.y) % 360;

        CityTile.Connection.ConnectionTypes top = CityTile.Connection.ConnectionTypes.None;
        CityTile.Connection.ConnectionTypes right = CityTile.Connection.ConnectionTypes.None;
        CityTile.Connection.ConnectionTypes bottom = CityTile.Connection.ConnectionTypes.None;
        CityTile.Connection.ConnectionTypes left = CityTile.Connection.ConnectionTypes.None;
        GetConnectionsFromTile(streetTile, yRot, ref top, ref right, ref bottom, ref left);

        openConnections[tilePos.x, tilePos.y] = new EmptyTileInfo(tilePos);
        for (int i = 0; i < 4; i++)
        {
            SimpleTileInfo tileInfo;
            Vector2Int tmpPos = tilePos;
            switch ((CityTile.Connection.Side)i)
            {
                case CityTile.Connection.Side.Top:
                    tmpPos.y++;
                    tileInfo = GetOrCreateOpenConnection(tmpPos);
                    if (tileInfo == null)
                    {
                        continue;
                    }
                    tileInfo.bottomType = top;
                    break;
                case CityTile.Connection.Side.Right:
                    tmpPos.x++;
                    tileInfo = GetOrCreateOpenConnection(tmpPos);
                    if (tileInfo == null)
                    {
                        continue;
                    }
                    tileInfo.leftType = right;
                    break;
                case CityTile.Connection.Side.Bottom:
                    tmpPos.y--;
                    tileInfo = GetOrCreateOpenConnection(tmpPos);
                    if (tileInfo == null)
                    {
                        continue;
                    }
                    tileInfo.topType = bottom;
                    break;
                case CityTile.Connection.Side.Left:
                    tmpPos.x--;
                    tileInfo = GetOrCreateOpenConnection(tmpPos);
                    if (tileInfo == null)
                    {
                        continue;
                    }
                    tileInfo.rightType = left;
                    break;
            }
        }
    }

    private SimpleTileInfo GetOrCreateOpenConnection(Vector2Int tmpPos)
    {
        if (tmpPos.x < 0 || tmpPos.y < 0 || tmpPos.x >= citySizeInTiles.x || tmpPos.y >= citySizeInTiles.y || openConnections[tmpPos.x, tmpPos.y] is EmptyTileInfo)
        {
            return null;
        }
        SimpleTileInfo tileInfo = openConnections[tmpPos.x, tmpPos.y] as SimpleTileInfo;
        if (tileInfo == null)
        {
            tileInfo = new SimpleTileInfo(tmpPos);
            openConnections[tmpPos.x, tmpPos.y] = tileInfo;
        }
        return tileInfo;
    }

    private static void GetConnectionsFromTile(CityTile streetTile, int yRot, ref CityTile.Connection.ConnectionTypes top, ref CityTile.Connection.ConnectionTypes right, ref CityTile.Connection.ConnectionTypes bottom, ref CityTile.Connection.ConnectionTypes left)
    {
        foreach (var connection in streetTile.connections)
        {
            switch (connection.side)
            {
                case CityTile.Connection.Side.Top:
                    top = connection.type;
                    break;
                case CityTile.Connection.Side.Right:
                    right = connection.type;
                    break;
                case CityTile.Connection.Side.Bottom:
                    bottom = connection.type;
                    break;
                case CityTile.Connection.Side.Left:
                    left = connection.type;
                    break;
            }

        }
    }

    private SimpleTileInfo GetFreeConnection()
    {
        int size = citySizeInTiles.x * citySizeInTiles.y;
        List<SimpleTileInfo> freeSpots = new List<SimpleTileInfo>();//size);
        for (int i = 0; i < size; i++)
        {
            int x = i % citySizeInTiles.x;
            int y = i / citySizeInTiles.y;

            EmptyTileInfo tileInfo = openConnections[x, y];
            if (tileInfo != null && tileInfo is SimpleTileInfo)
            {
                SimpleTileInfo simpleTileInfo = tileInfo as SimpleTileInfo;
                if (simpleTileInfo.topType != CityTile.Connection.ConnectionTypes.Street &&
                    simpleTileInfo.leftType != CityTile.Connection.ConnectionTypes.Street &&
                    simpleTileInfo.bottomType != CityTile.Connection.ConnectionTypes.Street &&
                    simpleTileInfo.rightType != CityTile.Connection.ConnectionTypes.Street)
                {
                    continue;
                }
                freeSpots.Add(simpleTileInfo);
            }
        }
        if (freeSpots.Count == 0)
        {
            return null;
        }
        int random = Random.Range(0, freeSpots.Count);
        SimpleTileInfo pickedSpot = freeSpots[random];
        return pickedSpot;
    }

    private bool CheckIfRoomFits(Vector2Int tilePos, CityTile streetTile)
    {
        int yRot = Mathf.RoundToInt(streetTile.transform.rotation.eulerAngles.y) % 360;

        CityTile.Connection.ConnectionTypes top = CityTile.Connection.ConnectionTypes.None;
        CityTile.Connection.ConnectionTypes right = CityTile.Connection.ConnectionTypes.None;
        CityTile.Connection.ConnectionTypes bottom = CityTile.Connection.ConnectionTypes.None;
        CityTile.Connection.ConnectionTypes left = CityTile.Connection.ConnectionTypes.None;
        GetConnectionsFromTile(streetTile, yRot, ref top, ref right, ref bottom, ref left);

        SimpleTileInfo spotInfo = openConnections[tilePos.x, tilePos.y] as SimpleTileInfo;
        bool fit = true;
        fit = fit && (spotInfo.topType == top || spotInfo.topType == CityTile.Connection.ConnectionTypes.None);
        fit = fit && (spotInfo.rightType == right || spotInfo.rightType == CityTile.Connection.ConnectionTypes.None);
        fit = fit && (spotInfo.bottomType == bottom || spotInfo.bottomType == CityTile.Connection.ConnectionTypes.None);
        fit = fit && (spotInfo.leftType == left || spotInfo.leftType == CityTile.Connection.ConnectionTypes.None);
        return fit;
    }
}
