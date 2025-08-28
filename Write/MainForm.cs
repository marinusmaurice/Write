using System.Text;

namespace Write
{
    public partial class MainForm : Form
    {
        private string currentFileName = "";
        private bool isDocumentModified = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeEditor();
        }

        private void InitializeEditor()
        {
            // Initialize font combo box with system fonts
            foreach (FontFamily font in FontFamily.Families)
            {
                fontComboBox.Items.Add(font.Name);
            }
            fontComboBox.Text = "Calibri";

            // Initialize font size combo box
            int[] fontSizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (int size in fontSizes)
            {
                fontSizeComboBox.Items.Add(size.ToString());
            }
            fontSizeComboBox.Text = "11";

            // Set initial cursor position
            UpdateCursorPosition();
            
            // Enable drag and drop for images (will be implemented later for Unicode editor)
            if (textEditor != null)
            {
                textEditor.AllowDrop = true;
                
                // Add some sample text with emojis to test Unicode handling
                textEditor.Text = "Start🚀 \r\nNormal ASCII: ABC xyz 123 !@#  \r\nEmoji combos: 👩‍💻👨‍👩‍👧‍👦🇿🇦🏳️‍🌈  \r\nZero-width joiner: a‍b‍c (looks like abc but has hidden chars)  \r\nZero-width space: hello​world (no visible space)  \r\nRight-to-left: שלום مرحبا (mixed directions)  \r\nCombining accents: é vs é vs ē (all look different/same depending on render)  \r\nFull-width chars: ＡＢＣ１２３＠＃％  \r\nSurrogate pair edge: 𐍈 (Gothic letter hwair, outside BMP)  \r\nControl chars: A\\bB\\tC\\nD\\rE  \r\nMath/Unicode: ∞ ∑ π 𝒜 𝔅 𝓒  \r\nQuotes & apostrophes: ' \" ’ “ ”  \r\nSpecial whitespace: [ ] [ ] [ ] [ ] (narrow no-break, non-breaking, en-space, em-space)  \r\nEnd🎯\r\n";
            }
        }

        #region File Operations
        private void NewDocument(object? sender, EventArgs e)
        {
            if (CheckSaveChanges())
            {
                textEditor.Clear();
                currentFileName = "";
                isDocumentModified = false;
                UpdateTitle();
                statusLabel.Text = "New document created";
            }
        }

        private void OpenDocument(object? sender, EventArgs e)
        {
            if (CheckSaveChanges() && openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string content = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                    textEditor.Text = content;
                    currentFileName = openFileDialog.FileName;
                    isDocumentModified = false;
                    UpdateTitle();
                    statusLabel.Text = $"Opened: {Path.GetFileName(currentFileName)}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveDocument(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFileName))
            {
                SaveAsDocument(sender, e);
            }
            else
            {
                SaveFile(currentFileName);
            }
        }

        private void SaveAsDocument(object? sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveFile(saveFileDialog.FileName);
                currentFileName = saveFileDialog.FileName;
                UpdateTitle();
            }
        }

        private void SaveFile(string fileName)
        {
            try
            {
                File.WriteAllText(fileName, textEditor.Text, Encoding.UTF8);
                isDocumentModified = false;
                UpdateTitle();
                statusLabel.Text = $"Saved: {Path.GetFileName(fileName)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExitApplication(object? sender, EventArgs e)
        {
            if (CheckSaveChanges())
            {
                Application.Exit();
            }
        }

        private bool CheckSaveChanges()
        {
            if (isDocumentModified)
            {
                var result = MessageBox.Show("Do you want to save changes to the current document?", 
                    "Save Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    SaveDocument(null, EventArgs.Empty);
                    return !isDocumentModified; // Return false if save was cancelled
                }
                else if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Edit Operations
        private void UndoEdit(object? sender, EventArgs e)
        {
            if (textEditor.CanUndo)
            {
                textEditor.Undo();
                statusLabel.Text = "Undo completed";
            }
        }

        private void RedoEdit(object? sender, EventArgs e)
        {
            if (textEditor.CanRedo)
            {
                textEditor.Redo();
                statusLabel.Text = "Redo completed";
            }
        }

        private void CutText(object? sender, EventArgs e)
        {
            if (textEditor.SelectionLength > 0)
            {
                textEditor.Cut();
                statusLabel.Text = "Text cut to clipboard";
            }
        }

        private void CopyText(object? sender, EventArgs e)
        {
            if (textEditor.SelectionLength > 0)
            {
                textEditor.Copy();
                statusLabel.Text = "Text copied to clipboard";
            }
        }

        private void PasteText(object? sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                textEditor.Paste();
                statusLabel.Text = "Text pasted from clipboard";
            }
            else if (Clipboard.ContainsImage())
            {
                // For now, we'll show a message that images aren't supported in text mode
                // In a future version, we could implement image support
                MessageBox.Show("Image pasting is not supported in Unicode text mode.", "Not Supported", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SelectAllText(object? sender, EventArgs e)
        {
            textEditor.SelectAll();
            statusLabel.Text = "All text selected";
        }
        #endregion

        #region Formatting Operations
        private bool _updatingFormatButtons = false; // Flag to prevent recursive updates
        private System.Windows.Forms.Timer? _fontChangeTimer; // Timer to debounce font changes

        private void ChangeFont(object? sender, EventArgs e)
        {
            var currentFont = textEditor.GetSelectionFont() ?? textEditor.CurrentFont;
            fontDialog.Font = currentFont;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                textEditor.ApplyFont(fontDialog.Font);
                UpdateFormatButtons();
                statusLabel.Text = "Font changed";
            }
        }

        private void ChangeColor(object? sender, EventArgs e)
        {
            colorDialog.Color = textEditor.GetSelectionColor();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                textEditor.ApplyColor(colorDialog.Color);
                UpdateFormatButtons();
                statusLabel.Text = "Text color changed";
            }
        }

        public void ApplyQuickColor(Color color)
        {
            if (textEditor != null)
            {
                textEditor.ApplyColor(color);
                UpdateFormatButtons();
                statusLabel.Text = $"Color changed to {color.Name}";
            }
        }
        
        private void FontComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Prevent recursive calls during format button updates
            if (_updatingFormatButtons || fontComboBox.SelectedItem == null || textEditor == null)
                return;

            string fontName = fontComboBox.SelectedItem.ToString()!;
            var currentFont = textEditor.GetSelectionFont() ?? textEditor.CurrentFont;
            
            // Only apply if the font actually changed
            if (string.Equals(fontName, currentFont.FontFamily.Name, StringComparison.OrdinalIgnoreCase))
                return;

            // Use a timer to debounce rapid changes (like when scrolling through the combo box)
            if (_fontChangeTimer != null)
            {
                _fontChangeTimer.Stop();
                _fontChangeTimer.Dispose();
            }

            _fontChangeTimer = new System.Windows.Forms.Timer();
            _fontChangeTimer.Interval = 100; // 100ms delay
            _fontChangeTimer.Tick += (s, args) =>
            {
                _fontChangeTimer?.Stop();
                _fontChangeTimer?.Dispose();
                _fontChangeTimer = null;
                
                ApplyFontChange(fontName, currentFont);
            };
            _fontChangeTimer.Start();
        }

        private void ApplyFontChange(string fontName, Font currentFont)
        {
            float fontSize = currentFont.Size;
            FontStyle fontStyle = currentFont.Style;
            
            try
            {
                var newFont = new Font(fontName, fontSize, fontStyle);
                textEditor.ApplyFont(newFont);
                UpdateFormatButtons();
                statusLabel.Text = $"Font changed to {fontName}";
            }
            catch
            {
                // Font not available, revert to previous selection
                _updatingFormatButtons = true;
                fontComboBox.Text = currentFont.FontFamily.Name;
                _updatingFormatButtons = false;
                statusLabel.Text = $"Font '{fontName}' is not available";
            }
        }

        private void FontSizeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Prevent recursive calls during format button updates
            if (_updatingFormatButtons || fontSizeComboBox.SelectedItem == null || textEditor == null)
                return;

            if (float.TryParse(fontSizeComboBox.SelectedItem.ToString(), out float fontSize))
            {
                var currentFont = textEditor.GetSelectionFont() ?? textEditor.CurrentFont;
                
                // Only apply if the size actually changed
                if (Math.Abs(fontSize - currentFont.Size) < 0.1f)
                    return;

                string fontName = currentFont.FontFamily.Name;
                FontStyle fontStyle = currentFont.Style;
                
                try
                {
                    var newFont = new Font(fontName, fontSize, fontStyle);
                    textEditor.ApplyFont(newFont);
                    UpdateFormatButtons();
                    statusLabel.Text = $"Font size changed to {fontSize}";
                }
                catch
                {
                    // Invalid size, revert
                    _updatingFormatButtons = true;
                    fontSizeComboBox.Text = currentFont.Size.ToString();
                    _updatingFormatButtons = false;
                }
            }
        }

        private void BoldText(object? sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Bold);
            UpdateFormatButtons();
            statusLabel.Text = boldToolStripButton.Checked ? "Bold applied" : "Bold removed";
        }

        private void ItalicText(object? sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Italic);
            UpdateFormatButtons();
            statusLabel.Text = italicToolStripButton.Checked ? "Italic applied" : "Italic removed";
        }

        private void UnderlineText(object? sender, EventArgs e)
        {
            ToggleFontStyle(FontStyle.Underline);
            UpdateFormatButtons();
            statusLabel.Text = underlineToolStripButton.Checked ? "Underline applied" : "Underline removed";
        }

        private void ToggleFontStyle(FontStyle style)
        {
            if (textEditor != null)
            {
                var currentFont = textEditor.GetSelectionFont() ?? textEditor.CurrentFont;
                FontStyle newStyle = currentFont.Style;
                
                if ((newStyle & style) == style)
                {
                    newStyle &= ~style; // Remove style
                }
                else
                {
                    newStyle |= style; // Add style
                }
                
                var newFont = new Font(currentFont.FontFamily, currentFont.Size, newStyle);
                textEditor.ApplyFont(newFont);
                UpdateFormatButtons();
            }
        }

        private bool IsStyleApplied(FontStyle style)
        {
            if (textEditor != null)
            {
                var currentFont = textEditor.GetSelectionFont() ?? textEditor.CurrentFont;
                return (currentFont.Style & style) == style;
            }
            return false;
        }
        #endregion

        #region Image Operations
        private void InsertImage(object? sender, EventArgs e)
        {
            // For now, show a message that images aren't supported in Unicode text mode
            MessageBox.Show("Image insertion is not supported in Unicode text mode.\nThis feature may be added in a future version.", 
                "Not Supported", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Event Handlers
        private void TextEditor_SelectionChanged(object? sender, EventArgs e)
        {
            UpdateCursorPosition();
            UpdateFormatButtons();
        }

        private void TextEditor_TextChanged(object? sender, EventArgs e)
        {
            isDocumentModified = true;
            UpdateTitle();
        }

        private void UpdateCursorPosition()
        {
            if (textEditor != null)
            {
                int line = textEditor.GetLineFromIndex(textEditor.CursorPosition) + 1;
                int column = textEditor.GetColumnFromIndex(textEditor.CursorPosition) + 1;
                cursorPositionLabel.Text = $"Line {line}, Col {column}";
            }
        }

        private void UpdateFormatButtons()
        {
            if (textEditor == null || _updatingFormatButtons)
                return;

            _updatingFormatButtons = true;
            try
            {
                var currentFont = textEditor.GetSelectionFont() ?? textEditor.CurrentFont;
                var currentColor = textEditor.GetSelectionColor();
                
                boldToolStripButton.Checked = IsStyleApplied(FontStyle.Bold);
                italicToolStripButton.Checked = IsStyleApplied(FontStyle.Italic);
                underlineToolStripButton.Checked = IsStyleApplied(FontStyle.Underline);
                
                fontComboBox.Text = currentFont.FontFamily.Name;
                fontSizeComboBox.Text = currentFont.Size.ToString();
                
                // Update font color button to show current color
                fontColorToolStripButton.ForeColor = currentColor;
            }
            finally
            {
                _updatingFormatButtons = false;
            }
        }

        private void UpdateTitle()
        {
            string fileName = string.IsNullOrEmpty(currentFileName) ? "Untitled" : Path.GetFileName(currentFileName);
            string modified = isDocumentModified ? "*" : "";
            this.Text = $"Write - {fileName}{modified}";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Handle keyboard shortcuts that might not be caught by menu items
            switch (keyData)
            {
                case Keys.Control | Keys.B:
                    BoldText(null, EventArgs.Empty);
                    return true;
                case Keys.Control | Keys.I:
                    ItalicText(null, EventArgs.Empty);
                    return true;
                case Keys.Control | Keys.U:
                    UnderlineText(null, EventArgs.Empty);
                    return true;
                case Keys.Control | Keys.Shift | Keys.C:
                    ChangeColor(null, EventArgs.Empty);
                    return true;
                case Keys.Control | Keys.Shift | Keys.F:
                    ChangeFont(null, EventArgs.Empty);
                    return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Clean up timer
            if (_fontChangeTimer != null)
            {
                _fontChangeTimer.Stop();
                _fontChangeTimer.Dispose();
                _fontChangeTimer = null;
            }

            if (!CheckSaveChanges())
            {
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }
        #endregion
    }
}
