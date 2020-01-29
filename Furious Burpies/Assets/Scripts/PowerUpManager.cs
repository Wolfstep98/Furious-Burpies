using System;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    #region Fields & Properties
    [Header("Prefabs")]
    [SerializeField]
    private GameObject[] powerUpPrefabs = new GameObject[0];
    public GameObject[] PowerUpPrefabs { get { return this.powerUpPrefabs; } }
	#endregion
	
	#region Methods
	#region Intialization
	private void Awake()
	{
		this.Initialize();
	}
	
	private void Initialize()
	{
		
	}
	#endregion
	#endregion
}
