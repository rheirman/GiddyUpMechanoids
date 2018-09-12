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
                    if (!target.IsHacked())
                    {
                        opts.Add(new FloatMenuOption("GU_BME_Reason_NotHacked".Translate(), null, MenuOptionPriority.Low));
                        return false;
                    }

                    if (!IsAllowedInModOptions(target.def.defName))
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

        public static bool IsAllowedInModOptions(String defName)
        {
            Log.Message("calling IsAllowedInModOptions for defName: " + defName);
            bool found = Base.mechSelector.Value.InnerList.TryGetValue(defName, out GiddyUpCore.AnimalRecord value);
            Log.Message("found: " + found);
            Log.Message("value: " + value);
            if (found && value.isSelected)
            {
                return true;
            }
            return false;
        }
    }
}
