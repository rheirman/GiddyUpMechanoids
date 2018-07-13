using GiddyUpCore.Utilities;
using Harmony;
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
        static void Postfix(Pawn pawn, List<FloatMenuOption> opts)
        {
            if (!IsMountableUtility.isMountable(pawn) && pawn.RaceProps.IsMechanoid)
            {
                if (!pawn.IsActivated())
                {
                    opts.Add(new FloatMenuOption("GU_BME_Reason_NotActivated".Translate(), null, MenuOptionPriority.Low));
                }
            }
        }
    }
}
