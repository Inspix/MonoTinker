using System.Collections.Generic;

namespace MonoTinker.Code.Components.GameComponents
{
    public class Stats
    {
        private int strength;
        private int agility;
        private int vitality;
        private int intelect;
        private int wisdom;

        private Stats()
        {
        }

        public Stats(int str, int agi, int vit, int intelect, int wis)
        {
            this.strength = str;
            this.agility = agi;
            this.vitality = vit;
            this.intelect = intelect;
            this.wisdom = wis;
        }

        public int Strenght
        {
            get { return this.strength; }
            set
            {
                //TODO Softcap of strenght
                this.strength = value;
            }
            
        }

        public int Agility
        {
            get { return this.agility; }
            set
            {
                //TODO Softcap of Agility
                this.agility = value;
            }
        }
        public int Vitality
        {
            get { return this.vitality; }
            set
            {
                //TODO Softcap of Vitality
                this.vitality = value;
            }
        }
        public int Intellect
        {
            get { return this.intelect; }
            set
            {
                //TODO Softcap of Intelect
                this.intelect = value;
            }
        }
        public int Wisdom
        {
            get { return this.wisdom; }
            set
            {
                //TODO Softcap of Wisdom
                this.wisdom = value;
            }
        }

        public static Stats Zero
        {
            get { return new Stats();}
        }

        public int TotalStats
        {
            get { return this.strength + this.agility + this.intelect + this.vitality + this.wisdom; }
        }

    }
}
