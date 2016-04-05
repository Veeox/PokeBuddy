using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using EloBuddy.SDK.Notifications;

using static PokeBuddy.PetMenu;
using static PokeBuddy.Pet;
using static PokeBuddy.Converters;
using static PokeBuddy.DrawStuff;
using static PokeBuddy.GameAssets;
using static PokeBuddy.PetItem;
using static PokeBuddy.XPSys;
using static PokeBuddy.Save;

namespace PokeBuddy
{
    internal class PetMain
    {
        private static string DragonBuff = "s5test_dragonslayerbuff";
        //private static string BaroonBuff = "exaltedwithbaronnashor";
        private static int AllyD;
        //private static int AllyB;
        public static float QuadraDelay;
        public static float DoubleDelay;
        public static float TrippleDelay;
        public static float PentaDelay;
        public static float AceDelay;
        public static float WardDelay;
        //public static float bDelay;

        public static AIHeroClient hero { get { return ObjectManager.Player; } }

        public static void Init()
        {
            PetInit();
            DrawInit();
            Game.OnNotify += OnGameNotify;
            Obj_AI_Base.OnBuffGain += Obj_AI_Base_OnBuffGain;
            
        }

        public static void DoWelcome()
        {
            Notifications.Show(new SimpleNotification("PokeBuddy", "PokeBuddy has been Loaded!"
            + System.Environment.NewLine + Bonuses.news));
        }

        internal static void OnGameNotify(GameNotifyEventArgs args)
        {
            var killer = args.NetworkId;
            var al = FindPlayerByNetworkId(killer);

            switch (args.EventId) //Check for XP events
            {

                case GameEventId.OnChampionDoubleKill:

                    if (killer == hero.NetworkId)
                    {
                        if (Game.Time > DoubleDelay)
                        {
                            Pet.CurXP += (Pet.MaxXP / 80) * Pet.XPMulti;
                            Pet.CashBalance += 5;
                            DoubleDelay = Game.Time + 3000;
                        }

                    }
                    break;
                case GameEventId.OnChampionPentaKill:

                    if (killer == hero.NetworkId)
                    {
                        if (Game.Time > PentaDelay)
                        {
                            Pet.CurXP += (Pet.MaxXP / 15) * Pet.XPMulti;
                            Pet.CashBalance += 50;
                            PentaDelay = Game.Time + 3000;
                        }

                    }
                    break;
                case GameEventId.OnChampionQuadraKill:

                    if (killer == hero.NetworkId)
                    {
                        if (Game.Time > QuadraDelay)
                        {
                            Pet.CurXP += (Pet.MaxXP / 45) * Pet.XPMulti;
                            Pet.CashBalance += 20;
                            QuadraDelay = Game.Time + 3000;
                        }

                    }
                    break;
                case GameEventId.OnChampionTripleKill:

                    if (killer == hero.NetworkId)
                    {
                        if (Game.Time > TrippleDelay)
                        {
                            Pet.CurXP += (Pet.MaxXP / 75) * Pet.XPMulti;
                            Pet.CashBalance += 10;
                            TrippleDelay = Game.Time + 3000;
                        }

                    }
                    break;
                case GameEventId.OnAce:

                    var pl = FindPlayerByNetworkId(killer);
                    if (Game.Time > AceDelay)
                    {
                        if (pl != null && pl.IsAlly || pl.IsMe)
                        {
                            Pet.CurXP += (Pet.MaxXP / 80) * Pet.XPMulti;
                            Pet.CashBalance += 5;
                            AceDelay = Game.Time + 3000;
                        }
                    }
                    break;
                case GameEventId.OnChampionKill:

                    if (killer == hero.NetworkId)
                    {
                        Pet.CurXP += (Pet.MaxXP / 75) * Pet.XPMulti;
                        Pet.CashBalance += 2;
                    }
                    break;
                case GameEventId.OnKillWard:

                    if (ObjectManager.Player.IsMe)
                    {
                        if (Game.Time > WardDelay)
                        {
                            KillWard();
                            WardDelay = Game.Time + 3000;
                        }

                        Console.WriteLine("Killed a ward!");
                    }
                    break;
                case GameEventId.OnChampionDie:
                    if (killer == hero.NetworkId && !Pet.Sick)
                    {
                        if (Pet.Lvl > 2)
                        {
                            Pet.GetSick();
                        }
                    }
                    break;
                    //case GameEventId.OnHQDie:
                    //    pl = FindPlayerByNetworkId(killer);
                    //    //if (pl != null && pl.IsAlly || pl.IsMe)
                    //    //{
                    //        Pet.CurXP += Pet.MaxXP / 10;
                    //        Pet.CashBalance += 100;
                    //        Converters.ConvertInt(Pet.Lvl, Pet.CurXP, Pet.MaxXP, Pet.CashBalance);
                    //        Save.SaveData();
                    //        Chat.Print("end");
                    //        if (Pet.Sick)
                    //        {
                    //            Pet.PetDie();
                    //        }
                    //   // }
                    //    break;
            }
        }

        public static AIHeroClient FindPlayerByNetworkId(uint id)
        {
            AIHeroClient player = null;
            foreach (var n in EntityManager.Heroes.AllHeroes)
            {
                if (n.NetworkId == id)
                    player = n;
            }
            return player;
        }

        private static void KillWard()
        {
            Pet.CurXP += (Pet.MaxXP / 100) * Pet.XPMulti;
            Pet.CashBalance += 1;
        }

        public static void DragonCheck()
        {
            var allyDbuff = hero.Buffs.Find(x => x.Name == DragonBuff);

            // ally kill dragon
            if (allyDbuff != null && allyDbuff.Count > AllyD)
            {
                AllyD = allyDbuff.Count;
                KillDrag();
            }
            // dragon 5 expiry
            if (allyDbuff != null && allyDbuff.Count < AllyD)
            {
                AllyD = allyDbuff.Count;
            }
        }

        public static void Obj_AI_Base_OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (sender.IsMe && (args.Buff.DisplayName.ToLower().Contains("hand of baron") || args.Buff.Name.ToLower().Contains("baron") || args.Buff.Name.ToLower().Contains("worm")))
            {
                KillBaroon();
            }
        }

        private static void KillDrag()
        {
            Pet.CurXP += (Pet.MaxXP / 30) * Pet.XPMulti;
            Pet.CashBalance += 10;
            Console.WriteLine("Drag Killed");
        }

        private static void KillBaroon()
        {
            Pet.CurXP += (Pet.MaxXP / 50) * Pet.XPMulti;
            Pet.CashBalance += 20;
            Console.WriteLine("Baroon Killed");
        }
        internal static void OnEnd(EventArgs args)
        {
            Converters.ConvertInt(Pet.Lvl, Pet.CurXP, Pet.MaxXP, Pet.CashBalance);
        }

        //internal static void OnGameEnd(GameEndEventArgs args)
        //{
        //    var winner = args.WinningTeam.IsAlly();
        //    if (winner)
        //    {
                
        //    }
        //}
       // internal static void OnNotify(GameNotifyEventArgs args)
       // {
       //     if (args.EventId == GameEventId.OnEndGame)
       //     {
       //         var killHQ = ObjectManager.Get<Obj_HQ>();
       //         if (killHQ != null)
       //         {
       //             Chat.Print("endededed");
       //             Pet.CurXP += Pet.MaxXP / 10;
       //             Pet.CashBalance += 100;
       //             Converters.ConvertInt(Pet.Lvl, Pet.CurXP, Pet.MaxXP, Pet.CashBalance);
       //             if (Pet.Sick)
       //             {
       //                 Pet.PetDie();
       //             }
       //             Console.WriteLine("endededed");
       //         }
       //     }
            
       //}
    }
}