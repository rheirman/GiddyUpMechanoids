using GiddyUpCore.Utilities;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using WhatTheHack;

namespace GiddyUpBattleMechs.Harmony
{
    [HarmonyPatch(typeof(GUC_FloatMenuUtility), "AddMountingOptions")]
    static class GU_FloatMenuUtility_AddMountingOptions
    {
        static bool Prefix(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
        {

            foreach (LocalTargetInfo current in GenUI.TargetsAt(clickPos, TargetingParameters.ForAttackHostile(), true))
            {

                if (!(current.Thing is Pawn target))
                {
                    return false;
                }

                if (target.RaceProps.IsMechanoid)
                {
                    if(target.health == null || !target.IsHacked())
                    {
                        return false;
                    }
                    if (target.health.hediffSet.HasHediff(WTH_DefOf.WTH_MountedTurret))
                    {
                        opts.Add(new FloatMenuOption("GU_BME_Reason_Turret".Translate(), null, MenuOptionPriority.Low));
                        return false;
                    }
                    if (!target.health.hediffSet.HasHediff(GU_BME_DefOf.GU_BME_GiddyUpModule))
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
            }
            return true;
        }

    }
}
