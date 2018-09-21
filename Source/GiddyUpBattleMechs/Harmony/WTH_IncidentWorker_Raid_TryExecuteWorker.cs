using Harmony;
using WhatTheHack.Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using WhatTheHack;
using Verse.AI;
using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using GiddyUpCore.Utilities;

namespace GiddyUpBattleMechs.Harmony
{
    [HarmonyPatch(typeof(IncidentWorker_Raid_TryExecuteWorker), "SpawnHackedMechanoids")]
    class WTH_IncidentWorker_Raid_TryExecuteWorker
    {
        static void Postfix(ref List<Pawn> list)
        {
            Log.Message("SpawnHackedMechanoids");
            List<Pawn> mechs = list.FindAll((Pawn p) => p.IsHacked());
            List<Pawn> humanlikes = list.FindAll((Pawn h) => h.RaceProps.Humanlike && GiddyUpCore.Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(h).mount == null);

            if(mechs.Count() == 0)
            {
                Log.Message("no hacked mechs in raid");
            }
            if(humanlikes.Count() == 0)
            {
                Log.Message("no humanlikes found");
            }
            Random r = new Random();
            foreach(Pawn mech in mechs)
            {
                Log.Message("trying to mount mech: " + mech.def.defName);
                if (Base.IsAllowedInModOptions(mech.def.defName))
                {
                    Log.Message("mech allowed in mod options");
                    if(humanlikes.Count > 0)
                    {

                        Pawn rider = humanlikes.Pop();
                        Log.Message("assigning rider to mech");
                        Log.Message("rider: " + rider);
                        Log.Message("mech: " + mech);
                        RideMech(mech, rider);
                    }
                }
            }
        }

        private static void RideMech(Pawn mech, Pawn rider)
        {
            ExtendedPawnData riderData = GiddyUpCore.Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(rider);
            ExtendedPawnData mechData = GiddyUpCore.Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(mech);
            riderData.mount = mech;
            TextureUtility.setDrawOffset(riderData);
            mechData.ownedBy = rider;
            riderData.owning = mech;
            if (mech.jobs == null)
            {
                mech.jobs = new Pawn_JobTracker(mech);
            }
            Job job = new Job(GUC_JobDefOf.Mounted, rider) { count = 1 };
            mech.jobs.StartJob(job, JobCondition.InterruptForced);

        }
    }
}
