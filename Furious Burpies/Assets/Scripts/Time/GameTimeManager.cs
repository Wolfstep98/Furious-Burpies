using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : MonoBehaviour 
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    [Range(0.0f, 2.0f)]
    private float timeScale = 1.0f;
    [SerializeField]
    private float lastTimeScale = 1.0f;
	#endregion

	#region Methods
	void Update () 
	{
		if(timeScale != lastTimeScale)
        {
            GameTime.timeScale = timeScale;
            this.lastTimeScale = this.timeScale;
        }
	}
	#endregion
}
