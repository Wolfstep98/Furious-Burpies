using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlipProperty
{
    bool IsEnable { get; }
}

public interface ISlipBehaviour
{
    void Slip(ISlipProperty property);
}

public class SlipProperty : MonoBehaviour , ISlipProperty
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private bool isEnable = true;
    public bool IsEnable { get { return this.isEnable; } }
    #endregion

    #region Methods
    #region Initializers
    // Use this for initialization
    void Awake () 
	{
		
	}
	#endregion
	#endregion
}
