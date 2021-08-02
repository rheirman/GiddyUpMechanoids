using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using GiddyUpCore;
using GiddyUpCore.Jobs;
using GiddyUpCore.Utilities;
using WhatTheHack;

namespace GiddyUpMechanoids.Harmony
{
    [HarmonyPatch(typeof(FloatMenuMakerMap), "AddDraftedOrders")]
    [HarmonyPatch(new Type[] { typeof(Vector3), typeof(Pawn), typeof(List<FloatMenuOption>) , typeof(bool)})]
    static class FloatMenuMakerMap_AddDraftedOrders
    {
        static void Postfix(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
        {
            if (Base.IsAllowedInModOptions(pawn.def.defName)) //don't allow mechs that are mountable to be mounted
            {
                return;
            }

            foreach (LocalTargetInfo current in GenUI.TargetsAt(clickPos, TargetingParameters.ForAttackHostile(), true))
            {

                if ((current.Thing is Pawn target) && target.IsHacked())
                {
                    GUC_FloatMenuUtility.AddMountingOptions(target, pawn, opts);
                }
            }
        }
    }
}
