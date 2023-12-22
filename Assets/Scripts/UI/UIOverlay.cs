using TMPro;
using UnityEngine;


public class UIOverlay : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI txtGemsCount = null;


  private void Awake()
  {
    GameManager.Instance.onMoneyCoinsChanged += SetGemsCount;
  }

  private void SetGemsCount( long oldCount, long newCount )
  {
    txtGemsCount.text = $"Gems: {newCount}";
  }
}