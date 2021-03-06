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
using FGECore.CoreSystems;

namespace FGECore.EntitySystem.PhysicsHelpers
{
    /// <summary>
    /// A sphere shape for an entity.
    /// </summary>
    public class EntitySphereShape : EntityShapeHelper
    {
        /// <summary>
        /// The radius of the sphere.
        /// </summary>
        [PropertyDebuggable]
        [PropertyAutoSavable]
        public double Size;

        /// <summary>
        /// Gets the BEPU shape object.
        /// </summary>
        /// <returns>The BEPU shape.</returns>
        public override EntityShape GetBEPUShape()
        {
            return new SphereShape(Size);
        }

        /// <summary>
        /// The string form of this shape helper.
        /// </summary>
        /// <returns>String form.</returns>
        public override string ToString()
        {
            return "SphereShape, size=" + Size;
        }
    }
}
