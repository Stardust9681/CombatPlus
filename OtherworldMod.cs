using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using System.Reflection;
using OtherworldMod.Common;
using Terraria.Enums;
using OtherworldMod.Common.ChangeNPC;
using static OtherworldMod.Common.ChangeNPC.Utilities.OtherworldNPCSets;
using System.IO;
using OtherworldMod.Common.ChangeNPC.Utilities;
using OtherworldMod.Core.Util;

namespace OtherworldMod
{
    public class OtherworldMod : Mod
    {
        //Tuple as (Requirement{Mod,value,Reason},value)
        //Set as value
        //Get as Tuple (Requirement{Mod,value,Reason},value)
        //Worth making entire struct?
        #region Server-Side Config
        public ConfigData<bool> NPCAIChanges;
        public ConfigData<bool> NPCDynamicHitboxes;
        public ConfigData<bool> NPCGrief;
        public ConfigData<bool> DSTXOver;
        public ConfigData<bool> ItemBalance;
        #endregion
        #region Client-Side Config
        public ConfigData<bool> UseStyleAlts;
        #endregion

        public static OtherworldMod Instance { get; private set; }
        public override void Load()
        {
            Instance = this;
            #region Detours
            Terraria.On_NPC.NewNPC += NewNPC;
            Terraria.On_WorldGen.TryGrowingAbigailsFlower += CorpseFlowerCheck;
            Terraria.On_Item.NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool += NewItem1;
            Terraria.On_Item.NewItem_IEntitySource_Rectangle_int_int_bool_int_bool_bool += NewItem2;
            Terraria.On_Item.NewItem_IEntitySource_Vector2_int_int_bool_int_bool_bool += NewItem3;
            Terraria.On_Item.NewItem_IEntitySource_Vector2_int_int_int_int_bool_int_bool_bool += NewItem4;
            Terraria.On_Item.NewItem_IEntitySource_Vector2_Vector2_int_int_bool_int_bool_bool += NewItem5;
            #endregion

            ModContent.GetInstance<OtherworldServerConfig>().SetupConfig();
        }
        public override void Unload()
        {
            base.Unload();
            //Instance.Unload();

        }
        #region Anti-DST
        private static bool IsDSTItem(int type)
        {
            switch (type)
            {
                case ItemID.BatBat:
                case ItemID.HamBat:
                case ItemID.AbigailsFlower:
                case ItemID.LucyTheAxe:
                case ItemID.WilsonBeardMagnificent:
                case ItemID.WilsonBeardShort:
                case ItemID.WilsonBeardLong:
                case ItemID.DontStarveShaderItem:
                case 5107:
                case ItemID.WeatherPain:
                case ItemID.PewMaticHorn:
                case ItemID.TentacleSpike:
                case ItemID.HoundiusShootius:
                case ItemID.GarlandHat:
                case ItemID.DeerThing:
                case ItemID.DeerclopsMask:
                case ItemID.DeerclopsMasterTrophy:
                case ItemID.DeerclopsPetItem:
                case ItemID.DeerclopsTrophy:
                case ItemID.MusicBoxDeerclops:
                case ItemID.DeerclopsBossBag:
                case ItemID.BerniePetItem:
                case ItemID.Eyebrella:
                case ItemID.BoneHelm:
                    return true;
            }
            return false;
        }
        private int NewItem1(Terraria.On_Item.orig_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool orig, IEntitySource source, int X, int Y, int Width, int Height, int Type, int Stack, bool noBroadcast, int pfix, bool noGrabDelay, bool reverseLookup)
        {
            if (DSTXOver)
                return orig.Invoke(source, X, Y, Width, Height, Type, Stack, noBroadcast, pfix, noGrabDelay, reverseLookup);
            if (IsDSTItem(Type)) return -1;
            return orig.Invoke(source, X, Y, Width, Height, Type, Stack, noBroadcast, pfix, noGrabDelay, reverseLookup);
        }
        private int NewItem2(Terraria.On_Item.orig_NewItem_IEntitySource_Rectangle_int_int_bool_int_bool_bool orig, IEntitySource source, Rectangle rectangle, int Type, int Stack, bool noBroadcast, int prefixGiven, bool noGrabDelay, bool reverseLookup)
        {
            if (DSTXOver)
                return orig.Invoke(source, rectangle, Type, Stack, noBroadcast, prefixGiven, noGrabDelay, reverseLookup);
            if (IsDSTItem(Type)) return -1;
            return orig.Invoke(source, rectangle, Type, Stack, noBroadcast, prefixGiven, noGrabDelay, reverseLookup);
        }
        private int NewItem3(Terraria.On_Item.orig_NewItem_IEntitySource_Vector2_int_int_bool_int_bool_bool orig, IEntitySource source, Vector2 position, int Type, int Stack, bool noBroadcast, int prefixGiven, bool noGrabDelay, bool reverseLookup)
        {
            if (DSTXOver)
                return orig.Invoke(source, position, Type, Stack, noBroadcast, prefixGiven, noGrabDelay, reverseLookup);
            if (IsDSTItem(Type)) return -1;
            return orig.Invoke(source, position, Type, Stack, noBroadcast, prefixGiven, noGrabDelay, reverseLookup);
        }
        private int NewItem4(Terraria.On_Item.orig_NewItem_IEntitySource_Vector2_int_int_int_int_bool_int_bool_bool orig, IEntitySource source, Vector2 pos, int Width, int Height, int Type, int Stack, bool noBroadcast, int prefixGiven, bool noGrabDelay, bool reverseLookup)
        {
            if (DSTXOver)
                return orig.Invoke(source, pos, Width, Height, Type, Stack, noBroadcast, prefixGiven, noGrabDelay, reverseLookup);
            if (IsDSTItem(Type)) return -1;
            return orig.Invoke(source, pos, Width, Height, Type, Stack, noBroadcast, prefixGiven, noGrabDelay, reverseLookup);
        }
        private int NewItem5(Terraria.On_Item.orig_NewItem_IEntitySource_Vector2_Vector2_int_int_bool_int_bool_bool orig, IEntitySource source, Vector2 pos, Vector2 randomBox, int Type, int Stack, bool noBroadcast, int prefixGiven, bool noGrabDelay, bool reverseLookup)
        {
            if (DSTXOver)
                return orig.Invoke(source, pos, randomBox, Type, Stack, noBroadcast, prefixGiven, noGrabDelay, reverseLookup);
            if (IsDSTItem(Type)) return -1;
            return orig.Invoke(source, pos, randomBox, Type, Stack, noBroadcast, prefixGiven, noGrabDelay, reverseLookup);
        }
        private bool CorpseFlowerCheck(Terraria.On_WorldGen.orig_TryGrowingAbigailsFlower orig, int i, int j)
        {
            if (DSTXOver)
                return orig.Invoke(i, j);
            return false;
        }
        private int NewNPC(Terraria.On_NPC.orig_NewNPC orig, IEntitySource source, int X, int Y, int Type, int Start, float ai0, float ai1, float ai2, float ai3, int Target)
        {
            if (DSTXOver)
                return orig.Invoke(source, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);
            if (Type == NPCID.Deerclops)
                return -1;
            return orig.Invoke(source, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);
        }
        #endregion
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            int type = reader.ReadInt32();
            switch (type)
            {
                case 1:
                    if (whoAmI != 255)
                    {
                        ModPacket packet = GetPacket(12);
                        packet.Write(1);
                        packet.Write((byte)whoAmI);
                        packet.Send(-1, whoAmI);
                    }
                    else
                    {
                        int playerIndex = reader.ReadByte();
                        TargetCollective.AddTarget(new PlayerTarget(playerIndex));
                    }
                    break;
            }
        }
        public override object Call(params object[] args)
        {
            /*
            //"ModifyAI", Action<NPC, int, int>
                //Delegate param pass for modifying AI
            if (args.Length == 3)
                if (args[0] is string && ((string)args[0]).ToLower().Equals("getconfig"))
                {
                    if (args[1] is string or null)
                        if (args[2] is Mod mod)
                            return GetConfig((string?)args[1], mod);
                }
                else if (args[0] is string && ((string)args[0]).ToLower().Equals("requireconfig"))
                    if (args[1] is string)
                        if (args[2] is Mod)
                            return null; //return RequireConfig((string)args[1], (Mod)args[2]);
            */
            if (args.Length == 0)
                return null;
            if (args[0] is string cmd)
            {
                cmd = cmd.ToLower();
                switch (cmd)
                {
                    case "getconfig":
                        if (args.Length < 3)
                            return null;
                        if (args[1] is string or null)
                            if (args[2] is Mod mod)
                                return GetConfig((string?)args[1], mod);
                        break;

                        //NOTE : Thinking about changing to style-based AI instead of per-NPC AI (less memory, would show any potential flaws with current system)
                        //NOTE : Important because that would also be what allows code below to *function*
                        #region Get AI
                        /*case "getai":
                            if(args.Length < 2)
                                return null;
                            if (args[1] is int style)
                            {
                                //return specific phase
                                if(args.Length > 2 && args[2] is string or null)
                                    return OtherworldNPC.Behaviour[style].GetPhase((string)args[2]);

                                //return behaviour
                                return OtherworldNPC.Behaviour[style];
                            }*/
                        #endregion
                }
            }
            return null;
        }
        public object GetConfig(string? name, Mod caller)
        {
            (string key, object val)[] KVPairs = new (string key, object val)[] {
                ("dst", Instance.DSTXOver.value),
                ("dontstarve", Instance.DSTXOver.value),
                ("dontstarvetogether", Instance.DSTXOver.value),
                ("dstogether", Instance.DSTXOver.value),

                ("npcgrief", Instance.NPCGrief.value),
                ("enemygrief", Instance.NPCGrief.value),

                ("ibalance", Instance.ItemBalance.value),
                ("itembalance", Instance.ItemBalance.value),
                ("balanceitem", Instance.ItemBalance.value),

                ("altusestyles", Instance.UseStyleAlts.value),
                ("usestylealts", Instance.UseStyleAlts.value),
            };
            if (name is null) //If Call wants to see what Configs there are, return array with config names
                return new string[]
                    {
                        "dst", "npcgrief", "itembalance", "altusestyles"
                    };
            name = name!.ToLower();
            for (int i = 0; i < KVPairs.Length; i++)
            {
                if (KVPairs[i].key.Equals(name))
                    return KVPairs[i].key;
            }
            Logging.PublicLogger.Warn($"[OtherworldMod] Could not find config \"{name}\" from {caller.Name}'s request.");
            return null;

            /*
            name = name.ToLower();
            if (AllConfig.ContainsKey(name))
                return AllConfig[name];
            string[] keys = new string[AllConfig.Keys.Count];
            AllConfig.Keys.CopyTo(keys, 0);
            string maybeKey = "";
            for (int i = 0; i < keys.Length; i++)
            {
                string key = keys[i].ToLower();
                if (key.Equals(name))
                {
                    Terraria.ModLoader.Logging.PublicLogger.Debug($"[OtherworldMod]: Incorrect config key capitalisation, using fallback, '{key}.'");
                    return AllConfig[keys[i]];
                }
                if (key.Contains(name))
                    maybeKey = key;
            }
            if (!maybeKey.Equals(""))
            {
                Terraria.ModLoader.Logging.PublicLogger.Warn($"[OtherworldMod] Warning from <{caller.Name}>: Could not find config key, name, or index, '{name},' using closest match.");
                return AllConfig[maybeKey];
            }
            Terraria.ModLoader.Logging.PublicLogger.Error($"[OtherworldMod] Warning from <{caller.Name}>: Could not find config key, name, or index, '{name}.' No match found.");
            return null;
            */
        }
    }

    public class OtherworldSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            TargetCollective.Update();
        }
    }

    [Label("Combat+ Mod Config (Server)")]
    public class OtherworldServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(true)]
        [Label("Toggle NPC AI Changes")]
        [Description("If disabled: NPCs will retain constant hitboxes, and will not grief terrain")]
        public bool aiChanges;

        [DefaultValue(true)]
        [Label("Toggle Dynamic NPC Hitboxes")]
        [Description("If disabled: hostile NPCs will always deal damage when colliding with the player")]
        public bool dynamicHitboxes;

        //True : Allows applicable NPCs to affect environment
        [DefaultValue(false)]
        [Label("Toggle Enemy Grief")]
        public bool enemyGrief;

        //False : Prevent DST garbage
        [DefaultValue(false)]
        [Label("Toggle DST Crossover")]
        public bool dst;

        //True : Enable damage, usetime, stack, rarity, sell price, liquid, etc balances.
        [DefaultValue(true)]
        [Label("Toggle Item Balances")]
        public bool itemBalance;

        public void SetupConfig()
        {
            SetupConfig(OtherworldMod.Instance);
        }
        public void SetupConfig(OtherworldMod instance)
        {
            if (instance != null)
            {
                instance.NPCAIChanges.value = aiChanges;
                if (aiChanges)
                {
                    instance.NPCDynamicHitboxes.value = dynamicHitboxes;
                    instance.NPCGrief.value = enemyGrief;
                }
                else
                {
                    instance.NPCDynamicHitboxes.value = false;
                    instance.NPCGrief.value = false;
                }
                instance.DSTXOver.value = dst;
                instance.ItemBalance.value = itemBalance;
            }
        }

        public override void OnChanged()
        {
            SetupConfig();
        }
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            if (Main.dedServ)
                return true;
            if (whoAmI == Main.myPlayer)
                return true;
            message = "Please contact the server host to change these settings.";
            System.Console.WriteLine($"{Main.player[whoAmI].name} tried to access server config.");
            return false;
        }

        private static OtherworldServerConfig AsThis(ModConfig config) => (OtherworldServerConfig)config;
    }

    [Label("Combat+ Mod Config (Client)")]
    public class OtherworldClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [DefaultValue(true)]
        [Label("Toggle Alternative Usestyles")]
        public bool altUsestyles;
        public override void OnChanged()
        {
            if(OtherworldMod.Instance!=null)
                OtherworldMod.Instance.UseStyleAlts.value = altUsestyles;
        }
        public override bool NeedsReload(ModConfig pendingConfig)
        {
            return OtherworldMod.Instance.UseStyleAlts.value != AsThis(pendingConfig).altUsestyles;
        }
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            return true;
        }
        private static OtherworldClientConfig AsThis(ModConfig config) => (OtherworldClientConfig)config;
    }
}