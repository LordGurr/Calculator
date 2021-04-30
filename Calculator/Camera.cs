﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator
{
    internal class Camera
    {
        public Camera(Viewport _viewport, GameWindow _Window)
        {
            viewport = _viewport;
            Window = _Window;
        }

        public Matrix transform { get; private set; }
        private float yVelocity = 0.0f;
        private float xVelocity = 0.0f;
        private float zoom = 1;
        private Vector2 centre;
        private Viewport viewport;
        private GameWindow Window;

        public float Zoom
        {
            get
            {
                return zoom;
            }
            set
            {
                zoom = value;
                if (zoom > 5 || zoom < 0.3f)
                {
                }
                zoom = MathHelper.Clamp(zoom, 0.3f, 5);
                //zoom = zoom < 0.3f ? 0.3f : zoom;
            }
        }

        //public void Follow(Vector2 pos, Rectangle hitbox, float deltaTime)
        //{
        //    var position = Matrix.CreateTranslation(new Vector3(-pos.X - (hitbox.Width / 2), -pos.Y - (hitbox.Height / 2), 0));

        //    var offset = Matrix.CreateTranslation(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2, 0);
        //    var scale = Matrix.CreateScale(new Vector3(2f, 2f, 0));
        //    //transform = position * offset;
        //    Matrix newPos = position * offset * scale;
        //    float smoothTime = 0.2f;
        //    float amountToMoveY = AdvancedMath.SmoothDamp(transform.Translation.Y, newPos.Translation.Y, ref yVelocity, smoothTime, float.MaxValue, deltaTime);
        //    float amountToMoveX = AdvancedMath.SmoothDamp(transform.Translation.X, newPos.Translation.X, ref xVelocity, smoothTime, float.MaxValue, deltaTime);
        //    transform = Matrix.CreateTranslation(new Vector3(amountToMoveX, amountToMoveY, 0));
        //}

        public void SetCamera(Vector2 pos, Rectangle hitbox)
        {
            var position = Matrix.CreateTranslation(-pos.X - (hitbox.Width / 2), -pos.Y - (hitbox.Height / 2), 0);

            var offset = Matrix.CreateTranslation(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2, 0);

            transform = position * offset;
            //Matrix newPos = position * offset;
            //float smoothTime = 0.2f;
            //float amountToMoveY = Game1.SmoothDamp(transform.Translation.Y, newPos.Translation.Y, ref yVelocity, smoothTime, float.MaxValue, deltaTime);
            //float amountToMoveX = Game1.SmoothDamp(transform.Translation.X, newPos.Translation.X, ref xVelocity, smoothTime, float.MaxValue, deltaTime);
            //transform = Matrix.CreateTranslation(amountToMoveX, amountToMoveY, 0);
        }

        public void UpdateCamera(Vector2 pos)
        {
            centre = pos;
            transform = Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0)) * Matrix.CreateScale(Zoom, Zoom, 1) * Matrix.CreateTranslation(viewport.Width / 2, viewport.Height / 2, 0);
        }

        public Vector2 ScreenToWorldSpace(in Vector2 point)
        {
            Matrix invertedMatrix = Matrix.Invert(transform);
            return Vector2.Transform(point, invertedMatrix);
            //        return
            //Matrix.CreateTranslation(new Vector3(centre.X, centre.Y, 0f)) *

            //Matrix.CreateScale(Zoom, Zoom, 0) *
            //Matrix.CreateTranslation(viewport.Width / 2, viewport.Height / 2, 0);
        }

        //public Vector2Dec ScreenToWorldSpace(in Vector2Dec point)
        //{
        //    try
        //    {
        //        Matrix invertedMatrix = Matrix.Invert(transform);
        //        return Vector2Dec.Transform(point, invertedMatrix);
        //    }
        //    catch (Exception e)
        //    {
        //        //Vector2 temp = ScreenToWorldSpace(new Vector2((float)point.X, (float)point.Y));
        //        //return new Vector2Dec((decimal)temp.X, (decimal)temp.Y);
        //        return point;
        //    }
        //    //        return
        //    //Matrix.CreateTranslation(new Vector3(centre.X, centre.Y, 0f)) *

        //    //Matrix.CreateScale(Zoom, Zoom, 0) *
        //    //Matrix.CreateTranslation(viewport.Width / 2, viewport.Height / 2, 0);
        //}
    }
}