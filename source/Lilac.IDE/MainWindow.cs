using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AILCompiler = Lilac.Compiler;
using System.IO;
using ScintillaNET;

namespace Lilac.IDE
{
    public partial class MainWindow : Form
    {
        private DataTable Errors = new DataTable();
        private bool ErrorOccured = false;
        private bool AILMode = false;
        private string Filename;
        //TextStyle Register = new TextStyle(Brushes.Green, null, FontStyle.Bold);
        //TextStyle Char = new TextStyle(Brushes.Blue, null, FontStyle.Italic);

        private void ErrorColours()
        {
            scintilla1.Styles[ScintillaNET.Style.Default].BackColor = Color.Red;
            scintilla1.Styles[ScintillaNET.Style.Default].ForeColor = Color.White;
            scintilla1.StyleClearAll();
            ErrorOccured = true;
        }

        private void NormalColours()
        {
            scintilla1.Styles[ScintillaNET.Style.Default].BackColor = Color.White;
            scintilla1.Styles[ScintillaNET.Style.Default].ForeColor = Color.Black;
            scintilla1.StyleClearAll();
            ErrorOccured = true;
        }

        public MainWindow()
        {
            SplashScreen sc = new SplashScreen();
            sc.ShowDialog();
            InitializeComponent();
        }

        public MainWindow(string file)
        {
            SplashScreen sc = new SplashScreen();
            sc.ShowDialog();
            InitializeComponent();
            FileDrop(file);
        }

        public void FileDrop (string fileToOpen)
        {
            try
            {
                if (File.Exists(fileToOpen))
                {
                    StreamReader sr = new StreamReader(File.OpenRead(fileToOpen));
                    scintilla1.Text = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void fastColoredTextBox1_Changed(object sender, EventArgs e)
        {
            if (AILMode == true)
            {
                checkSyntax();
                if (ErrorOccured == true)
                {
                    ErrorColours();
                }
                else
                {
                    NormalColours();
                }

                //e.ChangedRange.ClearStyle(Register, Char);

                //e.ChangedRange.SetStyle(Char, "\'\\\\?[A-Za-z0-9]\'");
                //e.ChangedRange.SetStyle(Register, @" \b(PC|IP|SP|SS|AL|AH|BL|BH|CL|CH|A|B|C|X|Y)");
                Errors.Clear();
                ErrorOccured = false;
            }
            else
            {
                NormalColours();
                Errors.Clear();
                ErrorOccured = false;
            }
            
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

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            debugApp(true);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newDocument();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openSourceFile.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(File.OpenRead(openSourceFile.FileName));
                scintilla1.Text = sr.ReadToEnd();
                sr.Close();
                this.Text = "Lilac IDE - " + openSourceFile.FileName;
                CurrentFileLbl.Text = openSourceFile.FileName;
                Filename = openSourceFile.FileName;
                if (!openSourceFile.FileName.EndsWith(".lsf"))
                {
                    AILMode = false;
                }
                else
                {
                    AILMode = true;
                }
            }
        }

        private void NewFileToolStripLabel_Click(object sender, EventArgs e)
        {
            newDocument();
        }


        private void newDocument()
        {
            scintilla1.Text = "";
            Errors.Clear();
            this.Text = "Lilac IDE - Untitled";
            CurrentFileLbl.Text = "";
            Filename = "";
        }

        private void saveDocumentAs()
        {
            if (saveSourceDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(File.OpenWrite(saveSourceDialog.FileName));
                sw.Write(scintilla1.Text);
                sw.Close();
                Filename = saveSourceDialog.FileName;
            }
        }

        private void saveDocument()
        {
            if (Filename != "")
            {
                StreamWriter sw = new StreamWriter(File.OpenWrite(Filename));
                sw.Write(scintilla1.Text);
                sw.Close();
            }
            else
            {

                if (saveSourceDialog.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(File.OpenWrite(saveSourceDialog.FileName));
                    sw.Write(scintilla1.Text);
                    sw.Close();
                    Filename = saveSourceDialog.FileName;
                }
            }

        }

        private void debugApp(bool DebugMode)
        {
            Console.Clear();
            Console.WriteLine("Starting...");
            Errors.Rows.Clear();
            try
            {
                AILCompiler.Compiler SourceOutput = new AILCompiler.Compiler(scintilla1.Text);
                Debugger Program = new Debugger(SourceOutput.Compile(), DebugMode);
                Program.Run();
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                ErrorColours();
                DataRow row = Errors.NewRow();
                row["Message"] = ex.Message;
                Errors.Rows.Add(row);
            }
            catch (AILCompiler.BuildException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine("Line number: " + ex.SrcLineNumber);
                Console.ForegroundColor = ConsoleColor.White;
                ErrorColours();
                DataRow row = Errors.NewRow();
                row["Message"] = ex.Message;
                row["Line Number"] = ex.SrcLineNumber.ToString();
                Errors.Rows.Add(row);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                ErrorColours();
                DataRow row = Errors.NewRow();
                row["Message"] = ex.Message;
                Errors.Rows.Add(row);
            }
        }

        private void checkSyntax()
        {
            Console.Clear();
            Errors.Rows.Clear();
            try
            {
                AILCompiler.Compiler SourceOutput = new AILCompiler.Compiler(scintilla1.Text);
                SourceOutput.Compile();
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                ErrorColours();
                DataRow row = Errors.NewRow();
                row["Message"] = ex.Message;
                Errors.Rows.Add(row);
            }
            catch (AILCompiler.BuildException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine("Line number: " + ex.SrcLineNumber);
                Console.ForegroundColor = ConsoleColor.White;
                ErrorColours();
                DataRow row = Errors.NewRow();
                row["Message"] = ex.Message;
                row["Line Number"] = ex.SrcLineNumber.ToString();
                Errors.Rows.Add(row);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                ErrorColours();
                DataRow row = Errors.NewRow();
                row["Message"] = ex.Message;
                Errors.Rows.Add(row);
            }
        }

        private void SaveToolStripLabel_Click(object sender, EventArgs e)
        {
            saveDocument();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveDocumentAs();
        }

        private void debugToolStripLabel_Click(object sender, EventArgs e)
        {
            debugApp(true);
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            debugApp(false);
        }

        private void runWithoutDebuggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            debugApp(false);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutForm = new About();
            aboutForm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void printToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveDocument();
        }
    }
}
