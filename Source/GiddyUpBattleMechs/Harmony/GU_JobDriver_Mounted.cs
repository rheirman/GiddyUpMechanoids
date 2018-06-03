using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GiddyUpBattleMechs.Harmony
{
    [HarmonyPatch(typeof(JobDriver_Mounted), "shouldCancelJob")]
    class GU_JobDriver_Mounted_ShouldCancelJob
    {
        static void Postfix(ExtendedPawnData riderData, JobDriver_Mounted __instance, ref bool __result)
        {
            if(__instance.pawn.RaceProps.IsMechanoid && !__instance.pawn.Drafted)
            {
                //only allow mechanoid mounts in battle for now
                __result = true;
            }
        }
    }
}
