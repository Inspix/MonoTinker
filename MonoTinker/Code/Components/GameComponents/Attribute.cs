namespace MonoTinker.Code.Components.GameComponents
{
    public enum AttributeType
    {
        Block,
        Charisma,
        Dodge,
        Stamina
        // TODO: Add more attributes
    }

    public class AttributeStat
    {
        private AttributeType type;
        private double currentValue;
        private double maxValue;

        public AttributeStat(double current, AttributeType type)
        {
            this.currentValue = current;
            this.type = type;
            this.maxValue = this.InitMax();
        }

        public double Current
        {
            get
            {
                return this.currentValue;
            }

            set
            {
                if (value > this.maxValue)
                {
                    this.currentValue = this.maxValue;
                    return;
                }
                this.currentValue = value;
            }

        }

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
                default:
                    return 33;
            }
        }
    }
}
