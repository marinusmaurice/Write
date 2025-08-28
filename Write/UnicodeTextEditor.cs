using System.Globalization;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace Write
{
    public class UnicodeTextEditor : Control
    {
        private readonly List<TextElement> _textElements;
        private readonly UndoRedoManager _undoRedoManager;
        private readonly System.Windows.Forms.Timer _blinkTimer;
        
        private int _cursorPosition;
        private int _selectionStart;
        private int _selectionLength;
        private bool _cursorVisible;
        private bool _isDragging;
        private Point _dragStartPoint;
        
        private Font _currentFont;
        private Color _currentTextColor;
        private Color _selectionBackColor;
        private Color _cursorColor;
        
        private readonly ScrollBars _scrollBars;
        private readonly VScrollBar _vScrollBar;
        private readonly HScrollBar _hScrollBar;
        
        private float _lineHeight;
        private int _topLine;
        private int _leftOffset;
        private readonly List<LineInfo> _lines;

        public UnicodeTextEditor()
        {
            _textElements = new List<TextElement>();
            _undoRedoManager = new UndoRedoManager();
            _lines = new List<LineInfo>();
            
            _currentFont = new Font("Calibri", 11f);
            _currentTextColor = Color.Black;
            _selectionBackColor = Color.FromArgb(51, 153, 255);
            _cursorColor = Color.Black;
            
            _blinkTimer = new System.Windows.Forms.Timer();
            _blinkTimer.Interval = 500;
            _blinkTimer.Tick += BlinkTimer_Tick;
            _blinkTimer.Start();
            
            _vScrollBar = new VScrollBar();
            _hScrollBar = new HScrollBar();
            
            SetStyle(ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.UserPaint | 
                     ControlStyles.DoubleBuffer | 
                     ControlStyles.ResizeRedraw |
                     ControlStyles.Selectable, true);
            
            TabStop = true;
            AllowDrop = true;
            
            InitializeScrollBars();
            CalculateLineHeight();
            RecalculateLines();
        }

        protected override bool IsInputKey(Keys keyData)
        {
            // Tell Windows Forms that we want to handle these keys
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                case Keys.Home:
                case Keys.End:
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.Delete:
                case Keys.Back:
                case Keys.Enter:
                case Keys.Tab:
                    return true;
            }
            
            // Also handle combinations with modifiers
            if ((keyData & Keys.Control) == Keys.Control)
            {
                switch (keyData & Keys.KeyCode)
                {
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Home:
                    case Keys.End:
                    case Keys.A:
                    case Keys.C:
                    case Keys.V:
                    case Keys.X:
                    case Keys.Z:
                    case Keys.Y:
                        return true;
                }
            }
            
            if ((keyData & Keys.Shift) == Keys.Shift)
            {
                switch (keyData & Keys.KeyCode)
                {
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Home:
                    case Keys.End:
                    case Keys.PageUp:
                    case Keys.PageDown:
                        return true;
                }
            }
            
            return base.IsInputKey(keyData);
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            // Ensure these keys are treated as input keys
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                case Keys.Home:
                case Keys.End:
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.Delete:
                case Keys.Back:
                case Keys.Enter:
                case Keys.Tab:
                    e.IsInputKey = true;
                    break;
            }
            
            base.OnPreviewKeyDown(e);
        }

        #region Properties
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Text
        {
            get => GetText();
            set => SetText(value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Font CurrentFont
        {
            get => _currentFont;
            set
            {
                _currentFont = value ?? throw new ArgumentNullException(nameof(value));
                CalculateLineHeight();
                RecalculateLines();
                Invalidate();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color CurrentTextColor
        {
            get => _currentTextColor;
            set
            {
                _currentTextColor = value;
                Invalidate();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CursorPosition
        {
            get => _cursorPosition;
            set
            {
                _cursorPosition = Math.Max(0, Math.Min(value, _textElements.Count));
                _selectionStart = _cursorPosition;
                _selectionLength = 0;
                EnsureCursorVisible();
                Invalidate();
                OnSelectionChanged();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionStart
        {
            get => _selectionStart;
            set
            {
                _selectionStart = Math.Max(0, Math.Min(value, _textElements.Count));
                Invalidate();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionLength
        {
            get => _selectionLength;
            set
            {
                _selectionLength = Math.Max(0, Math.Min(value, _textElements.Count - _selectionStart));
                Invalidate();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedText
        {
            get => GetSelectedText();
            set => ReplaceSelection(value);
        }

        public bool CanUndo => _undoRedoManager.CanUndo;
        public bool CanRedo => _undoRedoManager.CanRedo;
        #endregion

        #region Events
        public new event EventHandler? TextChanged;
        public event EventHandler? SelectionChanged;
        #endregion

        #region Unicode-aware text handling
        private void SetText(string text)
        {
            _textElements.Clear();
            if (!string.IsNullOrEmpty(text))
            {
                var elements = StringInfo.GetTextElementEnumerator(text);
                while (elements.MoveNext())
                {
                    var element = elements.GetTextElement();
                    _textElements.Add(new TextElement(element, _currentFont, _currentTextColor));
                }
            }
            
            _cursorPosition = 0;
            _selectionStart = 0;
            _selectionLength = 0;
            RecalculateLines();
            Invalidate();
            OnTextChanged();
        }

        private string GetText()
        {
            var sb = new StringBuilder();
            foreach (var element in _textElements)
            {
                sb.Append(element.Text);
            }
            return sb.ToString();
        }

        private string GetSelectedText()
        {
            if (_selectionLength == 0) return string.Empty;
            
            var sb = new StringBuilder();
            int start = Math.Min(_selectionStart, _selectionStart + _selectionLength);
            int length = Math.Abs(_selectionLength);
            
            for (int i = start; i < start + length && i < _textElements.Count; i++)
            {
                sb.Append(_textElements[i].Text);
            }
            return sb.ToString();
        }

        private void InsertText(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            var action = new TextAction
            {
                Type = TextActionType.Insert,
                Position = _cursorPosition,
                Text = text,
                OldSelectionStart = _selectionStart,
                OldSelectionLength = _selectionLength
            };

            DeleteSelection();

            var elements = StringInfo.GetTextElementEnumerator(text);
            int insertCount = 0;
            while (elements.MoveNext())
            {
                var element = elements.GetTextElement();
                _textElements.Insert(_cursorPosition + insertCount, 
                    new TextElement(element, _currentFont, _currentTextColor));
                insertCount++;
            }

            _cursorPosition += insertCount;
            _selectionStart = _cursorPosition;
            _selectionLength = 0;

            action.NewCursorPosition = _cursorPosition;
            _undoRedoManager.AddAction(action);

            RecalculateLines();
            EnsureCursorVisible();
            Invalidate();
            OnTextChanged();
            OnSelectionChanged();
        }

        private void DeleteSelection()
        {
            if (_selectionLength == 0) return;

            int start = Math.Min(_selectionStart, _selectionStart + _selectionLength);
            int length = Math.Abs(_selectionLength);

            for (int i = 0; i < length && start < _textElements.Count; i++)
            {
                _textElements.RemoveAt(start);
            }

            _cursorPosition = start;
            _selectionStart = start;
            _selectionLength = 0;
        }

        private void ReplaceSelection(string text)
        {
            if (_selectionLength > 0)
            {
                var action = new TextAction
                {
                    Type = TextActionType.Replace,
                    Position = Math.Min(_selectionStart, _selectionStart + _selectionLength),
                    Text = GetSelectedText(),
                    OldSelectionStart = _selectionStart,
                    OldSelectionLength = _selectionLength
                };

                DeleteSelection();
                InsertText(text);

                action.NewText = text;
                action.NewCursorPosition = _cursorPosition;
                _undoRedoManager.AddAction(action);
            }
            else
            {
                InsertText(text);
            }
        }

        private void DeleteAtCursor(bool forward)
        {
            if (_selectionLength > 0)
            {
                var selectedText = GetSelectedText();
                var action = new TextAction
                {
                    Type = TextActionType.Delete,
                    Position = Math.Min(_selectionStart, _selectionStart + _selectionLength),
                    Text = selectedText,
                    OldSelectionStart = _selectionStart,
                    OldSelectionLength = _selectionLength,
                    NewCursorPosition = Math.Min(_selectionStart, _selectionStart + _selectionLength)
                };

                DeleteSelection();
                _undoRedoManager.AddAction(action);
            }
            else if (forward && _cursorPosition < _textElements.Count)
            {
                var element = _textElements[_cursorPosition];
                var action = new TextAction
                {
                    Type = TextActionType.Delete,
                    Position = _cursorPosition,
                    Text = element.Text,
                    OldSelectionStart = _selectionStart,
                    OldSelectionLength = _selectionLength,
                    NewCursorPosition = _cursorPosition
                };

                _textElements.RemoveAt(_cursorPosition);
                _undoRedoManager.AddAction(action);
            }
            else if (!forward && _cursorPosition > 0)
            {
                _cursorPosition--;
                var element = _textElements[_cursorPosition];
                var action = new TextAction
                {
                    Type = TextActionType.Delete,
                    Position = _cursorPosition,
                    Text = element.Text,
                    OldSelectionStart = _selectionStart,
                    OldSelectionLength = _selectionLength,
                    NewCursorPosition = _cursorPosition
                };

                _textElements.RemoveAt(_cursorPosition);
                _selectionStart = _cursorPosition;
                _selectionLength = 0;
                _undoRedoManager.AddAction(action);
            }

            RecalculateLines();
            EnsureCursorVisible();
            Invalidate();
            OnTextChanged();
            OnSelectionChanged();
        }
        #endregion

        #region Line calculation and rendering
        private void CalculateLineHeight()
        {
            using var g = CreateGraphics();
            var size = g.MeasureString("Ag", _currentFont);
            _lineHeight = size.Height;
        }

        private void RecalculateLines()
        {
            _lines.Clear();
            if (_textElements.Count == 0)
            {
                // Calculate default line height for empty lines
                using var g1 = CreateGraphics();
                var defaultHeight = g1.MeasureString("Ag", _currentFont).Height;
                _lines.Add(new LineInfo { StartIndex = 0, Length = 0, Height = defaultHeight });
                return;
            }

            using var g = CreateGraphics();
            int lineStart = 0;
            float currentX = 0;
            float maxWidth = Width - _vScrollBar.Width - 10;

            for (int i = 0; i < _textElements.Count; i++)
            {
                var element = _textElements[i];
                
                if (element.Text == "\n" || element.Text == "\r\n")
                {
                    // Calculate line height for this line
                    float lineHeight = CalculateLineHeight(lineStart, i);
                    
                    _lines.Add(new LineInfo 
                    { 
                        StartIndex = lineStart, 
                        Length = i - lineStart + 1,
                        Height = lineHeight
                    });
                    lineStart = i + 1;
                    currentX = 0;
                    continue;
                }

                var size = g.MeasureString(element.Text, element.Font);
                if (currentX + size.Width > maxWidth && currentX > 0)
                {
                    // Calculate line height for this line
                    float lineHeight = CalculateLineHeight(lineStart, i - 1);
                    
                    _lines.Add(new LineInfo 
                    { 
                        StartIndex = lineStart, 
                        Length = i - lineStart,
                        Height = lineHeight
                    });
                    lineStart = i;
                    currentX = size.Width;
                }
                else
                {
                    currentX += size.Width;
                }
            }

            if (lineStart < _textElements.Count)
            {
                // Calculate line height for the last line
                float lineHeight = CalculateLineHeight(lineStart, _textElements.Count - 1);
                
                _lines.Add(new LineInfo 
                { 
                    StartIndex = lineStart, 
                    Length = _textElements.Count - lineStart,
                    Height = lineHeight
                });
            }

            UpdateScrollBars();
        }

        private float CalculateLineHeight(int startIndex, int endIndex)
        {
            if (startIndex > endIndex || startIndex >= _textElements.Count)
            {
                // Return default height for empty or invalid ranges
                using var graphics1 = CreateGraphics();
                return graphics1.MeasureString("Ag", _currentFont).Height;
            }

            using var graphics = CreateGraphics();
            float maxHeight = 0;

            for (int i = startIndex; i <= endIndex && i < _textElements.Count; i++)
            {
                var element = _textElements[i];
                if (element.Text == "\n" || element.Text == "\r\n") continue;

                // Measure the height of this text element
                var size = graphics.MeasureString("Ag", element.Font); // Use "Ag" for consistent height measurement
                maxHeight = Math.Max(maxHeight, size.Height);
            }

            // If no valid characters found, use default font
            if (maxHeight == 0)
            {
                maxHeight = graphics.MeasureString("Ag", _currentFont).Height;
            }

            return maxHeight;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);

            if (_lines.Count == 0) return;

            // Draw text and selection
            float y = 0;
            int currentTopLine = 0;
            
            // Calculate the Y offset based on _topLine
            for (int i = 0; i < _topLine && i < _lines.Count; i++)
            {
                y -= _lines[i].Height;
            }
            
            for (int lineIndex = _topLine; lineIndex < _lines.Count; lineIndex++)
            {
                var line = _lines[lineIndex];
                float lineHeight = line.Height;
                
                if (y > Height) break;
                if (y + lineHeight < 0)
                {
                    y += lineHeight;
                    continue;
                }

                float x = -_leftOffset;

                for (int i = line.StartIndex; i < line.StartIndex + line.Length && i < _textElements.Count; i++)
                {
                    var element = _textElements[i];
                    if (element.Text == "\n" || element.Text == "\r\n") continue;

                    var size = g.MeasureString(element.Text, element.Font);

                    // Draw selection background
                    if (IsInSelection(i))
                    {
                        g.FillRectangle(new SolidBrush(_selectionBackColor), 
                            x, y, size.Width, lineHeight);
                    }

                    // Draw text - align to baseline within the line height
                    float textY = y + (lineHeight - size.Height) / 2; // Center vertically within line height
                    g.DrawString(element.Text, element.Font, 
                        new SolidBrush(element.Color), x, textY);

                    x += size.Width;
                }

                y += lineHeight;
            }

            // Draw cursor
            if (Focused && _cursorVisible && _selectionLength == 0)
            {
                var cursorRect = GetCursorRectangle();
                g.FillRectangle(new SolidBrush(_cursorColor), cursorRect);
            }
        }

        private bool IsInSelection(int elementIndex)
        {
            if (_selectionLength == 0) return false;
            
            int start = Math.Min(_selectionStart, _selectionStart + _selectionLength);
            int end = Math.Max(_selectionStart, _selectionStart + _selectionLength);
            
            return elementIndex >= start && elementIndex < end;
        }

        private Rectangle GetCursorRectangle()
        {
            var pos = GetPositionFromIndex(_cursorPosition);
            
            // Find the line containing the cursor to get its height
            int lineIndex = GetLineFromIndex(_cursorPosition);
            float lineHeight = lineIndex < _lines.Count ? _lines[lineIndex].Height : _lineHeight;
            
            return new Rectangle(
                (int)(pos.X - _leftOffset), 
                (int)(pos.Y - GetYOffsetToLine(_topLine)), 
                1, 
                (int)lineHeight);
        }

        private PointF GetPositionFromIndex(int index)
        {
            if (_lines.Count == 0) return new PointF(0, 0);

            // Find which line contains this index
            int lineIndex = 0;
            for (int i = 0; i < _lines.Count; i++)
            {
                var line = _lines[i];
                if (index >= line.StartIndex && index <= line.StartIndex + line.Length)
                {
                    lineIndex = i;
                    break;
                }
            }

            // Calculate Y position based on cumulative line heights
            float y = GetYOffsetToLine(lineIndex);
            float x = 0;

            if (lineIndex < _lines.Count)
            {
                var line = _lines[lineIndex];
                using var graphics = CreateGraphics();
                
                for (int i = line.StartIndex; i < index && i < _textElements.Count; i++)
                {
                    var element = _textElements[i];
                    if (element.Text == "\n" || element.Text == "\r\n") continue;
                    
                    var size = graphics.MeasureString(element.Text, element.Font);
                    x += size.Width;
                }
            }

            return new PointF(x, y);
        }

        private float GetYOffsetToLine(int lineIndex)
        {
            float y = 0;
            for (int i = 0; i < lineIndex && i < _lines.Count; i++)
            {
                y += _lines[i].Height;
            }
            return y;
        }

        private int GetIndexFromPosition(Point point)
        {
            float adjustedX = point.X + _leftOffset;
            float adjustedY = point.Y + GetYOffsetToLine(_topLine);
            
            // Find which line contains this Y coordinate
            int lineIndex = 0;
            float currentY = 0;
            
            for (int i = 0; i < _lines.Count; i++)
            {
                if (adjustedY <= currentY + _lines[i].Height)
                {
                    lineIndex = i;
                    break;
                }
                currentY += _lines[i].Height;
                lineIndex = i + 1;
            }
            
            if (lineIndex < 0) return 0;
            if (lineIndex >= _lines.Count) return _textElements.Count;

            var line = _lines[lineIndex];
            float currentX = 0;
            
            using var g = CreateGraphics();
            
            for (int i = line.StartIndex; i < line.StartIndex + line.Length && i < _textElements.Count; i++)
            {
                var element = _textElements[i];
                if (element.Text == "\n" || element.Text == "\r\n") 
                    return i + 1;
                
                var size = g.MeasureString(element.Text, element.Font);
                if (adjustedX <= currentX + size.Width / 2)
                    return i;
                
                currentX += size.Width;
            }

            return Math.Min(line.StartIndex + line.Length, _textElements.Count);
        }
        #endregion

        #region Scrolling
        private void InitializeScrollBars()
        {
            _vScrollBar.Dock = DockStyle.Right;
            _vScrollBar.Scroll += VScrollBar_Scroll;
            _hScrollBar.Dock = DockStyle.Bottom;
            _hScrollBar.Scroll += HScrollBar_Scroll;
            
            Controls.Add(_vScrollBar);
            Controls.Add(_hScrollBar);
        }

        private void UpdateScrollBars()
        {
            int totalLines = _lines.Count;
            
            // Calculate total height and visible lines
            float totalHeight = 0;
            foreach (var line in _lines)
            {
                totalHeight += line.Height;
            }
            
            float visibleHeight = Height;
            int visibleLines = 0;
            float currentHeight = 0;
            
            // Calculate how many lines can fit in the visible area
            for (int i = 0; i < _lines.Count && currentHeight < visibleHeight; i++)
            {
                currentHeight += _lines[i].Height;
                visibleLines++;
            }
            
            _vScrollBar.Maximum = Math.Max(0, totalLines - 1);
            _vScrollBar.LargeChange = Math.Max(1, visibleLines);
            _vScrollBar.SmallChange = 1;
            _vScrollBar.Visible = totalHeight > visibleHeight;

            // Calculate maximum text width
            float maxWidth = 0;
            using var g = CreateGraphics();
            
            foreach (var line in _lines)
            {
                float lineWidth = 0;
                for (int i = line.StartIndex; i < line.StartIndex + line.Length && i < _textElements.Count; i++)
                {
                    var element = _textElements[i];
                    if (element.Text == "\n" || element.Text == "\r\n") continue;
                    
                    var size = g.MeasureString(element.Text, element.Font);
                    lineWidth += size.Width;
                }
                maxWidth = Math.Max(maxWidth, lineWidth);
            }

            int visibleWidth = Width - (_vScrollBar.Visible ? _vScrollBar.Width : 0);
            _hScrollBar.Maximum = Math.Max(0, (int)maxWidth - visibleWidth);
            _hScrollBar.LargeChange = Math.Max(1, visibleWidth);
            _hScrollBar.SmallChange = 20;
            _hScrollBar.Visible = maxWidth > visibleWidth;
        }

        private void VScrollBar_Scroll(object? sender, ScrollEventArgs e)
        {
            _topLine = e.NewValue;
            Invalidate();
        }

        private void HScrollBar_Scroll(object? sender, ScrollEventArgs e)
        {
            _leftOffset = e.NewValue;
            Invalidate();
        }

        private void EnsureCursorVisible()
        {
            var cursorPos = GetPositionFromIndex(_cursorPosition);
            
            // Vertical scrolling
            int cursorLine = GetLineFromIndex(_cursorPosition);
            
            // Calculate cumulative heights to determine if cursor line is visible
            float topLineY = GetYOffsetToLine(_topLine);
            float cursorLineY = GetYOffsetToLine(cursorLine);
            float cursorLineHeight = cursorLine < _lines.Count ? _lines[cursorLine].Height : _lineHeight;
            
            if (cursorLineY < topLineY)
            {
                // Cursor is above visible area
                _topLine = cursorLine;
                _vScrollBar.Value = Math.Max(0, _topLine);
            }
            else if (cursorLineY + cursorLineHeight > topLineY + Height)
            {
                // Cursor is below visible area - scroll up until cursor line is visible
                float targetY = cursorLineY + cursorLineHeight - Height;
                
                int newTopLine = 0;
                float currentY = 0;
                for (int i = 0; i < _lines.Count; i++)
                {
                    if (currentY >= targetY)
                    {
                        newTopLine = i;
                        break;
                    }
                    currentY += _lines[i].Height;
                    newTopLine = i + 1;
                }
                
                _topLine = Math.Min(newTopLine, _vScrollBar.Maximum);
                _vScrollBar.Value = _topLine;
            }

            // Horizontal scrolling
            int visibleWidth = Width - (_vScrollBar.Visible ? _vScrollBar.Width : 0);
            
            if (cursorPos.X < _leftOffset)
            {
                _leftOffset = (int)cursorPos.X;
                _hScrollBar.Value = Math.Max(0, _leftOffset);
            }
            else if (cursorPos.X >= _leftOffset + visibleWidth)
            {
                _leftOffset = (int)(cursorPos.X - visibleWidth + 50);
                _hScrollBar.Value = Math.Min(_leftOffset, _hScrollBar.Maximum);
            }

            Invalidate();
        }
        #endregion

        #region Event handlers
        private void BlinkTimer_Tick(object? sender, EventArgs e)
        {
            if (Focused)
            {
                _cursorVisible = !_cursorVisible;
                if (_selectionLength == 0)
                {
                    var cursorRect = GetCursorRectangle();
                    Invalidate(cursorRect);
                }
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _cursorVisible = true;
            _blinkTimer.Start();
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            _cursorVisible = false;
            _blinkTimer.Stop();
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();

            int index = GetIndexFromPosition(e.Location);
            
            if (ModifierKeys.HasFlag(Keys.Shift))
            {
                _selectionLength = index - _selectionStart;
            }
            else
            {
                _cursorPosition = index;
                _selectionStart = index;
                _selectionLength = 0;
            }

            _isDragging = true;
            _dragStartPoint = e.Location;
            
            Invalidate();
            OnSelectionChanged();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            
            if (_isDragging && e.Button == MouseButtons.Left)
            {
                int index = GetIndexFromPosition(e.Location);
                _selectionLength = index - _selectionStart;
                _cursorPosition = index;
                
                Invalidate();
                OnSelectionChanged();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isDragging = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            bool shift = ModifierKeys.HasFlag(Keys.Shift);
            bool ctrl = ModifierKeys.HasFlag(Keys.Control);

            switch (e.KeyCode)
            {
                case Keys.Left:
                    MoveCursor(-1, shift, ctrl);
                    e.Handled = true;
                    break;
                case Keys.Right:
                    MoveCursor(1, shift, ctrl);
                    e.Handled = true;
                    break;
                case Keys.Up:
                    MoveCursorVertical(-1, shift);
                    e.Handled = true;
                    break;
                case Keys.Down:
                    MoveCursorVertical(1, shift);
                    e.Handled = true;
                    break;
                case Keys.Home:
                    MoveCursorToLineStart(shift);
                    e.Handled = true;
                    break;
                case Keys.End:
                    MoveCursorToLineEnd(shift);
                    e.Handled = true;
                    break;
                case Keys.Delete:
                    DeleteAtCursor(true);
                    e.Handled = true;
                    break;
                case Keys.Back:
                    DeleteAtCursor(false);
                    e.Handled = true;
                    break;
                case Keys.Enter:
                    InsertText("\n");
                    e.Handled = true;
                    break;
                case Keys.Tab:
                    if (!ctrl)
                    {
                        InsertText("\t");
                        e.Handled = true;
                    }
                    break;
                case Keys.A when ctrl:
                    SelectAll();
                    e.Handled = true;
                    break;
                case Keys.C when ctrl:
                    Copy();
                    e.Handled = true;
                    break;
                case Keys.V when ctrl:
                    Paste();
                    e.Handled = true;
                    break;
                case Keys.X when ctrl:
                    Cut();
                    e.Handled = true;
                    break;
                case Keys.Z when ctrl:
                    Undo();
                    e.Handled = true;
                    break;
                case Keys.Y when ctrl:
                    Redo();
                    e.Handled = true;
                    break;
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            
            if (!char.IsControl(e.KeyChar))
            {
                InsertText(e.KeyChar.ToString());
                e.Handled = true;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RecalculateLines();
            UpdateScrollBars();
            EnsureCursorVisible();
        }
        #endregion

        #region Cursor movement
        private void MoveCursor(int direction, bool shift, bool word)
        {
            int newPosition = _cursorPosition;
            
            if (word)
            {
                newPosition = direction > 0 ? GetNextWordPosition() : GetPreviousWordPosition();
            }
            else
            {
                newPosition = Math.Max(0, Math.Min(_textElements.Count, _cursorPosition + direction));
            }

            if (shift)
            {
                _selectionLength = newPosition - _selectionStart;
            }
            else
            {
                _selectionStart = newPosition;
                _selectionLength = 0;
            }

            _cursorPosition = newPosition;
            EnsureCursorVisible();
            Invalidate();
            OnSelectionChanged();
        }

        private void MoveCursorVertical(int direction, bool shift)
        {
            int currentLine = GetLineFromIndex(_cursorPosition);
            int targetLine = currentLine + direction;
            
            if (targetLine < 0) targetLine = 0;
            if (targetLine >= _lines.Count) targetLine = _lines.Count - 1;
            
            if (targetLine == currentLine) return; // No movement needed
            
            // Calculate the horizontal position (column) within the current line
            var currentLineInfo = _lines[currentLine];
            int columnInCurrentLine = _cursorPosition - currentLineInfo.StartIndex;
            
            // Find the best position in the target line
            var targetLineInfo = _lines[targetLine];
            int newIndex;
            
            if (columnInCurrentLine >= targetLineInfo.Length)
            {
                // If the target line is shorter, go to the end of the target line
                // But don't include the newline character if it exists
                newIndex = targetLineInfo.StartIndex + targetLineInfo.Length;
                if (newIndex > 0 && newIndex <= _textElements.Count && 
                    newIndex < _textElements.Count && 
                    (_textElements[newIndex - 1].Text == "\n" || _textElements[newIndex - 1].Text == "\r\n"))
                {
                    newIndex--;
                }
            }
            else
            {
                // Move to the same column position in the target line
                newIndex = targetLineInfo.StartIndex + columnInCurrentLine;
            }
            
            // Ensure the new index is within bounds
            newIndex = Math.Max(0, Math.Min(newIndex, _textElements.Count));

            if (shift)
            {
                _selectionLength = newIndex - _selectionStart;
            }
            else
            {
                _selectionStart = newIndex;
                _selectionLength = 0;
            }

            _cursorPosition = newIndex;
            EnsureCursorVisible();
            Invalidate();
            OnSelectionChanged();
        }

        private void MoveCursorToLineStart(bool shift)
        {
            // Find the start of the current line
            int lineStart = _cursorPosition;
            while (lineStart > 0 && _textElements[lineStart - 1].Text != "\n" && _textElements[lineStart - 1].Text != "\r\n")
            {
                lineStart--;
            }

            if (shift)
            {
                _selectionLength = lineStart - _selectionStart;
            }
            else
            {
                _selectionStart = lineStart;
                _selectionLength = 0;
            }

            _cursorPosition = lineStart;
            EnsureCursorVisible();
            Invalidate();
            OnSelectionChanged();
        }

        private void MoveCursorToLineEnd(bool shift)
        {
            // Find the end of the current line
            int lineEnd = _cursorPosition;
            while (lineEnd < _textElements.Count && _textElements[lineEnd].Text != "\n" && _textElements[lineEnd].Text != "\r\n")
            {
                lineEnd++;
            }

            if (shift)
            {
                _selectionLength = lineEnd - _selectionStart;
            }
            else
            {
                _selectionStart = lineEnd;
                _selectionLength = 0;
            }

            _cursorPosition = lineEnd;
            EnsureCursorVisible();
            Invalidate();
            OnSelectionChanged();
        }

        private int GetNextWordPosition()
        {
            int pos = _cursorPosition;
            
            // Skip current word
            while (pos < _textElements.Count && !IsWordSeparator(_textElements[pos].Text))
                pos++;
            
            // Skip whitespace
            while (pos < _textElements.Count && IsWordSeparator(_textElements[pos].Text))
                pos++;
            
            return pos;
        }

        private int GetPreviousWordPosition()
        {
            int pos = _cursorPosition - 1;
            
            // Skip whitespace
            while (pos >= 0 && IsWordSeparator(_textElements[pos].Text))
                pos--;
            
            // Skip current word
            while (pos >= 0 && !IsWordSeparator(_textElements[pos].Text))
                pos--;
            
            return Math.Max(0, pos + 1);
        }

        private bool IsWordSeparator(string text)
        {
            return string.IsNullOrWhiteSpace(text) || char.IsPunctuation(text, 0) || char.IsSeparator(text, 0);
        }
        #endregion

        #region Clipboard operations
        public void Copy()
        {
            if (_selectionLength != 0)
            {
                Clipboard.SetText(GetSelectedText());
            }
        }

        public void Cut()
        {
            if (_selectionLength != 0)
            {
                Copy();
                DeleteSelection();
                RecalculateLines();
                EnsureCursorVisible();
                Invalidate();
                OnTextChanged();
                OnSelectionChanged();
            }
        }

        public void Paste()
        {
            if (Clipboard.ContainsText())
            {
                string clipboardText = Clipboard.GetText();
                InsertText(clipboardText);
            }
        }

        public void SelectAll()
        {
            _selectionStart = 0;
            _selectionLength = _textElements.Count;
            _cursorPosition = _textElements.Count;
            Invalidate();
            OnSelectionChanged();
        }
        #endregion

        #region Undo/Redo
        public void Undo()
        {
            if (_undoRedoManager.CanUndo)
            {
                var action = _undoRedoManager.Undo();
                ApplyAction(action, true);
            }
        }

        public void Redo()
        {
            if (_undoRedoManager.CanRedo)
            {
                var action = _undoRedoManager.Redo();
                ApplyAction(action, false);
            }
        }

        private void ApplyAction(TextAction action, bool isUndo)
        {
            switch (action.Type)
            {
                case TextActionType.Insert:
                    if (isUndo)
                    {
                        // Remove inserted text
                        for (int i = 0; i < action.Text.Length && action.Position < _textElements.Count; i++)
                        {
                            _textElements.RemoveAt(action.Position);
                        }
                        _cursorPosition = action.Position;
                    }
                    else
                    {
                        // Re-insert text
                        InsertTextAtPosition(action.Text, action.Position);
                        _cursorPosition = action.NewCursorPosition;
                    }
                    break;

                case TextActionType.Delete:
                    if (isUndo)
                    {
                        // Re-insert deleted text
                        InsertTextAtPosition(action.Text, action.Position);
                        _cursorPosition = action.Position + action.Text.Length;
                    }
                    else
                    {
                        // Re-delete text
                        for (int i = 0; i < action.Text.Length && action.Position < _textElements.Count; i++)
                        {
                            _textElements.RemoveAt(action.Position);
                        }
                        _cursorPosition = action.Position;
                    }
                    break;
            }

            _selectionStart = _cursorPosition;
            _selectionLength = 0;
            RecalculateLines();
            EnsureCursorVisible();
            Invalidate();
            OnTextChanged();
            OnSelectionChanged();
        }

        private void InsertTextAtPosition(string text, int position)
        {
            var elements = StringInfo.GetTextElementEnumerator(text);
            int insertCount = 0;
            while (elements.MoveNext())
            {
                var element = elements.GetTextElement();
                _textElements.Insert(position + insertCount, 
                    new TextElement(element, _currentFont, _currentTextColor));
                insertCount++;
            }
        }
        #endregion

        #region Public methods
        public void Clear()
        {
            _textElements.Clear();
            _cursorPosition = 0;
            _selectionStart = 0;
            _selectionLength = 0;
            _undoRedoManager.Clear();
            RecalculateLines();
            Invalidate();
            OnTextChanged();
            OnSelectionChanged();
        }

        public void ApplyFont(Font font)
        {
            if (_selectionLength > 0)
            {
                int start = Math.Min(_selectionStart, _selectionStart + _selectionLength);
                int length = Math.Abs(_selectionLength);

                for (int i = start; i < start + length && i < _textElements.Count; i++)
                {
                    _textElements[i] = new TextElement(_textElements[i].Text, font, _textElements[i].Color);
                }

                RecalculateLines();
                Invalidate();
            }
            else
            {
                _currentFont = font;
            }
        }

        public void ApplyColor(Color color)
        {
            if (_selectionLength > 0)
            {
                int start = Math.Min(_selectionStart, _selectionStart + _selectionLength);
                int length = Math.Abs(_selectionLength);

                for (int i = start; i < start + length && i < _textElements.Count; i++)
                {
                    _textElements[i] = new TextElement(_textElements[i].Text, _textElements[i].Font, color);
                }

                Invalidate();
            }
            else
            {
                _currentTextColor = color;
            }
        }

        public Font? GetSelectionFont()
        {
            if (_selectionLength > 0)
            {
                int start = Math.Min(_selectionStart, _selectionStart + _selectionLength);
                if (start < _textElements.Count)
                {
                    return _textElements[start].Font;
                }
            }
            return _currentFont;
        }

        public Color GetSelectionColor()
        {
            if (_selectionLength > 0)
            {
                int start = Math.Min(_selectionStart, _selectionStart + _selectionLength);
                if (start < _textElements.Count)
                {
                    return _textElements[start].Color;
                }
            }
            return _currentTextColor;
        }

        public int GetLineFromIndex(int index)
        {
            for (int i = 0; i < _lines.Count; i++)
            {
                var line = _lines[i];
                if (index >= line.StartIndex && index <= line.StartIndex + line.Length)
                {
                    return i;
                }
            }
            return _lines.Count > 0 ? _lines.Count - 1 : 0;
        }

        public int GetColumnFromIndex(int index)
        {
            int line = GetLineFromIndex(index);
            if (line < _lines.Count)
            {
                return index - _lines[line].StartIndex;
            }
            return 0;
        }
        #endregion

        #region Event invocation
        protected virtual void OnTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSelectionChanged()
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _blinkTimer?.Dispose();
                _currentFont?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    #region Supporting classes
    public class TextElement
    {
        public string Text { get; }
        public Font Font { get; }
        public Color Color { get; }

        public TextElement(string text, Font font, Color color)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Font = font ?? throw new ArgumentNullException(nameof(font));
            Color = color;
        }
    }

    public class LineInfo
    {
        public int StartIndex { get; set; }
        public int Length { get; set; }
        public float Height { get; set; }
    }

    public enum TextActionType
    {
        Insert,
        Delete,
        Replace
    }

    public class TextAction
    {
        public TextActionType Type { get; set; }
        public int Position { get; set; }
        public string Text { get; set; } = string.Empty;
        public string NewText { get; set; } = string.Empty;
        public int OldSelectionStart { get; set; }
        public int OldSelectionLength { get; set; }
        public int NewCursorPosition { get; set; }
    }

    public class UndoRedoManager
    {
        private readonly Stack<TextAction> _undoStack = new();
        private readonly Stack<TextAction> _redoStack = new();
        private const int MaxUndoLevels = 100;

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public void AddAction(TextAction action)
        {
            _undoStack.Push(action);
            _redoStack.Clear();

            // Limit undo stack size
            if (_undoStack.Count > MaxUndoLevels)
            {
                var items = _undoStack.ToArray();
                _undoStack.Clear();
                for (int i = items.Length - MaxUndoLevels; i < items.Length; i++)
                {
                    _undoStack.Push(items[i]);
                }
            }
        }

        public TextAction Undo()
        {
            if (!CanUndo) throw new InvalidOperationException("Nothing to undo");
            
            var action = _undoStack.Pop();
            _redoStack.Push(action);
            return action;
        }

        public TextAction Redo()
        {
            if (!CanRedo) throw new InvalidOperationException("Nothing to redo");
            
            var action = _redoStack.Pop();
            _undoStack.Push(action);
            return action;
        }

        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }
    }
    #endregion
}