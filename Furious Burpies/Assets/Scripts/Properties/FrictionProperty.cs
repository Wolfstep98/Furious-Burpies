using System;
using UnityEngine;

public interface IFrictionProperty
{
    bool IsEnable { get; }
    Vector2 FrictionCoef { get; }
}

public class FrictionProperty : MonoBehaviour, IFrictionProperty
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private bool isEnable = true;
    public bool IsEnable { get { return this.isEnable; } }
    [SerializeField]
    private Vector2 frictionCoef = new Vector2(0.5f, 0.5f);
    public Vector2 FrictionCoef { get { return this.frictionCoef; } }

    #endregion

    #region Methods
    #region Collisions
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == GameObjectsTags.Player)
    //    {
    //        IFrictionBehaviour frictionBehaviour = collision.gameObject.GetComponent<FrictionBehaviour>();
    //        frictionBehaviour.Friction(this.frictionCoef);
    //    }
    //}

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if(this.isEnable)
    //    {
    //        if(hit.gameObject.tag == GameObjectsTags.Player)
    //        {
    //            IFrictionBehaviour frictionBehaviour = hit.gameObject.GetComponent<CustomCharacterController>();
    //            frictionBehaviour.Friction(this.frictionCoef);
    //        }
    //    }
    //}
    #endregion
    #endregion
}
