using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
  #region Serialize Fields
  [SerializeField] private PlayerLadderDetector playerLadderDetector = null;
  [SerializeField] private CharacterController  characterController  = null;
  [Header( "Speed")]
  [SerializeField] private float moveSpeedHorizontal = 1.0f;
  [SerializeField] private float moveSpeedVertical = 1.0f;
  [Header( "Rotation")]
  [SerializeField] private float turnSmoothVelocity = 1f;
  [SerializeField] private float turnSmoothTime = 0.1f;
  [Header( "Gravity")]
  [SerializeField] private float gravityMultiplier = 3f;
  #endregion

  #region Public Properties
  public Vector2 MoveDirection    { get; set; }         = Vector2.zero;
  public bool    IsClimbing       { get; private set; } = false;
  public Vector3 Velocity         { get; private set; } = Vector3.zero;
  public bool    IsTouchingGround { get; private set; } = false;
  #endregion


  #region Unity Methods
  private void Awake()
  {
    if ( playerLadderDetector )
    {
      playerLadderDetector.onLadderChanged += OnLadderChanged;
    }
  }

  private void Update()
  {
    UpdateVelocityAndMove();
  }
  #endregion

  #region Private Methods
  private void UpdateVelocityAndMove()
  {
    Vector3 curVelocity    = GetMovementVelocity();
    Vector3 velocityToMove = curVelocity * Time.deltaTime;

    CollisionFlags collisionFlags = characterController.Move( velocityToMove );
    IsTouchingGround = collisionFlags.HasFlag( CollisionFlags.Below );
    if ( IsTouchingGround )
    {
      curVelocity.y = 0.0f;
      playerLadderDetector.Unclimb();
    }
    else if ( !IsClimbing )
      curVelocity = ApplyGravity( curVelocity );

    if ( !IsClimbing && velocityToMove.setY( 0.0f ).magnitude > 0.01f )
      RotateToMoveDirection( velocityToMove );

    Velocity = curVelocity;
  }
  private Vector3 GetMovementVelocity()
  {
    if ( IsClimbing )
      return Vector3.up * ( MoveDirection.y * moveSpeedVertical );
    
    Vector2 velocityIn2DSpace = MoveDirection.normalized * moveSpeedHorizontal;
    Vector3 velocityIn3DSpace = new Vector3( velocityIn2DSpace.x, Velocity.y, velocityIn2DSpace.y );

    return velocityIn3DSpace;
  }

  private Vector3 ApplyGravity( Vector3 velocity )
    => velocity.plusY( gravityMultiplier * Physics.gravity.y * Time.deltaTime );

  private void RotateToMoveDirection( Vector3 input )
  {
    float targetAngle = Mathf.Atan2( input.x, input.z ) * Mathf.Rad2Deg;
    float angle       = Mathf.SmoothDampAngle( transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime );
    transform.rotation = Quaternion.Euler( 0, angle, 0 );
  }

  private void OnLadderChanged( Ladder ladder )
  {
    IsClimbing = ladder != null;
    if ( !ladder )
      return;
    
    Velocity = Vector3.zero;

    transform.position = ladder.GetStartClimbPosition( transform.position );
    transform.rotation = ladder.GetRotationToLadder();
  }
  #endregion
}