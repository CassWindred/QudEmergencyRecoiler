using System;
using System.Collections.Generic;
using Qud.API;
using XRL.Core;
using XRL.Messages;

namespace XRL.World.Parts
{
    [Serializable]
    public class LocalRandomRecoilOnDeath : RandomRecoilOnDeath
    {
        public override void generateRecoilerPartonObject()
        {

            //MessageQueue.AddPlayerMessage($"Adding local Recoilondeath part!");
            RecoilOnDeath teleporter = ParentObject.RequirePart<RecoilOnDeath>();

            ZoneManager zoneManager = XRLCore.Core.Game.ZoneManager;
            
            teleporter.DestinationZone = zoneManager.ActiveZone.ZoneID;

            teleporter.DestinationX = -1;
            teleporter.DestinationY = -1;

        }
    }
}