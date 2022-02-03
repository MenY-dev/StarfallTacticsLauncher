using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarfallTactics.StarfallTacticsLauncher
{
    public class ConsoleReader : TextWriter
    {
        private RichTextBox textBox;

        public ConsoleReader(RichTextBox textBox)
        {
            this.textBox = textBox;
        }

        public override void Write(char value)
        {
            AppendText(value.ToString());
        }

        public override void Write(string value)
        {
            AppendText(value);
        }

        public override void WriteLine(string value)
        {
            AppendText(value + NewLine);
        }

        private void AppendText(string value)
        {
            if (textBox.InvokeRequired)
            {
                textBox.BeginInvoke((Action)(() =>
                {
                    AppendText(value);
                }));
            }
            else
            {
                textBox.AppendText(value);
                textBox.SelectionStart = textBox.Text.Length;
                textBox.ScrollToCaret();
            }
        }

        public override Encoding Encoding => Encoding.UTF8;
    }
}
