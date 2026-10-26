using EntityStates;
using EntityStates.Engi.EngiWeapon;
using RoR2;
using UnityEngine;

namespace Cloudburst.Characters.Engineer.EntityStates
{
    public class PlaceFlameTurret : PlaceTurret
    {
        private static GameObject walkerBlueprintPrefab;
        private static GameObject walkerWristDisplayPrefab;

        public override void OnEnter()
        {
            //borrow the blueprints from the walker turret state, since this state has no entitystateconfiguration
            if (!walkerBlueprintPrefab && EntityStateCatalog.InstantiateState(typeof(PlaceWalkerTurret)) is PlaceWalkerTurret walkerState)
            {
                walkerBlueprintPrefab = walkerState.blueprintPrefab;
                walkerWristDisplayPrefab = walkerState.wristDisplayPrefab;
            }

            blueprintPrefab = walkerBlueprintPrefab;
            wristDisplayPrefab = walkerWristDisplayPrefab;
            turretMasterPrefab = EngineerContent.flameTurretMaster;

            base.OnEnter();
        }
    }
}
