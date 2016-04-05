using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.IO;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Notifications;

namespace PokeBuddy
{
    internal class Save
    {
        //File name setup for saving
        public static string FileName;


        public static void SaveData()
        {
            //Grab data from text file else create it
            FileName = "PokeBuddy.txt";
            if (!Directory.Exists(EloBuddy.Sandbox.SandboxConfig.DataDirectory + @"\Data\PokeBuddy"))
            {
                Directory.CreateDirectory(EloBuddy.Sandbox.SandboxConfig.DataDirectory + @"\Data\PokeBuddy");
                FirstRun();

            }
            //else read the save
            else
            {
                ReadSave();

            }
        }
        //Used to read data
        public static void ReadSave()
        {
            string LvlStr = null;
            string CurXPStr = null;
            string MaxXPStr = null;
            string CashStr = null;

            using (var sr = new System.IO.StreamReader(EloBuddy.Sandbox.SandboxConfig.DataDirectory + @"\Data\PokeBuddy\" + FileName, true))
            {
                string line;
                int currentLineNumber = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    switch (++currentLineNumber)
                    {
                        case 1:
                            Pet.PetName = line;
                            break;
                        case 2:
                            Pet.mySprite = line;
                            break;
                        case 3:
                            LvlStr = line;
                            break;
                        case 4:
                            CurXPStr = line;
                            break;
                        case 5:
                            MaxXPStr = line;
                            break;
                        case 6:
                            CashStr = line;
                            break;
                    }
                }
                Converters.ConvertString(LvlStr, CurXPStr, MaxXPStr, CashStr);
            }
        }

        //Used to save data
        public static void SaveData(string lvl, string currxp, string maxxp, string cash)
        {
            File.WriteAllText(EloBuddy.Sandbox.SandboxConfig.DataDirectory + @"\Data\PokeBuddy\" + FileName, Pet.PetName + System.Environment.NewLine);

            using (var file = new StreamWriter(EloBuddy.Sandbox.SandboxConfig.DataDirectory + @"\Data\PokeBuddy\" + FileName, true))
            {
                //file.WriteLine(Pet.PetName);
                //file.WriteLine("\n");
                file.WriteLine(Pet.mySprite);
                file.WriteLine(lvl);
                file.WriteLine(currxp);
                file.WriteLine(maxxp);
                file.WriteLine(cash);
                file.Close();
            }
        }

        public static void FirstRun()
        {
            RandomName();
            Pet.Lvl = 1;
            Pet.CurXP = 0;
            Pet.MaxXP = 100;
            Pet.CashBalance = 0;
            
            //PetMain.DrawSprite();
            RandomSprite();
            Converters.ConvertInt(Pet.Lvl, Pet.CurXP, Pet.MaxXP, Pet.CashBalance);
            
        }

        //Name Gen Stuff
        public static void RandomName()
        {
            //Random Name Gen
            string[] NameDatabase1 = { "Ba", "Bax", "Dan", "Fi", "Fix", "Fiz", "Gi", "Gix", "Giz", "Gri", "Gree", "Greex", "Grex", "Ja", "Jax", "Jaz", "Jex", "Ji", "Jix", "Ka", "Kax", "Kay", "Kaz", "Ki", "Kix", "Kiz", "Klee", "Kleex", "Kwee", "Kweex", "Kwi", "Kwix", "Kwy", "Ma", "Max", "Ni", "Nix", "No", "Nox", "Qi", "Rez", "Ri", "Ril", "Rix", "Riz", "Ro", "Rox", "So", "Sox", "Vish", "Wi", "Wix", "Wiz", "Za", "Zax", "Ze", "Zee", "Zeex", "Zex", "Zi", "Zix", "Zot" };
            string[] NameDatabase2 = { "b", "ba", "be", "bi", "d", "da", "de", "di", "e", "eb", "ed", "eg", "ek", "em", "en", "eq", "ev", "ez", "g", "ga", "ge", "gi", "ib", "id", "ig", "ik", "im", "in", "iq", "iv", "iz", "k", "ka", "ke", "ki", "m", "ma", "me", "mi", "n", "na", "ni", "q", "qa", "qe", "qi", "v", "va", "ve", "vi", "z", "za", "ze", "zi", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            string[] NameDatabase3 = { "ald", "ard", "art", "az", "azy", "bit", "bles", "eek", "eka", "et", "ex", "ez", "gaz", "geez", "get", "giez", "iek", "igle", "ik", "il", "in", "ink", "inkle", "it", "ix", "ixle", "lax", "le", "lee", "les", "lex", "lyx", "max", "maz", "mex", "mez", "mix", "miz", "mo", "old", "rax", "raz", "reez", "rex", "riez", "tee", "teex", "teez", "to", "uek", "x", "xaz", "xeez", "xik", "xink", "xiz", "xonk", "yx", "zeel", "zil" };

            Random RandName = new Random();
            string Temp = NameDatabase1[RandName.Next(0, NameDatabase1.Length)] + NameDatabase2[RandName.Next(0, NameDatabase2.Length)] + NameDatabase3[RandName.Next(0, NameDatabase3.Length)];
            Pet.PetName = Temp;
        }

        public static void RandomSprite()
        {
            string[] spriteDb1 = { "g4148", "g4174", "g4205", "g4238", "path4249" };

            Random randSprite = new Random();
            string temp = spriteDb1[randSprite.Next(0, spriteDb1.Length)];
            Pet.mySprite = temp;

        }

        public static void NewPet()
        {
            if (PetMenu.MiscMenu["new"].Cast<CheckBox>().CurrentValue)
            {
                FirstRun();
                Notifications.Show(new SimpleNotification("PokeBuddy", "New Pet Adopted!"));
                PetMenu.MiscMenu["new"].Cast<CheckBox>().CurrentValue = false;
                Converters.ConvertInt(Pet.Lvl, Pet.CurXP, Pet.MaxXP, Pet.CashBalance);
            }
        }

        public static void ManualSave()
        {
            if (PetMenu.MiscMenu["save"].Cast<CheckBox>().CurrentValue)
            {
                Notifications.Show(new SimpleNotification("PokeBuddy", "Saving..."));
                PetMenu.MiscMenu["save"].Cast<CheckBox>().CurrentValue = false;
                Converters.ConvertInt(Pet.Lvl, Pet.CurXP, Pet.MaxXP, Pet.CashBalance);
                Notifications.Show(new SimpleNotification("PokeBuddy", "Progress Saved!"));
            }
        }
    }
}
