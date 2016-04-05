using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Notifications;

namespace PokeBuddy
{
    internal class DrawStuff
    {
        public static int xpos = 0;
        public static int ypos = 0;
        public static int drawX1 = (int)(Drawing.Width * 0.68);
        public static int drawY1 = (int)(Drawing.Height * 0.97);
        public static int drawX2 = (int)(Drawing.Width * 0.68);
        public static int drawY2 = (int)(Drawing.Height * 0.97) - 40;
        public static int myTeamDmgX = (int)(Drawing.Width * 0.68);
        public static int myTeamDmgY = (int)(Drawing.Height * 0.97) - 20;
        public static int enemyTeamDmgX = (int)(Drawing.Width * 0.68);
        public static int enemyTeamDmgY = (int)(Drawing.Height * 0.97) - 60;
        public static string tempSprite = null;

        //Sprite
        private static readonly TextureLoader TextureLoader = new TextureLoader();


        public static Sprite sprite = null;
        //public static string petSprite;

        internal static void DrawInit()
        {
            Drawing.OnEndScene += Drawing_OnDraw;

        }

        internal static void Drawing_OnDraw(EventArgs args)
        {
            if (!PetMenu.MiscMenu["track"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            if (sprite == null)
            {

                DrawSprite();
                tempSprite = Pet.mySprite.ToString();
                //sprite.Hide();
            }

            xpos = PetMenu.DrawingMenu["xpos"].Cast<Slider>().CurrentValue;
            ypos = PetMenu.DrawingMenu["ypos"].Cast<Slider>().CurrentValue;

            if (PetMenu.DrawingMenu["drawstats"].Cast<CheckBox>().CurrentValue && !PetMenu.DrawingMenu["disDraw"].Cast<CheckBox>().CurrentValue)
            {
                //Drawing Box

                var borderColor = Color.Black;
                var bgColor = Color.LightGray;
                var textColor = Color.Black;


                //Left
                Drawing.DrawLine(xpos - 7,
                                 ypos + 120,
                                 xpos - 7,
                                 ypos - 90, 3,
                                 borderColor);
                //Top
                Drawing.DrawLine(xpos - 8,
                                 ypos - 92,
                                 xpos + 170,
                                 ypos - 92, 3,
                                 borderColor);
                //Right
                Drawing.DrawLine(xpos + 168,
                                 ypos + 120,
                                 xpos + 168,
                                 ypos - 91, 3,
                                 borderColor);
                //Bottom
                Drawing.DrawLine(xpos - 8,
                                 ypos + 120,
                                 xpos + 170,
                                 ypos + 120, 3,
                                 borderColor);
                //Drawing Background

                Drawing.DrawLine(xpos + 81,
                                 ypos + 119,
                                 xpos + 81,
                                 ypos - 90, 171,
                                 bgColor);

                //Drawing Stats

                Drawing.DrawText(xpos + 25, ypos - 85, System.Drawing.Color.DarkRed, "PokeBuddy BETA");
                Drawing.DrawText(xpos, ypos + 20, textColor, "Pet Name: " + Pet.PetName);
                Drawing.DrawText(xpos, ypos + 40, textColor, "Level: " + (int)Pet.Lvl);
                Drawing.DrawText(xpos, ypos + 60, textColor, "XP: " + (int)Pet.CurXP + "/" + (int)Pet.MaxXP);
                Drawing.DrawText(xpos, ypos + 80, textColor, "PetBux: $" + (int)Pet.CashBalance);
                if (Pet.Sick)
                {
                    Drawing.DrawText(xpos, ypos + 100, System.Drawing.Color.Red, "Pet Health: Sick");
                }
                else
                {
                    Drawing.DrawText(xpos, ypos + 100, System.Drawing.Color.Green, "Pet Health: Fine");
                }
            }
            if (PetMenu.DrawingMenu["drawsprites"].Cast<CheckBox>().CurrentValue && sprite != null && !PetMenu.DrawingMenu["disDraw"].Cast<CheckBox>().CurrentValue)
            {
                
                DrawCurSprite();
                
                //sprite.X = xpos + 35;
                //sprite.Y = ypos - 60;

            }
            else
            {
                return;
            }


    }
        public static void DrawCurSprite()
        {
            if (Pet.mySprite.ToString() != tempSprite)
            {
                sprite = null;
                TextureLoader.Dispose();
                tempSprite = Pet.mySprite.ToString();
            }
            else
            {
                sprite.Draw(new Vector2(DrawStuff.xpos + 43, DrawStuff.ypos - 70));
            }
        }


        public static void DrawSprite()
        {
            xpos = PetMenu.DrawingMenu["xpos"].Cast<Slider>().CurrentValue;
            ypos = PetMenu.DrawingMenu["ypos"].Cast<Slider>().CurrentValue;

            //Draw Sprites
            //Get Pet Sprite
            GetPetSprite();
            sprite.Scale = new Vector2(0.2f, 0.2f);
            //sprite.Draw(new Vector2(xpos + 20, ypos - 75));

        }
        public static void GetPetSprite()
        {
            switch (Pet.mySprite)
            {
                case "zorua":
                    TextureLoader.Load("zorua", Resource1.zorua);
                    sprite = new Sprite(() => TextureLoader["zorua"]);
                    //sprite = new Sprite(Resources.Resource1.g4148, new Vector2(xpos + 20, ypos - 75));
                    break;
                default:
                    TextureLoader.Load("zorua", Resource1.zorua);
                    sprite = new Sprite(() => TextureLoader["zorua"]);
                    //sprite = new Sprite(Resources.Resource1.g4205, new Vector2(xpos + 20, ypos - 75));
                    Pet.mySprite = "zorua";
                    Converters.ConvertInt(Pet.Lvl, Pet.CurXP, Pet.MaxXP, Pet.CashBalance);
                    break;
            }
        }
    }
}
