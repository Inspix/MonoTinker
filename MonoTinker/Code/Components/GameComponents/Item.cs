using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.Components.GameComponents
{
    public enum ItemType
    {
        Weapon,Armor,Consumable,Trinket
    }

    public enum ItemRarity
    {
        Trash,Common,Uncommon,Rare,Epic,Legendary
    }

    public class Item
    {
        private string name;
        private int level;
        private Stats stats;
        private ItemRarity rarity;
        private List<int> effects; //TODO item effects 

        public Item(string name, Stats stats,ItemRarity rarity = ItemRarity.Common)
        {
            this.Name = name;
            this.rarity = rarity;
            this.stats = stats;
            this.CalculateLevel();
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Debug.Error("The item name cannot be empty or null");
                    return;
                }
                this.name = value;
            }
        }

        public ItemRarity Rarity
        {
            get { return this.rarity; }
        }

        public Stats Status
        {
            get { return this.stats; }
        }

        public int StatCount
        {
            get
            {
                if (this.effects == null)
                {
                    return 5;
                }
                return 5 + this.effects.Count;
            }
        }

        private void CalculateLevel()
        {
            this.level = stats.TotalStats/30;
        }

        // TODO ItemEnchantments

        public Color RarityColor()
        {
            switch (rarity)
            {
                case ItemRarity.Common:
                    return Color.WhiteSmoke;
                case ItemRarity.Uncommon:
                    return Color.Lime;
                case ItemRarity.Rare:
                    return Color.CadetBlue;
                case ItemRarity.Epic:
                    return Color.MediumPurple;
                case ItemRarity.Legendary:
                    return Color.Orange;
                default:
                    return Color.DarkSlateGray;
            }
        }
    }
}
