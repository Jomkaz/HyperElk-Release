using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;



// Changelog
// v1.0 First release

namespace HyperElk.Core
{
    public class AfflictionWarlock : CombatRoutine
    {
        //Spells,Buffs,Debuffs
        private string ShadowBolt = "Shadow Bolt";
        private string Corruption = "Corruption";
        private string DrainLife = "Drain Life";
        private string Agony = "Agony";
        private string HealthFunnel = "Health Funnel";
        private string SummonImp = "Summon Imp";
        private string SummonVoidwalker = "Summon Voidwalker";
        private string MaleficRapture = "Malefic Rapture";
        private string UnstableAffliction = "Unstable Affliction";
        private string SeedofCorruption = "Seed of Corruption";
        private string DrainSoul = "Drain Soul";
        private string SummonSuccubus = "Summon Succubus";
        private string SiphonLife = "Siphon Life";
        private string DarkPact = "Dark Pact";
        private string PhantomSingularity = "Phantom Singularity";
        private string VileTaint = "Vile Taint";
        private string SummonFelhunter = "Summon Felhunter";
        private string SummonDarkglare = "Summon Darkglare";
        private string Haunt = "Haunt";
        private string DarkSoulMisery = "Dark Soul:  Misery";
        private string GrimoireOfSacrifice = "Grimoire Of Sacrifice";
        private string ScouringTithe = "Scouring Tithe";
        private string Misdirection = "Misdirection";
        private string CovenantAbility = "Covenant Ability";
        private string SoulRot = "Soul Rot";
        private string ImpendingCatastrophe = "Impending Catastrophe";
        private string trinket1 = "trinket1";
        private string trinket2 = "trinket2";
        private string MortalCoil = "Mortal Coil";
        private string DecimatingBolt = "Decimating Bolt";
        private string ShadowEmbrace = "Shadow Embrace";
        private string PhialofSerenity = "Phial of Serenity";
        private string SpiritualHealingPotion = "Spiritual Healing Potion";
        private string PotionofPhantomFire = "Potion of Phantom Fire";
        private string PotionofSpectralIntellect = "Potion of Spectral Intellect";
        private string FelDomination = "Fel Domination";

        //Talents
        private bool TalentDrainSoul => API.PlayerIsTalentSelected(1, 3);
        private bool TalentSiphonLife => API.PlayerIsTalentSelected(2, 3);
        private bool TalentDarkPact => API.PlayerIsTalentSelected(3, 3);
        private bool TalentPhantomSingularity => API.PlayerIsTalentSelected(4, 2);
        private bool TalentVileTaint => API.PlayerIsTalentSelected(4, 3);
        private bool TalentHaunt => API.PlayerIsTalentSelected(6, 2);
        private bool TalentGrimoireOfSacrifice => API.PlayerIsTalentSelected(6, 3);
        private bool TalentDarkSoulMisery => API.PlayerIsTalentSelected(7, 3);
        private bool TalentMortalCoil => API.PlayerIsTalentSelected(5, 2);
        private bool TalentSowTheSeed => API.PlayerIsTalentSelected(4, 1);

        //Conduit
        private string CorruptingLeer = "Corrupting Leer";


        //Misc
        private static readonly Stopwatch DumpWatchLow = new Stopwatch();
        private static readonly Stopwatch DumpWatchHigh = new Stopwatch();


        private bool IsRange => API.TargetRange < 40;
        private int PlayerLevel => API.PlayerLevel;
        private bool NotMoving => !API.PlayerIsMoving;
        //        private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private int ShoulShardNumberMaleficRapture => CombatRoutine.GetPropertyInt("SoulShardNumberMaleficRapture");
        private int ShoulShardNumberDrainSoul => CombatRoutine.GetPropertyInt("SoulShardNumberDrainSoul");
        private int Trinket1Usage => CombatRoutine.GetPropertyInt("Trinket1");
        private int Trinket2Usage => CombatRoutine.GetPropertyInt("Trinket2");




        //CBProperties      
        bool DotCheck => API.TargetHasDebuff(Corruption) && API.TargetHasDebuff(Agony) && API.TargetHasDebuff(UnstableAffliction) && (API.TargetHasDebuff(SoulRot) || !API.TargetHasDebuff(SoulRot));
        bool CastingSOC => API.PlayerLastSpell == SeedofCorruption;
        bool CastingSOC1 => API.LastSpellCastInGame == SeedofCorruption;
        bool LastCastUnstableAffliction => API.LastSpellCastInGame == UnstableAffliction || API.LastSpellCastInGame == UnstableAffliction;
        bool CurrenCastUnstableAffliction => API.CurrentCastSpellID("player") == 316099;
        bool LastCastScouringTithe => API.LastSpellCastInGame == ScouringTithe || API.LastSpellCastInGame == ScouringTithe;
        bool CastingAgony => API.PlayerLastSpell == Agony || API.LastSpellCastInGame == Agony;
        bool CastingCorruption => API.PlayerLastSpell == Corruption || API.LastSpellCastInGame == Corruption;
        bool CastingSL => API.PlayerLastSpell == SiphonLife || API.LastSpellCastInGame == SiphonLife;
        bool LastSeed => API.CurrentCastSpellID("player") == 27243;
        bool IsPotion1 => (UsePotion1 == "with Cooldowns" && IsCooldowns || UsePotion1 == "always" || UsePotion1 == "on AOE" && IsAOE);
        bool IsPotion2 => (UsePotion2 == "with Cooldowns" && IsCooldowns || UsePotion2 == "always" || UsePotion2 == "on AOE" && API.TargetUnitInRangeCount >= 2 && IsAOE);


        bool LastUnstableAffliction => API.PlayerLastSpell == UnstableAffliction;
        //Trinket1
        private string UseTrinket1 => TrinketList1[CombatRoutine.GetPropertyInt(trinket1)];
        string[] TrinketList1 = new string[] { "always", "Cooldowns", "AOE", "never" };
        //Trinket2
        private string UseTrinket2 => TrinketList2[CombatRoutine.GetPropertyInt(trinket2)];
        string[] TrinketList2 = new string[] { "always", "Cooldowns", "AOE", "never" };
        //Potion of Phantom Fire
        private string PotionofPhantomFire => UsePotion1[CombatRoutine.GetPropertyInt(PotionofPhantomFire)];
        string[] UsePotion1 = new string[] { "always", "Cooldowns", "AOE", "never" };
        //Potion of Spectral Intellect
        private string PotionofSpectralIntellect => UsePotion2[CombatRoutine.GetPropertyInt(PotionofSpectralIntellect)];
        string[] UsePotion2 = new string[] { "always", "Cooldowns", "AOE", "never" };
        
        int[] numbList = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        private int PhialofSerenityLifePercent => numbList[CombatRoutine.GetPropertyInt(PhialofSerenity)];
        private int SpiritualHealingPotionLifePercent => numbList[CombatRoutine.GetPropertyInt(SpiritualHealingPotion)];
        private int DrainLifePercentProc => numbList[CombatRoutine.GetPropertyInt(DrainLife)];
        private int MortalCoilPercentProc => numbList[CombatRoutine.GetPropertyInt(DrainLife)];
        private int HealthFunnelPercentProc => numbList[CombatRoutine.GetPropertyInt(HealthFunnel)];

        string[] MisdirectionList = new string[] { "None", "Imp", "Voidwalker", "Succubus", "Felhunter", };
        private string isMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        private bool UseUA => (bool)CombatRoutine.GetProperty("UseUA");
        private bool UseAG => (bool)CombatRoutine.GetProperty("UseAG");
        private bool UseCO => (bool)CombatRoutine.GetProperty("UseCO");
        private bool UseSL => (bool)CombatRoutine.GetProperty("UseSL");
        private bool DumpShards => (bool)CombatRoutine.GetProperty("DumpShards");

        private int DarkPactPercentProc => numbList[CombatRoutine.GetPropertyInt(DarkPact)];
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private string UseCovenantAbility => CovenantAbilityList[CombatRoutine.GetPropertyInt(CovenantAbility)];

        string[] CovenantAbilityList = new string[] { "None", "always", "with Cooldowns", "AOE" };


        public override void Initialize()
        {
            CombatRoutine.Name = "Affliction Warlock Jom";
            API.WriteLog("Welcome to Affliction Warlock rotation By Jom");
            API.WriteLog("Version 1.0 - Beta - ");
            API.WriteLog("Thanks @Mufflon12 for Laying the Base Code.");
            API.WriteLog("Create the following mouseover macro and assigned to the bind:");
            API.WriteLog("/cast [@mouseover] Agony");
            API.WriteLog("/cast [@mouseover] Corruption");
            API.WriteLog("/cast [@cursor] Vile Taint");
            API.WriteLog("--------------------------------------------------------------------------------------------------------------------------");
            API.WriteLog("Put a Meele Pet Ability on your Action Bar for the AOE Detection");


            CombatRoutine.AddProp(DrainLife, "Drain Life", numbList, "Life percent at which " + DrainLife + " is used, set to 0 to disable", "Healing", 5);
            CombatRoutine.AddProp(MortalCoil, "Mortal Coil", numbList, "Life percent at which " + MortalCoil + " is used, set to 0 to disable", "Healing", 5);

            CombatRoutine.AddProp(HealthFunnel, "Health Funnel", numbList, "Life percent at which " + HealthFunnel + " is used, set to 0 to disable", "PETS", 0);
            CombatRoutine.AddProp(Misdirection, "Wich Pet", MisdirectionList, "Chose your Pet", "PETS", 0);
            CombatRoutine.AddProp("UseUA", "Use Unstable Affliction", true, "Should the rotation use Unstable Affliction", "Class specific");
            CombatRoutine.AddProp(DarkPact, "Dark Pact", numbList, "Life percent at which " + DarkPact + " is used, set to 0 to disable", "Healing", 2);
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddProp("SoulShardNumberMaleficRapture", "Soul Shards Malefic Rapture", 2, "How many Soul Shards to use Malefic Rapture", "Class specific");
            CombatRoutine.AddProp("SoulShardNumberDrainSoul", "Soul Shards Drain Shoul", 1, "How many Soul Shards to use Drain Shoul", "Class specific");
            CombatRoutine.AddProp("MouseoverInCombat", "Only Mouseover in combat", false, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");
            CombatRoutine.AddProp("UseAG", "Use Agony", true, "Use Agony for mouseover Multidots", "MultiDOTS");
            CombatRoutine.AddProp("UseCO", "Use Corruption", true, "Use Corruption for mouseover Multidots", "MultiDOTS");
            CombatRoutine.AddProp("UseSL", "Use Siphon Life", true, "Use Siphon Life for mouseover Multidots", "MultiDOTS");
            CombatRoutine.AddProp("Covenant Ability", "Use " + "Covenant Ability", CovenantAbilityList, "How to use Covenant Spell", "Covenant", 0);
            CombatRoutine.AddProp("Trinket1", "Trinket1 usage", TrinketList1, "When should trinket1 be used", "Trinket", 3);
            CombatRoutine.AddProp("Trinket2", "Trinket2 usage", TrinketList2, "When should trinket1 be used", "Trinket", 3);
            CombatRoutine.AddProp("DumpShards", "Dump Shards", true, "Collect 5 Soul Shards and befor using Malefic Rapture", "Class specific");
            CombatRoutine.AddProp(PhialofSerenity, PhialofSerenity + " Life Percent", numbList, " Life percent at which" + PhialofSerenity + " is used, set to 0 to disable", "ITEMS", 40);
            CombatRoutine.AddProp(SpiritualHealingPotion, SpiritualHealingPotion + " Life Percent", numbList, " Life percent at which" + SpiritualHealingPotion + " is used, set to 0 to disable", "ITEMS", 40);
            CombatRoutine.AddProp("PotionofPhantomFire", "Use " + "Potion of Phantom Fire", CDUsageWithAOE, "Use " + "Potion of Phantom Fire" + " always, with Cooldowns", "ITEMS", 0);
            CombatRoutine.AddProp("PotionofSpectralIntellect", "Use " + "Potion of Spectral Intellect", CDUsageWithAOE, "Use " + "Potion of Spectral Intellect" + " always, with Cooldowns", "ITEMS", 0);
            
            //Spells
            CombatRoutine.AddSpell(ShadowBolt, 686,"D1");
            CombatRoutine.AddSpell(DrainSoul, 198590,"D1");
            CombatRoutine.AddSpell(Corruption, 172,"D2");
            CombatRoutine.AddSpell(Agony, 980,"D3");
            CombatRoutine.AddSpell(MaleficRapture, 324536,"D4");
            CombatRoutine.AddSpell(UnstableAffliction, 316099,"D5");
            CombatRoutine.AddSpell(SeedofCorruption, 27243,"D6");
            CombatRoutine.AddSpell(SiphonLife, 63106,"D7");
            CombatRoutine.AddSpell(PhantomSingularity, 205179,"D8");
            CombatRoutine.AddSpell(Haunt, 48181,"D8");
            CombatRoutine.AddSpell(DarkSoulMisery, 113860,"D8");
            CombatRoutine.AddSpell(MortalCoil, 6789,"D9");
            CombatRoutine.AddSpell(VileTaint, 278350,"D8");
            CombatRoutine.AddSpell(ScouringTithe, 312321,"F1");
            CombatRoutine.AddSpell(SoulRot, 325640,"F1");
            CombatRoutine.AddSpell(ImpendingCatastrophe, 321792,"F1");
            CombatRoutine.AddSpell(DecimatingBolt, 325289,"F1");
            CombatRoutine.AddSpell(FelDomination, 333889);

            //Macro
            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);
            CombatRoutine.AddMacro(Agony + "MO", "F1");
            CombatRoutine.AddMacro(Corruption + "MO", "F2");
            CombatRoutine.AddMacro(SiphonLife + "MO", "F3");

            //
            CombatRoutine.AddSpell("Drain Life", 234153,"NumPad1");
            CombatRoutine.AddSpell("Health Funnel", 755,"NumPad2");
            CombatRoutine.AddSpell("Dark Pact", 108416,"NumPad3");
            CombatRoutine.AddSpell("Grimoire Of Sacrifice", 108503,"NumPad4");
            CombatRoutine.AddSpell("Summon Darkglare", 205180,"NumPad5");
            CombatRoutine.AddSpell("Summon Felhunter", 691,"NumPad6");
            CombatRoutine.AddSpell("Summon Succubus", 712,"NumPad7");
            CombatRoutine.AddSpell("Summon Voidwalker", 697,"NumPad8");
            CombatRoutine.AddSpell("Summon Imp", 688,"NumPad9");

            //Buffs
            CombatRoutine.AddBuff("Grimoire Of Sacrifice", 108503);
            CombatRoutine.AddBuff(FelDomination, 333889);

            //Conduit
            CombatRoutine.AddConduit(CorruptingLeer);

            //Debuffs
            CombatRoutine.AddDebuff(ImpendingCatastrophe, 321792);
            CombatRoutine.AddDebuff(Corruption, 146739);
            CombatRoutine.AddDebuff(Agony, 980);
            CombatRoutine.AddDebuff(UnstableAffliction, 316099);
            CombatRoutine.AddDebuff(SiphonLife, 63106);
            CombatRoutine.AddDebuff(SeedofCorruption, 27243);
            CombatRoutine.AddDebuff(VileTaint, 278350);
            CombatRoutine.AddDebuff(PhantomSingularity, 205179);
            CombatRoutine.AddDebuff(Haunt, 48181);
            CombatRoutine.AddDebuff(SoulRot, 325640);
            CombatRoutine.AddDebuff(ShadowEmbrace, 32390);

            //Item
            CombatRoutine.AddItem(PhialofSerenity, 177278);
            CombatRoutine.AddItem(SpiritualHealingPotion, 171267);
            CombatRoutine.AddItem(PotionofPhantomFire, 171349);
            CombatRoutine.AddItem(PotionofSpectralIntellect, 171273);

        }


        public override void Pulse()
        {
            //Summon Imp
            if (API.PlayerHasBuff(FelDomination) && API.PlayerIsInCombat && !TalentGrimoireOfSacrifice && API.CanCast(SummonImp)  && !API.PlayerHasPet && (isMisdirection == "Imp") && NotMoving && PlayerLevel >= 22)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonImp);
                return;
            }
            //Summon Voidwalker
            if (API.PlayerHasBuff(FelDomination) && API.PlayerIsInCombat && !TalentGrimoireOfSacrifice && API.CanCast(SummonVoidwalker)  && !API.PlayerHasPet && (isMisdirection == "Voidwalker") && NotMoving && PlayerLevel >= 22)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonVoidwalker);
                return;
            }
            //Summon Succubus
            if (API.PlayerHasBuff(FelDomination) && API.PlayerIsInCombat && !TalentGrimoireOfSacrifice && API.CanCast(SummonSuccubus) && !API.PlayerHasPet && (isMisdirection == "Succubus") && NotMoving && PlayerLevel >= 22)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonSuccubus);
                return;
            }
            //Summon Fellhunter
            if (API.PlayerHasBuff(FelDomination) && API.PlayerIsInCombat && !TalentGrimoireOfSacrifice && API.CanCast(SummonFelhunter) && !API.PlayerHasPet && (isMisdirection == "Felhunter") && NotMoving && PlayerLevel >= 23)
            {
                API.WriteLog("We are in Combat , use Fel Domination summon");
                API.CastSpell(SummonFelhunter);
                return;
            }
            if (!API.PlayerHasPet && API.PlayerIsInCombat && API.CanCast(FelDomination))
            {
                API.CastSpell(FelDomination);
                return;
            }
        }
        public override void CombatPulse()
        {
            if (IsMouseover)
            {
                if (UseCO)
                {
                    if (!CastingCorruption && API.CanCast(Corruption) && !API.MacroIsIgnored(Corruption + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(Corruption) <= 400 && !API.TargetHasDebuff(SeedofCorruption) && IsRange && PlayerLevel >= 2)
                    {
                        API.CastSpell(Corruption + "MO");
                        return;
                    }
                }
                if (UseAG)
                {
                    if (!CastingAgony && API.CanCast(Agony) && !API.MacroIsIgnored(Agony + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(Agony) <= 400 && IsRange && PlayerLevel >= 10)
                    {
                        API.CastSpell(Agony + "MO");
                        return;
                    }
                }
                if (UseSL)
                {
                    if (!CastingSL && API.CanCast(SiphonLife) && !API.MacroIsIgnored(SiphonLife + "MO") && API.PlayerCanAttackMouseover && TalentSiphonLife && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(SiphonLife) <= 400 && IsRange && PlayerLevel >= 10)
                    {
                        API.CastSpell(SiphonLife + "MO");
                        return;
                    }
                }
            }
            rotation();
            return;
        }


        private void rotation()
        {
            //#########################
            //##### MAIN ROTATION #####
            //#########################
            //Executed every time the actor is available.
            //actions=call_action_list,name=aoe,if=active_enemies>3
            if (IsAOE && API.TargetUnitInRangeCount >= 3)
                {
                    multi();
                }
            //actions+=/phantom_singularity,if=time>30
            if (API.CanCast(PhantomSingularity) && API.TargetTimeToDie > 3000)
                {
                    API.CastSpell(PhantomSingularity);
                    return;
                }
            //actions+=/call_action_list,name=darkglare_prep,if=covenant.venthyr&dot.impending_catastrophe_dot.ticking&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
            if (PlayerCovenantSettings == "Venthyr" && API.TargetHasDebuff(ImpendingCatastrophe) && API.SpellCDDuration(SummonDarkglare) < 200 && ( API.TargetDebuffRemainingTime(PhantomSingularity) >= 200 || !TalentPhantomSingularity)
                {
                    darkglare_prep();
                }
            //actions+=/call_action_list,name=darkglare_prep,if=covenant.night_fae&dot.soul_rot.ticking&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
            if (PlayerCovenantSettings == "Night Fae" && API.TargetHasDebuff(SoulRot) && API.SpellCDDuration(SummonDarkglare) < 200 && ( API.TargetDebuffRemainingTime(PhantomSingularity) >= 200 || !TalentPhantomSingularity)
                {
                    darkglare_prep();
                }
            //actions+=/call_action_list,name=darkglare_prep,if=(covenant.necrolord|covenant.kyrian|covenant.none)&dot.phantom_singularity.ticking&dot.phantom_singularity.remains<2
            if ((PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None") && API.TargetHasDebuff(PhantomSingularity) && API.TargetDebuffRemainingTime(PhantomSingularity) >= 200)
                {
                    darkglare_prep();
                }
            //actions+=/agony,if=dot.agony.remains<4
            if (API.CanCast(Agony) && API.TargetDebuffRemainingTime(Agony) <= 400 && IsRange && PlayerLevel >= 10)
                {
                    API.CastSpell(Agony);
                    return;
                }
            //actions+=/agony,cycle_targets=1,if=active_enemies>1,target_if=dot.agony.remains<4
            if (API.CanCast(Agony) && API.TargetUnitInRangeCount >= 1 && !API.MacroIsIgnored(Agony + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(Agony) <= 400 && IsRange && PlayerLevel >= 10)
                {
                    API.CastSpell(Agony + "MO");
                    return;
                }
            //actions+=/haunt
            if (API.CanCast(Haunt) && !API.SpellISOnCooldown(Haunt) && TalentHaunt && PlayerLevel >= 45)
                {
                    API.CastSpell(Haunt);
                    return;
                }
            //actions+=/call_action_list,name=darkglare_prep,if=active_enemies>2&covenant.venthyr&(cooldown.impending_catastrophe.ready|dot.impending_catastrophe_dot.ticking)&(dot.phantom_singularity.ticking|!talent.phantom_singularity.enabled)
            if (API.TargetUnitInRangeCount >= 2 && PlayerCovenantSettings == "Venthyr" && (API.CanCast(ImpendingCatastrophe) || API.TargetHasDebuff(ImpendingCatastrophe) && ( API.TargetHasDebuff(PhantomSingularity) || !TalentPhantomSingularity)
                {
                    darkglare_prep();
                }
            //actions+=/call_action_list,name=darkglare_prep,if=active_enemies>2&(covenant.necrolord|covenant.kyrian|covenant.none)&(dot.phantom_singularity.ticking|!talent.phantom_singularity.enabled)
            if (API.TargetUnitInRangeCount >= 2 && (PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None") && ( API.TargetHasDebuff(PhantomSingularity) || !TalentPhantomSingularity)
                {
                    darkglare_prep();
                }
            //actions+=/call_action_list,name=darkglare_prep,if=active_enemies>2&covenant.night_fae&(cooldown.soul_rot.ready|dot.soul_rot.ticking)&(dot.phantom_singularity.ticking|!talent.phantom_singularity.enabled)
            if (API.TargetUnitInRangeCount >= 2 && PlayerCovenantSettings == "Night Fae" && (API.CanCast(SoulRot) || API.TargetHasDebuff(SoulRot) && ( API.TargetHasDebuff(PhantomSingularity) || !TalentPhantomSingularity)
                {
                    darkglare_prep();
                }
            //actions+=/seed_of_corruption,if=active_enemies>2&talent.sow_the_seeds.enabled&!dot.seed_of_corruption.ticking&!in_flight
            if (API.CanCast(SeedofCorruption) && API.TargetUnitInRangeCount > 2 && TalentSowTheSeed && !API.TargetHasDebuff(SeedofCorruption) && !PlayerIsMoving)
                {
                    Api.CastSpell(SeedofCorruption);
                    return;
                }
            //actions+=/seed_of_corruption,if=active_enemies>2&talent.siphon_life.enabled&!dot.seed_of_corruption.ticking&!in_flight&dot.corruption.remains<4
            if (API.CanCast(SeedofCorruption) && API.TargetUnitInRangeCount > 2 && TalentSiphonLife && !API.TargetHasDebuff(SeedofCorruption) && !PlayerIsMoving && API.TargetDebuffRemainingTime(Corruption) < 4aoe)
                {
                    Api.CastSpell(SeedofCorruption);
                    return;
                }
            //actions+=/vile_taint,if=(soul_shard>1|active_enemies>2)&cooldown.summon_darkglare.remains>12
            if (API.CanCast(VileTaint && (PlayerCurrentSoulShards > 1 || API.TargetUnitInRangeCount >2) && Api.SpellCDDuration(SummonDarkglare) > 12 && !PlayerIsMoving )
                {
                    API.CastSpell(VileTaint);
                    return;
                }
            //actions+=/unstable_affliction,if=dot.unstable_affliction.remains<4
            if (API.CanCast(UnstableAffliction) && API.TargetDebuffRemainingTime(UnstableAffliction) <= 400 && !PlayerIsMoving && IsRange && NotChanneling && PlayerLevel >= 13)
                {
                    API.CastSpell(UnstableAffliction);
                    return;
                }
            //actions+=/siphon_life,if=dot.siphon_life.remains<4
            if (API.CanCast(SiphonLife) && API.TargetDebuffRemainingTime(SiphonLife) <= 400 && NotMoving)
                {
                    API.CastSpell(SiphonLife);
                    return;
                }
            //actions+=/siphon_life,cycle_targets=1,if=active_enemies>1,target_if=dot.siphon_life.remains<4
            if (API.CanCast(SiphonLife) && API.TargetUnitInRangeCount >= 1 && !API.MacroIsIgnored(SiphonLife + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(SiphonLife) <= 400 && IsRange && PlayerLevel >= 25)
                {
                    API.CastSpell(SiphonLife + "MO");
                    return;
                }
            //actions+=/call_action_list,name=covenant,if=!covenant.necrolord
            if (!PlayerCovenantSettings == "Necrolord")
                {
                    Covenant();
                    return;
                }
            //actions+=/corruption,if=active_enemies<4-(talent.sow_the_seeds.enabled|talent.siphon_life.enabled)&dot.corruption.remains<2
            if (API.CanCast(Corruption) && API.TargetUnitInRangeCount < 4 && (TalentSowTheSeed || TalentSiphonLife) && API.TargetDebuffRemainingTime(Corruption) <= 200 && IsRange && PlayerLevel >= 2)
                {
                    API.CastSpell(Corruption);
                    return;
                }
            //actions+=/corruption,cycle_targets=1,if=active_enemies<4-(talent.sow_the_seeds.enabled|talent.siphon_life.enabled),target_if=dot.corruption.remains<2
            if (API.CanCast(Corruption) && API.TargetUnitInRangeCount < 4 && (TalentSowTheSeed || TalentSiphonLife) && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(Corruption) <= 200 && IsRange && PlayerLevel >= 2)
                {
                    API.CastSpell(Corruption + "MO");
                    return;
                }
            //actions+=/phantom_singularity,if=covenant.necrolord|covenant.night_fae|covenant.kyrian|covenant.none
            if (API.CanCast(PhantomSingularity) && (PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Night Fae" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None"))
                {
                    API.CastSpell(PhantomSingularity);
                    return;
                }
            //actions+=/malefic_rapture,if=soul_shard>4
            if (API.CanCast(MaleficRapture) && PlayerCurrentSoulShards > 4)
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
            //actions+=/call_action_list,name=darkglare_prep,if=covenant.venthyr&(cooldown.impending_catastrophe.ready|dot.impending_catastrophe_dot.ticking)&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
            if (PlayerCovenantSettings == "Venthyr" && (API.CanCast(ImpendingCatastrophe) || API.TargetHasDebuff(ImpendingCatastrophe) && API.SpellCDDuration(SummonDarkglare) <= 200 && (API.TargetDebuffRemainingTime(PhantomSingularity) >= 200 || !TalentPhantomSingularity))
                {
                    darkglare_prep();
                    return;
                }
            //actions+=/call_action_list,name=darkglare_prep,if=(covenant.necrolord|covenant.kyrian|covenant.none)&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
            if ((PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None") && API.SpellCDDuration(SummonDarkglare) <= 200 && (API.TargetDebuffRemainingTime(PhantomSingularity) >= 200 || !TalentPhantomSingularity))
                {
                    darkglare_prep();
                    return;
                }
            //actions+=/call_action_list,name=darkglare_prep,if=covenant.night_fae&(cooldown.soul_rot.ready|dot.soul_rot.ticking)&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
            if (PlayerCovenantSettings == "Night Fae" && (API.CanCast(SoulRot) || API.TargetHasDebuff(SoulRot) && API.SpellCDDuration(SummonDarkglare) <= 200 && (API.TargetDebuffRemainingTime(PhantomSingularity) >= 200 || !TalentPhantomSingularity))
                {
                    darkglare_prep();
                    return;
                }
            //actions+=/dark_soul,if=cooldown.summon_darkglare.remains>time_to_die
            if (API.SpellCDDuration(SummonDarkglare) >= TargetTimeToDie && IsCooldowns)
                {
                    API.CastSpell(DarkSoulMisery);
                    return;
                }
            //actions+=/call_action_list,name=item
            if (IsCooldowns)
                {
                    item();
                    return:
                }
            //actions+=/call_action_list,name=se,if=debuff.shadow_embrace.stack<(2-action.shadow_bolt.in_flight)|debuff.shadow_embrace.remains<3
            if (API.TargetDebuffStacks <= 2 || API.TargetDebuffRemainingTime <= 300)
                {
                    se();
                    return;
                }
            //actions+=/malefic_rapture,if=dot.vile_taint.ticking
            if (API.TargetHasDebuff(VileTaint))
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
            //actions+=/malefic_rapture,if=dot.impending_catastrophe_dot.ticking
            if (API.TargetHasDebuff(ImpendingCatastrophe))
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
            //actions+=/malefic_rapture,if=dot.soul_rot.ticking
            if (API.TargetHasDebuff(SoulRot))
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
            //actions+=/malefic_rapture,if=talent.phantom_singularity.enabled&(dot.phantom_singularity.ticking|soul_shard>3|time_to_die<cooldown.phantom_singularity.remains)
            if (TalentPhantomSingularity && (API.TargetHasDebuff(PhantomSingularity) || Api.PlayerCurrentSoulShards >=3 || API.TargetTimeToDie < API.SpellCDDuration(PhantomSingularity)))
                {
                    API.CastSpell(MaleficRapture)
                }
            //actions+=/malefic_rapture,if=talent.sow_the_seeds.enabled
            if (TalentSowTheSeed)
                {
                    Api.CastSpell(MaleficRapture);
                    return;
                }
            //actions+=/drain_life,if=buff.inevitable_demise.stack>40|buff.inevitable_demise.up&time_to_die<4
            if (API.TargetDebuffStacks > 40 || API.TargetHasDebuff(InevitableDemise) && API.TargetTimeToDie < 400)
                {
                    API.CastSpell(DrainLife);
                    return;
                }
            //actions+=/call_action_list,name=covenant
            if ()
                {
                    covenant();
                    return;
                }
            //actions+=/agony,if=refreshable
            if (API.CanCast(Agony) && API.TargetDebuffRemainingTime(Agony) <= 500 && IsRange && PlayerLevel >= 10)
                {
                    API.CastSpell(Agony);
                    return;
                }
            //actions+=/agony,cycle_targets=1,if=active_enemies>1,target_if=refreshable
            if (API.CanCast(Agony) && API.TargetUnitInRangeCount >= 1 && !API.MacroIsIgnored(Agony + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(Agony) <= 400 && IsRange && PlayerLevel >= 10)
                {
                    API.CastSpell(Agony + "MO");
                    return;
                }
            //actions+=/corruption,if=refreshable&active_enemies<4-(talent.sow_the_seeds.enabled|talent.siphon_life.enabled)
            if (API.CanCast(Corruption) && API.TargetUnitInRangeCount < 4 && (TalentSowTheSeed || TalentSiphonLife) && API.TargetDebuffRemainingTime(Corruption) < 400 && NotMoving && IsRange && PlayerLevel >= 2)
                {
                    API.CastSpell(Corruption);
                    return;
                }
            //actions+=/unstable_affliction,if=refreshable
            if (API.CanCast(UnstableAffliction) && API.TargetDebuffRemainingTime(UnstableAffliction) <= 600 && NotMoving && IsRange && PlayerLevel >= 13)
                {
                    API.CastSpell(UnstableAffliction);
                    return;
                }
            //actions+=/siphon_life,if=refreshable
            if (API.CanCast(SiphonLife) && API.TargetDebuffRemainingTime(SiphonLife) <= 400 && NotMoving && IsRange)
                {
                    API.CastSpell(SiphonLife);
                    return;
                }
            //actions+=/siphon_life,cycle_targets=1,if=active_enemies>1,target_if=refreshable
            if (API.CanCast(SiphonLife) && API.TargetUnitInRangeCount > 1 && API.TargetDebuffRemainingTime(SiphonLife) <= 400 && NotMoving)
                {
                    API.CastSpell(SiphonLife);
                    return;
                }
            //actions+=/corruption,cycle_targets=1,if=active_enemies<4-(talent.sow_the_seeds.enabled|talent.siphon_life.enabled),target_if=refreshable
            if (API.CanCast(Corruption) && API.TargetUnitInRangeCount < 4 && (TalentSowTheSeed || TalentSiphonLife) && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(Corruption) <= 400 && IsRange && PlayerLevel >= 2)
                {
                    API.CastSpell(Corruption + "MO");
                    return;
                }
            //actions+=/drain_soul,interrupt=1
            if (API.CanCast(DrainSoul) && NotMoving && IsRange && NotChanneling)
                {
                    Api.CastSpell(DrainSoul);
                    return;
                }
            //actions+=/shadow_bolt
            if (API.CanCast(ShadowBolt) && NotMoving && IsRange && NotChanneling)
                {
                    API.CastSpell(ShadowBolt)
                }
        }            
        private void multi()
        {
            //#########################
            //##### AoE ROTATION #####
            //#########################
            //actions.aoe=phantom_singularity
            if (IsAOE && API.CanCast(PhantomSingularity))
                {
                    API.CastSpell(PhantomSingularity);
                    return;
                }
            //actions.aoe+=/haunt
            if (IsAOE && API.CanCast(Haunt))
                {
                    API.CastSpell(Haunt);
                    return;
                }
            //actions.aoe+=/call_action_list,name=darkglare_prep,if=covenant.venthyr&dot.impending_catastrophe_dot.ticking&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
            if (IsAOE && IsCooldowns && PlayerCovenantSettings == "Venthyr" && API.TargetHasDebuff(ImpendingCatastrophe) && API.SpellCDDuration(SummonDarkglare) < 200 && ( API.TargetDebuffRemainingTime(PhantomSingularity) >= 200 || !TalentPhantomSingularity)
                {
                    darkglare_prep();
                }
            //actions.aoe+=/call_action_list,name=darkglare_prep,if=covenant.night_fae&dot.soul_rot.ticking&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
            if (IsAOE && IsCooldowns && PlayerCovenantSettings == "Night Fae" && API.TargetHasDebuff(SoulRot) && API.SpellCDDuration(SummonDarkglare) < 200 && ( API.TargetDebuffRemainingTime(PhantomSingularity) >= 200 || !TalentPhantomSingularity)
                {
                    darkglare_prep();
                }
            //actions.aoe+=/call_action_list,name=darkglare_prep,if=(covenant.necrolord|covenant.kyrian|covenant.none)&dot.phantom_singularity.ticking&dot.phantom_singularity.remains<2
            if (IsAOE && IsCooldowns && (PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None") && API.TargetHasDebuff(PhantomSingularity) && API.TargetDebuffRemainingTime(PhantomSingularity) >= 200)
                {
                    darkglare_prep();
                }
            //actions.aoe+=/seed_of_corruption,if=talent.sow_the_seeds.enabled&can_seed
            if (IsAOE && API.CanCast(SeedofCorruption) && TalentSowTheSeed && !API.TargetHasDebuff(SeedofCorruption) && !PlayerIsMoving)
                {
                    Api.CastSpell(SeedofCorruption);
                    return;
                }
            //actions.aoe+=/seed_of_corruption,if=!talent.sow_the_seeds.enabled&!dot.seed_of_corruption.ticking&!in_flight&dot.corruption.refreshable
             if (IsAOE && API.CanCast(Corruption) && !TalentSowTheSeed && !API.TargetHasDebuff(SeedofCorruption) && !PlayerIsMoving && API.MouseoverDebuffRemainingTime(Corruption) <= 400 && IsRange && PlayerLevel >= 2)
                {
                    API.CastSpell(Corruption);
                    return;
                }
            //actions.aoe+=/agony,cycle_targets=1,if=active_dot.agony<4,target_if=!dot.agony.ticking
            if (IsAOE && API.CanCast(Agony) && API.TargetUnitInRangeCount < 4 && !API.MacroIsIgnored(Agony + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && !API.TargetHasDebuff(Agony) <= 400 && IsRange && PlayerLevel >= 10)
                {
                    API.CastSpell(Agony + "MO");
                    return;
                }
            //actions.aoe+=/agony,cycle_targets=1,if=active_dot.agony>=4,target_if=refreshable&dot.agony.ticking
            if (IsAOE && API.CanCast(Agony) && API.TargetUnitInRangeCount < 4 && !API.MacroIsIgnored(Agony + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.MouseoverDebuffRemainingTime(Agony) <= 400 && IsRange && PlayerLevel >= 10)
                {
                    API.CastSpell(Agony + "MO");
                    return;
                }
            //actions.aoe+=/unstable_affliction,if=dot.unstable_affliction.refreshable
            if (IsAOE && API.CanCast(UnstableAffliction) && API.TargetDebuffRemainingTime(unstable_affliction) <= 600)
                {
                    API.CastSpell(UnstableAffliction);
                    return;
                }
            //actions.aoe+=/vile_taint,if=soul_shard>1
            if (IsAOE && API.CanCast(VileTaint) && API.PlayerCurrentSoulShards > 1)
                {
                    API.CastSpell(VileTaint);
                    return;
                }
            //actions.aoe+=/call_action_list,name=covenant,if=!covenant.necrolord
            if (IsAOE && !PlayerCovenantSettings == "Necrolord")
                {
                    Covenant();
                    return;
                }
            //actions.aoe+=/call_action_list,name=darkglare_prep,if=covenant.venthyr&(cooldown.impending_catastrophe.ready|dot.impending_catastrophe_dot.ticking)&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
            if (IsAOE && PlayerCovenantSettings == "Venthyr" && (API.CanCast(ImpendingCatastrophe) || API.TargetHasDebuff(ImpendingCatastrophe) && API.SpellCDDuration(SummonDarkglare) <= 200 && (API.TargetDebuffRemainingTime(PhantomSingularity) >= 200 || !TalentPhantomSingularity))
                {
                    darkglare_prep();
                    return;
                }
            //actions.aoe+=/call_action_list,name=darkglare_prep,if=(covenant.necrolord|covenant.kyrian|covenant.none)&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
            if (IsAOE (PlayerCovenantSettings == "Necrolord" || PlayerCovenantSettings == "Kyrian" || PlayerCovenantSettings == "None") && API.SpellCDDuration(SummonDarkglare) <= 200 && (API.TargetDebuffRemainingTime(PhantomSingularity) >= 200 || !TalentPhantomSingularity))
                {
                    darkglare_prep();
                    return;
                }
            //actions.aoe+=/call_action_list,name=darkglare_prep,if=covenant.night_fae&(cooldown.soul_rot.ready|dot.soul_rot.ticking)&cooldown.summon_darkglare.remains<2&(dot.phantom_singularity.remains>2|!talent.phantom_singularity.enabled)
            if (PlayerCovenantSettings == "Night Fae" && (API.CanCast(SoulRot) || API.TargetHasDebuff(SoulRot) && API.SpellCDDuration(SummonDarkglare) <= 200 && (API.TargetDebuffRemainingTime(PhantomSingularity) >= 200 || !TalentPhantomSingularity))
                {
                    darkglare_prep();
                    return;
                }
            //actions.aoe+=/dark_soul,if=cooldown.summon_darkglare.remains>time_to_die
            if (IsAOE && API.CanCast(DarkSoulMisery) && API.SpellCDDuration(SummonDarkglare) >= TargetTimeToDie && IsCooldowns)
                {
                    API.CastSpell(DarkSoulMisery);
                    return;
                }
            //actions.aoe+=/call_action_list,name=item
            if (IsAOE && IsCooldowns)
                {
                    item();
                    return:
                }
            //actions.aoe+=/malefic_rapture,if=dot.vile_taint.ticking
            if (IsAOE && API.CanCast(MaleficRapture) && API.TargetHasDebuff(VileTaint))
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
            //actions.aoe+=/malefic_rapture,if=dot.soul_rot.ticking&!talent.sow_the_seeds.enabled
            if (IsAOE && API.CanCast(MaleficRapture) && API.TargetHasDebuff(SoulRot) && !TalentSowTheSeed)
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
            //actions.aoe+=/malefic_rapture,if=!talent.vile_taint.enabled
            if (IsAOE && API.CanCast(MaleficRapture) && !TalentVileTaint)
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
            //actions.aoe+=/malefic_rapture,if=soul_shard>4
            if (IsAOE && API.CanCast(MaleficRapture) && PlayerCurrentSoulShards > 4)
                {
                    API.CastSpell(MaleficRapture);
                    return;
                }
            //actions.aoe+=/siphon_life,cycle_targets=1,if=active_dot.siphon_life<=3,target_if=!dot.siphon_life.ticking
            if (IsAOE && API.CanCast(SiphonLife) && API.TargetUnitInRangeCount <= 3 && !API.MacroIsIgnored(SiphonLife + "MO") && API.PlayerCanAttackMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && !API.TargetHasDebuff(SiphonLife) && IsRange && PlayerLevel >= 25)
                {
                    API.CastSpell(SiphonLife + "MO");
                    return;
                }
            //actions.aoe+=/call_action_list,name=covenant
            if ()
                {
                    covenant();
                    return;
                }
            //actions.aoe+=/drain_life,if=buff.inevitable_demise.stack>=50|buff.inevitable_demise.up&time_to_die<5|buff.inevitable_demise.stack>=35&dot.soul_rot.ticking
            if (IsAOE && API.TargetDebuffStacks(InevitableDemise) >= 50 || API.TargetHasDebuff(InevitableDemise) && API.TargetTimeToDie < 500 || API.TargetDebuffStacks(InevitableDemise) >= 35 && API.TargetHasDebuff(SoulRot))
                {
                    API.CastSpell(DrainLife);
                    return;
                }
            //actions.aoe+=/drain_soul,interrupt=1
            if (IsAOE && API.CanCast(DrainSoul) && NotMoving && IsRange && NotChanneling)
                {
                    Api.CastSpell(DrainSoul);
                    return;
                }
            //actions.aoe+=/shadow_bolt
            if (IsAOE && API.CanCast(ShadowBolt) && NotMoving && IsRange && NotChanneling)
                {
                    API.CastSpell(ShadowBolt);
                    return;
                }
        }
            private void covenant()
        {
            //#########################
            //### Covenant Ablities ###
            //#########################
            //actions.covenant=impending_catastrophe,if=cooldown.summon_darkglare.remains<10|cooldown.summon_darkglare.remains>50
            if (API.CanCast(ImpendingCatastrophe) && API.SpellCDDuration(SummonDarkglare) < 1000 || API.SpellCDDuration(SummonDarkglare) < 5000)
                {
                    API.CastSpell(ImpendingCatastrophe);
                    return;
                }
            //actions.covenant+=/decimating_bolt,if=cooldown.summon_darkglare.remains>5&(debuff.haunt.remains>4|!talent.haunt.enabled)
            if (API.CanCast(DecimatingBolt) && API.SpellCDDuration(SummonDarkglare) > 500 && (API.TargetDebuffRemainingTime(Haunt) > 400 || !TalentHaunt))
                {
                    API.CastSpell(DecimatingBolt);
                    return;
                }
            //actions.covenant+=/soul_rot,if=cooldown.summon_darkglare.remains<5|cooldown.summon_darkglare.remains>50|cooldown.summon_darkglare.remains>25&conduit.corrupting_leer.enabled
            if (API.CanCast(SoulRot) && API.SpellCDDuration(SummonDarkglare) < 500 || API.SpellCDDuration(SummonDarkglare) > 5000 || API.SpellCDDuration(SummonDarkglare) > 2500 && API.PlayerHasConduit(CorruptingLeer))
                {
                    API.CastSpell(SoulRot);
                    return;
                }
            //actions.covenant+=/scouring_tithe
            if (API.CanCast(ScouringTithe))
                {
                    API.CastSpell(ScouringTithe);
                    return;
                }
        }
            private void darkglare_prep()
        {
            //#########################
            //#### DarkGlare Prep #####
            //#########################
            //actions.darkglare_prep=vile_taint,if=cooldown.summon_darkglare.remains<2
            if (API.CanCast(VileTaint) && !API.SpellISOnCooldown(VileTaint) && API.SpellCDDuration(SummonDarkglare) < 200)
                {
                    API.CastSpell(VileTaint);
                    return;
                }
            //actions.darkglare_prep+=/dark_soul
            if (API.CanCast(DarkSoulMisery) && !API.SpellISOnCooldown(SummonDarkglare))
                {
                    API.CastSpell(DarkSoulMisery);
                    return;
                }
            actions.darkglare_prep+=/potion
            if (API.PlayerItemCanUse(PotionofPhantomFire) && API.TargetUnitInRangeCount == 1)
                {
                    API.CastSpell(PotionofPhantomFire);
                    return;
                }
            if (API.PlayerItemCanUse(PotionofSpectralIntellect) && API.TargetUnitInRangeCount >= 2)
                {
                    API.CastSpell(PotionofSpectralIntellect);
                    return;
                }
            actions.darkglare_prep+=/fireblood
            if (PlayerRaceSettings == "Dark Iron Dwarf" && API.CanCast(RacialSpell1) && isRacial)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
            actions.darkglare_prep+=/blood_fury
            if (PlayerRaceSettings == "Orc" && API.CanCast(RacialSpell1) && isRacial)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
            actions.darkglare_prep+=/berserking
            if (PlayerRaceSettings == "Troll" && API.CanCast(RacialSpell1) && isRacial)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
            actions.darkglare_prep+=/call_action_list,name=covenant,if=!covenant.necrolord&cooldown.summon_darkglare.remains<2
            if (!PlayerCovenantSettings == "Necrolord" && API.SpellCDDuration(SummonDarkglare) < 200)
                {
                    covenant();
                    return;
                }            
            actions.darkglare_prep+=/summon_darkglare
            if (API.CanCast(SummonDarkglare) && !PlayerIsMoving)
                {
                    API.CastSpell(SummonDarkglare):
                    return;
                }
        }
            private void item()
        {
            //#########################
            //#####     Items     #####
            //#########################
            actions.item=use_items
            if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && IsTrinkets1)
            {
                API.CastSpell("Trinket1");
                return;
            }
            if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && IsTrinkets2)
            {
                API.CastSpell("Trinket2");
                return;
            }
        }
            private void se()
        {
            //#########################
            //##### ShadowEmbrace #####
            //#########################
            actions.se=haunt
            actions.se+=/drain_soul,interrupt_global=1,interrupt_if=debuff.shadow_embrace.stack>=3
            actions.se+=/shadow_bolt
        }   
        public override void OutOfCombatPulse()
        {
            //Grimoire Of Sacrifice
            if (API.PlayerHasPet && TalentGrimoireOfSacrifice && API.PlayerHasBuff("Grimoire Of Sacrifice"))
            {
                API.CastSpell(GrimoireOfSacrifice);
                return;
            }
            //Summon Imp
            if (!TalentGrimoireOfSacrifice && API.CanCast(SummonImp) && !API.PlayerIsCasting(true) && !API.PlayerHasPet && (isMisdirection == "Imp") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 3)
            {
                API.CastSpell(SummonImp);
                return;
            }
            //Summon Voidwalker
            if (!TalentGrimoireOfSacrifice && API.CanCast(SummonVoidwalker) && !API.PlayerIsCasting(true) && !API.PlayerHasPet && (isMisdirection == "Voidwalker") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 10)
            {
                API.WriteLog("Looks like we have no Pet , lets Summon one");
                API.CastSpell(SummonVoidwalker);
                return;
            }
            //Summon Succubus
            if (!TalentGrimoireOfSacrifice && API.CanCast(SummonSuccubus) && !API.PlayerIsCasting(true) && !API.PlayerHasPet && (isMisdirection == "Succubus") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 19)
            {
                API.WriteLog("Looks like we have no Pet , lets Summon one");
                API.CastSpell(SummonSuccubus);
                return;
            }
            //Summon Fellhunter
            if (!TalentGrimoireOfSacrifice && API.CanCast(SummonFelhunter) && !API.PlayerIsCasting(true) && !API.PlayerHasPet && (isMisdirection == "Felhunter") && NotMoving && IsRange && NotChanneling && PlayerLevel >= 23)
            {
                API.WriteLog("Looks like we have no Pet , lets Summon one");
                API.CastSpell(SummonFelhunter);
                return;
            }
        }
    }
}