﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using FreneticGameGraphics.ClientSystem;

namespace FreneticGameGraphics.LightingSystem
{
    /// <summary>
    /// Represents an orthographic light.
    /// </summary>
    class LightOrtho : Light
    {
        /// <summary>
        /// Gets the matrix of the light.
        /// </summary>
        /// <param name="view">The relevant view system.</param>
        /// <returns>The relevant matrix.</returns>
        public override Matrix4 GetMatrix(View3D view)
        {
            Vector3d c = view.RenderRelative.ToOpenTK3D();
            Vector3d e = eye - c;
            Vector3d d = target - c;
            return Matrix4.LookAt(new Vector3((float)e.X, (float)e.Y, (float)e.Z), new Vector3((float)d.X, (float)d.Y, (float)d.Z), up) * Matrix4.CreateOrthographicOffCenter(-FOV * 0.5f, FOV * 0.5f, -FOV * 0.5f, FOV * 0.5f, 1, maxrange);
        }
    }
}