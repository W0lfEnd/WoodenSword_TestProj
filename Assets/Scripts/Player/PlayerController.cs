using UnityEngine;


public class PlayerController : MonoBehaviour
{
  #region Serialize Fields
  [SerializeField] private PlayerAnimator          playerAnimator          = null;
  [SerializeField] private PlayerInput             playerInput             = null;
  [SerializeField] private PlayerMovement          playerMovement          = null;
  [SerializeField] private PlayerCollisionDetector playerCollisionDetector = null;
  #endregion


  #region Unity Methods
  private void Awake()
  {
    InitSubscriptions();
  }

  private void Update()
  {
    playerAnimator.SetClimbing( playerMovement.IsClimbing );
    playerAnimator.SetVelocity( playerMovement.Velocity );
  }
  #endregion

  #region Private Methods
  private void InitSubscriptions()
  {
    if ( playerCollisionDetector )
    {
      playerCollisionDetector.onCollisionMineCell += OnMineCellCollision;
      playerCollisionDetector.onCollisionMineGem += OnMineGemCollision;
    }

    if ( playerInput )
    {
      playerInput.onDirectionInput += OnMoveInput;
    }
  }

  private void OnMoveInput( Vector2 input )
  {
    playerMovement.MoveDirection = input;
  }

  private void OnMineCellCollision( MineCell cell )
  {
    cell.Break();
    playerAnimator.SetDiggingTrigger();
  }

  private void OnMineGemCollision( MineGem gem )
  {
    gem.Release();
    GameManager.Instance.MoneyCoins += 1;
  }
  #endregion
}