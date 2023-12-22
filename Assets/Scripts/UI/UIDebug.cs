using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIDebug : MonoBehaviour
{
    [SerializeField] private TMP_InputField txtInput     = null;
    [SerializeField] private Button         btnSetLayer  = null;
    [SerializeField] private Button         btnNextLayer = null;


    void Awake()
    {
        btnNextLayer.onClick.AddListener( NextLayer );
        btnSetLayer.onClick.AddListener( SetLayer );
    }

    private void NextLayer()
    {
        GameManager.Instance.MineController.CurLayerNum++;
    }

    private void SetLayer()
    {
        if ( !int.TryParse( txtInput.text, out int layer ) )
        {
            Debug.LogError( "Invalid layer string" );
            return;
        }

        GameManager.Instance.MineController.CurLayerNum = layer;
    }
}
