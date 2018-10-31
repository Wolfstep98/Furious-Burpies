using System;
using UnityEngine;

public class BounceProperty : MonoBehaviour
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private bool isEnabled = true;
    [SerializeField]
    private Vector2 bounceCoef = new Vector2(0.5f, 0.5f);

    #endregion

    #region Methods
    #region Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == GameObjectsTags.Player)
        {
            IBounceBehaviour bounceBehaviour = collision.gameObject.GetComponent<BounceBehaviour>();
            bounceBehaviour.Bounce(this.bounceCoef, -collision.contacts[0].normal);
            Debug.Log("BOUNCE !");
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(this.isEnabled)
        {
            if(hit.gameObject.tag == GameObjectsTags.Player)
            {
                IBounceBehaviour bounceBehaviour = hit.gameObject.GetComponent<CustomCharacterController>();
                bounceBehaviour.Bounce(this.bounceCoef, hit.normal);
            }
        }
    }
    #endregion
    #endregion
}
