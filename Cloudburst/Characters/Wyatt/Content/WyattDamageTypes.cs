using R2API;

namespace Cloudburst.Characters.Wyatt
{
    public class WyattDamageTypes
    {
        public static DamageAPI.ModdedDamageType antiGravDamage;

        public static void InitDamageTypes()
        {
            antiGravDamage = DamageAPI.ReserveDamageType();
        }
    }
}