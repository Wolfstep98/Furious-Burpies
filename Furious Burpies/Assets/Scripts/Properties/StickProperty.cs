using UnityEngine;

public class StickProperty : MonoBehaviour
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private bool isEnable = true;
    #endregion

    #region Methods
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == GameObjectsTags.Player)
        {
            if (this.isEnable)
            {
                IStickBehaviour stickBehaviour = collision.gameObject.GetComponent<StickBehaviour>();
                stickBehaviour.Stick(this.gameObject);
                Debug.Log("Stick !!!");
            }
        }
    }
    #endregion
}
