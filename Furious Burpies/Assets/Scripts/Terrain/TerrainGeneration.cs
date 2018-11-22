using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    #region Fields & Properties
    #region Constants
    public const int MinimumTerrainQueueLenght = 4;
    public const int MaximumTerrainQueueLenght = 6;
    #endregion

    [Header("Procedural Generation")]
    [SerializeField]
    private bool randomSeed = false;
    [SerializeField]
    private int seed = 42;

    [Header("Terrains")]
    [SerializeField]
    private Transform terrainsParent = null;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject[] spawnerPrefabs = new GameObject[0];
    [SerializeField]
    private TerrainData[] basicTerrainData = new TerrainData[0];

    [Header("Spawns", order = 1)]
    [SerializeField]
    private GameObject[] spawners = new GameObject[0];
    [Header("Basics", order = 2)]
    [SerializeField]
    private GameObject[] basics = new GameObject[0];

    [Header("Data")]
    [SerializeField]
    private GameObject lastTerrainAdded = null;
    [SerializeField]
    private Queue<GameObject> terrainQueue = new Queue<GameObject>(4);

    [Header("References")]
    [SerializeField]
    private GameObject player = null;
    #endregion

    #region Methods
    #region Initialization
    private void Awake()
    {
        this.Initialize();    
    }

    private void Initialize()
    {
#if UNITY_EDITOR
        if (this.terrainsParent == null)
            Debug.LogError("[Missing Reference] - TerrainsParent is missing !");

        if (this.spawnerPrefabs.Length == 0)
            Debug.LogError("[Missing Terrain GameObjects] - spawnerPrefabs list is empty, put at least 1 spawner prefab !");
        if (this.basicTerrainData.Length == 0)
            Debug.LogError("[Missing Terrain GameObjects] - basicPrefabs list is empty, put at least 1 basic prefab !");

        if (this.player == null)
            Debug.LogError("[Missing Reference] - player is missing !");
#endif
        this.InstanciateTerrains();
        this.InitiliazeProcedural();

        this.GenerateStartTerrain();
    }

    private void InstanciateTerrains()
    {
        this.spawners = new GameObject[this.spawnerPrefabs.Length];
        for(int i = 0; i < this.spawners.Length;i++)
        {
            GameObject terrain = Instantiate<GameObject>(this.spawnerPrefabs[i], Vector3.zero, Quaternion.identity, this.terrainsParent);
            terrain.SetActive(false);
            this.spawners[i] = terrain;
        }

        this.basics = new GameObject[this.basicTerrainData.Length];
        for (int i = 0; i < this.basics.Length; i++)
        {
            GameObject terrain = Instantiate<GameObject>(this.basicTerrainData[i].Prefab, Vector3.zero, Quaternion.identity, this.terrainsParent);
            terrain.SetActive(false);
            this.basics[i] = terrain;
        }

        Debug.Log("[Succes] - Terrains instanciation correctly initialized !");
    }

    private void InitiliazeProcedural()
    {
        this.seed = (this.randomSeed) ? Random.Range(1, int.MaxValue) : this.seed;
        Random.InitState(this.seed);

        Debug.Log("[Succes] - Procedural Generation correctly initialized !");
    }

    #endregion
    public void GenerateStartTerrain()
    {
        this.GenerateNextTerrain(TerrainType.Spawner);
        this.GenerateNextTerrain(TerrainType.Basic);
        this.GenerateNextTerrain(TerrainType.Basic);
        GameObject terrain = this.terrainQueue.Peek();
        this.player.transform.position = terrain.transform.Find("SpawnPoint").position;
        Debug.Log("[Succes] - Start Terrain succesfully generated !");
    }
    public void GenerateBasicTerrain()
    {
        this.GenerateNextTerrain(TerrainType.Basic);
        //this.DisabledLastTerrain();
        Debug.Log("[Succes] - Basic Terrain succesfully generated !");
    }

    private void GenerateNextTerrain(TerrainType type)
    {
        int index = 0;
        switch(type)
        {
            case TerrainType.Spawner:
                index = Random.Range(0, this.spawners.Length);
                break;
            case TerrainType.Basic:
                index = this.PickRandomAvailableBasicTerrain();
                break;
            default:
                Debug.LogError("[Argument Exception] - The terrain type is not recognized.");
                break;
        }
        this.EnableNextTerrain(type, index);
    }

    private int PickRandomAvailableBasicTerrain()
    {
        int index = Random.Range(0, this.basics.Length);
        while(this.basics[index].activeInHierarchy)
        {
            index++;
            index %= this.basics.Length;
        }
        return index;
    }

    public void EnableNextTerrain(TerrainType type, int index)
    {
        GameObject terrain = null;
        switch(type)
        {
            case TerrainType.Spawner:
                terrain = this.spawners[index];
                terrain.SetActive(true);
                terrain.transform.position = Vector3.zero;
                this.terrainQueue.Enqueue(terrain);
                this.lastTerrainAdded = terrain;
                break;
            case TerrainType.Basic:
                terrain = Instantiate<GameObject>(this.basics[index]);
                terrain.SetActive(true);
                terrain.transform.Find("Trigger - GenerateNextTerrain").gameObject.SetActive(true);
                Vector3 lastPos = this.lastTerrainAdded.transform.position;
                Vector3 endPoint = this.lastTerrainAdded.transform.Find("EndPoint").position;
                Vector3 startPoint = terrain.transform.Find("StartPoint").transform.position;
                Vector3 translation = endPoint - startPoint;
                terrain.transform.Translate(translation, Space.World);
                this.terrainQueue.Enqueue(terrain);
                this.lastTerrainAdded = terrain;
                break;
            default:
                Debug.LogError("[Argument Exception] - The terrain type is not recognized.");
                break;
        }
    }

    public void DisabledLastTerrain()
    {
        GameObject lastTerrain = this.terrainQueue.Dequeue();
        lastTerrain.SetActive(false);
    }

    public void ToggleBounce()
    {
        for (int i = 0; i < this.basics.Length; i++)
        {
            Transform parent = this.basics[i].transform.Find("Platforms");
            for (int c = 0; c < parent.childCount;c++)
            {
                StickProperty property = parent.GetChild(c).GetComponent<StickProperty>();
                //property.IsEnable = !property.IsEnable;
            }
        }
    }
    #endregion
}
