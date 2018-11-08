using System;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private float speed = 5.0f;

    [Header("References")]
    [SerializeField]
    private Transform startingPoint = null;
    [SerializeField]
    private Transform killZone = null;
	#endregion
	
	#region Methods
	#region Intialization
	private void Awake()
	{
		this.Initialize();
	}
	
	private void Initialize()
	{
#if UNITY_EDITOR
        if (this.killZone == null)
            Debug.LogError("[Missing Reference] - killZone is missing ! ");
        if (this.startingPoint == null)
            Debug.LogError("[Missing Reference] - startingPoint is missing ! ");
# endif

        this.killZone.position = this.startingPoint.position;
    }
    #endregion

    private void Update()
    {
        this.killZone.position += Vector3.right * this.speed * GameTime.deltaTime;
    }

    #endregion
}
