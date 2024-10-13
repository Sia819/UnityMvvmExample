using Unity.VisualScripting;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    private void Start()
    {
        Singleton<UIManager>.instance.Func1();
    }
}