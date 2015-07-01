using System;

namespace MonoTinker.Code.Components.GameComponents
{
    public enum AttributeType
    {
        Block,
        Charisma,
        Dodge,
        Stamina,
        Health,
        Mana,
        Spirit
        // TODO: Add more attributes
    }

    public class AttributeStat
    {
        private readonly AttributeType type;
        private double currentValue;
        private readonly double maxValue;

        public AttributeStat(AttributeType type, Func<double> calculation)
        {
            this.type = type;
            this.maxValue = this.InitMax();
        }

        public double Current
        {
            get
            {
                return this.currentValue;
            }
        }

        public void Calculate(Stats stats)
        {
            double result = Calculation(stats);
            if (result > this.maxValue)
            {
                this.currentValue = this.maxValue;
                return;
            }
            this.currentValue = result;
        }

        public Func<Stats,double> Calculation { get; set; }

        private double InitMax()
        {
            switch (this.type)
            {
                case AttributeType.Block:
                    return 95;
                case AttributeType.Charisma:
                    return 75;
                case AttributeType.Dodge:
                    return 50;
                case AttributeType.Stamina:
                    return 200;
                    case AttributeType.Health:
                    return 10000;
                default:
                    return 33;
            }
        }
    }
}
