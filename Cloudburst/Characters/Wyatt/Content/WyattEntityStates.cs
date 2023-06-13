using Cloudburst.CEntityStates.Wyatt;

namespace Cloudburst.Characters.Wyatt
{
    public class WyattEntityStates
    {
        public static void AddEntityStates()
        {
            R2API.ContentAddition.AddEntityState<WyattBaseMeleeAttack>(out _);
            R2API.ContentAddition.AddEntityState<TrashOut>(out _);
            R2API.ContentAddition.AddEntityState<ActivateFlow>(out _);
            R2API.ContentAddition.AddEntityState<DeployMaid>(out _);
            R2API.ContentAddition.AddEntityState<RetrieveMaid>(out _);
        }
    }
}