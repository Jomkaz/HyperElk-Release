﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;



namespace HyperElk.Core
{
    public class BrewmasterMonk : CombatRoutine
    {
        //Toggles

        //General
        private int PlayerLevel => API.PlayerLevel;
        private bool IsMelee => API.TargetRange < 6;
        private bool NotChanneling => !API.PlayerIsChanneling;


        //CLASS SPECIFIC
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        //CBProperties
        private int VivifyLifePercentProc => numbList[CombatRoutine.GetPropertyInt(Vivify)];
        private int ExpelHarmLifePercentProc => numbList[CombatRoutine.GetPropertyInt(ExpelHarm)];
        private int CelestialBrewLifePercentProc => numbList[CombatRoutine.GetPropertyInt(CelestialBrew)];
        private int FortifyingBrewLifePercentProc => numbList[CombatRoutine.GetPropertyInt(FortifyingBrew)];
        private int HealingElixirLifePercentProc => numbList[CombatRoutine.GetPropertyInt(HealingElixir)];
        private int ChiWaveLifePercentProc => numbList[CombatRoutine.GetPropertyInt(HealingElixir)];
        private int DampenHarmLifePercentProc => numbList[CombatRoutine.GetPropertyInt(DampenHarm)];


        string[] InvokeNiuzaoList = new string[] { "always", "with Cooldowns", "On AOE", "Manual" };
        string[] StaggerList = new string[] { "always", "Light Stagger", "Moderate Stagger", "Heavy Stagger" };
        string[] TouchofDeathList = new string[] { "always", "with Cooldowns", "Manual" };
        private string UseInvokeNiuzao => InvokeNiuzaoList[CombatRoutine.GetPropertyInt(InvokeNiuzao)];
        private string UseTouchofDeath => TouchofDeathList[CombatRoutine.GetPropertyInt(TouchofDeath)];
        private string UseStagger => StaggerList[CombatRoutine.GetPropertyInt(Stagger)];
        private int PurifyingBrewStaggerPercentProc => CombatRoutine.GetPropertyInt("PurifyingBrewStaggerPercentProc");
        //Kyrian
        private string UseWeaponsofOrder => WeaponsofOrderList[CombatRoutine.GetPropertyInt(WeaponsofOrder)];
        string[] WeaponsofOrderList = new string[] { "always", "with Cooldowns", "AOE" };
        //Necrolords
        private int FleshcraftPercentProc => numbList[CombatRoutine.GetPropertyInt(Fleshcraft)];
        string[] BonedustBrewList = new string[] { "always", "with Cooldowns", "AOE" };
        private string UseBonedustBrew => BonedustBrewList[CombatRoutine.GetPropertyInt(BonedustBrew)];
        //Nigh Fae
        string[] FaelineStompList = new string[] { "always", "with Cooldowns", "AOE" };
        private string UseFaelineStomp => FaelineStompList[CombatRoutine.GetPropertyInt(FaelineStomp)];
        //Venthyr 
        string[] FallenOrderList = new string[] { "always", "with Cooldowns", "AOE" };
        private string UseFallenOrder => FallenOrderList[CombatRoutine.GetPropertyInt(FallenOrder)];
        //Trinket1
        private string UseTrinket1 => TrinketList1[CombatRoutine.GetPropertyInt(trinket1)];
        string[] TrinketList1 = new string[] { "always", "Cooldowns", "AOE", "never" };
        //Trinket2
        private string UseTrinket2 => TrinketList2[CombatRoutine.GetPropertyInt(trinket2)];
        string[] TrinketList2 = new string[] { "always", "Cooldowns", "AOE", "never" };
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };


        private bool TalentDampenHarm => API.PlayerIsTalentSelected(5, 3);
        //Spells,Buffs,Debuffs
        private string TigerPalm = "Tiger Palm";
        private string BlackOutKick = "Blackout Kick";
        private string Vivify = "Vivify";
        private string SpinningCraneKick = "Spinning Crane Kick";
        private string ExpelHarm = "Expel Harm";
        private string SpearHandStrike = "Spear Hand Strike";
        private string PurifyingBrew = "Purifying Brew";
        private string CelestialBrew = "Celestial Brew";
        private string FortifyingBrew = "Fortifying Brew";
        private string KegSmash = "Keg Smash";
        private string BreathOfFire = "Breath of Fire";
        private string ZenMeditation = "Zen Meditation";
        private string TouchofDeath = "Touch of Death";
        private string BlackOxBrew = "Black Ox Brew";
        private string HealingElixir = "Healing Elixir";
        private string ChiBurst = "Chi Burst";
        private string RushingJadeWind = "Rushing Jade Wind";
        private string InvokeNiuzao = "Invoke Niuzao,  the Black Ox";
        private string ExplodingKeg = "Exploding Keg";
        private string Stagger = "Stagger";
        private string WeaponsofOrder = "Weapons of Order";
        private string BonedustBrew = "Bonedust Brew";
        private string Fleshcraft = "Fleshcraft";
        private string FaelineStomp = "FaelineStomp";
        private string FallenOrder = "Fallen Order";
        private string Detox = "Detox";
        private string trinket1 = "trinket1";
        private string trinket2 = "trinket2";
        private string LightStagger = "Light Stagger";
        private string ModerateStagger = "Moderate Stagger";
        private string HeavyStagger = "Heavy Stagger";
        private string ChiWave = "Chi Wave";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string DampenHarm = "Dampen Harm";
        private string LegSweep = "Leg Sweep";
        public override void Initialize()
        {
            CombatRoutine.Name = "Brewmaster Monk @Mufflon12";
            API.WriteLog("Welcome to Brewmaster Monk rotation @ Mufflon12");

            CombatRoutine.AddProp(Vivify, "Vivify", numbList, "Life percent at which " + Vivify + " is used, set to 0 to disable", "Healing", 5);
            CombatRoutine.AddProp(ChiWave, "Chi Wave", numbList, "Life percent at which " + ChiWave + " is used, set to 0 to disable", "Healing", 5);

            CombatRoutine.AddProp(ExpelHarm, "Expel Harm", numbList, "Life percent at which " + ExpelHarm + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 90);
            CombatRoutine.AddProp(CelestialBrew, "Celestial Brew", numbList, "Life percent at which " + CelestialBrew + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 50);
            CombatRoutine.AddProp(FortifyingBrew, "Fortifying Brew", numbList, "Life percent at which " + FortifyingBrew + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 40);
            CombatRoutine.AddProp(HealingElixir, "Healing Elixir", numbList, "Life percent at which " + HealingElixir + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 80);
            CombatRoutine.AddProp("PurifyingBrewStaggerPercentProc", "PurifyingBrew", 4, "Use PurifyingBrew, compared to max life. On 1000 HP 20% means Cast if stagger is higher than 200", "Stagger Management");
            CombatRoutine.AddProp(InvokeNiuzao, "Use " + InvokeNiuzao, InvokeNiuzaoList, "Use " + InvokeNiuzao + "always, with Cooldowns, On AOE", "Cooldowns", 0);
            CombatRoutine.AddProp(Stagger, "Use " + PurifyingBrew, StaggerList, "Use " + PurifyingBrew + " 2nd charge always, Light / Moderate / Heavy Stagger", "Stagger Management", 1);
            CombatRoutine.AddProp(TouchofDeath, "Use " + TouchofDeath, TouchofDeathList, "Use " + TouchofDeath + "always, with Cooldowns", "Cooldowns", 1);
            //Kyrian
            CombatRoutine.AddProp("Weapons of Order", "Use " + "Weapons of Order", WeaponsofOrderList, "How to use Weapons of Order", "Covenant Kyrian", 0);
            //Necrolords
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", numbList, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Covenant Necrolord", 5);
            CombatRoutine.AddProp(BonedustBrew, "Use " + BonedustBrew, BonedustBrewList, "How to use Bonedust Brew", "Covenant Necrolord", 0);
            //Nigh Fae
            CombatRoutine.AddProp(FaelineStomp, "Use " + FaelineStomp, FaelineStompList, "How to use Faeline Stomp", "Covenant Night Fae", 0);
            //Venthyr 
            CombatRoutine.AddProp(FallenOrder, "Use " + FallenOrder, FallenOrderList, "How to use Fallen Order", "Covenant Venthyr", 0);

            CombatRoutine.AddProp("Trinket1", "Trinket1 usage", TrinketList1, "When should trinket1 be used", "Trinket", 3);
            CombatRoutine.AddProp("Trinket2", "Trinket2 usage", TrinketList2, "When should trinket1 be used", "Trinket", 3);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(DampenHarm, "Dampen Harm", numbList, "Life percent at which " + DampenHarm + " is used, set to 0 to disable set 100 to use it everytime", "Healing", 40);



            //Spells
            CombatRoutine.AddSpell(TigerPalm, 100780,"D1");
            CombatRoutine.AddSpell(BlackOutKick, 205523,"D2");
            CombatRoutine.AddSpell(SpinningCraneKick, 101546,"D3");
            CombatRoutine.AddSpell(SpearHandStrike, 116705,"D4");
            CombatRoutine.AddSpell(BreathOfFire, 115181,"D5");
            CombatRoutine.AddSpell(KegSmash, 121253,"D6");
            CombatRoutine.AddSpell(TouchofDeath, 322109,"D7");
            CombatRoutine.AddSpell(InvokeNiuzao, 132578,"D8");
            CombatRoutine.AddSpell(ChiBurst, 123986,"D9");
            CombatRoutine.AddSpell(RushingJadeWind, 116847,"D0");
            CombatRoutine.AddSpell(ExplodingKeg, 325153,"Oem6");


            CombatRoutine.AddSpell(Vivify, 116670,"NumPad1");
            CombatRoutine.AddSpell(ChiWave, 115098,"NumPad1");

            CombatRoutine.AddSpell(ExpelHarm, 322101,"NumPad2");
            CombatRoutine.AddSpell(PurifyingBrew, 119582,"NumPad3");
            CombatRoutine.AddSpell(CelestialBrew, 322507,"NumPad4");
            CombatRoutine.AddSpell(FortifyingBrew, 115203,"NumPad5");
            CombatRoutine.AddSpell(BlackOxBrew, 115399,"NumPad6");
            CombatRoutine.AddSpell(HealingElixir, 122281,"NumPad7");
            CombatRoutine.AddSpell(Detox, 115450,"NumPad8");
            CombatRoutine.AddSpell(WeaponsofOrder, 310454,"Oem6");
            CombatRoutine.AddSpell(BonedustBrew, 325216,"Oem6");
            CombatRoutine.AddSpell(Fleshcraft, 324631,"OemOpenBrackets");
            CombatRoutine.AddSpell(FaelineStomp, 327104,"Oem6");
            CombatRoutine.AddSpell(FallenOrder, 326860,"Oem6");
            CombatRoutine.AddSpell(DampenHarm, 122278, "F1");
            CombatRoutine.AddSpell(LegSweep, 119381);
            //Macro
            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);

            //Buffs



            //Debuffs
            CombatRoutine.AddDebuff(LightStagger, 124275);
            CombatRoutine.AddDebuff(ModerateStagger, 124274);
            CombatRoutine.AddDebuff(HeavyStagger, 124273);

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);

        }

        public override void Pulse()
        {
        }
        public override void CombatPulse()
        {
            if (API.PlayerItemCanUse("Healthstone") && API.PlayerItemRemainingCD("Healthstone") == 0 && API.PlayerHealthPercent <= HealthStonePercent)
            {
                API.CastSpell("Healthstone");
                return;
            }
            //HEALING
            if (API.PlayerItemCanUse(PhialofSerenity) && API.PlayerItemRemainingCD(PhialofSerenity) == 0 && API.PlayerHealthPercent <= PhialofSerenityLifePercent)
            {
                API.CastSpell(PhialofSerenity);
                return;
            }
            if (API.PlayerItemCanUse(SpiritualHealingPotion) && API.PlayerItemRemainingCD(SpiritualHealingPotion) == 0 && API.PlayerHealthPercent <= SpiritualHealingPotionLifePercent)
            {
                API.CastSpell(SpiritualHealingPotion);
                return;
            }
            if (API.PlayerHealthPercent <= DampenHarmLifePercentProc && !API.SpellISOnCooldown(DampenHarm) && !API.PlayerIsMounted && TalentDampenHarm && NotChanneling)
            {
                API.CastSpell(DampenHarm);
                return;
            }
            //NECROLORDS FLESHCRAFT
            if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercentProc && NotChanneling)
            {
                API.CastSpell(Fleshcraft);
                return;
            }
            //Healing Elixir
            if (API.PlayerHealthPercent <= HealingElixirLifePercentProc && !API.SpellISOnCooldown(HealingElixir) && API.PlayerIsTalentSelected(5, 2) && NotChanneling)
            {
                API.CastSpell(HealingElixir);
                return;
            }
            //ChiWave
            if (API.PlayerHealthPercent <= ChiWaveLifePercentProc && !API.SpellISOnCooldown(ChiWave) && API.PlayerIsTalentSelected(1, 2) && NotChanneling)
            {
                API.CastSpell(ChiWave);
                return;
            }
            //Expel Harm
            if (API.PlayerHealthPercent <= ExpelHarmLifePercentProc && !API.SpellISOnCooldown(ExpelHarm) && !API.PlayerIsMounted && API.PlayerEnergy > 30 && PlayerLevel >= 8 && NotChanneling)
            {
                API.CastSpell(ExpelHarm);
                return;
            }
            //Celestial Brew
            if (API.PlayerHealthPercent <= CelestialBrewLifePercentProc && !API.SpellISOnCooldown(CelestialBrew) && PlayerLevel >= 27 && NotChanneling)
            {
                API.CastSpell(CelestialBrew);
                return;
            }
            //Purifying Brew
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 23 && PlayerLevel <= 47 && NotChanneling)
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Purifying Brew 2nd Charge
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 47 && API.SpellCharges(PurifyingBrew) >= 2 && NotChanneling)
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Purifying Brew 2nd Charge
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 47 && (UseStagger == "always") && NotChanneling)
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Purifying Brew 2nd Charge Light stagger
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 47 && (UseStagger == "Light Stagger") && API.PlayerHasDebuff(LightStagger) && NotChanneling)
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Purifying Brew 2nd Charge Moderate stagger
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 47 && (UseStagger == "Moderate Stagger") && API.PlayerHasDebuff(ModerateStagger) && NotChanneling)
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Purifying Brew 2nd Charge Heavy stagger
            if (!API.SpellISOnCooldown(PurifyingBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && PlayerLevel >= 47 && (UseStagger == "Heavy Stagger") && API.PlayerHasDebuff(HeavyStagger) && NotChanneling)
            {
                API.CastSpell(PurifyingBrew);
                return;
            }
            //Fortifying Brew
            if (API.PlayerHealthPercent <= FortifyingBrewLifePercentProc && !API.SpellISOnCooldown(FortifyingBrew) && PlayerLevel >= 28 && NotChanneling)
            {
                API.CastSpell(FortifyingBrew);
                return;
            }
            //Vivify
            if (API.PlayerHealthPercent <= VivifyLifePercentProc && API.CanCast(Vivify) && PlayerLevel >= 4 && NotChanneling)
            {
                API.CastSpell(Vivify);
                return;
            }
            //COOLDOWNN
            if (IsCooldowns && UseTrinket1 == "Cooldowns" && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                API.CastSpell(trinket1);
            if (IsCooldowns && UseTrinket2 == "Cooldowns" && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                API.CastSpell(trinket2);
            //Covenant Kyrian
            //WeaponsofOrder
            if (API.CanCast(WeaponsofOrder) && IsCooldowns && PlayerCovenantSettings == "Kyrian" && UseWeaponsofOrder == "with Cooldowns")
            {
                API.CastSpell(WeaponsofOrder);
                return;
            }
            //Covenant Necrolord
            //BonedustBrew
            if (API.CanCast(BonedustBrew) && IsCooldowns && PlayerCovenantSettings == "Necrolord" && UseBonedustBrew == "with Cooldowns")
            {
                API.CastSpell(BonedustBrew);
                return;
            }
            //Covenant Night Fae
            //FaelineStomp
            if (API.CanCast(FaelineStomp) && IsCooldowns && PlayerCovenantSettings == "Night Fae" && UseFaelineStomp == "with Cooldowns")
            {
                API.CastSpell(FaelineStomp);
                return;
            }
            //Covenant Venthyr
            //Fallen Order
            if (API.CanCast(FallenOrder) && IsCooldowns && PlayerCovenantSettings == "Venthyr" && UseFallenOrder == "with Cooldowns")
            {
                API.CastSpell(FallenOrder);
                return;
            }
            //BlackOxBrew
            if (API.SpellISOnCooldown(CelestialBrew) && !API.SpellISOnCooldown(BlackOxBrew) && API.PlayerStaggerPercent >= PurifyingBrewStaggerPercentProc && API.PlayerIsTalentSelected(3, 3))
            {
                API.CastSpell(BlackOxBrew);
                return;
            }
            //Touch of Death
            if (IsCooldowns && !API.SpellISOnCooldown(TouchofDeath) && API.TargetHealthPercent >= 50 && API.TargetMaxHealth < API.PlayerMaxHealth && PlayerLevel >= 10 && (UseTouchofDeath == "with Cooldowns"))
            {
                API.CastSpell(TouchofDeath);
                return;
            }
            //KICK
            if (isInterrupt && !API.SpellISOnCooldown(SpearHandStrike) && IsMelee && PlayerLevel >= 18)
            {
                API.CastSpell(SpearHandStrike);
                return;
            }
            if (isInterrupt && API.SpellISOnCooldown(SpearHandStrike) && API.CanCast(LegSweep) && IsMelee && PlayerLevel >= 18)
            {
                API.CastSpell(LegSweep);
                return;
            }
            rotation();
            return;
        }
        private void rotation()
        {
            //ROTATION AOE
            if (IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && NotChanneling && IsMelee)
            {
                if (UseTrinket1 == "AOE" || UseTrinket1 == "always" && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                    API.CastSpell(trinket1);
                if (UseTrinket2 == "AOE" || UseTrinket1 == "always" && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                    API.CastSpell(trinket2);
                //InvokeNiuzao
                if (!API.SpellISOnCooldown(InvokeNiuzao) && API.PlayerLevel >= 42 && (UseInvokeNiuzao == "always" || UseInvokeNiuzao == "On AOE" || UseInvokeNiuzao == "with Cooldowns" && IsCooldowns))
                {
                    API.CastSpell(InvokeNiuzao);
                    return;
                }
                //Covenant Kyrian
                //WeaponsofOrder
                if (API.CanCast(WeaponsofOrder) && PlayerCovenantSettings == "Kyrian" && (UseWeaponsofOrder == "AOE" || UseWeaponsofOrder == "always"))
                {
                    API.CastSpell(WeaponsofOrder);
                    return;
                }
                //Covenant Necrolord
                //BonedustBrew
                if (API.CanCast(BonedustBrew) && PlayerCovenantSettings == "Necrolord" && (UseBonedustBrew == "AOE" || UseBonedustBrew == "always"))
                {
                    API.CastSpell(BonedustBrew);
                    return;
                }
                //Covenant Night Fae
                //FaelineStomp
                if (API.CanCast(FaelineStomp) && PlayerCovenantSettings == "Night Fae" && (UseFaelineStomp == "AOE" || UseFaelineStomp == "always"))
                {
                    API.CastSpell(FaelineStomp);
                    return;
                }
                //Covenant Venthyr
                //Fallen Order
                if (API.CanCast(FallenOrder) && PlayerCovenantSettings == "Venthyr" && (UseFallenOrder == "AOE" || UseFallenOrder == "always"))
                {
                    API.CastSpell(FallenOrder);
                    return;
                }
                //KegSmash
                if (!API.SpellISOnCooldown(KegSmash) && API.PlayerEnergy > 40 && API.PlayerLevel >= 21)
                {
                    API.CastSpell(KegSmash);
                    return;
                }
                //BlackOutKick
                if (API.CanCast(BlackOutKick) && API.PlayerLevel >= 2)
                {
                    API.CastSpell(BlackOutKick);
                    return;
                }
                //Breath of Fire
                if (!API.SpellISOnCooldown(BreathOfFire) && API.PlayerLevel >= 29)
                {
                    API.CastSpell(BreathOfFire);
                    return;
                }
                //ChiBurst
                if (API.CanCast(ChiBurst) && API.PlayerIsTalentSelected(1, 3))
                {
                    API.CastSpell(ChiBurst);
                    return;
                }
                //Rushing Jade Wind
                if (API.CanCast(RushingJadeWind) && !API.SpellISOnCooldown(RushingJadeWind) && API.PlayerIsTalentSelected(6, 2))
                {
                    API.CastSpell(RushingJadeWind);
                    return;
                }
                //Exploping Kek
                if (API.CanCast(ExplodingKeg) && API.PlayerIsTalentSelected(6, 3))
                {
                    API.CastSpell(ExplodingKeg);
                    return;
                }
                // Sinning Crane Kick
                if (API.CanCast(SpinningCraneKick) && API.PlayerEnergy >= 50)
                {
                    API.CastSpell(SpinningCraneKick);
                    return;
                }
                //Tiger Palm -> nothing else to do 
                if (API.CanCast(TigerPalm) && API.PlayerEnergy >= 40)
                {
                    API.CastSpell(TigerPalm);
                    return;
                }
            }
            //ROTATION  SINGLE TARGET
            if ((API.PlayerUnitInMeleeRangeCount <= AOEUnitNumber || !IsAOE) && NotChanneling && IsMelee)
            {
                if (UseTrinket1 == "always" && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                    API.CastSpell(trinket1);
                if (UseTrinket1 == "always" && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                    API.CastSpell(trinket2);
                //Covenant Kyrian
                //WeaponsofOrder
                if (API.CanCast(WeaponsofOrder) && PlayerCovenantSettings == "Kyrian" && UseWeaponsofOrder == "always")
                {
                    API.CastSpell(WeaponsofOrder);
                    return;
                }
                //Covenant Necrolord
                //BonedustBrew
                if (API.CanCast(BonedustBrew) && PlayerCovenantSettings == "Necrolord" && UseBonedustBrew == "always")
                {
                    API.CastSpell(BonedustBrew);
                    return;
                }
                //Covenant Night Fae
                //FaelineStomp
                if (API.CanCast(FaelineStomp) && PlayerCovenantSettings == "Night Fae" && UseFaelineStomp == "always")
                {
                    API.CastSpell(FaelineStomp);
                    return;
                }
                //Covenant Venthyr
                //Fallen Order
                if (API.CanCast(FallenOrder) && PlayerCovenantSettings == "Venthyr" && UseFaelineStomp == "always")
                {
                    API.CastSpell(FallenOrder);
                    return;
                }
                //Touch of Death
                if (!API.SpellISOnCooldown(TouchofDeath) && API.TargetHealthPercent >= 0 && API.TargetMaxHealth < API.PlayerMaxHealth && PlayerLevel >= 10 && (UseTouchofDeath == "always"))
                {
                    API.CastSpell(TouchofDeath);
                    return;
                }
                //InvokeNiuzao
                if (!API.SpellISOnCooldown(InvokeNiuzao) && API.PlayerLevel >= 42 && (UseInvokeNiuzao == "always" || UseInvokeNiuzao == "with Cooldowns" && IsCooldowns))
                {
                    API.CastSpell(InvokeNiuzao);
                    return;
                }
                //Keg Smash
                if (!API.SpellISOnCooldown(KegSmash) && API.PlayerEnergy > 40 && API.PlayerLevel >= 21)
                {
                    API.CastSpell(KegSmash);
                    return;
                }
                //Blackout Kick
                if (API.CanCast(BlackOutKick) && API.PlayerLevel >= 2)
                {
                    API.CastSpell(BlackOutKick);
                    return;
                }
                //Breath of Fire
                if (!API.SpellISOnCooldown(BreathOfFire) && API.PlayerLevel >= 29)
                {
                    API.CastSpell(BreathOfFire);
                    return;
                }
                //ChiBurst
                if (API.CanCast(ChiBurst) && API.PlayerIsTalentSelected(1, 3))
                {
                    API.CastSpell(ChiBurst);
                    return;
                }
                //Tiger Palm -> nothing else to do 
                if (API.CanCast(TigerPalm) && API.PlayerEnergy >= 40)
                {
                    API.CastSpell(TigerPalm);
                    return;
                }
            }
         }
        public override void OutOfCombatPulse()
        {

            //Vivify
            if (API.PlayerHealthPercent <= VivifyLifePercentProc && API.CanCast(Vivify) && PlayerLevel >= 4)
            {
                API.CastSpell(Vivify);
                return;
            }
        }
    }
}
