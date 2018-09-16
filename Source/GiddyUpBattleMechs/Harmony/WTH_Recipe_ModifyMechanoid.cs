using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using WhatTheHack.Recipes;

namespace GiddyUpBattleMechs.Harmony
{
    [HarmonyPatch(typeof(Recipe_ModifyMechanoid), "CanApplyOn")]
    class WTH_Recipe_ModifyMechanoid_CanApplyOn
    {
        static void Postfix(Recipe_ModifyMechanoid __instance, Pawn pawn, ref bool __result)
        {
            if (__instance.recipe == GU_BME_DefOf.GU_BME_InstallGiddyUpModule && !Base.IsAllowedInModOptions(pawn.def.defName))
            {
                __result = false;
            }
        }
    }
}
