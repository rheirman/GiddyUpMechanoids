using GiddyUpCore.Utilities;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;
using WhatTheHack;

namespace GiddyUpBattleMechs.Harmony
{
    //For some reason, using a normal harmonypatch here with the folloing types doesn't work, as harmony throws an error that it can't find the method. 
    //[HarmonyPatch(new Type[] { typeof(Pawn), typeof(IsMountableUtility.Reason) })]
    [HarmonyPatch]
    class GU_IsMountableUtility_isMountable
    {
        static MethodBase TargetMethod()
        {
            return typeof(IsMountableUtility).GetMethods(AccessTools.all).FirstOrDefault(m => m.Name.StartsWith("isMountable") && m.GetParameters().Count() == 2);
        }
        static bool Prefix(Pawn pawn, ref bool __result)
        {
            if (pawn.IsHacked() && pawn.IsActivated())
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
}
