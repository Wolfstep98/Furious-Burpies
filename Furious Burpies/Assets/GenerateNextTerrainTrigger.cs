using UnityEngine;

[RequireComponent(typeof(Collider2D))]
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == GameObjectsTags.Player)
        {
            this.terrainGeneration.GenerateBasicTerrain();
            this.gameObject.SetActive(false);
        }
    }
    #endregion
}
