using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrontsDebug : MonoBehaviour
{
    private TextMeshProUGUI _targetText;
    [SerializeField]
    private bool showDebug = true;

    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        _targetText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (showDebug)
        {
            string mag = string.Format("跳跃高度:{0}",playerTransform.position.y);
            
            _targetText.text = mag;
        }
    }
}
