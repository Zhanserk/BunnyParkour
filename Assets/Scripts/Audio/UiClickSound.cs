using UnityEngine;
using UnityEngine.UI;

// Вешается на кнопку: при нажатии звучит щелчок.
[RequireComponent(typeof(Button))]
public class UiClickSound : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Click);   // подписываемся на нажатие
    }

    void Click()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClick();
    }
}
