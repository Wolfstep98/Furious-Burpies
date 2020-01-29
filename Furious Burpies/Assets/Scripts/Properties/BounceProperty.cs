using System;
using UnityEngine;

public interface IBounceProperty
{
    bool IsEnable { get; }
    float UpwardForceAdded { get; }
    Vector2 BounceCoef { get; }
}

public class BounceProperty : MonoBehaviour, IBounceProperty
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private bool isEnable = true;
    public bool IsEnable { get { return this.isEnable; } }
    [SerializeField]
    private Vector2 bounceCoef = new Vector2(0.5f, 0.5f);
    public Vector2 BounceCoef { get { return this.bounceCoef; } }
    [SerializeField]
    private float upwardForceAdded = 0.0f;
    public float UpwardForceAdded { get { return this.upwardForceAdded; } }

    #endregion

    #region Methods
    #region Collisions
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == GameObjectsTags.Player)
    //    {
    //        IBounceBehaviour bounceBehaviour = collision.gameObject.GetComponent<BounceBehaviour>();
    //        bounceBehaviour.Bounce(this.bounceCoef, -collision.contacts[0].normal);
    //        Debug.Log("BOUNCE !");
    //    }
    //}

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if(this.isEnabled)
    //    {
    //        if(hit.gameObject.tag == GameObjectsTags.Player)
    //        {
    //            Debug.Log("Bounce Detected");
    //            IBounceBehaviour bounceBehaviour = hit.gameObject.GetComponent<CustomCharacterController>();
    //            bounceBehaviour.Bounce(this.bounceCoef, hit.normal);
    //        }
    //    }
    //}
    #endregion
    #endregion
}
