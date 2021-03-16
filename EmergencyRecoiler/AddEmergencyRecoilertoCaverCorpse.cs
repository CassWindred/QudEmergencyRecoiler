using HarmonyLib;
using XRL.World;
using XRL.World.Parts;

namespace Fyrefly.EmergencyRecoiler.HarmonyPatches {

    [HarmonyPatch(typeof(CaverCorpseLoot))]
    class AddEmergencyRecoilertoCaverCorpse{
        
        [HarmonyPatch("FireEvent")]
        static void Prefix(CaverCorpseLoot __instance, Event E){
            if (E.ID == "EnteredCell")
			{
				if (__instance.bCreated)
				{
					return;
				}
				Physics obj = __instance.ParentObject.GetPart("Physics") as Physics;
				GameObject randomEmergencyRecoiler = obj.CurrentCell.AddObject(GameObjectFactory.Factory.CreateObject("EmergencyRecoilerRandom"));
                randomEmergencyRecoiler.UseEnergy(323, "Item");
				return;
			}


        }

    }

}