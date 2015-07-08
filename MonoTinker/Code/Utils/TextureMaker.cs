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

        public static Texture2D CheckBox(GraphicsDevice device, int size, Color? middle = null, Color? tip = null, Color? bottom = null)
        {
            
            RenderTarget2D target = new RenderTarget2D(device, size, size);
            VertexPositionColor[] vertices = new VertexPositionColor[6];
           // device.RasterizerState = RasterizerState.CullNone;
            BasicEffect effect = new BasicEffect(device);
            effect.Projection = Matrix.CreateOrthographic(size, size, -1, 1);
            effect.VertexColorEnabled = true;
            vertices[0].Position = new Vector3(-size / 4f, size/4f,0);
            vertices[0].Color = tip ?? Color.Transparent;
            vertices[1].Position = new Vector3(size/32f, -size/32f, 0);
            vertices[1].Color =  middle ?? Color.Transparent;
            vertices[2].Position = new Vector3(size/10.66f, -size/2f, 0);
            vertices[2].Color = bottom ??Color.White;

            vertices[3].Position = new Vector3(size / 32f, -size / 32f, 0);
            vertices[3].Color = middle ?? Color.Transparent;
            vertices[4].Position = new Vector3(size/2.66f, size/2f, 0);
            vertices[4].Color = tip ?? Color.White;
            vertices[5].Position = new Vector3(size / 10.66f, -size / 2f, 0);
            vertices[5].Color = bottom ?? Color.White;

            device.SetRenderTarget(target);
            device.Clear(Color.Transparent);
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 2, VertexPositionColor.VertexDeclaration);
            }
            device.SetRenderTarget(null);
            return target;
        }

        public static Texture2D ClassSplash(GraphicsDevice device, CharacterClass clClass)
        {
            Sprite body, head, torso, legs, feet, shoulders = null, hands = null, weapon = null,arrow = null;

            switch (clClass)
            {
                case CharacterClass.Archer:
                    body = AssetManager.Instance.Get<Animation>(An.Bow.BodyHuman + "Left")[4].DirectClone();
                    head = AssetManager.Instance.Get<Animation>(An.Bow.HeadLeatherHat + "Left")[4].DirectClone();
                    torso = AssetManager.Instance.Get<Animation>(An.Bow.TorsoLeatherArmor + "Left")[4].DirectClone();
                    legs = AssetManager.Instance.Get<Animation>(An.Bow.TorsoLeatherArmor + "Left")[4].DirectClone();
                    feet = AssetManager.Instance.Get<Animation>(An.Bow.FeetShoesBrown + "Left")[4].DirectClone();
                    weapon = AssetManager.Instance.Get<Animation>(An.Bow.WeaponBow + "Left")[4].DirectClone();
                    arrow = AssetManager.Instance.Get<Animation>(An.Bow.WeaponArrow + "Left")[4].DirectClone();

                    break;
                case CharacterClass.Wizard:
                    body = AssetManager.Instance.Get<Animation>(An.SpellCast.BodyHuman + "Down")[4].DirectClone();
                    head = AssetManager.Instance.Get<Animation>(An.SpellCast.HeadRobeHood + "Down")[4].DirectClone();
                    torso = AssetManager.Instance.Get<Animation>(An.SpellCast.TorsoRobeShirt + "Down")[4].DirectClone();
                    legs = AssetManager.Instance.Get<Animation>(An.SpellCast.LegsRobeSkirt + "Down")[4].DirectClone();
                    feet = AssetManager.Instance.Get<Animation>(An.SpellCast.FeetShoesBrown + "Down")[4].DirectClone();
                    hands = AssetManager.Instance.Get<Animation>(An.SpellCast.HandPlateArmorGloves + "Down")[4].DirectClone();
                    break;
                default:

                    body = AssetManager.Instance.Get<Animation>(An.Slash.BodyHuman + "Down")[1].DirectClone();
                    head = AssetManager.Instance.Get<Animation>(An.Slash.HeadPlateArmorHelm + "Down")[1].DirectClone();
                    torso = AssetManager.Instance.Get<Animation>(An.Slash.TorsoPlateArmor + "Down")[1].DirectClone();
                    legs = AssetManager.Instance.Get<Animation>(An.Slash.LegsPlateArmorPants + "Down")[1].DirectClone();
                    feet = AssetManager.Instance.Get<Animation>(An.Slash.FeetPlateArmorShoes + "Down")[1].DirectClone();
                    shoulders = AssetManager.Instance.Get<Animation>(An.Slash.TorsoPlateArmorShoulders + "Down")[1].DirectClone();
                    hands =
                        AssetManager.Instance.Get<Animation>(An.Slash.HandPlateArmorGloves + "Down")[1].DirectClone();
                    weapon = AssetManager.Instance.Get<Animation>(An.Slash.WeaponLongSword + "Down")[1].DirectClone();
                    break;
            }
            int widht = 64, height = 64;
            if (weapon != null)
            {
                if (weapon.SourceHeight > 64)
                {
                    weapon.Position -= new Vector2(64);
                }
            }
            SpriteBatch sb = new SpriteBatch(device);
            RenderTarget2D result = new RenderTarget2D(device, widht, height);
            device.SetRenderTarget(result);
            device.Clear(Color.TransparentBlack);
            sb.Begin();
            body.Draw(sb);
            feet.Draw(sb);
            legs.Draw(sb);
            if (shoulders != null)
            {
                shoulders.Draw(sb);
            }
            torso.Draw(sb);
            head.Draw(sb);
            if (hands != null)
            {
                hands.Draw(sb);
            }
            if (weapon != null)
            {
                weapon.Draw(sb);
            }
            if (arrow != null)
            {
                arrow.Draw(sb);
            }
            if (clClass == CharacterClass.Wizard)
            {
                Sprite glow = AssetManager.Instance.Get<Sprite>("lighting").DirectClone(true);
                glow.Clr = new Color(0,25,100,10);
                glow.ScaleF = 0.25f;
                glow.Position = new Vector2(0);
                glow.Draw(sb);
            }
            sb.End();
            device.SetRenderTarget(null);
            return result;
        }
    }
}
