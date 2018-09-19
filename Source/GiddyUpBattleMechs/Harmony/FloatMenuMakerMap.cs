using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using GiddyUpCore;
using GiddyUpCore.Jobs;
using GiddyUpCore.Utilities;
using WhatTheHack;

namespace GiddyUpBattleMechs.Harmony
{
    [HarmonyPatch(typeof(FloatMenuMakerMap), "AddDraftedOrders")]
    [HarmonyPatch(new Type[] { typeof(Vector3), typeof(Pawn), typeof(List<FloatMenuOption>) })]
    static class FloatMenuMakerMap_AddDraftedOrders
    {
        static void Postfix(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
        {
            if (pawn.IsHacked())
            {
                GUC_FloatMenuUtility.AddMountingOptions(clickPos, pawn, opts);
            }
        }
    }
}
