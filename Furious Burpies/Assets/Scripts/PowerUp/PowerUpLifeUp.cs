using System;
using UnityEngine;

public interface IPowerUpLifeUp
{
    int LifeAmount { get; }
}

[RequireComponent(typeof(Collider))]
public class PowerUpLifeUp : MonoBehaviour, IPowerUpLifeUp
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private int lifeAmount = 1;
    public int LifeAmount { get { return this.lifeAmount; } }
	#endregion
}
