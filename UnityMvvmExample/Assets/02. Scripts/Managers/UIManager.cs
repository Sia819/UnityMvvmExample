using UnityEngine;
using Unity.VisualScripting;

[Singleton(Name = "UIManager", Automatic = true, Persistent = true)]
public class UIManager : MonoBehaviour, ISingleton
{
    public void Func1()
    {
        Debug.Log("a");
    }
}
