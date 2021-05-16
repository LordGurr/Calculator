using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Calculator
{
    internal class InputBox : Button
    {
        private Stopwatch timeSinceCursor;
        private Stopwatch timeSinceWritten;
        private float delayAfterWrittenCursor = 1f;
        private float cursorFlash = 0.5f;
        private float ratio;
        private float percentage = 0.98f;
        private SpriteFont font;
        private StringBuilder stringBuilder;
        private bool mulitpleLines = false;
        private string previousString = string.Empty;
        public bool modifiedText = false;

        public InputBox(Rectangle _rectangle, Texture2D _texture, string _text, bool _mulitpleLines)
            : base(_rectangle, _texture, _text)
        {
            timeSinceCursor = new Stopwatch();
            timeSinceCursor.Start();
            timeSinceWritten = new Stopwatch();
            timeSinceWritten.Start();
            //ratio = (float)_rectangle.Width / (float)_rectangle.Height;
            //rectangle = new Rectangle((int)(rectangle.X + rectangle.Width * (1 - percentage)), (int)Math.Round(rectangle.Y + rectangle.Height * ((1 - percentage) * ratio)), (int)(rectangle.Width * MathF.Pow(percentage, 2)), (int)(rectangle.Height * (MathF.Pow((percentage - 1) * ratio + 1, 2))));
            stringBuilder = new StringBuilder();
            stringBuilder.Append(text);
            mulitpleLines = _mulitpleLines;
        }

        public InputBox(Rectangle _rectangle, Texture2D _texture, string _text, Color _rectangleColor, bool _mulitpleLines)
            : base(_rectangle, _texture, _text, _rectangleColor)
        {
            timeSinceCursor = new Stopwatch();
            timeSinceCursor.Start();
            timeSinceWritten = new Stopwatch();
            timeSinceWritten.Start();
            //ratio = (float)_rectangle.Width / (float)_rectangle.Height;
            //rectangle = new Rectangle((int)(rectangle.X + rectangle.Width * (1 - percentage)), (int)Math.Round(rectangle.Y + rectangle.Height * ((1 - percentage) * ratio)), (int)(rectangle.Width * MathF.Pow(percentage, 2)), (int)(rectangle.Height * (MathF.Pow((percentage - 1) * ratio + 1, 2))));
            stringBuilder = new StringBuilder();
            stringBuilder.Append(text);
            mulitpleLines = _mulitpleLines;
        }

        public InputBox(Rectangle _rectangle, Texture2D _texture, string _text, Color _rectangleColor, Color _textColor, bool _mulitpleLines)
            : base(_rectangle, _texture, _text, _rectangleColor, _textColor)
        {
            timeSinceCursor = new Stopwatch();
            timeSinceCursor.Start();
            timeSinceWritten = new Stopwatch();
            timeSinceWritten.Start();
            //ratio = (float)_rectangle.Width / (float)_rectangle.Height;
            //rectangle = new Rectangle((int)(rectangle.X + rectangle.Width * (1 - percentage)), (int)Math.Round(rectangle.Y + rectangle.Height * ((1 - percentage) * ratio)), (int)(rectangle.Width * MathF.Pow(percentage, 2)), (int)(rectangle.Height * (MathF.Pow((percentage - 1) * ratio + 1, 2))));
            stringBuilder = new StringBuilder();
            stringBuilder.Append(text);
            mulitpleLines = _mulitpleLines;
        }

        private bool isSelected = false;

        public bool Selected(GameWindow window)
        {
            if (Input.GetMouseButtonDown(0))
            {
                bool previosly = isSelected;
                isSelected = rectangle.Contains(Input.MousePos());
                if (previosly != isSelected)
                {
                    if (isSelected)
                    {
                        window.TextInput += TextInputHandler;
                        window.KeyDown += KeyHandler;
                        timeSinceCursor.Restart();
                        showingCursor = true;
                    }
                    else
                    {
                        window.TextInput -= TextInputHandler;
                        window.KeyDown -= KeyHandler;
                    }
                }
            }
            return isSelected;
        }

        public void SetSelected(GameWindow window, bool toBe)
        {
            bool previosly = isSelected;
            isSelected = toBe;
            if (previosly != isSelected)
            {
                if (isSelected)
                {
                    window.TextInput += TextInputHandler;
                    window.KeyDown += KeyHandler;
                    timeSinceCursor.Restart();
                    showingCursor = true;
                }
                else
                {
                    window.TextInput -= TextInputHandler;
                    window.KeyDown -= KeyHandler;
                }
            }
        }

        public void AddText(string input)
        {
            previousString = text;
            text += input;
            modifiedText = true;
        }

        private int cursorPos = -1;

        public void AddText(char input)
        {
            if (input == '\u0016') // ctr V
            {
                previousString = text;
                string temp = System.Windows.Forms.Clipboard.GetText();
                if (font.MeasureString(text + temp).X > rectangle.Width && mulitpleLines)
                {
                    text += "\n";
                    stringBuilder.Append("\n");
                }
                //text += input;
                text = text.Insert(cursorPos + 1, temp.ToString());
                stringBuilder.Insert(cursorPos + 1, temp);
                cursorPos += temp.Length;
                modifiedText = true;
            }
            else if (input == '\u0003') //ctr C
            {
                if (text != null && text != string.Empty)
                {
                    System.Windows.Forms.Clipboard.SetText(text);
                }
            }
            else if (input == '\u0018')// ctr X
            {
                if (text != null && text != string.Empty)
                {
                    previousString = text;
                    System.Windows.Forms.Clipboard.SetText(text);
                    text = string.Empty;
                    cursorPos = -1;
                    modifiedText = true;
                }
            }
            else if (input == '\u001a')// ctr Z
            {
                string temp = text;
                text = previousString;
                previousString = temp;
                cursorPos = MathHelper.Clamp(cursorPos, -1, text.Length - 1);
                modifiedText = true;
            }
            else if (input == '\b' || input == '\r') // \b == backspace. \r == remove
            {
                previousString = text;
                if (input == '\b')
                {
                    if (text.Length > 0 && cursorPos > -1)
                    {
                        text = text.Remove(cursorPos, 1);
                        stringBuilder.Remove(cursorPos, 1);
                        cursorPos--;
                    }
                }
                else if (input == '\r' && mulitpleLines)
                {
                    text = text.Insert(cursorPos + 1, "\n");
                    stringBuilder.Insert(cursorPos + 1, "\n");
                    cursorPos++;
                }
                modifiedText = true;
            }
            else
            {
                if (font.Characters.Contains(input))
                {
                    previousString = text;
                    if (font.MeasureString(text + input).X > rectangle.Width && mulitpleLines)
                    {
                        text += "\n";
                        stringBuilder.Append("\n");
                    }
                    //text += input;
                    text = text.Insert(cursorPos + 1, input.ToString());
                    stringBuilder.Insert(cursorPos + 1, input);
                    cursorPos++;
                    modifiedText = true;
                }
            }
        }

        private void TextInputHandler(object sender, TextInputEventArgs args)
        {
            var pressedKey = args.Key;
            var character = args.Character;
            AddText(character);
            timeSinceWritten.Restart();
            showingCursor = true;
            // do something with the character (and optionally the key)
            // ...
        }

        private void KeyHandler(object sender, InputKeyEventArgs args)
        {
            if (args.Key == Keys.Left)
            {
                cursorPos--;
                cursorPos = MathHelper.Clamp(cursorPos, -1, text.Length - 1);
            }
            if (args.Key == Keys.Right)
            {
                cursorPos++;
                cursorPos = MathHelper.Clamp(cursorPos, -1, text.Length - 1);
            }
            if (args.Key == Keys.Delete)
            {
                if (text.Length > 0 && cursorPos < text.Length - 1)
                {
                    text = text.Remove(cursorPos + 1, 1);
                    stringBuilder.Remove(cursorPos + 1, 1);
                    modifiedText = true;
                }
            }
            if (args.Key == Keys.Up)
            {
                if (mulitpleLines)
                {
                    cursorPos -= AmountToMoveOnLineChange();
                    cursorPos = MathHelper.Clamp(cursorPos, -1, text.Length - 1);
                }
                else
                {
                    cursorPos = -1;
                }
            }
            if (args.Key == Keys.Down)
            {
                if (mulitpleLines)
                {
                    cursorPos += AmountToMoveOnLineChange();
                    cursorPos = MathHelper.Clamp(cursorPos, -1, text.Length - 1);
                }
                else
                {
                    cursorPos = text.Length - 1;
                }
            }
            timeSinceWritten.Restart();
            showingCursor = true;
        }

        private int AmountToMoveOnLineChange()
        {
            string[] lines = text.Split("\n");
            int lineWithCursor = -1;
            int cursor = cursorPos;
            //cursor -= lines.Length;
            int temp = -1;
            int cursorPosInLine = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (i + 1 < lines.Length)
                {
                    lines[i] += "\n";
                }
            }
            for (int i = 0; i < lines.Length; i++)
            {
                //if (cursor >= lines[i].Length && i + 1 < lines.Length)
                //{
                //    cursor -= lines[i].Length;
                //}
                //else
                //{
                //    lineWithCursor = i;
                //    break;
                //}
                if (temp >= cursor || i + 1 >= lines.Length)
                {
                    //lineWithCursor = i + 1 < lines.Length ? i + 1 : i;
                    lineWithCursor = i;
                    //cursorPosInLine = temp - cursor + 1;
                    cursorPosInLine = cursor - (temp - lines[i].Length) + 1;
                    break;
                }
                temp += lines[i].Length;
            }
            for (int i = 0; i < lineWithCursor; i++)
            {
            }
            if (lineWithCursor > -1)
            {
                //int newCursoPos = 0;
                //for (int i = 0; i < lineWithCursor; i++)
                //{
                //    newCursoPos += lines[i].Length + 1;
                //}
                //cursorPos = newCursoPos;
                //cursorPos -= lines[lineWithCursor].Length + 1;
                return cursorPosInLine;
                return lines[lineWithCursor].Length;
            }
            return 0;
        }

        private bool showingCursor = false;

        public override void Draw(SpriteBatch _spriteBatch, SpriteFont _font)
        {
            if (isSelected)
            {
                if (font != _font)
                {
                    font = _font;
                }
                if (timeSinceCursor.Elapsed.TotalSeconds > cursorFlash)
                {
                    timeSinceCursor.Restart();
                    if (timeSinceWritten.Elapsed.TotalSeconds > delayAfterWrittenCursor)
                    {
                        showingCursor = !showingCursor;
                    }
                    else
                    {
                        showingCursor = true;
                    }
                    //showingCursor = true;
                }
                if (showingCursor)
                {
                    string realText = text;
                    text = text.Insert(cursorPos + 1, "|");
                    base.Draw(_spriteBatch, _font);
                    text = realText;
                }
                else
                {
                    base.Draw(_spriteBatch, _font);
                }
            }
            else
            {
                base.Draw(_spriteBatch, _font);
            }
            modifiedText = false;
        }

        // Mask Text: https://stackoverflow.com/questions/7946321/drawing-a-textbox-with-clipping-of-the-text-inside
        public bool Modified()
        {
            return modifiedText;
        }
    }
}