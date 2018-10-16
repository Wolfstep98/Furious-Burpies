using System;
using UnityEngine;

public interface ILockBehaviour
{
    void Lock();
    void Unlock();
}

[RequireComponent(typeof(Rigidbody2D))]
public class LockBehaviour : MonoBehaviour
{
    #region Fields & Properties
    [Header("Data")]
    [SerializeField]
    private bool locked = false;

    [Header("References")]
    [SerializeField]
    private GameObject player = null;
    [SerializeField]
    private Rigidbody2D rigid = null;
    #endregion

    #region Methods
    public void Lock()
    {
        this.rigid.velocity = Vector2.zero;
        this.rigid.angularVelocity = 0.0f;
        this.rigid.isKinematic = true;
        this.locked = true;
    }

    public void Unlock()
    {
        this.rigid.isKinematic = false;
        this.locked = false;
    }
    #endregion
}
