namespace MonoTinker.Code.Components.UI
{
    using System;
    using System.Text;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Utils;
    using Interfaces;

#if DEBUG
    using Elements.DebugGraphics;
#endif

    /// <summary>
    /// Basic text class used to draw letters to the screen. Extends <see cref="Fadeable"/> and implements <seealso cref="ITransformable"/>
    /// </summary>
    public class Text : Fadeable , ITransformable
    {
        /// <summary>
        /// Stores a reference to a <see cref="SpriteFont"/>.
        /// </summary>
        private readonly SpriteFont font;

        /// <summary>
        /// Stores the <see cref="Text"/> bounding box. Used to track if the mouse is over it.
        /// </summary>
        private Rectangle bounds;

        /// <summary>
        /// Stores the position of the <see cref="Text"/>
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// Stores the current scale of the <see cref="Text"/>
        /// </summary>
        private Vector2 scale;

        /// <summary>
        /// Stores the contents of the <see cref="Text"/>.
        /// </summary>
        private StringBuilder contents;

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="font">Font to draw with.</param>
        /// <param name="position">Position of the Text.</param>
        /// <param name="contents">The contents of the Text.(message)</param>
        /// <param name="alpha">The transperancy of the Text.</param>
        /// <param name="isVisible">If the text should be Visible on initialisation.</param>
        public Text(SpriteFont font, Vector2 position, string contents, float alpha = 1, bool isVisible = true)
        {
            this.font = font;
            this.contents = new StringBuilder(contents);
            this.ScaleF = 1;
            this.isVisible = isVisible;
            this.Position = position;
            this.bounds = new Rectangle(this.Position.ToPoint(),this.Size.ToPoint());
            this.Clr = Color.White;
            this.alpha = alpha;
            this.DefaultAlpha = 1;
        }

        /// <summary>
        /// Get or set the OnLabelChange delegate
        /// </summary>
        public Action<Text> OnLabelChange { get; set; }

        /// <summary>
        /// Gets or sets the position of the <see cref="Text"/>.
        /// <remarks>Also recalculates its bounding box.</remarks>
        /// </summary>
        public Vector2 Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
                bounds.Location = position.ToPoint();
            }
        }

        /// <summary>
        /// Gets or sets the position of the <see cref="Text"/> on the X axis
        /// </summary>
        public float PosX
        {
            get { return this.Position.X; }
            set
            {
                this.Position = new Vector2(value, Position.Y);
            }
        }

        /// <summary>
        /// Gets or sets the position of the <see cref="Text"/> on the Y axis
        /// </summary>
        public float PosY
        {
            get { return this.Position.Y; }
            set
            {
                this.Position = new Vector2(Position.X, value);
            }
        }

        /// <summary>
        /// Gets or sets the rotation of the <see cref="Text"/>.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets the current size of the <see cref="Text"/>.
        /// </summary>
        public Vector2 Size
        {
            get { return this.font.MeasureString(this.Contents)*Scale; }
        }

        /// <summary>
        /// Sets the scale of the <see cref="Text"/>(X,Y individualy).
        /// <remarks>Also recalculates its bounding box.</remarks>
        /// </summary>
        public Vector2 Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;
                bounds.Size = Size.ToPoint();
            }
        }

        /// <summary>
        /// Gets or sets the 1/1 Scale of the <see cref="Text"/>.
        /// <remarks>
        /// Gets the approximate scale between X and Y scales
        /// Sets the X and Y scales to the value
        /// </remarks>
        /// </summary>
        public float ScaleF
        {
            get { return (this.Scale.X + this.Scale.Y) / 2; }
            set { this.Scale = new Vector2(value, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="Color"/> tint of the <see cref="Text"/>.
        /// </summary>
        public Color Clr { get; set; }

        /// <summary>
        /// Gets or sets the contents of the <see cref="Text"/> instance.
        /// <remarks>Invokes the <see cref="OnLabelChange"/> delegate if there are any subscribers</remarks>
        /// </summary>
        public string Contents
        {
            get
            {
                return this.contents.ToString();
            }
            set {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.contents = new StringBuilder("...");
                    if (OnLabelChange == null)
                    {
                        return;
                    }
                    OnLabelChange.Invoke(this);
                }
                else
                {
                    if (value == this.Contents)
                    {
                        return;
                    }
                    this.contents = new StringBuilder(value);
                    if (OnLabelChange == null)
                    {
                        return;
                    }
                    OnLabelChange.Invoke(this);
                }
            }
        }

        /// <summary>
        /// Checks if the bounding box of the <see cref="Text"/> contains given point.
        /// </summary>
        /// <param name="pos">Point to check with</param>
        /// <returns>If the point is inside the <see cref="Text"/> bounds</returns>
        public bool Contains(Vector2 pos)
        {
            return bounds.Contains(pos - new Vector2(0,5.5f)); // Some offset for the strange measuring of the font
        }

        /// <summary>
        /// Append a string to the current content of the <see cref="Text"/> instance.
        /// </summary>
        /// <remarks>Invokes the <see cref="OnLabelChange"/> delegate if there are any subscribers.</remarks>
        /// <param name="text"><see cref="string"/> to append.</param>
        public void Append(string text)
        {
            this.contents.Append(text);
            if (OnLabelChange == null)
            {
                return;
            }
            OnLabelChange.Invoke(this);
        }

        /// <summary>
        /// Append a character to the current content of the <see cref="Text"/> instance.
        /// </summary>
        /// <remarks>Invokes the <see cref="OnLabelChange"/> delegate if there are any subscribers.</remarks>
        /// <param name="c"><see cref="char"/> to append.</param>
        public void Append(char c)
        {
            this.contents.Append(c);
        }

        /// <summary>
        /// Removes the last character of the current content.
        /// </summary>
        public void RemoveLast()
        {
            try
            {
                this.contents.Remove(contents.Length - 1, 1);

            }
            catch (Exception)
            {
                Debug.Warning("Text: Tried to remove a character from a empty content");
            }
        }

        /// <summary>
        /// Updates the Label based on time snapshot
        /// <remarks>Should be moved to the <see cref="Fadeable"/> class.</remarks>
        /// </summary>
        /// <param name="gameTime">GameTime snapshot.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (this.fadeIn || this.fadeOut)
            {
                this.Transition();
            }
        }

        /// <summary>
        /// Draw the <see cref="Text"/> with given <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> to draw with.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.font,this.Contents,this.Position,this.Clr * this.Alpha,this.Rotation,Vector2.Zero, this.Scale,SpriteEffects.None, 0);
#if DEBUG
            DebugShapes.DrawRectagnle(spriteBatch,this.Position,this.Size,1f,Color.Red);
#endif
        }
    }
}
