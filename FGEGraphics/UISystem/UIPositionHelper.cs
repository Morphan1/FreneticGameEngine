//
// This file is part of the Frenetic Game Engine, created by Frenetic LLC.
// This code is Copyright (C) Frenetic LLC under the terms of a strict license.
// See README.md or LICENSE.txt in the FreneticGameEngine source root for the contents of the license.
// If neither of these are available, assume that neither you nor anyone other than the copyright holder
// hold any right or permission to use this software until such time as the official license is identified.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FGEGraphics.ClientSystem;
using FGECore.MathHelpers;

namespace FGEGraphics.UISystem
{
    /// <summary>
    /// Helper for positioning.
    /// </summary>
    public class UIPositionHelper
    {
        /// <summary>
        /// Constructs the UI Position Helper.
        /// </summary>
        /// <param name="uiview">The backing view.</param>
        public UIPositionHelper(ViewUI2D uiview)
        {
            View = uiview;
        }

        /// <summary>
        /// Helper to return a zero.
        /// </summary>
        /// <returns>The zero.</returns>
        public static int GetZero()
        {
            return 0;
        }

        /// <summary>
        /// Helper to return a zero.
        /// </summary>
        /// <returns>The zero.</returns>
        public static float GetZeroF()
        {
            return 0f;
        }

        /// <summary>
        /// The view backing this element's positioning logic.
        /// </summary>
        public ViewUI2D View;

        /// <summary>
        /// The element this is the position for.
        /// </summary>
        public UIElement For;

        /// <summary>
        /// Whether this position helper has been modified since the last <see cref="UIElement.UpdatePositions(IList{UIElement}, double)"/>.
        /// </summary>
        public bool Dirty;

        // TODO: move below fields to an InternalData struct?

        /// <summary>
        /// The main positional anchor.
        /// </summary>
        public UIAnchor MainAnchor = UIAnchor.CENTER;

        /// <summary>
        /// Position mode for X.
        /// </summary>
        public UIPosMode PM_X = UIPosMode.CONSTANT;

        /// <summary>
        /// Constant value for X, if valid.
        /// </summary>
        public int Const_X = 0;

        /// <summary>
        /// Getter value for X, if valid.
        /// </summary>
        public Func<int> Getter_X = GetZero;

        /// <summary>
        /// Position mode for Y.
        /// </summary>
        public UIPosMode PM_Y = UIPosMode.CONSTANT;

        /// <summary>
        /// Constant value for Y, if valid.
        /// </summary>
        public int Const_Y = 0;

        /// <summary>
        /// Getter value for Y, if valid.
        /// </summary>
        public Func<int> Getter_Y = GetZero;

        /// <summarHeight>
        /// Position mode for Width.
        /// </summarHeight>
        public UIPosMode PM_Width = UIPosMode.CONSTANT;

        /// <summarHeight>
        /// Constant value for Width, if valid.
        /// </summarHeight>
        public int Const_Width = 0;

        /// <summary>
        /// Getter value for Width, if valid.
        /// </summary>
        public Func<int> Getter_Width = GetZero;

        /// <summarHeight>
        /// Position mode for Height.
        /// </summarHeight>
        public UIPosMode PM_Height = UIPosMode.CONSTANT;

        /// <summarHeight>
        /// Constant value for Height, if valid.
        /// </summarHeight>
        public int Const_Height = 0;

        /// <summary>
        /// Getter value for Height, if valid.
        /// </summary>
        public Func<int> Getter_Height = GetZero;

        /// <summarHeight>
        /// Position mode for Rotation.
        /// </summarHeight>
        public UIPosMode PM_Rot = UIPosMode.CONSTANT;

        /// <summarHeight>
        /// Constant value for Rotation, if valid.
        /// </summarHeight>
        public float Const_Rot = 0;

        /// <summary>
        /// Getter value for Rotation, if valid.
        /// </summary>
        public Func<float> Getter_Rot = GetZeroF;

        /// <summary>
        /// Sets an anchor.
        /// </summary>
        /// <param name="anchor">The anchor.</param>
        /// <returns>This object.</returns>
        public UIPositionHelper Anchor(UIAnchor anchor)
        {
            MainAnchor = anchor;
            Dirty = true;
            return this;
        }

        /// <summary>
        /// Sets a constant X value.
        /// </summary>
        /// <param name="x">The X value.</param>
        /// <returns>This object.</returns>
        public UIPositionHelper ConstantX(int x)
        {
            PM_X = UIPosMode.CONSTANT;
            Const_X = x;
            Dirty = true;
            return this;
        }

        /// <summary>
        /// Sets a constant Y value.
        /// </summary>
        /// <param name="y">The Y value.</param>
        /// <returns>This object.</returns>
        public UIPositionHelper ConstantY(int y)
        {
            PM_Y = UIPosMode.CONSTANT;
            Const_Y = y;
            Dirty = true;
            return this;
        }

        /// <summary>
        /// Sets a constant X and Y value.
        /// </summary>
        /// <param name="x">The X value.</param>
        /// <param name="y">The Y value.</param>
        /// <returns>This object.</returns>
        public UIPositionHelper ConstantXY(int x, int y)
        {
            PM_X = UIPosMode.CONSTANT;
            Const_X = x;
            PM_Y = UIPosMode.CONSTANT;
            Const_Y = y;
            Dirty = true;
            return this;
        }

        /// <summary>
        /// Sets a constant Width value.
        /// </summary>
        /// <param name="width">The Width value.</param>
        /// <returns>This object.</returns>
        public UIPositionHelper ConstantWidth(int width)
        {
            PM_Width = UIPosMode.CONSTANT;
            Const_Width = width;
            Dirty = true;
            return this;
        }

        /// <summary>
        /// Sets a constant Height value.
        /// </summary>
        /// <param name="height">The Height value.</param>
        /// <returns>This object.</returns>
        public UIPositionHelper ConstantHeight(int height)
        {
            PM_Height = UIPosMode.CONSTANT;
            Const_Height = height;
            Dirty = true;
            return this;
        }

        /// <summary>
        /// Sets a constant Width and Height value.
        /// </summary>
        /// <param name="width">The Width value.</param>
        /// <param name="height">The Height value.</param>
        /// <returns>This object.</returns>
        public UIPositionHelper ConstantWidthHeight(int width, int height)
        {
            PM_Width = UIPosMode.CONSTANT;
            Const_Width = width;
            PM_Height = UIPosMode.CONSTANT;
            Const_Height = height;
            Dirty = true;
            return this;
        }

        /// <summary>
        /// Sets a constant Rotation value.
        /// </summary>
        /// <param name="rotation">The Rotation value.</param>
        /// <returns>This object.</returns>
        public UIPositionHelper ConstantRotation(float rotation)
        {
            PM_Rot = UIPosMode.CONSTANT;
            Const_Rot = rotation;
            Dirty = true;
            return this;
        }

        /// <summary>
        /// Sets a getter X value.
        /// </summary>
        /// <param name="x">The X getter.</param>
        /// <returns>This object.</returns>
        public UIPositionHelper GetterX(Func<int> x)
        {
            PM_X = UIPosMode.GETTER;
            Getter_X = x;
            Dirty = true;
            return this;
        }

        /// <summary>
        /// Sets a getter Y value.
        /// </summary>
        /// <param name="y">The Y getter.</param>
        /// <returns>This object.</returns>
        public UIPositionHelper GetterY(Func<int> y)
        {
            PM_Y = UIPosMode.GETTER;
            Getter_Y = y;
            return this;
        }

        /// <summary>
        /// Sets a getter X and Y value.
        /// </summary>
        /// <param name="x">The X getter.</param>
        /// <param name="y">The Y getter.</param>
        /// <returns>This object.</returns>
        public UIPositionHelper GetterXY(Func<int> x, Func<int> y)
        {
            PM_X = UIPosMode.GETTER;
            Getter_X = x;
            PM_Y = UIPosMode.GETTER;
            Getter_Y = y;
            Dirty = true;
            return this;
        }

        /// <summary>
        /// Sets a getter Width value.
        /// </summary>
        /// <param name="width">The Width getter.</param>
        /// <returns>This object.</returns>
        public UIPositionHelper GetterWidth(Func<int> width)
        {
            PM_Width = UIPosMode.GETTER;
            Getter_Width = width;
            Dirty = true;
            return this;
        }

        /// <summary>
        /// Sets a getter Height value.
        /// </summary>
        /// <param name="height">The Height getter.</param>
        /// <returns>This object.</returns>
        public UIPositionHelper GetterHeight(Func<int> height)
        {
            PM_Height = UIPosMode.GETTER;
            Getter_Height = height;
            Dirty = true;
            return this;
        }

        /// <summary>
        /// Sets a constant Width and Height value.
        /// </summary>
        /// <param name="width">The Width getter.</param>
        /// <param name="height">The Height getter.</param>
        /// <returns>This object.</returns>
        public UIPositionHelper GetterWidthHeight(Func<int> width, Func<int> height)
        {
            PM_Width = UIPosMode.GETTER;
            Getter_Width = width;
            PM_Height = UIPosMode.GETTER;
            Getter_Height = height;
            Dirty = true;
            return this;
        }

        /// <summary>
        /// Sets a getter Rotation value.
        /// </summary>
        /// <param name="rotation">The Rotation getter.</param>
        /// <returns>This object.</returns>
        public UIPositionHelper GetterRotation(Func<float> rotation)
        {
            PM_Rot = UIPosMode.GETTER;
            Getter_Rot = rotation;
            Dirty = true;
            return this;
        }

        /// <summary>
        /// Matches the parent element for all values.
        /// <para>If there is no parent element, matches the client's window instead.</para>
        /// </summary>
        /// <returns>This object.</returns>
        // TODO: individual methods for this?
        public UIPositionHelper MatchParent()
        {
            PM_X = UIPosMode.MATCH_PARENT;
            PM_Y = UIPosMode.MATCH_PARENT;
            PM_Width = UIPosMode.MATCH_PARENT;
            PM_Height = UIPosMode.MATCH_PARENT;
            PM_Rot = UIPosMode.MATCH_PARENT;
            Dirty = true;
            return this;
        }

        /// <summary>
        /// Gets the absolute X coordinate.
        /// </summary>
        public int X
        {
            get
            {
                if (PM_X == UIPosMode.MATCH_PARENT)
                {
                    return For.Parent?.LastAbsolutePosition.X ?? 0;
                }
                int anch = For.Parent != null ? MainAnchor.GetX(For) : 0;
                if (PM_X == UIPosMode.CONSTANT)
                {
                    return anch + Const_X;
                }
                if (PM_X == UIPosMode.GETTER)
                {
                    return anch + Getter_X();
                }
                return anch;
            }
        }

        /// <summary>
        /// Gets the absolute Y coordinate.
        /// </summary>
        public int Y
        {
            get
            {
                if (PM_Y == UIPosMode.MATCH_PARENT)
                {
                    return For.Parent?.LastAbsolutePosition.Y ?? 0;
                }
                int anch = For.Parent != null ? MainAnchor.GetY(For) : 0;
                if (PM_Y == UIPosMode.CONSTANT)
                {
                    return anch + Const_Y;
                }
                if (PM_Y == UIPosMode.GETTER)
                {
                    return anch + Getter_Y();
                }
                return anch;
            }
        }

        /// <summary>
        /// Gets the X coordinate relative to the parent.
        /// </summary>
        public int RelativeX
        {
            get
            {
                if (PM_X == UIPosMode.CONSTANT)
                {
                    return Const_X;
                }
                if (PM_X == UIPosMode.GETTER)
                {
                    return Getter_X();
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the Y coordinate relative to the parent.
        /// </summary>
        public int RelativeY
        {
            get
            {
                if (PM_Y == UIPosMode.CONSTANT)
                {
                    return Const_Y;
                }
                if (PM_Y == UIPosMode.GETTER)
                {
                    return Getter_Y();
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width
        {
            get
            {
                if (PM_Width == UIPosMode.CONSTANT)
                {
                    return Const_Width;
                }
                if (PM_Width == UIPosMode.GETTER)
                {
                    return Getter_Width();
                }
                if (PM_Width == UIPosMode.MATCH_PARENT)
                {
                    return For.Parent?.LastAbsoluteSize.X ?? For.Engine.Window.Width;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height
        {
            get
            {
                if (PM_Height == UIPosMode.CONSTANT)
                {
                    return Const_Height;
                }
                if (PM_Height == UIPosMode.GETTER)
                {
                    return Getter_Height();
                }
                if (PM_Height == UIPosMode.MATCH_PARENT)
                {
                    return For.Parent?.LastAbsoluteSize.Y ?? For.Engine.Window.Height;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the local Rotation value.
        /// </summary>
        public float Rotation
        {
            get
            {
                if (PM_Rot == UIPosMode.CONSTANT)
                {
                    return Const_Rot;
                }
                if (PM_Rot == UIPosMode.GETTER)
                {
                    return Getter_Rot();
                }
                if (PM_Rot == UIPosMode.MATCH_PARENT)
                {
                    return For.Parent?.LastAbsoluteRotation ?? 0f;
                }
                return 0f;
            }
        }

        /// <summary>
        /// Gets the X/Y coordinate pair.
        /// </summary>
        public Vector2i Position
        {
            get
            {
                return new Vector2i(X, Y);
            }
        }

        /// <summary>
        /// Gets the relative X/Y coordinate pair.
        /// </summary>
        public Vector2i RelativePosition
        {
            get
            {
                return new Vector2i(RelativeX, RelativeY);
            }
        }

        /// <summary>
        /// Gets the Width/Height coordinate pair.
        /// </summary>
        public Vector2i Size
        {
            get
            {
                return new Vector2i(Width, Height);
            }
        }

        /// <summary>
        /// Whether any of this helper's <see cref="UIPosMode"/>-type fields are currently set to <see cref="UIPosMode.GETTER"/>.
        /// <para><seealso cref="PM_X"/>, <seealso cref="PM_Y"/>, <seealso cref="PM_Rot"/>, <seealso cref="PM_Width"/>, <seealso cref="PM_Height"/></para>
        /// </summary>
        public bool HasAnyGetter()
        {
            return PM_X == UIPosMode.GETTER || PM_Y == UIPosMode.GETTER || PM_Rot == UIPosMode.GETTER || PM_Width == UIPosMode.GETTER || PM_Height == UIPosMode.GETTER;
        }

        /// <summary>
        /// Whether any of this helper's <see cref="UIPosMode"/>-type fields are currently set to <see cref="UIPosMode.MATCH_PARENT"/>.
        /// <para><seealso cref="PM_X"/>, <seealso cref="PM_Y"/>, <seealso cref="PM_Rot"/>, <seealso cref="PM_Width"/>, <seealso cref="PM_Height"/></para>
        /// </summary>
        public bool HasAnyMatchParent()
        {
            return PM_X == UIPosMode.MATCH_PARENT || PM_Y == UIPosMode.MATCH_PARENT || PM_Rot == UIPosMode.MATCH_PARENT || PM_Width == UIPosMode.MATCH_PARENT || PM_Height == UIPosMode.MATCH_PARENT;
        }

        /// <summary>
        /// Converts this position helper's present data to a simplified debug string.
        /// </summary>
        /// <returns>The debug string.</returns>
        public override string ToString()
        {
            return "UIPositionHelper:PresentState{XY: " + X + ", " + Y + " / WH: " + Width + ", " + Height + " / Rot: " + Rotation + "}";
        }
    }

    /// <summary>
    /// Modes for the <see cref="UIPositionHelper"/>.
    /// </summary>
    public enum UIPosMode : byte
    {
        /// <summary>
        /// A constant position.
        /// </summary>
        CONSTANT = 0,
        /// <summary>
        /// A getter function.
        /// </summary>
        GETTER = 1,
        /// <summary>
        /// Match the element's parent.
        /// </summary>
        MATCH_PARENT = 2
    }
}
