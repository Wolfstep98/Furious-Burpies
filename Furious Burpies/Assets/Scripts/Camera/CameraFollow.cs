using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Fields & Properties
    #region Enums
    public enum FollowState
    {
        Lock,
        Follow
    }
    #endregion
    [Header("Parameters")]
    [SerializeField]
    private FollowState xAxis = FollowState.Follow;
    [SerializeField]
    private FollowState yAxis = FollowState.Follow;
    [SerializeField]
    private FollowState zAxis = FollowState.Follow;

    [SerializeField]
    private Vector3 currentPosition = Vector3.zero;
    [SerializeField]
    private Vector3 offset = Vector3.zero;

    [Header("References")]
    [SerializeField]
    private Transform objToFollow = null;
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
        if (this.objToFollow == null)
            Debug.LogError("[Missing Reference- objToFollow is missing !");
#endif
    }
    #endregion

    private void Update()
    {
        this.UpdatePosition();
        this.transform.position = this.currentPosition;
    }

    private void UpdatePosition()
    {
        Vector3 position = Vector3.back * 10;
        if (this.xAxis == FollowState.Follow)
            position.x = this.objToFollow.position.x + this.offset.x;
        if (this.yAxis == FollowState.Follow)
            position.y = this.objToFollow.position.y + this.offset.y;
        if (this.zAxis == FollowState.Follow)
            position.z = this.objToFollow.position.z + this.offset.z;
        this.currentPosition = position;
    }
    #endregion
}
