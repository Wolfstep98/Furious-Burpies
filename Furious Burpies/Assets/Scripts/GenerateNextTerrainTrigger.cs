using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GenerateNextTerrainTrigger : MonoBehaviour
{
    #region Fields & Properties
    [Header("References")]
    [SerializeField]
    private TerrainGeneration terrainGeneration = null;
    #endregion

    #region Methods
    private void Awake()
    {
        if (this.terrainGeneration == null)
            this.terrainGeneration = GameObject.Find("TerrainGeneration").GetComponent<TerrainGeneration>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == GameObjectsTags.Player)
        {
            Debug.Log("Generate Terrain");
            this.terrainGeneration.GenerateBasicTerrain();
            this.gameObject.SetActive(false);
        }
    }
    #endregion
}
