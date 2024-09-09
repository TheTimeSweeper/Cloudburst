using UnityEngine;

public class FunnyCSSMenuSoundComponent : MonoBehaviour
{
    void OnEnable()
    {
        RoR2.Util.PlaySound("Play_Wyatt_Groove_CSS", gameObject);
    }

    void OnDisable()
    {
        RoR2.Util.PlaySound("Stop_Wyatt_Groove_CSS", gameObject);
    }
}
