using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Notepad
{
    public partial class main_window : Form
    {
        public bool autosave = false;
        public string filename;

        public main_window()
        {
            InitializeComponent();
            update_cursor_position_label();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private int current_line()
        {
            return file_content.GetLineFromCharIndex(
                file_content.SelectionStart
            );
        }

        private int current_char()
        {
            return file_content.SelectionStart - file_content.GetFirstCharIndexFromLine(current_line());
        }

        private void update_cursor_position_label()
        {
            cursor_position.Text = "Col " + (current_char() + 1) + " | Line " + (current_line() + 1);
        }

        private void save_file()
        {
            using (StreamWriter target_file = new StreamWriter(filename))
                target_file.Write(file_content.Text);
            
        }

        private void paste_text()
        {
            file_content.SelectedText = Clipboard.GetText();
        }

        private void cut_text()
        {
            if (copy_text())
                file_content.SelectedText = "";
            else
                file_content.Text = "";
        }

        private bool copy_text()
        {
            if (file_content.SelectedText != "")
            {
                Clipboard.SetText(file_content.SelectedText);
                return true;
            }
            else
            {
                Clipboard.SetText(file_content.Text);
                return false;
            }
        }

        private void clear_text()
        {
            if (file_content.SelectedText != "")
                file_content.SelectedText = "";
            else
                file_content.Text = "";
        }

        private void file_content_TextChanged(object sender, EventArgs e)
        {
            update_cursor_position_label();

            if (autosave)
                save_file();
        }

        private void autosaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autosave = autosave ? false : true;
            autosaveToolStripMenuItem.Checked = autosave ? true : false;
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clear_text();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paste_text();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copy_text();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cut_text();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialog_result = font_dialog.ShowDialog();

            if (dialog_result == DialogResult.OK)
                file_content.Font = font_dialog.Font;
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialog_result = save_file_dialog.ShowDialog();

            if (dialog_result == DialogResult.OK)
            {
                file_content.Text = "";
                File.Create(save_file_dialog.FileName);
                filename = save_file_dialog.FileName;
            }
        }

        private void openAFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialog_result = open_file_dialog.ShowDialog();

            if (dialog_result == DialogResult.OK)
            {
                if (File.Exists(open_file_dialog.FileName)) {
                    filename = open_file_dialog.FileName;
                    file_content.Text = new StreamReader(filename).ReadToEnd();
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save_file();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialog_result = save_file_dialog.ShowDialog();

            if (dialog_result == DialogResult.OK)
            {
                if (File.Exists(save_file_dialog.FileName) == false)
                {
                    File.Create(save_file_dialog.FileName);
                }
                filename = save_file_dialog.FileName;
                save_file();
            }
        }

        private void updater__Tick(object sender, EventArgs e)
        {
            if (filename != "")
            {
                file_content.ReadOnly = true;
            }
            else
            {
                file_content.ReadOnly = false;
            }
            update_cursor_position_label();
        }
    }
}
