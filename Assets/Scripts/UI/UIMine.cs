using TMPro;
using UnityEngine;


public class UIMine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtCurrentLayer = null;


    void Awake()
    {
        GameManager.Instance.MineController.onLayerChanged += SetLayer;
    }

    private void SetLayer( int layerNum )
    {
        txtCurrentLayer.text = $"Layer: {layerNum}";
    }
}
