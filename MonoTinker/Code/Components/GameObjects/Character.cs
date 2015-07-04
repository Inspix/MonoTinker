using Microsoft.Xna.Framework;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.GameComponents;
using MonoTinker.Code.Components.UI;
using MonoTinker.Code.Managers;

namespace MonoTinker.Code.Components.GameObjects
{
    public enum CharacterType
    {
        Player,
        Enemy,
        Neutral,
        NPC,
        Friendly
    }

    public enum CharacterClass
    {
        Warrior,Wizard,Archer
    }

    public abstract class Character : Entity
    {
        private Text name;
        private Stats stats;
        private CharacterType type;
        private AttributeStat health;
        private AttributeStat mana;
        private AttributeStat spirit;
        
        protected Character(AnimationController animation, Vector2 position,Stats stats,CharacterType type = CharacterType.Player) : base(animation, position)
        {
            this.stats = stats;
            this.type = type;
            this.health.Calculation = GameManager.CalculateHealth;
            this.mana.Calculation = GameManager.CalculateMana;
            this.spirit.Calculation = GameManager.CalculateSpirit;
            this.stats.OnStatChange = CalculateStats;
        }

        public string State
        {
            get { return this.animation.CurrentState; }
            set { this.animation.ChangeState(value); }
        }

        public bool ResetOnStateChange
        {
            get { return base.animation.ResetOnStateChange; }
            set { base.animation.ResetOnStateChange = value; }
        }

        public Stats CharacterStats
        {
            get { return this.stats; }
        }

        public double Health
        {
            get { return this.health.Current; }
        }

        public double Mana
        {
            get { return this.mana.Current; }
        }

        public double Spirit
        {
            get { return this.spirit.Current; }
        }

        protected void CalculateStats()
        {
            this.health.Calculate(this.stats);
            this.mana.Calculate(this.stats);
            this.spirit.Calculate(this.stats);
        }
        
        
    }
}
