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
using ScintillaNET;

namespace Lilac.IDE
{
    public partial class MainWindow : Form
    {
        private DataTable Errors = new DataTable();
        private bool ErrorOccured = false;
        TextStyle Register = new TextStyle(Brushes.Green, null, FontStyle.Bold);
        TextStyle Char = new TextStyle(Brushes.Blue, null, FontStyle.Italic);

        private void ErrorColours()
        {
            scintilla1.Styles[ScintillaNET.Style.Default].BackColor = Color.Red;
            scintilla1.Styles[ScintillaNET.Style.Default].ForeColor = Color.White;
            scintilla1.StyleClearAll();
            ErrorOccured = true;
        }

        public MainWindow()
        {
            SplashScreen sc = new SplashScreen();
            sc.ShowDialog();
            InitializeComponent();
        }

        private void fastColoredTextBox1_Changed(object sender, EventArgs e)
        {
            if (ErrorOccured == true)
            {
                scintilla1.Styles[ScintillaNET.Style.Default].BackColor = Color.White;
                scintilla1.Styles[ScintillaNET.Style.Default].ForeColor = Color.Black;
                scintilla1.StyleClearAll();
            }
            
            //e.ChangedRange.ClearStyle(Register, Char);
            
            //e.ChangedRange.SetStyle(Char, "\'\\\\?[A-Za-z0-9]\'");
            //e.ChangedRange.SetStyle(Register, @" \b(PC|IP|SP|SS|AL|AH|BL|BH|CL|CH|A|B|C|X|Y)");
            Errors.Clear();
            ErrorOccured = false;
            PositionLbl.Text = "" + scintilla1.CurrentPosition;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Errors.Clear();
            Errors.Columns.Add("Message");
            Errors.Columns.Add("Line Number");
            dataGridView1.DataSource = Errors;
            Console.WriteLine("Loaded successfully!");
            CurrentFileLbl.Text = "Untitled";
            scintilla1.Margins[0].Width = 16;
            scintilla1.TextChanged += this.fastColoredTextBox1_Changed;
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.Paste();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.Redo();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.SelectAll();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buildToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Errors.Rows.Clear();
            try
            {
                AILCompiler.Compiler SourceCompiler = new AILCompiler.Compiler(scintilla1.Text);
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
                scintilla1.Styles[ScintillaNET.Style.Default].BackColor = Color.Red;
                scintilla1.Styles[ScintillaNET.Style.Default].ForeColor = Color.White;
                scintilla1.StyleClearAll();
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

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.Clear();
            Console.WriteLine("Starting...");
            Errors.Rows.Clear();
            try
            {
                AILCompiler.Compiler SourceOutput = new AILCompiler.Compiler(scintilla1.Text);
                Debugger Program = new Debugger(SourceOutput.Compile());
                Program.Run();
            }
            catch (ArgumentException ex)
            {
                ErrorColours();
                DataRow row = Errors.NewRow();
                row["Message"] = ex.Message;
                Errors.Rows.Add(row);
            }
            catch (AILCompiler.BuildException ex)
            {
                ErrorColours();
                DataRow row = Errors.NewRow();
                row["Message"] = ex.Message;
                row["Line Number"] = ex.SrcLineNumber.ToString();
                Errors.Rows.Add(row);
            }
            catch (Exception ex)
            {
                ErrorColours();
                DataRow row = Errors.NewRow();
                row["Message"] = ex.Message;
                Errors.Rows.Add(row);
            }
            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.Text = "";
            Errors.Clear();
            this.Text = "Lilac IDE - Untitled";
            CurrentFileLbl.Text = "";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(File.OpenRead(openFileDialog1.FileName));
                scintilla1.Text = sr.ReadToEnd();
                sr.Close();
                this.Text = "Lilac IDE - " + openFileDialog1.FileName;
                CurrentFileLbl.Text = openFileDialog1.FileName;
            }
        }
    }
}
