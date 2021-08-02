﻿using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GiddyUpMechanoids.Harmony
{
    [HarmonyPatch(typeof(JobDriver_Mounted), "shouldCancelJob")]
    class GU_JobDriver_Mounted_ShouldCancelJob
    {
        static void Postfix(ExtendedPawnData riderData, JobDriver_Mounted __instance, ref bool __result)
        {
            if(__instance.pawn.Faction == Faction.OfPlayer && __instance.pawn.RaceProps.IsMechanoid && !__instance.Rider.Drafted)
            {
                //only allow mechanoid mounts in battle for now
                __result = true;
            }
        }
    }
}
