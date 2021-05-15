using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Calculator
{
    internal class Button
    {
        public Rectangle rectangle { protected set; get; }
        public Texture2D texture { protected set; get; }
        public string text { protected set; get; }
        public bool pressed { protected set; get; }
        public Color rectangleColor { protected set; get; }
        public Color textColor { protected set; get; }

        public Button(Rectangle _rectangle, Texture2D _texture, string _text)
        {
            rectangle = _rectangle;
            texture = _texture;
            text = _text;
            pressed = false;
            rectangleColor = Color.White;
            textColor = Color.White;
        }

        public Button(Rectangle _rectangle, Texture2D _texture, string _text, Color _rectangleColor)
        {
            rectangle = _rectangle;
            texture = _texture;
            text = _text;
            pressed = false;
            rectangleColor = _rectangleColor;
            textColor = Color.White;
        }

        public Button(Rectangle _rectangle, Texture2D _texture, string _text, Color _rectangleColor, Color _textColor)
        {
            rectangle = _rectangle;
            texture = _texture;
            text = _text;
            pressed = false;
            rectangleColor = _rectangleColor;
            textColor = _textColor;
        }

        public bool Clicked()
        {
            if (pressed)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (rectangle.Contains(Input.MousePos().X, Input.MousePos().Y))
                    {
                        pressed = false;
                        return true;
                    }
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (rectangle.Contains(Input.MousePos().X, Input.MousePos().Y))
                {
                    pressed = true;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                pressed = false;
            }
            return false;
        }

        public void setPos(int x, int y)
        {
            rectangle = new Rectangle(x, y, rectangle.Width, rectangle.Height);
            //rectangle.Y = y;
        }

        public void setPos(Vector2 pos)
        {
            rectangle = new Rectangle((int)pos.X, (int)pos.Y, rectangle.Width, rectangle.Height);
            //rectangle.Y = y;
        }

        public void setSize(int width, int height)
        {
            rectangle = new Rectangle(rectangle.X, rectangle.Y, width, height);
            //rectangle.Y = y;
        }

        public void setSize(Vector2 size)
        {
            rectangle = new Rectangle(rectangle.X, rectangle.Y, (int)size.X, (int)size.Y);
            //rectangle.Y = y;
        }

        public void setRectangle(Rectangle _rectangle)
        {
            rectangle = _rectangle;
            //rectangle.Y = y;
        }

        public virtual void Draw(SpriteBatch _spriteBatch, SpriteFont font/*, Vector3 offset*/)
        {
            //setPos(rectangle.X - (int)offset.X, rectangle.Y - (int)offset.Y);
            //_spriteBatch.Draw(texture, new Rectangle(rectangle.X - (int)offset.X, rectangle.Y - (int)offset.Y, rectangle.Width, rectangle.Height), Color.White);
            _spriteBatch.Draw(texture, rectangle, rectangleColor);
            Vector2 size = font.MeasureString(text);

            _spriteBatch.DrawString(font, text, new Vector2(rectangle.X + 5, rectangle.Y), textColor, 0, new Vector2(/*0, size.Y / 2*/), 1.2f, SpriteEffects.None, 1);
        }

        public void Draw(SpriteBatch _spriteBatch, SpriteFont font, Color color/*, Vector3 offset*/)
        {
            //setPos(rectangle.X - (int)offset.X, rectangle.Y - (int)offset.Y);
            //_spriteBatch.Draw(texture, new Rectangle(rectangle.X - (int)offset.X, rectangle.Y - (int)offset.Y, rectangle.Width, rectangle.Height), Color.White);
            _spriteBatch.Draw(texture, rectangle, color);
            Vector2 size = font.MeasureString(text);

            _spriteBatch.DrawString(font, text, new Vector2(rectangle.X + rectangle.Width / 2 /*- offset.X*/, rectangle.Y + rectangle.Height / 2 /*- offset.Y*/), textColor, 0, new Vector2(size.X / 2, size.Y / 2), 1.2f, SpriteEffects.None, 1);
        }

        //public void Draw(SpriteBatch _spriteBatch, SpriteFont font, GraphicsDeviceManager _graphics/*, Vector3 offset*/)
        //{
        //    //setPos(rectangle.X - (int)offset.X, rectangle.Y - (int)offset.Y);
        //    //_spriteBatch.Draw(texture, new Rectangle(rectangle.X - (int)offset.X, rectangle.Y - (int)offset.Y, rectangle.Width, rectangle.Height), Color.White);
        //    _spriteBatch.Draw(texture, rectangle, rectangleColor);
        //    Vector2 size = font.MeasureString(text);
        //    if (true || _graphics.PreferredBackBufferWidth / 960 >= 1)
        //    {
        //        _spriteBatch.DrawString(font, text, new Vector2(rectangle.X + rectangle.Width / 2 /*- offset.X*/, rectangle.Y + rectangle.Height / 2 /*- offset.Y*/), textColor, 0, new Vector2(size.X / 2, size.Y / 2), (float)_graphics.PreferredBackBufferWidth / 1600, SpriteEffects.None, 1);
        //    }
        //    else
        //    {
        //        _spriteBatch.DrawString(font, text, new Vector2(rectangle.X + rectangle.Width / 2 /*- offset.X*/, rectangle.Y + rectangle.Height / 2 /*- offset.Y*/), textColor, 0, new Vector2(size.X / 2, size.Y / 2), 0.6f, SpriteEffects.None, 1);
        //    }
        //}
    }
}