
namespace MonoTinker.Code.Utils
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public static class Debug
    {
        private static Form debugForm;
        private static RichTextBox warningBox;
        private static RichTextBox ErrorBox;

        static void Init()
        {
            debugForm = new Form() { Text = "MonoTinker Debug Form"};
            debugForm.Width = 610;
            debugForm.Height = 510;
            warningBox = new RichTextBox();
            warningBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            warningBox.ForeColor = Color.DarkOrange;
            warningBox.Multiline = true;
            warningBox.AutoScrollOffset = new Point(20,0);
            warningBox.Location = new Point(1,1);
            warningBox.Width = 300;
            warningBox.Height = 500;
            ErrorBox = new RichTextBox();
            ErrorBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            ErrorBox.ForeColor = Color.DarkRed;
            ErrorBox.Multiline = true;
            ErrorBox.Location = new Point(310, 1);
            ErrorBox.Width = 300;
            ErrorBox.Height = 500;
            ErrorBox.Enabled = false;

            debugForm.Controls.Add(warningBox);
            debugForm.Controls.Add(ErrorBox);
            debugForm.Show();
        }

        public static void Error(string text)
        {
            if (debugForm == null)
            {
                Init();
            }
            if (!ErrorBox.IsDisposed)
            {
                ErrorBox.AppendText(text + "\n");
            }
        }

        public static void Error(string text,params object[] args)
        {
            Error(string.Format(text, args) + "\n");
        }


        public static void Warning(string text)
        {
            if (debugForm == null)
            {
                Init();
            }
            if (!warningBox.IsDisposed)
            {
                warningBox.AppendText(text + "\n");
            }
        }

        public static void Warning(string text,params object[] args)
        {
            Warning(string.Format(text, args) + "\n");
        }

        public static void Message(string text)
        {
            if (debugForm == null)
            {
                Init();
            }
            if (!warningBox.IsDisposed)
            {
                warningBox.SelectionColor = Color.Blue;
                warningBox.AppendText(text + "\n");
                warningBox.SelectionColor = warningBox.ForeColor;
            }
        }

        public static void Message(string text, params object[] args)
        {
            Message(string.Format(text, args) + "\n");
        }
    }
}
