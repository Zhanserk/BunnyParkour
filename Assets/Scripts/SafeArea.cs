using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.UIElements;
[RequireComponent(typeof(RectTransform))]
public class SafeArea  : MonoBehaviour
{
    private RectTransform _panel;

    private Rect _last;


     void Awake()
    {

        _panel = GetComponent<RectTransform>();
        
        Apply();
        

        


        
    }
     void Update()
    {
        if(Screen.safeArea != _last)
        {
            Apply();
        }
    }

    void Apply()
    {
        _last = Screen.safeArea;
        Vector2 min = _last.position;
        Vector2 max = _last.position + _last.size;
        
        min.x /= Screen.width;
        min.y/= Screen.height;
        max.x/= Screen.width;
        max.y/= Screen.height;


        _panel.anchorMax = max;
        _panel.anchorMin = min;





    }




}
