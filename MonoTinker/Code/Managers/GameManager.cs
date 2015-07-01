using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.GameComponents;
using MonoTinker.Code.Components.UI;

namespace MonoTinker.Code.Managers
{
    public class GameManager
    {
        private static ItemTile item;
        private static bool itemMoving;
        private static double cooldown = TimeSpan.FromSeconds(0.2).TotalSeconds;
        private static double timeElapsed;
        private static bool itemPut;
        

        public static void GetItemFromInventory(Inventory source, int itemslot)
        {
            item = source[itemslot].DirectCloneFromInventory();
            source.RemoveItemFromSlot(itemslot+1);
            itemMoving = true;
        }

        public static void PutItemToInventory(Inventory destination, int itemslot)
        {
            if (item == null)
            {
                return;
            }
            itemPut = true;
            item.Selected = false;
            destination.AddItemToSlot(item,itemslot+1);
            item = null;
        }

        public static bool ItemMoving
        {
            get
            {
                return itemMoving;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (item != null)
            {
                item.Draw(spriteBatch);
            }
        }

        public static void Update(GameTime gameTime)
        {
            if (itemPut)
            {
                timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeElapsed >= cooldown)
                {
                    itemPut = false;
                    itemMoving = false;
                }
            }
            if (item != null)
            {
                item.Update(gameTime);
            }
        }



        public static double CalculateHealth(Stats stats)
        {
            return (stats.Vitality*15d)+(stats.Strenght*0.25f);
        }

        public static double CalculateMana(Stats stats)
        {
            return (stats.Intellect*20) + (stats.Wisdom*0.35);
        }

        public static double CalculateSpirit(Stats stats)
        {
            return stats.TotalStats/2d;
        }
    }
}
