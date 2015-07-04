using MonoTinker.Code.Components.GameComponents;
using MonoTinker.Code.Components.GameObjects;

namespace MonoTinker.Code.Utils
{

    public class Prefabs
    {
        
        public static Stats BaseStats(CharacterClass cClass)
        {
            switch (cClass)
            {
                    case CharacterClass.Warrior:
                    return new Stats(20, 10, 20, 5, 5);
                    case CharacterClass.Archer:
                    return new Stats(10,25,15,7,3);
                    case CharacterClass.Wizard:
                    return new Stats(5,5,12,20,20);
            }
            return Stats.Ten;
        }
    }
}
