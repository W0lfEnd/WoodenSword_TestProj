using UnityEngine;


public class PlayerAnimator : MonoBehaviour
{
  #region Serialize Fields
  [SerializeField] private Animator animator                  = null;
  [SerializeField] private float    horizontalSpeedMultiplier = 1.0f;
  [SerializeField] private float    verticalSpeedMultiplier   = 1.0f;
  #endregion

  #region Animator Keys
  private static readonly int keySpeedHorizontal  = Animator.StringToHash( "SpeedHorizontal" );
  private static readonly int keySpeedVertical    = Animator.StringToHash( "SpeedVertical" );
  private static readonly int keySpeedAbsVertical = Animator.StringToHash( "SpeedAbsVertical" );

  private static readonly int keyTriggerClimb   = Animator.StringToHash( "TriggerClimb" );
  private static readonly int keyTriggerUnclimb = Animator.StringToHash( "TriggerUnclimb" );

  private static readonly int keyTriggerDigging = Animator.StringToHash( "TriggerDigging" );
  #endregion

  #region Private Fields
  private bool? isClimbing = null;
  #endregion


  #region Public Methods
  public void SetVelocity( Vector3 velocity )
  {
    animator.SetFloat( keySpeedHorizontal, velocity.setY( 0 ).magnitude * horizontalSpeedMultiplier );

    float verticalSpeed = velocity.y * verticalSpeedMultiplier;
    animator.SetFloat( keySpeedVertical,    verticalSpeed );
    animator.SetFloat( keySpeedAbsVertical, Mathf.Abs( verticalSpeed ) );
  }

  public void SetClimbing( bool isClimbing )
  {
    if ( this.isClimbing == isClimbing )
      return;

    this.isClimbing = isClimbing;
    if ( isClimbing )
    {
      animator.SetTrigger( keyTriggerClimb );
      animator.ResetTrigger( keyTriggerUnclimb );
      return;
    }

    animator.SetTrigger( keyTriggerUnclimb );
    animator.ResetTrigger( keyTriggerClimb );
  }

  public void SetDiggingTrigger()
  {
    animator.SetTrigger( keyTriggerDigging );
  }
  #endregion
}