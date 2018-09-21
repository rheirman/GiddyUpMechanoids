using GiddyUpCore;
using GiddyUpCore.Utilities;
using HugsLib;
using HugsLib.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpMechanoids
{
    public class Base : ModBase
    {
        public override string ModIdentifier => "GiddyUpMechanoids";
        internal static SettingHandle<int> mountChance;
        public static SettingHandle<DictAnimalRecordHandler> mechSelector;
        private static Color highlight1 = new Color(0.5f, 0, 0, 0.1f);

        internal static SettingHandle<float> bodySizeFilter;

        public override void DefsLoaded()
        {
            base.DefsLoaded();
            List<ThingDef> mechDefs = GetMechDefs();
            mountChance = Settings.GetHandle<int>("mountChance", "GU_BME_MountChance_Title".Translate(), "GU_BME_MountChance_Description".Translate(), 40, Validators.IntRangeValidator(0, 100));
            bodySizeFilter = Settings.GetHandle<float>("bodySizeFilter", "GU_BME_BodySizeFilter_Title".Translate(), "GU_BME_BodySizeFilter_Description".Translate(), 1.01f);
            mechSelector = Settings.GetHandle<DictAnimalRecordHandler>("mechSelector", "GU_BME_Mechselection_Title".Translate(), "GU_BME_Mechselection_Description".Translate(), null);
            bodySizeFilter.CustomDrawer = rect => { return DrawUtility.CustomDrawer_Filter(rect, bodySizeFilter, false, 0, 5, highlight1); };
            mechSelector.CustomDrawer = rect => { return DrawUtility.CustomDrawer_MatchingAnimals_active(rect, mechSelector, GetMechDefs(), bodySizeFilter, "GUC_SizeOk".Translate(), "GUC_SizeNotOk".Translate()); };
            DrawUtility.filterAnimals(ref mechSelector, mechDefs, bodySizeFilter);
        }
        private static List<ThingDef> GetMechDefs()
        {
            //TODO: adapt this!
            Predicate<ThingDef> isMech = (ThingDef d) => d.race != null && d.race.IsMechanoid;
            List<ThingDef> mechs = new List<ThingDef>();
            foreach (ThingDef thingDef in from td in DefDatabase<ThingDef>.AllDefs
                                          where isMech(td)
                                          select td)
            {
                mechs.Add(thingDef);
            }
            return mechs;
        }

        public static bool IsAllowedInModOptions(String defName)
        {
            bool found = Base.mechSelector.Value.InnerList.TryGetValue(defName, out GiddyUpCore.AnimalRecord value);
            if (found && value.isSelected)
            {
                return true;
            }
            return false;
        }
    }

}
