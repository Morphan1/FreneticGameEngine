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
using BEPUphysics.CollisionShapes;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUutilities;
using FGECore.CoreSystems;

namespace FGECore.EntitySystem.PhysicsHelpers
{
    /// <summary>
    /// A convex hull shape for an entity.
    /// </summary>
    public class EntityConvexHullShape : EntityShapeHelper
    {
        /// <summary>
        /// The internal convex hull shape.
        /// </summary>
        public ConvexHullShape Internal;

        // TODO: Savable vertex/index set?

        /// <summary>
        /// The center offset for this shape.
        /// </summary>
        [PropertyDebuggable]
        [PropertyAutoSavable]
        public Vector3 Center;

        /// <summary>
        /// Gets the center offset.
        /// </summary>
        /// <returns>The center offset.</returns>
        public override Vector3 GetCenterOffset()
        {
            return Center;
        }

        /// <summary>
        /// Gets the BEPU shape object.
        /// </summary>
        /// <returns>The BEPU shape.</returns>
        public override EntityShape GetBEPUShape()
        {
            return Internal;
        }

        /// <summary>
        /// The string form of this shape helper.
        /// </summary>
        /// <returns>String form.</returns>
        public override string ToString()
        {
            return "ConvexHullShape";
        }
    }
}
