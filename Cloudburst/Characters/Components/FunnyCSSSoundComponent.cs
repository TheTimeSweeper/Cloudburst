using UnityEngine;

public class FunnyCSSSoundComponent : MonoBehaviour
{
    void Awake()
    {
        RoR2.Util.PlaySound("Play_Wyatt_Groove_CSS", gameObject);
    }

    void OnDisable()
    {
        RoR2.Util.PlaySound("Stop_Wyatt_Groove_CSS", gameObject);
    }
}
