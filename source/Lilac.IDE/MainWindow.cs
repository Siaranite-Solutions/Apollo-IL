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

        #region TabMethods

        private void AddTab()
        {
            Scintilla Body = new Scintilla();
            Body.Name = "Body";
            Body.Dock = DockStyle.Fill;
            Body.ContextMenuStrip = contextMenuStrip1;
            TabPage NewPage = new TabPage();
            TabCount += 1;
            string DocumentText = "Document " + TabCount;
            NewPage.Name = DocumentText;
            NewPage.Text = DocumentText;
            NewPage.Controls.Add(Body);
            tabControl1.TabPages.Add(NewPage);
            groupBox1.Text = "Source View";
            tabControl1.SelectedTab = NewPage;
            GetCurrentDocument.Margins[0].Width = 16;
            GetCurrentDocument.TextChanged += this.FastColoredTextBox1_Changed;
        }

        private void RemoveTab()
        {
            if (tabControl1.TabPages.Count != 1)
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
            else
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                tabControl1.TabPages.Add(StartPage_Tab);
            }
        }

        private void RemoveAllTabsButThis()
        {
            foreach (TabPage page in tabControl1.TabPages)
            {
                if (page.Name != tabControl1.SelectedTab.Name)
                {
                    tabControl1.TabPages.Remove(page);
                }
            }
        }

        private void RemoveAllTabs()
        {
            foreach (TabPage page in tabControl1.TabPages)
            {
                tabControl1.TabPages.Remove(page);
            }
            tabControl1.TabPages.Add(StartPage_Tab);
            TabCount = 0;
        }
        #endregion

        #region Properties 
        private Scintilla GetCurrentDocument
        {
            get
            {
                return (Scintilla)tabControl1.SelectedTab.Controls["Body"];
            }
        }


        private int TabCount = 0;
        private DataTable Errors = new DataTable();
        private bool ErrorOccured = false;
        private bool AILMode = false;
        private string Filename;
        //TextStyle Register = new TextStyle(Brushes.Green, null, FontStyle.Bold);
        //TextStyle Char = new TextStyle(Brushes.Blue, null, FontStyle.Italic);

        #endregion

        #region Editor Methods
        private void ErrorColours()
        {
            if (GetCurrentDocument != null)
            {
                GetCurrentDocument.Styles[Style.Default].BackColor = Color.Red;
                GetCurrentDocument.Styles[Style.Default].ForeColor = Color.White;
                GetCurrentDocument.StyleClearAll();
                ErrorOccured = true;
            }
        }

        private void NormalColours()
        {
            if (GetCurrentDocument != null)
            {
                GetCurrentDocument.Styles[Style.Default].BackColor = Color.White;
                GetCurrentDocument.Styles[Style.Default].ForeColor = Color.Black;
                GetCurrentDocument.StyleClearAll();
                ErrorOccured = true;
            }
        }
        #endregion

        public MainWindow()
        {
            SplashScreen sc = new SplashScreen();
            sc.ShowDialog();
            InitializeComponent();
            AddRecentFiles();
        }

        public MainWindow(string file)
        {
            SplashScreen sc = new SplashScreen();
            sc.ShowDialog();
            InitializeComponent();
            FileDrop(file);
            AddRecentFiles();
        }

        public void FileDrop (string fileToOpen)
        {
            AddTab();
            try
            {
                if (File.Exists(fileToOpen))
                {
                    StreamReader sr = new StreamReader(File.OpenRead(fileToOpen));
                    GetCurrentDocument.Text = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void FastColoredTextBox1_Changed(object sender, EventArgs e)
        {
            if (AILMode == true)
            {
                CheckSyntax();
                if (ErrorOccured != true)
                {
                    NormalColours();
                }
                else
                {
                    
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
            
            PositionLbl.Text = "" + GetCurrentDocument.CurrentPosition;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Errors.Clear();
            Errors.Columns.Add("Message");
            Errors.Columns.Add("Line Number");
            dataGridView1.DataSource = Errors;
            Console.WriteLine("Loaded successfully!");
            CurrentFileLbl.Text = "Untitled";
            groupBox1.Text = "";
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.Paste();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.Redo();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectAll();
        }

        private void buildToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BuildExe();
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DebugApp(true);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewDocument();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDocument();   
        }

        private void NewFileToolStripLabel_Click(object sender, EventArgs e)
        {
            NewDocument();
        }

        private void AddRecentFiles()
        {
            string[] RecentFiles = Properties.Settings.Default.RecentFiles.Split('\n');
            foreach (string File in RecentFiles)
            {
                ListViewItem RecentFile = new ListViewItem(File);
                RecentFile.Name = File;
                RecentFile.Text = File;
                listView1.Items.Add(RecentFile);
            }
        }

        private void BuildExe()
        {
            if (GetCurrentDocument != null)
            {
                Errors.Rows.Clear();
                try
                {
                    AILCompiler.Compiler SourceCompiler = new AILCompiler.Compiler(GetCurrentDocument.Text);
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
                    GetCurrentDocument.Styles[Style.Default].BackColor = Color.Red;
                    GetCurrentDocument.Styles[Style.Default].ForeColor = Color.White;
                    GetCurrentDocument.StyleClearAll();
                    DataRow row = Errors.NewRow();
                    row["Message"] = ex.Message;
                    row["Line Number"] = ex.SrcLineNumber.ToString();
                    Errors.Rows.Add(row);
                }
            }
        }

        private void OpenDocument()
        {
            AddTab();
            if (openSourceFile.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(File.OpenRead(openSourceFile.FileName));
                GetCurrentDocument.Text = sr.ReadToEnd();
                sr.Close();
                this.Text = "Lilac IDE - " + openSourceFile.FileName;
                CurrentFileLbl.Text = openSourceFile.FileName;
                Filename = openSourceFile.FileName;
                Properties.Settings.Default.RecentFiles = Properties.Settings.Default.RecentFiles + openSourceFile.FileName + "\n";
                ListViewItem item = new ListViewItem(openSourceFile.FileName);
                item.Name = openSourceFile.FileName;
                if (!listView1.Items.Contains(item))
                {
                    listView1.Items.Add(item);
                }
                else
                {
                    listView1.Items[item.Name].Name = item.Name;
                    listView1.Items[item.Name].Text = item.Text;
                }
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

        private void NewDocument()
        {
            Scintilla Body = new Scintilla();

            Body.Name = "Body";
            Body.Dock = DockStyle.Fill;
            Body.ContextMenuStrip = contextMenuStrip1;

            TabPage NewPage = new TabPage();
            TabCount += 1;

            string DocumentText = "Document " + TabCount;
            NewPage.Name = DocumentText;
            NewPage.Text = DocumentText;
            NewPage.Controls.Add(Body);

            tabControl1.TabPages.Add(NewPage);
            Body.Text = "";
            Errors.Clear();
            this.Text = "Lilac IDE - Untitled";
            CurrentFileLbl.Text = "";
            Filename = "";
            tabControl1.SelectedTab = NewPage;
        }

        private void SaveDocumentAs()
        {
            if (GetCurrentDocument != null)
            {
                saveSourceDialog.FileName = tabControl1.SelectedTab.Name;
                saveSourceDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (saveSourceDialog.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(File.OpenWrite(saveSourceDialog.FileName));
                    sw.Write(GetCurrentDocument.Text);
                    sw.Close();
                    Filename = saveSourceDialog.FileName;
                }
            }
        }

        private void SaveDocument()
        {
            if (GetCurrentDocument != null)
            {
                if (Filename != "")
                {
                    StreamWriter sw = new StreamWriter(File.OpenWrite(Filename));
                    sw.Write(GetCurrentDocument.Text);
                    sw.Close();
                }
                else
                {
                    saveSourceDialog.FileName = tabControl1.SelectedTab.Name;
                    saveSourceDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    if (saveSourceDialog.ShowDialog() == DialogResult.OK)
                    {
                        StreamWriter sw = new StreamWriter(File.OpenWrite(saveSourceDialog.FileName));
                        sw.Write(GetCurrentDocument.Text);
                        sw.Close();
                        Filename = saveSourceDialog.FileName;
                    }
                }
            }
        }

        private void DebugApp(bool DebugMode)
        {
            if (GetCurrentDocument != null)
            {
                Console.Clear();
                Console.WriteLine("Starting...");
                Errors.Rows.Clear();
                try
                {
                    AILCompiler.Compiler SourceOutput = new AILCompiler.Compiler(GetCurrentDocument.Text + "\nKEI 2");
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
        }

        private void CheckSyntax()
        {
            if (GetCurrentDocument != null)
            {
                Console.Clear();
                Errors.Rows.Clear();
                try
                {
                    AILCompiler.Compiler SourceOutput = new AILCompiler.Compiler(GetCurrentDocument.Text + "\nKEI 2");
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
        }

        private void SaveToolStripLabel_Click(object sender, EventArgs e)
        {
            SaveDocument();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDocumentAs();
        }

        private void debugToolStripLabel_Click(object sender, EventArgs e)
        {
            DebugApp(true);
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            DebugApp(false);
        }

        private void runWithoutDebuggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DebugApp(false);
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDocument();
        }

        private void undoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (GetCurrentDocument != null)
            {
                GetCurrentDocument.Undo();
            }
        }

        private void redoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (GetCurrentDocument != null)
            {
                GetCurrentDocument.Redo();
            }
        }

        private void cutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (GetCurrentDocument != null)
            {
                GetCurrentDocument.Cut();
            }
        }

        private void copyToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (GetCurrentDocument != null)
            {
                GetCurrentDocument.Copy();
            }
        }

        private void pasteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (GetCurrentDocument != null)
            {
                GetCurrentDocument.Paste();
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (GetCurrentDocument != null)
            {
                SaveDocument();
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveTab();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenDocument();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NewDocument();
        }

        private void cutToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            if (GetCurrentDocument != null)
            {
                GetCurrentDocument.Cut();
            }
        }

        private void copyToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            if (GetCurrentDocument != null)
            {
                GetCurrentDocument.Copy();
            }
        }

        private void pasteToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            if (GetCurrentDocument != null)
            {
                GetCurrentDocument.Paste();
            }
        }

        private void undoToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            if (GetCurrentDocument != null)
            {
                GetCurrentDocument.Undo();
            }
        }

        private void redoToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            if (GetCurrentDocument != null)
            {
                GetCurrentDocument.Redo();
            }
        }

        private void selectAllToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (GetCurrentDocument != null)
            {
                GetCurrentDocument.SelectAll();
            }
        }

        private void saveToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            if (GetCurrentDocument != null)
            {
                SaveDocument();
            }
        }

        private void closeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            RemoveTab();
        }

        private void closeAllExceptThisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveAllTabsButThis();
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveAllTabs();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                string itemFile = item.Text.ToString();
                if (File.Exists(itemFile))
                {
                    AddTab();
                    GetCurrentDocument.Text = File.ReadAllText(itemFile);
                }
                else
                {
                    MessageBox.Show("File does not exist!");
                    listView1.Items.Remove(item);
                }
                //StreamReader sr = new StreamReader(File.OpenRead(itemFile));
                //sr.ReadToEnd();
                //sr.Close();
                this.Text = "Lilac IDE - " + openSourceFile.FileName;
                CurrentFileLbl.Text = openSourceFile.FileName;
                Filename = item.Name;
                //Properties.Settings.Default.RecentFiles = Properties.Settings.Default.RecentFiles + item.Name + "\n";
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
    }
}
