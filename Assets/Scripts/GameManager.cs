using System;
using UnityEngine;


public class GameManager : MonoBehaviourSingleton<GameManager>
{
  #region Events
  public event Action<long, long> onMoneyCoinsChanged = delegate {};
  #endregion

  #region Serialize Fields
  [SerializeField] private MineController mineController = null;
  #endregion

  #region Properties
  public MineController MineController => mineController;

  public long MoneyCoins
  {
    get => moneyCoins;
    set
    {
      long oldValue = moneyCoins;
      moneyCoins = value;

      // Debug.Log( $"Coins amount changed {moneyCoins} (delta: {moneyCoins - oldValue})" );
      onMoneyCoinsChanged( oldValue, moneyCoins );
    }
  }
  #endregion

  #region Private Fields
  private long moneyCoins = 0;
  #endregion
}