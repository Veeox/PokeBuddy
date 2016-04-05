using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using SharpDX;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Notifications;
using EloBuddy.SDK.Rendering;

namespace PokeBuddy
{
    internal class Pet
    {
        //Main Pet Vars
        public static int CurXP;
        public static int MaxXP;
        public static int Lvl;
        public static string PetName;
        public static int CashBalance = 0;
        public static bool nSick, Sick = false;
        public static bool FoodXP = false;
        public static int XPMulti = 1;
        public static string mySprite;
        public static Obj_HQ EneMyNexus = null;
        public static Obj_HQ MyNexus = null;
        private static bool gameEnded = false;

        //Sprite Vars
        public static int minion_buffer_size = 60;
        public static EloBuddy.SDK.Rendering.Sprite[] sprites = new EloBuddy.SDK.Rendering.Sprite[minion_buffer_size];
        //public EloBuddy.SDK.Rendering.Sprite PetSprite;
        

        internal static void PetInit()
        {
            Save.SaveData();
            Game.OnTick += Game_OnTick;
            Game.OnUpdate += Game_OnUpdate;
            PetMenu.InitMenu();
            if (EneMyNexus == null)
            {
                EneMyNexus = ObjectManager.Get<Obj_HQ>().First(n => n.IsEnemy);
            }
            if (MyNexus == null)
            {
                MyNexus = ObjectManager.Get<Obj_HQ>().First(n => n.IsAlly);
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (!PetMenu.MiscMenu["track"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }
            else
            {
                XPSys.LevelUp();
                OnEndGame();
            }
        }

        private static void OnEndGame()
        {
            if (EneMyNexus != null && (EneMyNexus.Health > 1))
            {
                Core.DelayAction(OnEndGame, 20000);
                return;
            }
            if (MyNexus != null && (MyNexus.Health < 1) && !gameEnded)
            {
                Converters.ConvertInt(Pet.Lvl, Pet.CurXP, Pet.MaxXP, Pet.CashBalance);
                gameEnded = true;
            }
            if (EneMyNexus != null && (EneMyNexus.Health < 1) && !gameEnded)
            {
                Pet.CurXP += Pet.MaxXP / 10;
                Pet.CashBalance += 15;
                Converters.ConvertInt(Pet.Lvl, Pet.CurXP, Pet.MaxXP, Pet.CashBalance);
                if (Pet.Sick)
                {
                    Pet.PetDie();
                }
                gameEnded = true;
            }

        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (!PetMenu.MiscMenu["track"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }
            else
            {
                PetMain.DragonCheck();
                Shop.ShopBuy();
                Save.ManualSave();
                Save.NewPet();
            }
        }

        public static void GetSick()
        {
            Random rnd = new Random();

            int r = rnd.Next(10) + 1;

            if (r >= 7)
            {
                Sick = true;
                nSick = true;
                NotiSick();
            }
        }

        

        public static void NotiSick()
        {
            Notifications.Show(new SimpleNotification("PokeBuddy", "Your pet is sick!"));
            Notifications.Show(new SimpleNotification("PokeBuddy", "Buy Medicine from the Shop to cure! If your pet is sick for too long it will die!"));
            
        }

        public static void PetDie()
        {
            Notifications.Show(new SimpleNotification("PokeBuddy", "Your pet has died!"));
            Save.FirstRun();
            Converters.ConvertInt(Pet.Lvl, Pet.CurXP, Pet.MaxXP, Pet.CashBalance);
            Notifications.Show(new SimpleNotification("PokeBuddy", "New Pet Created!"));
        }
    }
}
