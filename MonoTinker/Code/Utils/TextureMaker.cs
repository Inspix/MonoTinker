using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.GameObjects;
using MonoTinker.Code.Managers;

namespace MonoTinker.Code.Utils
{
    public class TextureMaker
    {
        public static Texture2D Arrow(GraphicsDevice device,int size,Color? top = null,Color? tip = null, Color? bottom = null)
        {
            RenderTarget2D target = new RenderTarget2D(device, size, size);
            VertexPositionColor[] vertices = new VertexPositionColor[3];
            
            BasicEffect effect = new BasicEffect(device);
            effect.Projection = Matrix.CreateOrthographic(size, size, -1,1);
            effect.VertexColorEnabled = true;
            vertices[0].Position = new Vector3(-size / 2f, size / 2f, 0);
            vertices[0].Color = top ?? Color.Transparent;
            vertices[1].Position = new Vector3(size / 2f, 0,0);
            vertices[1].Color = tip ?? Color.White;
            vertices[2].Position = new Vector3(-size / 2f, -size / 2f, 0);
            vertices[2].Color = bottom ?? Color.Transparent;
            
            device.SetRenderTarget(target);
            device.Clear(Color.Transparent);
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 1, VertexPositionColor.VertexDeclaration);
            }
            device.SetRenderTarget(null);
            return target;
        }

        /*public static Texture2D ClassSplash(GraphicsDevice device, CharacterClass clClass)
        {
            Sprite body, head, torso, legs, feet, hands, weapon;

            switch (clClass)
            {
                case CharacterClass.Archer:
                    body = AssetManager.Instance.Get<Animation>(An.Slash.BodyHuman + "Left")[4].DirectClone();
                    head = AssetManager.Instance.Get<Animation>(An.Slash.HeadPlateArmorHelm + "Left")[4].DirectClone();
                    torso = AssetManager.Instance.Get<Animation>(An.Slash.TorsoPlateArmor + "Left")[4].DirectClone();
                    legs = AssetManager.Instance.Get<Animation>(An.Slash.LegsPlateArmorPants + "Left")[4].DirectClone();
                    feet = AssetManager.Instance.Get<Animation>(An.Slash.FeetPlateArmorShoes + "Left")[4].DirectClone();
                    hands =
                        AssetManager.Instance.Get<Animation>(An.Slash.HandPlateArmorGloves + "Left")[4].DirectClone();
                    weapon = AssetManager.Instance.Get<Animation>(An.Slash.WeaponLongSword + "Left")[4].DirectClone();
                    break;
                case CharacterClass.Wizard:
                    body = AssetManager.Instance.Get<Animation>(An.Slash.BodyHuman + "Left")[4].DirectClone();
                    head = AssetManager.Instance.Get<Animation>(An.Slash.HeadRobeHood + "Left")[4].DirectClone();
                    torso = AssetManager.Instance.Get<Animation>(An.Slash.TorsoRobeShirt + "Left")[4].DirectClone();
                    legs = AssetManager.Instance.Get<Animation>(An.Slash.LegsRobeSkirt + "Left")[4].DirectClone();
                    feet = AssetManager.Instance.Get<Animation>(An.Slash.FeetShoesBrown + "Left")[4].DirectClone();
                    hands =
                        AssetManager.Instance.Get<Animation>(An.Slash.HandPlateArmorGloves + "Left")[4].DirectClone();
                    weapon = AssetManager.Instance.Get<Animation>(An.Slash.WeaponLongSword + "Left")[4].DirectClone();
                    break;
                default:

                    body = AssetManager.Instance.Get<Animation>(An.Slash.BodyHuman + "Left")[4].DirectClone();
                    head = AssetManager.Instance.Get<Animation>(An.Slash.HeadLeatherHat + "Left")[4].DirectClone();
                    torso = AssetManager.Instance.Get<Animation>(An.Slash.TorsoLeatherArmor + "Left")[4].DirectClone();
                    legs = AssetManager.Instance.Get<Animation>(An.Slash.LegsPants + "Left")[4].DirectClone();
                    feet = AssetManager.Instance.Get<Animation>(An.Slash.FeetShoesBrown + "Left")[4].DirectClone();
                    weapon = AssetManager.Instance.Get<Animation>(An.Slash.we + "Left")[4].DirectClone();

                    break;
            }
            SpriteBatch sb = new SpriteBatch(device);
            RenderTarget2D result = new RenderTarget2D(device, body.DefaultSource.Width, body.DefaultSource.Height);
        }*/
    }
}
