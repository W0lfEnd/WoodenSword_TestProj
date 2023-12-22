using System.Collections;
using UnityEngine;


public class MineCellAnimator : MonoBehaviour
{
  #region Serialize Fields
  [SerializeField] private     Animator   animator       = null;
  [SerializeField] private     GameObject cellGameObject = null;
  [SerializeField] private new Collider   collider       = null;
  #endregion

  #region Private Methods
  private Coroutine disableCoroutine            = null;
  private float     timeToDisableCellAfterBreak = 0.5f;
  #endregion


  #region Public Methods
  public void Init( bool isBroke )
  {
    bool isRepaired = !isBroke;

    collider.enabled = isRepaired;
    animator.Play( isBroke ? "CubeBreak" : "CubeIdle" );
    if ( isRepaired )
      cellGameObject.SetActive( true );

    if ( disableCoroutine != null )
    {
      StopCoroutine( disableCoroutine );
      disableCoroutine = null;
    }

    if ( isRepaired )
      return;

    disableCoroutine = StartCoroutine( DisableCellDestroyedPartsAfter( timeToDisableCellAfterBreak ) );
  }
  #endregion

  #region Private Methods
  private IEnumerator DisableCellDestroyedPartsAfter( float seconds )
  {
    yield return new WaitForSeconds( seconds );

    cellGameObject.SetActive( false );
    disableCoroutine = null;
  }
  #endregion
}