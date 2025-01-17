﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;


namespace HyperElk.Core
{
    public class MistweaverMonk : CombatRoutine
    {
        //Spell Strings
        //Combat
        private string TigerPalm = "Tiger Palm";
        private string BlackoutKick = "Blackout Kick";
        private string SpinningCraneKick = "Spinning Crane Kick";
        private string ExpelHarm = "Expel Harm";
        private string RisingSunKick = "Rising Sun Kick";
        private string DampenHarm = "Dampen Harm";
        private string HealingElixir = "Healing Elixir";
        //Heal
        private string Vivify = "Vivify";
        private string EnvelopingMist = "Enveloping Mist";
        private string RenewingMist = "Renewing Mist";
        private string EssenceFont = "Essence Font";
        private string SoothingMist = "Soothing Mist";
        private string LifeCocoon = "Life Cocoon";
        private string Revival = "Revival";
        private string Yulon = "Invoke Yu'lon, the Jade Serpent";
        private string ManaTea = "Mana Tea";
        private string ThunderFocusTea = "Thunder Focus Tea";
        private string ChiWave = "Chi Wave";
        private string ChiBurst = "Chi Burst";
        private string RefreshingJadeWind = "Refreshing Jade Wind";
        private string SummonJadeSerpentStatue = "Summon Jade Serpent Statue";
        private string InvokeChiJi = "Invoke Chi-Ji, the Red Crane";
        private string Detox = "Detox";
        private string SpiritualManaPotion = "Spiritual Mana Potion";
        private string WeaponsofOrder = "Weapons of Order";
        private string WeaponsofOrderAOE = "Weapons of Order AOE";

        private string Fleshcraft = "Fleshcraft";
        private string FaelineStomp = "FaelineStomp";
        private string FallenOrder = "Fallen Order";

        private string trinket1 = "trinket1";
        private string trinket2 = "trinket2";
        //Target Misc
        private string Party1 = "party1";
        private string Party2 = "party2";
        private string Party3 = "party3";
        private string Party4 = "party4";
        private string Player = "player";
        private string AoE = "AOE";
        private string PartySwap = "Target Swap";
        private string TargetChange = "Target Change";
        //Talents
        private bool TalentChiWave => API.PlayerIsTalentSelected(1, 2);
        private bool TalentChiBurst => API.PlayerIsTalentSelected(1, 3);

        private bool TalentManaTea => API.PlayerIsTalentSelected(3, 3);
        private bool TalentDampenHarm => API.PlayerIsTalentSelected(5, 3);
        private bool TalentHealingElixir => API.PlayerIsTalentSelected(5, 1);
        private bool TalentRefreshingJadeWind => API.PlayerIsTalentSelected(6, 2);
        private bool TalentSummonJadeSerpentStatue => API.PlayerIsTalentSelected(6, 1);
        private bool TalentInvokeChiJi => API.PlayerIsTalentSelected(6, 3);



        //CBProperties
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        int[] numbPartyList = new int[] { 0, 1, 2, 3, 4, 5, };
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40 };
        string[] units = { "player", "party1", "party2", "party3", "party4" };
        string[] raidunits = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };
        int PlayerHealth => API.TargetHealthPercent;
        string[] PlayerTargetArray = { "player", "party1", "party2", "party3", "party4" };
        string[] RaidTargetArray = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };
        string[] DetoxList = { "Chilled", "Frozen Binds", "Clinging Darkness", "Rasping Scream", "Heaving Retch", "Goresplatter", "Slime Injection", "Gripping Infection", "Debilitating Plague", "Burning Strain", "Blightbeak", "Corroded Claws", "Wasting Blight", "Hurl Spores", "Corrosive Gunk", "Cytotoxic Slash", "Venompiercer", "Wretched Phlegm", "Bewildering Pollen", "Repulsive Visage", "Soul Split", "Anima Injection", "Bewildering Pollen2", "Bramblethorn Entanglement", "Debilitating Poison", "Sinlight Visions", "Siphon Life", "Turn to Stone", "Stony Veins", "Cosmic Artifice", "Wailing Grief", "Shadow Word:  Pain", "Anguished Cries", "Wrack Soul", "Dark Lance", "Insidious Venom", "Charged Anima", "Lost Confidence", "Burden of Knowledge", "Internal Strife", "Forced Confession", "Insidious Venom2", "Soul Corruption", "Genetic Alteration", "Withering Blight", "Decaying Blight" };
        private int TargetChangePercent => numbList[CombatRoutine.GetPropertyInt(TargetChange)];
        private int PartySwapPercent => numbList[CombatRoutine.GetPropertyInt(PartySwap)];


        private int UnitBelowHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercent(int HealthPercent) => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(HealthPercent) : UnitBelowHealthPercentParty(HealthPercent);
        private bool IsAutoSwap => API.ToggleIsEnabled("Auto Target");
        private bool IsAutoDetox => API.ToggleIsEnabled("Auto Detox");
        private bool AoEHeal => API.ToggleIsEnabled("AOE Heal");

        //General
        private static readonly Stopwatch JadeSerpentStatueWatch = new Stopwatch();

        int ViVifyCounter = 0;
        string[] ThunderFocusTeaList = new string[] { "always", "Cooldowns", "Manual", };
        private string UseThunderFocusTea => ThunderFocusTeaList[CombatRoutine.GetPropertyInt(ThunderFocusTea)];
        private bool NotChanneling => API.PlayerCurrentCastTimeRemaining == 0;
        private bool NotCasting => !API.PlayerIsCasting(false);
        private int AoENumber => numbPartyList[CombatRoutine.GetPropertyInt(AoE)];
        private int ExpelHarmtPercent => numbList[CombatRoutine.GetPropertyInt(ExpelHarm)];
        private bool WeaponsofOrderAoE => UnitBelowHealthPercent(WeaponsofOrderPercent) >= AoENumber;

        private bool RevivalAoE => UnitBelowHealthPercent(RevivalPercent) >= AoENumber;
        private bool EssenceFontAoE => UnitBelowHealthPercent(EssenceFontPercent) >= AoENumber;
        private bool RefreshingJadeWindAoE => UnitBelowHealthPercent(RefreshingJadeWindPercent) >= AoENumber;
        private int WeaponsofOrderPercent => numbList[CombatRoutine.GetPropertyInt(WeaponsofOrderAOE)];
        private int EnvelopingMistPercent => numbList[CombatRoutine.GetPropertyInt(EnvelopingMist)];
        private int VivifyPercent => numbList[CombatRoutine.GetPropertyInt(Vivify)];
        private int SoothingMistPercent => numbList[CombatRoutine.GetPropertyInt(SoothingMist)];
        private int LifeCocoonPercent => numbList[CombatRoutine.GetPropertyInt(LifeCocoon)];
        private int RenewingMistPercent => numbList[CombatRoutine.GetPropertyInt(RenewingMist)];
        private int RevivalPercent => numbList[CombatRoutine.GetPropertyInt(Revival)];
        private int EssenceFontPercent => numbList[CombatRoutine.GetPropertyInt(EssenceFont)];
        private int ManaTeaPercent => numbList[CombatRoutine.GetPropertyInt(ManaTea)];
        private int ChiWavePercent => numbList[CombatRoutine.GetPropertyInt(ChiWave)];
        private int ChiBurstPercent => numbList[CombatRoutine.GetPropertyInt(ChiBurst)];
        private int DampenHarmPercent => numbList[CombatRoutine.GetPropertyInt(DampenHarm)];
        private int HealingElixirPercent => numbList[CombatRoutine.GetPropertyInt(HealingElixir)];
        private int RefreshingJadeWindPercent => numbList[CombatRoutine.GetPropertyInt(RefreshingJadeWind)];
        private int SpiritualManaPotionManaPercent => numbList[CombatRoutine.GetPropertyInt(SpiritualManaPotion)];

        private string UseWeaponsofOrder => WeaponsofOrderList[CombatRoutine.GetPropertyInt(WeaponsofOrder)];
        string[] WeaponsofOrderList = new string[] { "always", "Cooldowns", "Manual", "AOE", };
        private int FleshcraftPercentProc => numbList[CombatRoutine.GetPropertyInt(Fleshcraft)];
        bool ChannelSoothingMist => API.CurrentCastSpellID("player") == 115175;

        string[] FaelineStompList = new string[] { "always", "Cooldowns", "AOE" };
        private string UseFaelineStomp => FaelineStompList[CombatRoutine.GetPropertyInt(FaelineStomp)];
        string[] FallenOrderList = new string[] { "always", "Cooldowns", "AOE" };
        private string UseFallenOrder => FallenOrderList[CombatRoutine.GetPropertyInt(FallenOrder)];
        private string UseTrinket1 => TrinketList1[CombatRoutine.GetPropertyInt(trinket1)];
        string[] TrinketList1 = new string[] { "always", "Cooldowns", "never" };

        private string UseTrinket2 => TrinketList2[CombatRoutine.GetPropertyInt(trinket2)];
        string[] TrinketList2 = new string[] { "always", "Cooldowns", "never" };
        private static bool CanDetoxTarget(string debuff)
        {
            return API.TargetHasDebuff(debuff, false, true);
        }
        private static bool CanDetoxTarget(string debuff, string unit)
        {
            return API.UnitHasDebuff(debuff, unit, false, true);
        }

        public override void Initialize()
        {
            CombatRoutine.Name = "Mistweaver Monk by Mufflon12";
            API.WriteLog("Welcome to Mistweaver Monk by Mufflon12");
            API.WriteLog("Be advised this a Beta Rotation");
            API.WriteLog("Use /cast [@cursor] Summon Jade Serpent Statue");
            API.WriteLog("Invoke Chi-Ji, the Red Crane is not supported yet");
            API.WriteLog("Make sure you use a /stopcasting macro and bind it in the macro section of your spellbook");

            //Combat
            CombatRoutine.AddSpell(TigerPalm, 100780, "D1");
            CombatRoutine.AddSpell(BlackoutKick, 205523, "D2");
            CombatRoutine.AddSpell(SpinningCraneKick, 101546, "D3");
            CombatRoutine.AddSpell(ExpelHarm, 322101, "D4");
            CombatRoutine.AddSpell(RisingSunKick, 107428, "D5");
            CombatRoutine.AddSpell(HealingElixir, 122281);
            CombatRoutine.AddSpell(DampenHarm, 122278);
            //heal
            CombatRoutine.AddSpell(Vivify, 116670, "NumPad1");
            CombatRoutine.AddSpell(EnvelopingMist, 124682, "NumPad2");
            CombatRoutine.AddSpell(RenewingMist, 115151, "NumPad3");
            CombatRoutine.AddSpell(EssenceFont, 191837, "NumPad4");
            CombatRoutine.AddSpell(Yulon, 322118, "NumPad5");
            CombatRoutine.AddSpell(RefreshingJadeWind, 196725, "NumPad6");
            CombatRoutine.AddSpell(SummonJadeSerpentStatue, 115313, "NumPad6");
            CombatRoutine.AddSpell(Revival, 115310, "NumPad7");
            CombatRoutine.AddSpell(SoothingMist, 115175, "NumPad8");
            CombatRoutine.AddSpell(ManaTea, 197908, "NumPad9");
            CombatRoutine.AddSpell(LifeCocoon, 116849, "F");
            CombatRoutine.AddSpell(ThunderFocusTea, 116680, "D6");
            CombatRoutine.AddSpell(ChiWave, 115098);
            CombatRoutine.AddSpell(ChiBurst, 123986);
            CombatRoutine.AddSpell(Detox, 115450);
            //Cov
            CombatRoutine.AddSpell(WeaponsofOrder, 310454, "Oem6");
            CombatRoutine.AddSpell(Fleshcraft, 324631, "OemOpenBrackets");
            CombatRoutine.AddSpell(FaelineStomp, 327104, "Oem6");
            CombatRoutine.AddSpell(FallenOrder, 326860, "Oem6");



            //Macros
            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);

            CombatRoutine.AddMacro(Player, "F1");
            CombatRoutine.AddMacro(Party1, "F2");
            CombatRoutine.AddMacro(Party2, "F3");
            CombatRoutine.AddMacro(Party3, "F4");
            CombatRoutine.AddMacro(Party4, "F5");

            CombatRoutine.AddMacro("stopcasting");
            CombatRoutine.AddMacro("raid1");
            CombatRoutine.AddMacro("raid2");
            CombatRoutine.AddMacro("raid3");
            CombatRoutine.AddMacro("raid4");
            CombatRoutine.AddMacro("raid5");
            CombatRoutine.AddMacro("raid6");
            CombatRoutine.AddMacro("raid7");
            CombatRoutine.AddMacro("raid8");
            CombatRoutine.AddMacro("raid9");
            CombatRoutine.AddMacro("raid10");
            CombatRoutine.AddMacro("raid11");
            CombatRoutine.AddMacro("raid12");
            CombatRoutine.AddMacro("raid13");
            CombatRoutine.AddMacro("raid14");
            CombatRoutine.AddMacro("raid15");
            CombatRoutine.AddMacro("raid16");
            CombatRoutine.AddMacro("raid17");
            CombatRoutine.AddMacro("raid18");
            CombatRoutine.AddMacro("raid19");
            CombatRoutine.AddMacro("raid20");
            CombatRoutine.AddMacro("raid21");
            CombatRoutine.AddMacro("raid22");
            CombatRoutine.AddMacro("raid23");
            CombatRoutine.AddMacro("raid24");
            CombatRoutine.AddMacro("raid25");
            CombatRoutine.AddMacro("raid26");
            CombatRoutine.AddMacro("raid27");
            CombatRoutine.AddMacro("raid28");
            CombatRoutine.AddMacro("raid29");
            CombatRoutine.AddMacro("raid30");
            CombatRoutine.AddMacro("raid31");
            CombatRoutine.AddMacro("raid32");
            CombatRoutine.AddMacro("raid33");
            CombatRoutine.AddMacro("raid34");
            CombatRoutine.AddMacro("raid35");
            CombatRoutine.AddMacro("raid36");
            CombatRoutine.AddMacro("raid37");
            CombatRoutine.AddMacro("raid38");
            CombatRoutine.AddMacro("raid39");
            CombatRoutine.AddMacro("raid40");

            //Buffs
            CombatRoutine.AddBuff(EnvelopingMist, 124682);
            CombatRoutine.AddBuff(RenewingMist, 119611);
            CombatRoutine.AddBuff(ThunderFocusTea, 116680);

            //Debuffs / Detox
            CombatRoutine.AddDebuff("Chilled", 328664);
            CombatRoutine.AddDebuff("Frozen Binds", 320788);
            CombatRoutine.AddDebuff("Clinging Darkness", 323347);
            CombatRoutine.AddDebuff("Rasping Scream", 324293);
            CombatRoutine.AddDebuff("Heaving Retch", 320596);
            CombatRoutine.AddDebuff("Goresplatter", 338353);
            CombatRoutine.AddDebuff("Slime Injection", 329110);
            CombatRoutine.AddDebuff("Gripping Infection", 328180);
            CombatRoutine.AddDebuff("Debilitating Plague", 324652);
            CombatRoutine.AddDebuff("Burning Strain", 322358);
            CombatRoutine.AddDebuff("Blightbeak", 327882);
            CombatRoutine.AddDebuff("Corroded Claws", 320512);
            CombatRoutine.AddDebuff("Wasting Blight", 320542);
            CombatRoutine.AddDebuff("Hurl Spores", 328002);
            CombatRoutine.AddDebuff("Corrosive Gunk", 319070);
            CombatRoutine.AddDebuff("Cytotoxic Slash", 325552);
            CombatRoutine.AddDebuff("Venompiercer", 328395);
            CombatRoutine.AddDebuff("Wretched Phlegm", 334926);
            CombatRoutine.AddDebuff("Bewildering Pollen", 323137);
            CombatRoutine.AddDebuff("Repulsive Visage", 328756);
            CombatRoutine.AddDebuff("Soul Split", 322557);
            CombatRoutine.AddDebuff("Anima Injection", 325224);
            CombatRoutine.AddDebuff("Bewildering Pollen2", 321968);
            CombatRoutine.AddDebuff("Bramblethorn Entanglement", 324859);
            CombatRoutine.AddDebuff("Debilitating Poison", 326092);
            CombatRoutine.AddDebuff("Sinlight Visions", 339237);
            CombatRoutine.AddDebuff("Siphon Life", 325701);
            CombatRoutine.AddDebuff("Turn to Stone", 326607);
            CombatRoutine.AddDebuff("Stony Veins", 326632);
            CombatRoutine.AddDebuff("Cosmic Artifice", 325725);
            CombatRoutine.AddDebuff("Wailing Grief", 340026);
            CombatRoutine.AddDebuff("Shadow Word:  Pain", 332707);
            CombatRoutine.AddDebuff("Anguished Cries", 325885);
            CombatRoutine.AddDebuff("Wrack Soul", 321038);
            CombatRoutine.AddDebuff("Dark Lance", 327481);
            CombatRoutine.AddDebuff("Insidious Venom", 323636);
            CombatRoutine.AddDebuff("Charged Anima", 338731);
            CombatRoutine.AddDebuff("Lost Confidence", 322818);
            CombatRoutine.AddDebuff("Burden of Knowledge", 317963);
            CombatRoutine.AddDebuff("Internal Strife", 327648);
            CombatRoutine.AddDebuff("Forced Confession", 328331);
            CombatRoutine.AddDebuff("Insidious Venom2", 317661);
            CombatRoutine.AddDebuff("Soul Corruption", 333708);
            CombatRoutine.AddDebuff("Genetic Alteration", 320248);
            CombatRoutine.AddDebuff("Withering Blight", 341949);
            CombatRoutine.AddDebuff("Decaying Blight", 330700);


            //Items
            CombatRoutine.AddItem(SpiritualManaPotion, 171268);


            //Toggle
            //CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddToggle("Auto Target");
            CombatRoutine.AddToggle("Auto Detox");
            CombatRoutine.AddToggle("AOE Heal");

            CombatRoutine.AddProp(SpiritualManaPotion, SpiritualManaPotion + " Mana Percent", numbList, " Life percent at which" + SpiritualManaPotion + " is used, set to 0 to disable", "General", 40);


            CombatRoutine.AddProp(ExpelHarm, ExpelHarm + " Life Percent", numbList, "Life percent at which" + ExpelHarm + "is used, set to 0 to disable", "Combat", 80);
            CombatRoutine.AddProp(DampenHarm, DampenHarm + " Life Percent", numbList, "Life percent at which" + DampenHarm + "is used, set to 0 to disable", "Combat", 80);
            CombatRoutine.AddProp(HealingElixir, HealingElixir + " Life Percent", numbList, "Life percent at which" + HealingElixir + "is used, set to 0 to disable", "Combat", 85);

            CombatRoutine.AddProp(AoE, "Number of units for AoE Healing ", numbPartyList, " Units for AoE Healing", "Healing", 3);
            CombatRoutine.AddProp(ManaTea, ManaTea + " Life Percent", numbList, "Mana percent at which" + ManaTea + "is used, set to 0 to disable", "Healing", 80);
            CombatRoutine.AddProp(EnvelopingMist, EnvelopingMist + " Life Percent", numbList, "Life percent at which" + EnvelopingMist + "is used, set to 0 to disable", "Healing", 80);
            CombatRoutine.AddProp(Vivify, Vivify + " Life Percent", numbList, "Life percent at which" + Vivify + "is used, set to 0 to disable", "Healing", 50);
            CombatRoutine.AddProp(SoothingMist, SoothingMist + " Life Percent", numbList, "Life percent at which" + SoothingMist + "is, set to 0 to disable", "Healing", 95);
            CombatRoutine.AddProp(LifeCocoon, LifeCocoon + " Life Percent", numbList, "Life percent at which" + LifeCocoon + "is, set to 0 to disable", "Healing", 50);
            CombatRoutine.AddProp(RenewingMist, RenewingMist + " Life Percent", numbList, "Life percent at which" + RenewingMist + "is used, set to 0 to disable", "Healing", 95);
            CombatRoutine.AddProp(Revival, Revival + " Life Percent", numbList, "Life percent at which" + Revival + "is used, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(EssenceFont, EssenceFont + " Life Percent", numbList, "Life percent at which" + EssenceFont + "is used when three members are at life percent, set to 0 to disable", "Healing", 85);
            CombatRoutine.AddProp(ChiBurst, ChiBurst + " Life Percent", numbList, "Life percent at which" + ChiBurst + "is used when three members are at life percent, set to 0 to disable", "Healing", 85);
            CombatRoutine.AddProp(ChiWave, ChiWave + " Life Percent", numbList, "Life percent at which" + ChiWave + "is used when three members are at life percent, set to 0 to disable", "Healing", 85);
            CombatRoutine.AddProp(RefreshingJadeWind, RefreshingJadeWind + " Life Percent", numbList, "Life percent at which" + RefreshingJadeWind + "is used when three members are at life percent, set to 0 to disable", "Healing", 85);
            CombatRoutine.AddProp(ThunderFocusTea, "Use " + ThunderFocusTea, ThunderFocusTeaList, "Use " + ThunderFocusTea + "always, Cooldowns, AOE", "Healing", 0);
            CombatRoutine.AddProp(WeaponsofOrder, "Use " + WeaponsofOrder, WeaponsofOrderList, "How to use Weapons of Order", "Covenant Kyrian", 0);
            CombatRoutine.AddProp(WeaponsofOrderAOE, WeaponsofOrderAOE + " Life Percent", numbList, "Life percent at which " + WeaponsofOrderAOE + "is used when three members are at life percent, set to 0 to disable", "Covenant Kyrian", 85);

            CombatRoutine.AddProp(Fleshcraft, Fleshcraft, numbList, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Covenant Necrolord", 5);
            CombatRoutine.AddProp(FaelineStomp, "Use " + FaelineStomp, FaelineStompList, "How to use Faeline Stomp", "Covenant Night Fae", 0);
            CombatRoutine.AddProp(FallenOrder, "Use " + FallenOrder, FallenOrderList, "How to use Fallen Order", "Covenant Venthyr", 0);

            CombatRoutine.AddProp("Trinket1", "Trinket1 usage", TrinketList1, "When should trinket1 be used", "Trinket");
            CombatRoutine.AddProp("Trinket2", "Trinket2 usage", TrinketList2, "When should trinket1 be used", "Trinket");
        }
        public override void Pulse()
        {
            if (API.CanCast(WeaponsofOrder) && WeaponsofOrderAoE && PlayerCovenantSettings == "Kyrian" && UseWeaponsofOrder == "AOE" && API.PlayerIsInCombat)
            {
                API.CastSpell(WeaponsofOrder);
                return;
            }
            if (API.CanCast(WeaponsofOrder) && PlayerCovenantSettings == "Kyrian" && UseWeaponsofOrder == "Cooldowns" && IsCooldowns && API.PlayerIsInCombat)
            {
                API.CastSpell(WeaponsofOrder);
                return;
            }
            if (API.CanCast(WeaponsofOrder) && PlayerCovenantSettings == "Kyrian" && UseWeaponsofOrder == "always" && API.PlayerIsInCombat)
            {
                API.CastSpell(WeaponsofOrder);
                return;
            }
            if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.TargetHealthPercent <= FleshcraftPercentProc)
            {
                API.CastSpell(Fleshcraft);
                return;
            }
            if (IsCooldowns && UseTrinket1 == "Cooldowns" && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                API.CastSpell(trinket1);
            if (IsCooldowns && UseTrinket2 == "Cooldowns" && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                API.CastSpell(trinket2);
            if (UseTrinket1 == "always" && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                API.CastSpell(trinket1);
            if (UseTrinket1 == "always" && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                API.CastSpell(trinket2);
            if (API.PlayerItemCanUse(SpiritualManaPotion) && API.PlayerItemRemainingCD(SpiritualManaPotion) == 0 && API.PlayerMana <= SpiritualManaPotionManaPercent)
            {
                API.CastSpell(SpiritualManaPotion);
                return;
            }
            if (API.PlayerIsInGroup)
            {
                for (int i = 0; i < units.Length; i++)
                {
                    if (API.PlayerLastSpell == (units[i]) && ChannelSoothingMist)
                    {
                        API.CastSpell("stopcasting");
                        return;
                    }
                }
            }
            if (!API.PlayerIsInCombat || !API.TargetIsIncombat)
            {
                JadeSerpentStatueWatch.Stop();
                JadeSerpentStatueWatch.Reset();
            }
            if (JadeSerpentStatueWatch.IsRunning && JadeSerpentStatueWatch.ElapsedMilliseconds >= 900000)
            {
                JadeSerpentStatueWatch.Stop();
                JadeSerpentStatueWatch.Reset();
            }
            if (API.CanCast(SummonJadeSerpentStatue) && !JadeSerpentStatueWatch.IsRunning && TalentSummonJadeSerpentStatue && NotCasting && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && API.TargetIsIncombat)
            {
                JadeSerpentStatueWatch.Start();
                API.CastSpell(SummonJadeSerpentStatue);
                return;
            }
            if (AoEHeal)
            {
                if (API.CanCast(Revival) && RevivalAoE && !API.PlayerCanAttackTarget)
                {
                    API.CastSpell(Revival);
                    return;
                }
                if (API.CanCast(RefreshingJadeWind) && TalentRefreshingJadeWind && RefreshingJadeWindAoE && !API.PlayerCanAttackTarget)
                {
                    API.CastSpell(RefreshingJadeWind);
                    return;
                }
                if (API.CanCast(EssenceFont) && EssenceFontAoE && !API.PlayerCanAttackTarget)
                {
                    API.CastSpell(EssenceFont);
                    return;
                }
            }
            if (API.CanCast(ThunderFocusTea) && !API.SpellISOnCooldown(ThunderFocusTea) && !API.PlayerHasBuff(ThunderFocusTea) && API.PlayerMana >= ManaTeaPercent && TalentManaTea && UseThunderFocusTea == "Cooldowns" && IsCooldowns && API.PlayerIsInCombat)
            {
                API.CastSpell(ThunderFocusTea);
                return;
            }
            if (API.CanCast(ThunderFocusTea) && !API.SpellISOnCooldown(ThunderFocusTea) && !API.PlayerHasBuff(ThunderFocusTea) && UseThunderFocusTea == "Cooldowns" && IsCooldowns && API.PlayerIsInCombat)
            {
                API.CastSpell(ThunderFocusTea);
                return;
            }
            if (API.CanCast(ThunderFocusTea) && !API.SpellISOnCooldown(ThunderFocusTea) && !API.PlayerHasBuff(ThunderFocusTea) && UseThunderFocusTea == "always" && API.PlayerIsInCombat)
            {
                API.CastSpell(ThunderFocusTea);
                return;
            }
            if (API.CanCast(ChiWave) && TalentChiWave && API.TargetHealthPercent <= ChiWavePercent && NotCasting && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && API.TargetIsIncombat)
            {
                API.CastSpell(ChiWave);
                return;
            }
            if (API.PlayerMana <= ManaTeaPercent && NotCasting && TalentManaTea && API.CanCast(ManaTea) && API.TargetIsIncombat)
            {
                API.CastSpell(ManaTea);
                return;
            }
            if (IsCooldowns && API.CanCast(Yulon) && !TalentInvokeChiJi && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && API.TargetIsIncombat)
            {
                API.CastSpell(Yulon);
                return;
            }
            if (API.CanCast(LifeCocoon) && API.TargetHealthPercent <= LifeCocoonPercent && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && API.TargetIsIncombat)
            {
                API.CastSpell(LifeCocoon);
                return;
            }
            if (API.CanCast(RenewingMist) && NotCasting && API.TargetHealthPercent <= RenewingMistPercent && !API.TargetHasBuff(RenewingMist) && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && API.TargetIsIncombat)
            {
                API.CastSpell(RenewingMist);
                return;
            }
            if (API.CanCast(EnvelopingMist) && ChannelSoothingMist && !API.TargetHasBuff(EnvelopingMist) && API.TargetHealthPercent <= EnvelopingMistPercent && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && API.TargetIsIncombat)
            {
                API.CastSpell(EnvelopingMist);
                return;
            }
            if (API.CanCast(Vivify) && NotCasting && API.TargetHasBuff(EnvelopingMist) && API.TargetHealthPercent <= VivifyPercent && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && API.TargetIsIncombat)
            {
                API.CastSpell(Vivify);
                if (API.PlayerCurrentCastTimeRemaining <= 0)
                {
                    ViVifyCounter++;
                }
                return;
            }
            if (API.CanCast(SoothingMist) && NotCasting && API.TargetHealthPercent <= SoothingMistPercent && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0 && API.TargetIsIncombat)
            {
                API.CastSpell(SoothingMist);
                ViVifyCounter = 0;
                return;
            }
            // Auto Target
            if (IsAutoSwap)
            {
                if (API.PlayerIsInGroup)
                {
                    for (int i = 0; i < units.Length; i++)
                        for (int j = 0; j < DetoxList.Length; j++)
                            if (IsAutoDetox)
                            {
                                if (API.CanCast(Detox))
                                {
                                    if (CanDetoxTarget(DetoxList[j], units[i]))
                                    {
                                        API.CastSpell(Detox);
                                        return;
                                    }
                                }
                                if (API.UnitHealthPercent(units[i]) <= LifeCocoonPercent && (PlayerHealth >= LifeCocoonPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                                {
                                    API.CastSpell(PlayerTargetArray[i]);
                                    return;
                                }
                                if (API.UnitHealthPercent(units[i]) <= FleshcraftPercentProc && (PlayerHealth >= FleshcraftPercentProc || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                                {
                                    API.CastSpell(PlayerTargetArray[i]);
                                    return;
                                }
                                if (API.UnitHealthPercent(units[i]) <= ChiWavePercent && (PlayerHealth >= ChiWavePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                                {
                                    API.CastSpell(PlayerTargetArray[i]);
                                    return;
                                }
                                if (API.UnitHealthPercent(units[i]) <= RenewingMistPercent && (PlayerHealth >= RenewingMistPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                                {
                                    API.CastSpell(PlayerTargetArray[i]);
                                    return;
                                }
                                if (API.UnitHealthPercent(units[i]) <= EnvelopingMistPercent && (PlayerHealth >= EnvelopingMistPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                                {
                                    API.CastSpell(PlayerTargetArray[i]);
                                    return;
                                }
                                if (API.UnitHealthPercent(units[i]) <= VivifyPercent && (PlayerHealth >= VivifyPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                                {
                                    API.CastSpell(PlayerTargetArray[i]); ;
                                    return;
                                }
                                if (API.UnitHealthPercent(units[i]) <= SoothingMistPercent && (PlayerHealth >= SoothingMistPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                                {
                                    API.CastSpell(PlayerTargetArray[i]);
                                    return;
                                }
                            }
                    if (API.PlayerIsInRaid)
                    {
                        for (int i = 0; i < raidunits.Length; i++)
                            for (int j = 0; j < DetoxList.Length; j++)
                                if (IsAutoDetox)
                                {
                                    if (API.CanCast(Detox))
                                    {
                                        if (CanDetoxTarget(DetoxList[j], units[i]))
                                        {
                                            API.CastSpell(Detox);
                                            return;
                                        }
                                    }
                                    {
                                        if (API.UnitHealthPercent(raidunits[i]) <= 15 && (PlayerHealth >= 15 || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                        {
                                            API.CastSpell(RaidTargetArray[i]);
                                            return;
                                        }
                                        if (API.UnitHealthPercent(raidunits[i]) <= LifeCocoonPercent && (PlayerHealth >= LifeCocoonPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                        {
                                            API.CastSpell(RaidTargetArray[i]);
                                            return;
                                        }
                                        if (API.UnitHealthPercent(raidunits[i]) <= FleshcraftPercentProc && (PlayerHealth >= FleshcraftPercentProc || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                        {
                                            API.CastSpell(RaidTargetArray[i]);
                                            return;
                                        }
                                        if (API.UnitHealthPercent(raidunits[i]) <= ChiWavePercent && (PlayerHealth >= ChiWavePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                        {
                                            API.CastSpell(RaidTargetArray[i]);
                                            return;
                                        }
                                        if (API.UnitHealthPercent(raidunits[i]) <= RenewingMistPercent && (PlayerHealth >= RenewingMistPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                        {
                                            API.CastSpell(RaidTargetArray[i]);
                                            return;
                                        }
                                        if (API.UnitHealthPercent(raidunits[i]) <= EnvelopingMistPercent && (PlayerHealth >= EnvelopingMistPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                        {
                                            API.CastSpell(RaidTargetArray[i]); ;
                                            return;
                                        }
                                        if (API.UnitHealthPercent(raidunits[i]) <= VivifyPercent && (PlayerHealth >= VivifyPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                        {
                                            API.CastSpell(RaidTargetArray[i]);
                                            return;
                                        }
                                        if (API.UnitHealthPercent(raidunits[i]) <= SoothingMistPercent && (PlayerHealth >= SoothingMistPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                                        {
                                            API.CastSpell(RaidTargetArray[i]);
                                            return;
                                        }
                                    }
                                }
                    }
                }
            }
        }
        public override void CombatPulse()
        {
            if (IsCooldowns && API.CanCast(FallenOrder) && IsCooldowns && PlayerCovenantSettings == "Venthyr" && UseFallenOrder == "Cooldowns")
            {
                API.CastSpell(FallenOrder);
                return;
            }
            if (API.CanCast(FallenOrder) && IsCooldowns && PlayerCovenantSettings == "Venthyr" && UseFallenOrder == "always")
            {
                API.CastSpell(FallenOrder);
                return;
            }
            if (IsAOE && API.CanCast(FallenOrder) && PlayerCovenantSettings == "Venthyr" && (UseFallenOrder == "AOE" || UseFallenOrder == "always"))
            {
                API.CastSpell(FallenOrder);
                return;
            }
            if (IsCooldowns && API.CanCast(FaelineStomp) && IsCooldowns && PlayerCovenantSettings == "Night Fae" && UseFaelineStomp == "Cooldowns")
            {
                API.CastSpell(FaelineStomp);
                return;
            }
            if (IsAOE && API.CanCast(FaelineStomp) && PlayerCovenantSettings == "Night Fae" && (UseFaelineStomp == "AOE" || UseFaelineStomp == "always"))
            {
                API.CastSpell(FaelineStomp);
                return;
            }
            if (API.CanCast(HealingElixir) && TalentHealingElixir && API.PlayerHealthPercent <= HealingElixirPercent)
            {
                API.CanCast(HealingElixir);
                return;
            }
            if (API.CanCast(DampenHarm) && TalentDampenHarm && API.PlayerHealthPercent <= DampenHarmPercent)
            {
                API.CanCast(DampenHarm);
                return;
            }
            if (API.CanCast(ExpelHarm) && API.PlayerHealthPercent <= ExpelHarmtPercent)
            {
                API.CastSpell(ExpelHarm);
                return;
            }
            if (API.CanCast(BlackoutKick))
            {
                API.CastSpell(BlackoutKick);
                return;
            }
            if (API.CanCast(RisingSunKick) && !API.SpellISOnCooldown(RisingSunKick))
            {
                API.CastSpell(RisingSunKick);
                return;
            }
            if (API.CanCast(TigerPalm))
            {
                API.CastSpell(TigerPalm);
                return;
            }
            if (IsAOE && API.PlayerUnitInMeleeRangeCount >= 2 && API.CanCast(SpinningCraneKick) && NotChanneling)
            {
                API.CastSpell(SpinningCraneKick);
                return;
            }
        }
        public override void OutOfCombatPulse()
        {
            if (AoEHeal)
            {
                if (API.CanCast(Revival) && RevivalAoE && !API.PlayerCanAttackTarget)
                {
                    API.CastSpell(Revival);
                    return;
                }
                if (API.CanCast(RefreshingJadeWind) && TalentRefreshingJadeWind && RefreshingJadeWindAoE && !API.PlayerCanAttackTarget)
                {
                    API.CastSpell(RefreshingJadeWind);
                    return;
                }
                if (API.CanCast(EssenceFont) && EssenceFontAoE && !API.PlayerCanAttackTarget)
                {
                    API.CastSpell(EssenceFont);
                    return;
                }
            }
            if (API.CanCast(ThunderFocusTea) && !API.SpellISOnCooldown(ThunderFocusTea) && !API.PlayerHasBuff(ThunderFocusTea) && API.PlayerMana >= ManaTeaPercent && TalentManaTea && UseThunderFocusTea == "Cooldowns" && IsCooldowns)
            {
                API.CastSpell(ThunderFocusTea);
                return;
            }
            if (API.CanCast(ThunderFocusTea) && !API.SpellISOnCooldown(ThunderFocusTea) && !API.PlayerHasBuff(ThunderFocusTea) && UseThunderFocusTea == "Cooldowns" && IsCooldowns)
            {
                API.CastSpell(ThunderFocusTea);
                return;
            }
            if (API.CanCast(ThunderFocusTea) && !API.SpellISOnCooldown(ThunderFocusTea) && !API.PlayerHasBuff(ThunderFocusTea) && UseThunderFocusTea == "always")
            {
                API.CastSpell(ThunderFocusTea);
                return;
            }
            if (API.CanCast(ChiWave) && TalentChiWave && API.TargetHealthPercent <= ChiWavePercent && NotCasting && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0)
            {
                API.CastSpell(ChiWave);
                return;
            }
            if (API.PlayerMana <= ManaTeaPercent && NotCasting && TalentManaTea && API.CanCast(ManaTea) && API.TargetIsIncombat)
            {
                API.CastSpell(ManaTea);
                return;
            }
            if (API.CanCast(RenewingMist) && NotCasting && API.TargetHealthPercent <= RenewingMistPercent && !API.TargetHasBuff(RenewingMist) && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0)
            {
                API.CastSpell(RenewingMist);
                return;
            }
            if (API.CanCast(EnvelopingMist) && NotCasting && !API.TargetHasBuff(EnvelopingMist) && API.TargetHealthPercent <= EnvelopingMistPercent && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0)
            {
                API.CastSpell(EnvelopingMist);
                return;
            }
            if (API.CanCast(Vivify) && NotCasting && API.TargetHasBuff(EnvelopingMist) && API.TargetHealthPercent <= VivifyPercent && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0)
            {
                API.CastSpell(Vivify);
                if (API.PlayerCurrentCastTimeRemaining <= 0)
                {
                    ViVifyCounter++;
                }
                return;
            }
            if (API.CanCast(SoothingMist) && API.TargetHealthPercent <= SoothingMistPercent && !API.PlayerCanAttackTarget && API.TargetHealthPercent > 0)
            {
                API.CastSpell(SoothingMist);
                ViVifyCounter = 0;
                return;
            }
            if (IsAutoSwap)
            {
                if (API.PlayerIsInGroup)
                {
                    for (int i = 0; i < units.Length; i++)
                    {
                        if (API.UnitHealthPercent(units[i]) <= LifeCocoonPercent && (PlayerHealth >= LifeCocoonPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                        {
                            API.CastSpell(PlayerTargetArray[i]);
                            return;
                        }
                        if (API.UnitHealthPercent(units[i]) <= FleshcraftPercentProc && (PlayerHealth >= FleshcraftPercentProc || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                        {
                            API.CastSpell(PlayerTargetArray[i]);
                            return;
                        }
                        if (API.UnitHealthPercent(units[i]) <= ChiWavePercent && (PlayerHealth >= ChiWavePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                        {
                            API.CastSpell(PlayerTargetArray[i]);
                            return;
                        }
                        if (API.UnitHealthPercent(units[i]) <= RenewingMistPercent && (PlayerHealth >= RenewingMistPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                        {
                            API.CastSpell(PlayerTargetArray[i]);
                            return;
                        }
                        if (API.UnitHealthPercent(units[i]) <= EnvelopingMistPercent && (PlayerHealth >= EnvelopingMistPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                        {
                            API.CastSpell(PlayerTargetArray[i]);
                            return;
                        }
                        if (API.UnitHealthPercent(units[i]) <= VivifyPercent && (PlayerHealth >= VivifyPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                        {
                            API.CastSpell(PlayerTargetArray[i]); ;
                            return;
                        }
                        if (API.UnitHealthPercent(units[i]) <= SoothingMistPercent && (PlayerHealth >= SoothingMistPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(units[i]) > 0)
                        {
                            API.CastSpell(PlayerTargetArray[i]);
                            return;
                        }
                    }
                    if (API.PlayerIsInRaid)
                    {
                        for (int i = 0; i < raidunits.Length; i++)
                        {
                            if (API.UnitHealthPercent(raidunits[i]) <= 15 && (PlayerHealth >= 15 || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= LifeCocoonPercent && (PlayerHealth >= LifeCocoonPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= FleshcraftPercentProc && (PlayerHealth >= FleshcraftPercentProc || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= ChiWavePercent && (PlayerHealth >= ChiWavePercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= RenewingMistPercent && (PlayerHealth >= RenewingMistPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= EnvelopingMistPercent && (PlayerHealth >= EnvelopingMistPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]); ;
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= VivifyPercent && (PlayerHealth >= VivifyPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                            if (API.UnitHealthPercent(raidunits[i]) <= SoothingMistPercent && (PlayerHealth >= SoothingMistPercent || API.PlayerCanAttackTarget) && API.UnitHealthPercent(raidunits[i]) > 0)
                            {
                                API.CastSpell(RaidTargetArray[i]);
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}