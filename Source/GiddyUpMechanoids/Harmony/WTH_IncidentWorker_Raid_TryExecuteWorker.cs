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

namespace GiddyUpMechanoids.Harmony
{
    [HarmonyPatch(typeof(IncidentWorker_Raid_TryExecuteWorker), "SpawnHackedMechanoids")]
    class WTH_IncidentWorker_Raid_TryExecuteWorker
    {
        static void Postfix(ref IEnumerable<Pawn> __result)
        {

            List<Pawn> mechs = __result.ToList().FindAll((Pawn p) => p.IsHacked());
            List<Pawn> humanlikes = __result.ToList().FindAll((Pawn h) => h.RaceProps.Humanlike && GiddyUpCore.Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(h).mount == null);

            Random r = new Random();
            foreach(Pawn mech in mechs)
            {
                if (Rand.Chance((Base.mountChance)/100f) && Base.IsAllowedInModOptions(mech.def.defName))
                {
                    if(humanlikes.Count > 0)
                    {

                        Pawn rider = humanlikes.Pop();
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
            mech.health.AddHediff(GU_Mech_DefOf.GU_Mech_GiddyUpModule);
            if (mech.jobs == null)
            {
                mech.jobs = new Pawn_JobTracker(mech);
            }
            Job job = new Job(GUC_JobDefOf.Mounted, rider) { count = 1 };
            mech.jobs.StartJob(job, JobCondition.InterruptForced);


        }
    }
}
