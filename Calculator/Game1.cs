using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Calculator
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D square;
        private Vector2 position;
        private Camera camera;
        private SpriteFont font;
        private int lines = 0;
        private List<List<Vector2>> allEquations = new List<List<Vector2>>();
        private List<Tuple<InputBox, List<Vector2>>> inputButtons;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            allEquations.Add(new List<Vector2>());
            Equation(allEquations[0], -40, 80);
            allEquations.Add(new List<Vector2>());
            OtherEquation(allEquations[1], -40, 80);
            allEquations.Add(new List<Vector2>());
            SetEquation(allEquations[allEquations.Count - 1], -40, 80, "sin(yrad2deg)");
            input = "25y";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera = new Camera(new Viewport(new Rectangle(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight)), Window);
            camera.Zoom = 4;
            position = new Vector2();
            //Window.TextInput += TextInputHandler;
            inputButtons = new List<Tuple<InputBox, List<Vector2>>>();
            Input.setCameraStuff(camera);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            square = Content.Load<Texture2D>("Square1");
            font = Content.Load<SpriteFont>("font");
            inputButtons.Add(new Tuple<InputBox, List<Vector2>>(new InputBox(new Rectangle(20, 20, 300, 100), square, "", Color.Green, false), new List<Vector2>()));

            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                Input.GetState(true);
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();
                for (int i = 0; i < inputButtons.Count; i++)
                {
                    inputButtons[i].Item1.Selected(Window);
                    if (inputButtons[i].Item1.Modified())
                    {
                        List<Vector2> temp = new List<Vector2>();
                        try
                        {
                            SetEquation(temp, -40, 80, inputButtons[i].Item1.text);
                            inputButtons[i].Item2.Clear();
                            for (int a = 0; a < temp.Count; a++)
                            {
                                inputButtons[i].Item2.Add(temp[a]);
                            }
                            inputButtons[i].Item1.modifiedText = false;
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("Failed to update graph: " + e.Message);
                            inputButtons[i].Item2.Clear();
                        }
                    }
                }
                // TODO: Add your update logic here
                camera.Zoom = (float)(Input.clampedScrollWheelValue * 0.001) + 1;
                camera.UpdateCamera(position);
                UpdateMouse(gameTime);
            }
            else
            {
                Input.GetState(false);
                lastMousePosition = Input.MousePos();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            lines = 0;
            GraphicsDevice.Clear(Color.White);
            //_spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, transformMatrix: camera.transform);
            int size = 50;
            int width = 5;
            DrawGrid(size, width);
            //for (int i = 0; i < Window.ClientBounds.Height; i += size)
            //{
            //    DrawLine(_spriteBatch, new Vector2(0, i), new Vector2(Window.ClientBounds.Width, i), width);
            //    lines++;
            //}
            //for (int a = 0; a < Window.ClientBounds.Width; a += size)
            //{
            //    DrawLine(_spriteBatch, new Vector2(a, 0), new Vector2(a, Window.ClientBounds.Height), width);
            //    lines++;
            //}
            List<Color> färger = new List<Color>();
            färger.Add(Color.Red);
            färger.Add(Color.Brown);
            färger.Add(Color.CornflowerBlue);
            färger.Add(Color.Green);
            for (int a = 0; a < allEquations.Count; a++)
            {
                for (int i = 0; i < allEquations[a].Count; i++)
                {
                    if (i + 1 < allEquations[a].Count)
                    {
                        DrawLine(_spriteBatch, allEquations[a][i], allEquations[a][i + 1], (int)(width * 1.5), a < färger.Count ? färger[a] : Color.Blue);
                    }
                }
            }
            for (int i = 0; i < inputButtons.Count; i++)
            {
                for (int a = 0; a < inputButtons[i].Item2.Count; a++)
                {
                    if (a + 1 < inputButtons[i].Item2.Count)
                    {
                        DrawLine(_spriteBatch, inputButtons[i].Item2[a], inputButtons[i].Item2[a + 1], (int)(width * 1.5), i < färger.Count ? färger[i] : Color.Blue);
                    }
                }
            }
            _spriteBatch.Draw(square, new Rectangle(300, 300, 200, 200), Color.Red);
            //Vector2 right = AdvancedMath.Right(AdvancedMath.AngleBetween(new Vector2(), camera.ScreenToWorldSpace(Input.MousePos())));
            //DrawLine(_spriteBatch, new Vector2(0, 0) * (5 * right), camera.ScreenToWorldSpace(Input.MousePos()) * (5 * right));

            // TODO: Add your drawing code here

            Vector3 temp = camera.transform.Translation;

            //for (int i = ((int)Math.Round(camera.ScreenToWorldSpace(new Vector2()).Y) % size); i < ((int)Math.Round(camera.ScreenToWorldSpace(new Vector2(0, Window.ClientBounds.Width)).Y) % size); i += size)
            //{
            //    DrawLine(_spriteBatch, new Vector2((int)Math.Round(camera.ScreenToWorldSpace(new Vector2()).X), (i)), new Vector2((int)Math.Round(camera.ScreenToWorldSpace(new Vector2(Window.ClientBounds.Width, 0)).X), (i)), width);
            //    lines++;
            //}
            //for (int a = 0; a < Window.ClientBounds.Width; a++)
            //{
            //    DrawLine(_spriteBatch, new Vector2((a + 1) * size, 0), new Vector2((a + 1) * size, Window.ClientBounds.Height), width);
            //}

            //for (float i = -width*4 - _graphics.PreferredBackBufferWidth * Math.Abs(position.X) / 100 * 30 - (Math.Abs(position.X) > -1 || Math.Abs(position.X) < 1 ? _graphics.PreferredBackBufferWidth : 0); i < _graphics.PreferredBackBufferWidth + position.X; i += 15)//                     for (float i = -120 - _graphics.PreferredBackBufferWidth * Math.Clamp(Math.Abs(position.X), 1, float.MaxValue) / 100 * 30 - (Math.Abs(position.X) > -1 || Math.Abs(position.X) < 1 ? _graphics.PreferredBackBufferWidth : 0); i < _graphics.PreferredBackBufferWidth + position.X; i += 15)

            //{
            //    _spriteBatch.Draw(gräsTile, new Vector2(i, ground), null, Color.White, 0, new Vector2(7.5f, -0), 2, SpriteEffects.None, 1);
            //}
            //for (float a = 0 + 30; a < _graphics.PreferredBackBufferHeight + _graphics.PreferredBackBufferHeight / 8; a += 30)
            //{
            //    for (float i = -120 - _graphics.PreferredBackBufferWidth * Math.Abs(position.X) / 100 * 30 - (Math.Abs(position.X) > -1 || Math.Abs(position.X) < 1 ? _graphics.PreferredBackBufferWidth : 0); i < _graphics.PreferredBackBufferWidth + position.X; i += 15) //                         for (float i = -120 - _graphics.PreferredBackBufferWidth * Math.Clamp(Math.Abs(position.X), 1, float.MaxValue) / 100 * 30 - (Math.Abs(position.X) > -1 || Math.Abs(position.X) < 1 ? _graphics.PreferredBackBufferWidth : 0); i < _graphics.PreferredBackBufferWidth + position.X; i += 15)

            //    {
            //        _spriteBatch.Draw(jordTile, new Vector2(i, a), null, Color.White, 0, new Vector2(7.5f, -0), 2, SpriteEffects.None, 1);
            //    }
            //}
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            _spriteBatch.DrawString(font, "pos: " + position.ToString(), new Vector2(5, 5), Color.Red);
            _spriteBatch.DrawString(font, "mousePos: " + Input.MousePos().ToString(), new Vector2(5, 25), Color.Red);
            _spriteBatch.DrawString(font, "lines: " + lines.ToString(), new Vector2(5, 45), Color.Red);
            _spriteBatch.DrawString(font, "rest: " + (camera.ScreenToWorldSpace(new Vector2()).Y % width).ToString(), new Vector2(5, 65), Color.Red);
            _spriteBatch.DrawString(font, "nearest: " + (AdvancedMath.GetNearestMultiple((int)Math.Round(camera.ScreenToWorldSpace(new Vector2()).Y), size)).ToString(), new Vector2(5, 85), Color.Red);
            _spriteBatch.DrawString(font, "zoom: " + (camera.Zoom).ToString(), new Vector2(5, 105), Color.Red);
            for (int i = 0; i < inputButtons.Count; i++)
            {
                inputButtons[i].Item1.Draw(_spriteBatch, font);
                //for (int a = 0; a < inputButtons[i].Item2.Count; a++)
                //{
                //    if (a + 1 < inputButtons[i].Item2.Count)
                //    {
                //        DrawLine(_spriteBatch, inputButtons[i].Item2[a], inputButtons[i].Item2[a], (int)(width * 1.5), i < färger.Count ? färger[i] : Color.Blue);
                //    }
                //}
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawGridOld(int gridSize, int lineWidth)
        {
            for (int i = AdvancedMath.GetNearestMultiple((int)Math.Round(camera.ScreenToWorldSpace(new Vector2()).Y), gridSize); i < AdvancedMath.GetNearestMultiple((int)Math.Round(camera.ScreenToWorldSpace(new Vector2(0, Window.ClientBounds.Height)).Y), gridSize); i += gridSize)
            {
                DrawLine(_spriteBatch, new Vector2((int)Math.Round(camera.ScreenToWorldSpace(new Vector2()).X), (i)), new Vector2((int)Math.Round(camera.ScreenToWorldSpace(new Vector2(Window.ClientBounds.Width, 0)).X), (i)), lineWidth);
                lines++;
            }
            int vertical = 0;
            for (int i = AdvancedMath.GetNearestMultiple((int)Math.Round(camera.ScreenToWorldSpace(new Vector2()).X), gridSize); i < AdvancedMath.GetNearestMultiple((int)Math.Round(camera.ScreenToWorldSpace(new Vector2(Window.ClientBounds.Width, 0)).X), gridSize); i += gridSize)
            {
                DrawLine(_spriteBatch, new Vector2((i), (int)Math.Round(camera.ScreenToWorldSpace(new Vector2()).Y)), new Vector2((i), (int)Math.Round(camera.ScreenToWorldSpace(new Vector2(0, Window.ClientBounds.Height)).Y)), lineWidth);
                lines++;
                vertical++;
            }
        }

        private void DrawGrid(int gridSize, int lineWidth)
        {
            for (int i = AdvancedMath.GetNearestMultiple((int)Math.Round(camera.ScreenToWorldSpace(new Vector2()).Y), gridSize); i < AdvancedMath.GetNearestMultiple((int)Math.Round(camera.ScreenToWorldSpace(new Vector2(0, Window.ClientBounds.Height)).Y), gridSize) + gridSize; i += gridSize)
            {
                int myLineWidth = i / gridSize % 5 == 0 ? i / gridSize % 25 == 0 ? lineWidth * 2 : lineWidth : lineWidth / 2;
                if ((float)myLineWidth * (float)camera.Zoom > 0.9f)
                {
                    DrawLine(_spriteBatch, new Vector2((int)Math.Round(camera.ScreenToWorldSpace(new Vector2(-10, 0)).X), (i)), new Vector2((int)Math.Round(camera.ScreenToWorldSpace(new Vector2(Window.ClientBounds.Width + 10, 0)).X), (i)), myLineWidth);
                    lines++;
                }
                else
                {
                }
            }
            int vertical = 0;
            for (int i = AdvancedMath.GetNearestMultiple((int)Math.Round(camera.ScreenToWorldSpace(new Vector2()).X), gridSize); i < AdvancedMath.GetNearestMultiple((int)Math.Round(camera.ScreenToWorldSpace(new Vector2(Window.ClientBounds.Width, 0)).X), gridSize) + gridSize; i += gridSize)
            {
                int myLineWidth = i / gridSize % 5 == 0 ? i / gridSize % 25 == 0 ? lineWidth * 2 : lineWidth : lineWidth / 2;
                if ((float)myLineWidth * (float)camera.Zoom > 0.9f)
                {
                    DrawLine(_spriteBatch, new Vector2((i), (int)Math.Round(camera.ScreenToWorldSpace(new Vector2(0, -10)).Y)), new Vector2((i), (int)Math.Round(camera.ScreenToWorldSpace(new Vector2(0, Window.ClientBounds.Height + 10)).Y)), myLineWidth);
                    lines++;
                    vertical++;
                }
                else
                {
                }
            }
        }

        private void Equation(List<Vector2> curEquation, int start, int iterations)
        {
            curEquation.Clear();
            int width = 50;
            iterations += start;
            for (int x = start; x < iterations; x++)
            {
                curEquation.Add(new Vector2(x * width, -(float)(Math.Pow(x, 2) / 45 * width)));
            }
        }

        private void OtherEquation(List<Vector2> curEquation, int start, int iterations)
        {
            curEquation.Clear();
            int width = 50;
            iterations += start;
            for (int x = start; x < iterations; x++)
            {
                float temp = (float)Math.Pow(x, 1.8);
                if (float.IsNaN(temp))
                {
                    curEquation.Add(new Vector2(x * width, (float)(Math.Pow(-x, 1.8) / 10 * width)));
                }
                else
                {
                    float temp2 = (float)(Math.Pow(x, 1.8) / 10);
                    curEquation.Add(new Vector2(x * width, -(float)(Math.Pow(x, 1.8) / 10 * width)));
                }
            }
        }

        private void SetEquation(List<Vector2> curEquation, int start, int iterations, string expression)
        {
            curEquation.Clear();
            int width = 50;
            iterations += start;
            if (!AdvancedMath.ContainsNewCons(expression, 'y'))
            {
                for (float x = start; x <= iterations; x += 0.25f)
                {
                    string newExpress = AdvancedMath.ReplaceXWithConstant(expression, x, "x");
                    curEquation.Add(new Vector2(x * width, -((float)AdvancedMath.Eval(newExpress) * width)));
                }
            }
            else
            {
                for (float y = start; y <= iterations; y += 0.25f)
                {
                    string newExpress = AdvancedMath.ReplaceXWithConstant(expression, y, "y");
                    curEquation.Add(new Vector2(((float)AdvancedMath.Eval(newExpress) * width), -y * width));
                }
                AdvancedMath.ResetX();
            }
        }

        private void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);

            sb.Draw(square, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 10), null, Color.Black, angle, new Vector2(0, 0), SpriteEffects.None, 0);
            /*
             sb.Draw(t,
                  new Rectangle(// rectangle defines shape of line and position of start of line
                      (int)start.X,
                      (int)start.Y,
                      (int)edge.Length(), //sb will strech the texture to fill this rectangle
                      1), //width of line, change this to make thicker line
                  null,
                  Color.Red, //colour of line
                  angle,     //angle of line (calulated above)
                  new Vector2(0, 0), // point in line about which to rotate
                  SpriteEffects.None,
                  0);
                  */
        }

        private void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, int width)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);

            sb.Draw(square, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), width), null, Color.Black, angle, new Vector2(0, 0), SpriteEffects.None, 0);
            /*
             sb.Draw(t,
                  new Rectangle(// rectangle defines shape of line and position of start of line
                      (int)start.X,
                      (int)start.Y,
                      (int)edge.Length(), //sb will strech the texture to fill this rectangle
                      1), //width of line, change this to make thicker line
                  null,
                  Color.Red, //colour of line
                  angle,     //angle of line (calulated above)
                  new Vector2(0, 0), // point in line about which to rotate
                  SpriteEffects.None,
                  0);
                  */
        }

        private void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, int width, Color color)
        {
            if (float.IsNaN(start.X) || float.IsNaN(start.Y) || float.IsNaN(end.X) || float.IsNaN(end.Y))
            {
                return;
            }
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);

            sb.Draw(square, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), width), null, color, angle, new Vector2(0, 0), SpriteEffects.None, 0);
            /*
             sb.Draw(t,
                  new Rectangle(// rectangle defines shape of line and position of start of line
                      (int)start.X,
                      (int)start.Y,
                      (int)edge.Length(), //sb will strech the texture to fill this rectangle
                      1), //width of line, change this to make thicker line
                  null,
                  Color.Red, //colour of line
                  angle,     //angle of line (calulated above)
                  new Vector2(0, 0), // point in line about which to rotate
                  SpriteEffects.None,
                  0);
                  */
        }

        private MouseState mouse;
        private Vector2 mouseDelta;
        private Vector2 lastMousePosition;
        private bool enableMouseDragging;

        private void UpdateMouse(GameTime gameTime)
        {
            if ((Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(0)) && !enableMouseDragging)
                enableMouseDragging = true;
            else if ((Input.GetMouseButtonUp(2) && !Input.GetMouseButton(0) || Input.GetMouseButtonUp(0) && !Input.GetMouseButton(2)) && enableMouseDragging)
                enableMouseDragging = false;

            if (enableMouseDragging)
            {
                Vector2 delta = lastMousePosition - Input.MousePos();

                if (delta != Vector2.Zero)
                {
                    position += delta / camera.Zoom;
                    mouseDelta = delta;
                }
            }
            else
            {
                //if (mouseDelta != Vector2.Zero)
                //{
                //    Debug.WriteLine(mouseDelta);
                //    position += mouseDelta * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //    mouseDelta = Vector2.LerpPrecise(mouseDelta, new Vector2(0f, 0f), 0.08f * (float)gameTime.ElapsedGameTime.TotalSeconds);
                //}
            }

            lastMousePosition = Input.MousePos();
        }

        private string input = string.Empty;

        private void TextInputHandler(object sender, TextInputEventArgs args)
        {
            var pressedKey = args.Key;
            var character = args.Character;
            input += character;
            if (args.Key == Keys.Escape)
            {
                Exit();
            }
            //SetEquation(allEquations[allEquations.Count - 1], -40, 80, input);
            // do something with the character (and optionally the key)
            // ...
        }
    }
}