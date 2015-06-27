namespace MonoTinker.Code.Components.Elements
{
    using System;

    /// <summary>
    /// Animation event arguments
    /// </summary>
    public class AnimationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationEventArgs"/> class.
        /// </summary>
        /// <param name="statename">Animation state name</param>
        public AnimationEventArgs(string statename)
        {
            this.StateName = statename;
        }

        /// <summary>
        /// Gets the animation state name
        /// </summary>
        public string StateName { get; internal set; }
    }
}
