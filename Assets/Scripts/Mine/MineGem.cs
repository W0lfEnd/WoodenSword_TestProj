using System;
using UnityEngine;


public class MineGem : MonoBehaviour
{
  #region Private Fields
  private Action   releaseAction  = null;
  private Collider colliderCached = null;
  #endregion

  #region Properties
  private new Collider collider => ( colliderCached ??= GetComponent<Collider>() );
  #endregion


  #region Public Methods
  public void Init( Action releaseAction )
  {
    this.releaseAction = releaseAction;
    IsInteractable = false;
  }

  public bool IsInteractable
  {
    get => collider.enabled;
    set
    {
      collider.enabled = value;
    }
  }

  public void Release() => releaseAction?.Invoke();
  #endregion
}
