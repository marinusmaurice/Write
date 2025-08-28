namespace Write
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.newToolStripMenuItem = new ToolStripMenuItem();
            this.openToolStripMenuItem = new ToolStripMenuItem();
            this.saveToolStripMenuItem = new ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.exitToolStripMenuItem = new ToolStripMenuItem();
            this.editToolStripMenuItem = new ToolStripMenuItem();
            this.undoToolStripMenuItem = new ToolStripMenuItem();
            this.redoToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.cutToolStripMenuItem = new ToolStripMenuItem();
            this.copyToolStripMenuItem = new ToolStripMenuItem();
            this.pasteToolStripMenuItem = new ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new ToolStripMenuItem();
            this.formatToolStripMenuItem = new ToolStripMenuItem();
            this.fontToolStripMenuItem = new ToolStripMenuItem();
            this.colorToolStripMenuItem = new ToolStripMenuItem();
            this.insertToolStripMenuItem = new ToolStripMenuItem();
            this.imageToolStripMenuItem = new ToolStripMenuItem();
            this.toolStrip = new ToolStrip();
            this.newToolStripButton = new ToolStripButton();
            this.openToolStripButton = new ToolStripButton();
            this.saveToolStripButton = new ToolStripButton();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.cutToolStripButton = new ToolStripButton();
            this.copyToolStripButton = new ToolStripButton();
            this.pasteToolStripButton = new ToolStripButton();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.fontComboBox = new ToolStripComboBox();
            this.fontSizeComboBox = new ToolStripComboBox();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.boldToolStripButton = new ToolStripButton();
            this.italicToolStripButton = new ToolStripButton();
            this.underlineToolStripButton = new ToolStripButton();
            this.toolStripSeparator6 = new ToolStripSeparator();
            this.fontColorToolStripButton = new ToolStripButton();
            this.insertImageToolStripButton = new ToolStripButton();
            this.textEditor = new UnicodeTextEditor();
            this.statusStrip = new StatusStrip();
            this.statusLabel = new ToolStripStatusLabel();
            this.cursorPositionLabel = new ToolStripStatusLabel();
            this.openFileDialog = new OpenFileDialog();
            this.saveFileDialog = new SaveFileDialog();
            this.fontDialog = new FontDialog();
            this.colorDialog = new ColorDialog();
            this.imageOpenFileDialog = new OpenFileDialog();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.formatToolStripMenuItem,
            this.insertToolStripMenuItem});
            this.menuStrip.Location = new Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new Size(1200, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.N)));
            this.newToolStripMenuItem.Size = new Size(146, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new EventHandler(this.NewDocument);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.O)));
            this.openToolStripMenuItem.Size = new Size(146, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new EventHandler(this.OpenDocument);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.S)));
            this.saveToolStripMenuItem.Size = new Size(146, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new EventHandler(this.SaveDocument);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new Size(146, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As...";
            this.saveAsToolStripMenuItem.Click += new EventHandler(this.SaveAsDocument);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new Size(146, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new EventHandler(this.ExitApplication);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator2,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.selectAllToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.Z)));
            this.undoToolStripMenuItem.Size = new Size(164, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            this.undoToolStripMenuItem.Click += new EventHandler(this.UndoEdit);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.Y)));
            this.redoToolStripMenuItem.Size = new Size(164, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            this.redoToolStripMenuItem.Click += new EventHandler(this.RedoEdit);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(161, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.X)));
            this.cutToolStripMenuItem.Size = new Size(164, 22);
            this.cutToolStripMenuItem.Text = "Cu&t";
            this.cutToolStripMenuItem.Click += new EventHandler(this.CutText);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.C)));
            this.copyToolStripMenuItem.Size = new Size(164, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new EventHandler(this.CopyText);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.V)));
            this.pasteToolStripMenuItem.Size = new Size(164, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            this.pasteToolStripMenuItem.Click += new EventHandler(this.PasteText);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.A)));
            this.selectAllToolStripMenuItem.Size = new Size(164, 22);
            this.selectAllToolStripMenuItem.Text = "Select &All";
            this.selectAllToolStripMenuItem.Click += new EventHandler(this.SelectAllText);
            // 
            // formatToolStripMenuItem
            // 
            this.formatToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.fontToolStripMenuItem,
            this.colorToolStripMenuItem});
            this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
            this.formatToolStripMenuItem.Size = new Size(57, 20);
            this.formatToolStripMenuItem.Text = "F&ormat";
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new Size(103, 22);
            this.fontToolStripMenuItem.Text = "&Font...";
            this.fontToolStripMenuItem.Click += new EventHandler(this.ChangeFont);
            // 
            // colorToolStripMenuItem
            // 
            this.colorToolStripMenuItem.Name = "colorToolStripMenuItem";
            this.colorToolStripMenuItem.Size = new Size(103, 22);
            this.colorToolStripMenuItem.Text = "&Color...";
            this.colorToolStripMenuItem.Click += new EventHandler(this.ChangeColor);
            // 
            // insertToolStripMenuItem
            // 
            this.insertToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.imageToolStripMenuItem});
            this.insertToolStripMenuItem.Name = "insertToolStripMenuItem";
            this.insertToolStripMenuItem.Size = new Size(48, 20);
            this.insertToolStripMenuItem.Text = "&Insert";
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new Size(109, 22);
            this.imageToolStripMenuItem.Text = "&Image...";
            this.imageToolStripMenuItem.Click += new EventHandler(this.InsertImage);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.toolStripSeparator3,
            this.cutToolStripButton,
            this.copyToolStripButton,
            this.pasteToolStripButton,
            this.toolStripSeparator4,
            this.fontComboBox,
            this.fontSizeComboBox,
            this.toolStripSeparator5,
            this.boldToolStripButton,
            this.italicToolStripButton,
            this.underlineToolStripButton,
            this.toolStripSeparator6,
            this.fontColorToolStripButton,
            this.insertImageToolStripButton});
            this.toolStrip.Location = new Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new Size(1200, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new Size(35, 22);
            this.newToolStripButton.Text = "New";
            this.newToolStripButton.Click += new EventHandler(this.NewDocument);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new Size(40, 22);
            this.openToolStripButton.Text = "Open";
            this.openToolStripButton.Click += new EventHandler(this.OpenDocument);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new Size(35, 22);
            this.saveToolStripButton.Text = "Save";
            this.saveToolStripButton.Click += new EventHandler(this.SaveDocument);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 25);
            // 
            // cutToolStripButton
            // 
            this.cutToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.cutToolStripButton.Name = "cutToolStripButton";
            this.cutToolStripButton.Size = new Size(30, 22);
            this.cutToolStripButton.Text = "Cut";
            this.cutToolStripButton.Click += new EventHandler(this.CutText);
            // 
            // copyToolStripButton
            // 
            this.copyToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.copyToolStripButton.Name = "copyToolStripButton";
            this.copyToolStripButton.Size = new Size(39, 22);
            this.copyToolStripButton.Text = "Copy";
            this.copyToolStripButton.Click += new EventHandler(this.CopyText);
            // 
            // pasteToolStripButton
            // 
            this.pasteToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.pasteToolStripButton.Name = "pasteToolStripButton";
            this.pasteToolStripButton.Size = new Size(39, 22);
            this.pasteToolStripButton.Text = "Paste";
            this.pasteToolStripButton.Click += new EventHandler(this.PasteText);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(6, 25);
            // 
            // fontComboBox
            // 
            this.fontComboBox.Name = "fontComboBox";
            this.fontComboBox.Size = new Size(140, 25);
            this.fontComboBox.SelectedIndexChanged += new EventHandler(this.FontComboBox_SelectedIndexChanged);
            // 
            // fontSizeComboBox
            // 
            this.fontSizeComboBox.Name = "fontSizeComboBox";
            this.fontSizeComboBox.Size = new Size(75, 25);
            this.fontSizeComboBox.SelectedIndexChanged += new EventHandler(this.FontSizeComboBox_SelectedIndexChanged);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new Size(6, 25);
            // 
            // boldToolStripButton
            // 
            this.boldToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.boldToolStripButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.boldToolStripButton.Name = "boldToolStripButton";
            this.boldToolStripButton.Size = new Size(23, 22);
            this.boldToolStripButton.Text = "B";
            this.boldToolStripButton.Click += new EventHandler(this.BoldText);
            // 
            // italicToolStripButton
            // 
            this.italicToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.italicToolStripButton.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            this.italicToolStripButton.Name = "italicToolStripButton";
            this.italicToolStripButton.Size = new Size(23, 22);
            this.italicToolStripButton.Text = "I";
            this.italicToolStripButton.Click += new EventHandler(this.ItalicText);
            // 
            // underlineToolStripButton
            // 
            this.underlineToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.underlineToolStripButton.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            this.underlineToolStripButton.Name = "underlineToolStripButton";
            this.underlineToolStripButton.Size = new Size(23, 22);
            this.underlineToolStripButton.Text = "U";
            this.underlineToolStripButton.Click += new EventHandler(this.UnderlineText);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new Size(6, 25);
            // 
            // fontColorToolStripButton
            // 
            this.fontColorToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.fontColorToolStripButton.Name = "fontColorToolStripButton";
            this.fontColorToolStripButton.Size = new Size(23, 22);
            this.fontColorToolStripButton.Text = "A";
            this.fontColorToolStripButton.Click += new EventHandler(this.ChangeColor);
            // 
            // insertImageToolStripButton
            // 
            this.insertImageToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.insertImageToolStripButton.Name = "insertImageToolStripButton";
            this.insertImageToolStripButton.Size = new Size(23, 22);
            this.insertImageToolStripButton.Text = "📷";
            this.insertImageToolStripButton.Click += new EventHandler(this.InsertImage);
            // 
            // textEditor
            // 
            this.textEditor.Dock = DockStyle.Fill;
            this.textEditor.CurrentFont = new Font("Calibri", 11F);
            this.textEditor.Location = new Point(0, 49);
            this.textEditor.Name = "textEditor";
            this.textEditor.Size = new Size(1200, 379);
            this.textEditor.TabIndex = 2;
            this.textEditor.Text = "";
            this.textEditor.SelectionChanged += new EventHandler(this.TextEditor_SelectionChanged);
            this.textEditor.TextChanged += new EventHandler(this.TextEditor_TextChanged);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new ToolStripItem[] {
            this.statusLabel,
            this.cursorPositionLabel});
            this.statusStrip.Location = new Point(0, 428);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new Size(1200, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new Size(39, 17);
            this.statusLabel.Text = "Ready";
            // 
            // cursorPositionLabel
            // 
            this.cursorPositionLabel.Name = "cursorPositionLabel";
            this.cursorPositionLabel.Size = new Size(74, 17);
            this.cursorPositionLabel.Text = "Line 1, Col 1";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Rich Text Format (*.rtf)|*.rtf|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Rich Text Format (*.rtf)|*.rtf|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            // 
            // imageOpenFileDialog
            // 
            this.imageOpenFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files (*.*)|*.*";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 450);
            this.Controls.Add(this.textEditor);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Write - Document Editor";
            this.WindowState = FormWindowState.Maximized;
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripMenuItem formatToolStripMenuItem;
        private ToolStripMenuItem fontToolStripMenuItem;
        private ToolStripMenuItem colorToolStripMenuItem;
        private ToolStripMenuItem insertToolStripMenuItem;
        private ToolStripMenuItem imageToolStripMenuItem;
        private ToolStrip toolStrip;
        private ToolStripButton newToolStripButton;
        private ToolStripButton openToolStripButton;
        private ToolStripButton saveToolStripButton;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton cutToolStripButton;
        private ToolStripButton copyToolStripButton;
        private ToolStripButton pasteToolStripButton;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripComboBox fontComboBox;
        private ToolStripComboBox fontSizeComboBox;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripButton boldToolStripButton;
        private ToolStripButton italicToolStripButton;
        private ToolStripButton underlineToolStripButton;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripButton fontColorToolStripButton;
        private ToolStripButton insertImageToolStripButton;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private ToolStripStatusLabel cursorPositionLabel;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private FontDialog fontDialog;
        private ColorDialog colorDialog;
        private OpenFileDialog imageOpenFileDialog;
        private UnicodeTextEditor textEditor;
    }
}
