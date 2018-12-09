using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using AILCompiler = Lilac.Compiler;
using System.IO;

namespace Lilac.IDE
{
    public partial class MainWindow : Form
    {
        private DataTable Errors = new DataTable();
        TextStyle Register = new TextStyle(Brushes.Green, null, FontStyle.Bold);
        TextStyle Char = new TextStyle(Brushes.Blue, null, FontStyle.Italic);

        public MainWindow()
        {
            SplashScreen sc = new SplashScreen();
            sc.ShowDialog();
            InitializeComponent();
        }

        private void fastColoredTextBox1_Changed(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (SourceInput.BackColor == Color.Red)
                SourceInput.BackColor = Color.White;
            e.ChangedRange.ClearStyle(Register, Char);
            
            e.ChangedRange.SetStyle(Char, "\'\\\\?[A-Za-z0-9]\'");
            e.ChangedRange.SetStyle(Register, @" \b(PC|IP|SP|SS|AL|AH|BL|BH|CL|CH|A|B|C|X|Y)");
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SourceInput.Text = "";
            Errors.Clear();
            this.Text = "Lilac IDE - Untitled";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Errors.Clear();
            Errors.Columns.Add("Message");
            Errors.Columns.Add("Line Number");
            dataGridView1.DataSource = Errors;
            Console.WriteLine("Loaded successfully!");
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SourceInput.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SourceInput.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SourceInput.Paste();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SourceInput.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SourceInput.Redo();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SourceInput.SelectAll();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buildToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                AILCompiler.Compiler SourceCompiler = new AILCompiler.Compiler(SourceInput.Text);
                SourceCompiler.Compile();
                if (saveBinaryDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileStream bry = File.OpenWrite(saveBinaryDialog.FileName);
                        bry.Write(SourceCompiler.ByteCode, 0, SourceCompiler.ByteCode.Length);
                        bry.Close();
                    }
                    catch (ArgumentException ax)
                    { // this is from the user abandoning the save 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "File Save Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
            }
            catch (AILCompiler.BuildException ex)
            {
                SourceInput.BackColor = Color.Red;
                DataRow row = Errors.NewRow();
                row["Message"] = ex.Message;
                row["Line Number"] = ex.SrcLineNumber.ToString();
                Errors.Rows.Add(row);
            }
        }

        private void saveBinaryDialog_FileOk(object sender, CancelEventArgs e)
        {
            
        }

        private void saveSourceDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(File.OpenRead(openFileDialog1.FileName));
                SourceInput.Text = sr.ReadToEnd();
                sr.Close();
                this.Text = "Lilac IDE - " + openFileDialog1.FileName;
            }
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Starting...");
            AILCompiler.Compiler SourceOutput = new AILCompiler.Compiler(SourceInput.Text);
            Debugger Program = new Debugger(SourceOutput.Compile());
            Program.Run();
            
        }
    }
}
