using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using EloBuddy.SDK.Notifications;
using EloBuddy.SDK.Menu.Values;

namespace PokeBuddy
{
    internal class Shop
    {
        public static void ShopBuy()
        {

            if (!PetMenu.ShopMenu["food1"].Cast<CheckBox>().CurrentValue && (!PetMenu.ShopMenu["food2"].Cast<CheckBox>().CurrentValue))
            {
                return;
            }

            if (PetMenu.ShopMenu["food1"].Cast<CheckBox>().CurrentValue)
            {
                var CanBuy = GameAssets.PurchaseAvailable(GameAssets.med);

                if (CanBuy)
                {
                    if (!Pet.Sick)
                    {
                        Notifications.Show(new SimpleNotification("PokeBuddy", "Your pet is not Sick!"));
                        PetMenu.ShopMenu["food1"].Cast<CheckBox>().CurrentValue = false;
                        return;
                    }
                    else
                    {
                        Notifications.Show(new SimpleNotification("PokeBuddy", GameAssets.med.Name + " Bought!"));
                        Notifications.Show(new SimpleNotification("PokeBuddy", "Your pet has been cured!"));
                        Pet.Sick = false;

                    }

                    //Deduct Cost
                    Pet.CashBalance -= GameAssets.med.Cost;
                }
                else
                {
                    Notifications.Show(new SimpleNotification("PokeBuddy", "Not enough PetBux!"));
                }

                PetMenu.ShopMenu["food1"].Cast<CheckBox>().CurrentValue = false;
            }

            if (PetMenu.ShopMenu["food2"].Cast<CheckBox>().CurrentValue)
            {
                var CanBuy = GameAssets.PurchaseAvailable(GameAssets.expdouble);

                if (CanBuy)
                {
                    if (Pet.FoodXP)
                    {
                        Notifications.Show(new SimpleNotification("PokeBuddy", "Cannot Buy Twice!"));
                        PetMenu.ShopMenu["food2"].Cast<CheckBox>().CurrentValue = false;
                        return;
                    }
                    else
                    {
                        Notifications.Show(new SimpleNotification("PokeBuddy", GameAssets.expdouble.Name + " Bought!"));
                        Pet.FoodXP = true;
                        Pet.XPMulti = 2;
                    }

                    //Deduct Cost
                    Pet.CashBalance -= GameAssets.expdouble.Cost;
                }
                else
                {
                    Notifications.Show(new SimpleNotification("PokeBuddy", "Not enough PetBux!"));
                }

                PetMenu.ShopMenu["food2"].Cast<CheckBox>().CurrentValue = false;
            }

        }
    }
}
