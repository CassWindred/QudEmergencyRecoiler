using System;
using System.Collections.Generic;
using System.Text;
using Qud.API;
using XRL.Core;
using XRL.Messages;

namespace XRL.World.Parts
{
    [Serializable]
    public class RandomRecoilOnDeath : RandomRuinRecoiler
    {


        [NonSerialized]
        private int NameCacheTick;

        [NonSerialized]
        private string NameCache;

        public override bool SameAs(IPart p)
        {
            if ((p as RandomRecoilOnDeath).additionalDescription != additionalDescription)
            {
                return false;
            }
            return base.SameAs(p);
        }

        public override bool HandleEvent(GetDisplayNameEvent E)
        {
            if (ParentObject.Understood())
            {
                if (XRLCore.Core.Game != null && XRLCore.Core.Game.ZoneManager != null && (NameCache == null || XRLCore.Core.Game.ZoneManager.NameUpdateTick > NameCacheTick))
                {
                    Teleporter part = ParentObject.GetPart<Teleporter>();
                    if (part != null)
                    {
                        string destinationZone = part.DestinationZone;
                        if (!string.IsNullOrEmpty(destinationZone))
                        {
                            string text = XRLCore.Core.Game.ZoneManager.GetZoneReferenceDisplayName(destinationZone);
                            if (!string.IsNullOrEmpty(text))
                            {
                                text += " ";
                            }
                            NameCache = text;
                        }
                    }
                    NameCacheTick = XRLCore.Core.Game.ZoneManager.NameUpdateTick;
                }
                if (!string.IsNullOrEmpty(NameCache))
                {
                    string text2 = E.DB.PrimaryBase;
                    int num = 10;
                    if (text2 == null)
                    {
                        text2 = "emergency recoiler";
                    }
                    else
                    {
                        num = E.DB[text2];
                        E.DB.Remove(text2);
                    }
                    StringBuilder stringBuilder = Event.NewStringBuilder();
                    stringBuilder.Append(NameCache);
                    if (text2.StartsWith("random-point "))
                    {
                        stringBuilder.Append(text2.Substring(13));
                    }
                    else
                    {
                        stringBuilder.Append(text2);
                    }
                    E.AddBase(stringBuilder.ToString(), num - 10);
                }
            }
            return true;
        }

        public virtual void generateRecoilerPartonObject()
        {
            List<JournalMapNote> mapNotes = JournalAPI.GetMapNotes((JournalMapNote note) => note.Has("ruins") || note.Has("historic"));
            if (mapNotes.Count > 0)
            {
                //MessageQueue.AddPlayerMessage($"Adding Recoilondeath part!");
                ZoneManager zoneManager = XRLCore.Core.Game.ZoneManager;
                JournalMapNote randomElement = mapNotes.GetRandomElement();
                RecoilOnDeath teleporter = ParentObject.RequirePart<RecoilOnDeath>();

				Zone destinationZone = zoneManager.GetZone(randomElement.zoneid);
                List<Cell> emptyReachableCells = destinationZone.GetEmptyReachableCells();
                Cell destinationcell = ((emptyReachableCells.Count <= 0) ? destinationZone.GetCell(40, 20) : emptyReachableCells.GetRandomElement());

                teleporter.DestinationZone = randomElement.zoneid;
                teleporter.DestinationX = destinationcell.X;
                teleporter.DestinationY = destinationcell.Y;
                //MessageQueue.AddPlayerMessage($"DestinationZone: {randomElement.zoneid}!");
            }
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            if (E.Context != "Sample" && E.Context != "Initialization" && XRLCore.Core.Game != null)
            {
                generateRecoilerPartonObject();
            }
            return true;
        }

    }
}