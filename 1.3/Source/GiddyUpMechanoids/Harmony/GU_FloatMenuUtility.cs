using GiddyUpCore.Utilities;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using WhatTheHack;

namespace GiddyUpMechanoids.Harmony
{
    [HarmonyPatch(typeof(GUC_FloatMenuUtility), "AddMountingOptions")]
    static class GU_FloatMenuUtility_AddMountingOptions
    {
        static bool Prefix(Pawn target, Pawn pawn, List<FloatMenuOption> opts)
        {
            if (target.RaceProps.IsMechanoid && target.IsHacked())
            {
                if (target.health.hediffSet.HasHediff(WTH_DefOf.WTH_MountedTurret))
                {
                    opts.Add(new FloatMenuOption("GU_BME_Reason_Turret".Translate(), null, MenuOptionPriority.Low));
                    return false;
                }
                if (!target.health.hediffSet.HasHediff(GU_Mech_DefOf.GU_Mech_GiddyUpModule))
                {
                    opts.Add(new FloatMenuOption("GU_BME_Reason_NoModule".Translate(), null, MenuOptionPriority.Low));
                    return false;
                }                  
                if (!Base.IsAllowedInModOptions(target.def.defName))
                {
                    opts.Add(new FloatMenuOption("GUC_NotInModOptions".Translate(), null, MenuOptionPriority.Low));
                    return false;
                }
                if (!target.IsActivated())
                {
                    opts.Add(new FloatMenuOption("GU_BME_Reason_NotActivated".Translate(), null, MenuOptionPriority.Low));
                    return false;
                }
            }
            return true;
        }
    }
}
