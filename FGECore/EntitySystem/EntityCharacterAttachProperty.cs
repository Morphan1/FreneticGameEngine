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
using BEPUutilities;
using BEPUphysics.Character;
using FGECore.UtilitySystems;
using FGECore.MathHelpers;
using FGECore.PhysicsSystem;
using FGECore.CoreSystems;

namespace FGECore.EntitySystem
{   
     /// <summary>
     /// Attaches an entity to a character.
     /// </summary>
    public class EntityCharacterAttachProperty<T, T2, T3> : EntitySimpleAttachProperty<T, T2> where T : BasicEntity<T, T2> where T2 : BasicEngine<T, T2> where T3 : EntityPhysicsProperty<T, T2>
    {
        /// <summary>
        /// The entity this entity is attached to.
        /// </summary>
        public override T AttachedTo
        {
            get
            {
                return base.AttachedTo;
            }
            set
            {
                base.AttachedTo = value;
                Character = AttachedTo.GetProperty<T3>().OriginalObject as CharacterController;
            }
        }

        /// <summary>
        /// The character controller of <see cref="AttachedTo"/>.
        /// </summary>
        public CharacterController Character;

        /// <summary>
        /// The view height multiplier of the character controller.
        /// <para>That is, standing height * 0.5 * view height = used height.</para>
        /// </summary>
        public double ViewHeight = 0.95;

        /// <summary>
        /// Handles the spawn event.
        /// </summary>
        public override void OnSpawn()
        {
            base.OnSpawn();
            Entity.OnTick += Tick;
        }

        /// <summary>
        /// Gets the relative quaternion for this attachment.
        /// </summary>
        /// <returns>The relative quaternion.</returns>
        public MathHelpers.Quaternion GetRelativeQuaternion()
        {
            // TODO: Less complicated option?!
            Matrix relative = Matrix.CreateLookAtRH(Vector3.Zero, Character.ViewDirection, -Character.Down);
            return BEPUutilities.Quaternion.CreateFromRotationMatrix(relative).ToCore().Inverse();
        }

        /// <summary>
        /// Gets the accurate location for this attachment.
        /// </summary>
        /// <param name="basePos">The base entity position of the character.</param>
        /// <returns>The accurate position.</returns>
        public Location GetAccuratePosition(Location basePos)
        {
            return basePos + new Location(Character.Down) * (Character.StanceManager.StandingHeight * ViewHeight * (-0.5));
        }

        /// <summary>
        /// Gets the relative quaternion for this attachment.
        /// </summary>
        /// <param name="viewDir">The stand-in view direction.</param>
        /// <param name="downDir">The stand-in down direction.</param>
        /// <returns>The relative quaternion.</returns>
        public MathHelpers.Quaternion GetRelativeQuaternion(Vector3 viewDir, Vector3 downDir)
        {
            // TODO: Less complicated option?!
            Matrix relative = Matrix.CreateLookAtRH(Vector3.Zero, viewDir, -downDir);
            return BEPUutilities.Quaternion.CreateFromRotationMatrix(relative).ToCore().Inverse();
        }

        /// <summary>
        /// Gets the accurate location for this attachment.
        /// </summary>
        /// <param name="basePos">The base entity position of the character.</param>
        /// <param name="downDir">The stand-in down direction.</param>
        /// <returns>The accurate position.</returns>
        public Location GetAccuratePosition(Location basePos, Location downDir)
        {
            return basePos + downDir * (Character.StanceManager.StandingHeight * ViewHeight * (-0.5));
        }

        /// <summary>
        /// Set relative offset, based on an entity's offsets from the default positioning.
        /// </summary>
        /// <param name="relPos">The relative position.</param>
        /// <param name="relQuat">The relative quaternion.</param>
        public void SetRelativeForEntity(Location relPos, MathHelpers.Quaternion relQuat)
        {
            Entity.SetPosition(GetAccuratePosition(AttachedTo.LastKnownPosition, -Location.UnitZ) + relPos);
            Entity.SetOrientation(relQuat);
            SetRelativeToCurrent(Vector3.UnitY, -Location.UnitZ);
            Tick();
        }

        /// <summary>
        /// Set the relative offset to the current relative locations and orientation.
        /// </summary>
        /// <param name="viewDir">The stand-in view direction.</param>
        /// <param name="downDir">The stand-in down direction.</param>
        public void SetRelativeToCurrent(Vector3 viewDir, Location downDir)
        {
            SetRelativeBasedOn(GetRelativeQuaternion(viewDir, downDir.ToBVector()), GetAccuratePosition(AttachedTo.LastKnownPosition, downDir));
        }

        /// <summary>
        /// Set the relative offset to the current relative locations and orientation.
        /// </summary>
        public override void SetRelativeToCurrent()
        {
            SetRelativeBasedOn(GetRelativeQuaternion(), GetAccuratePosition(AttachedTo.LastKnownPosition));
        }

        /// <summary>
        /// Fixes this entity's position based on its attachment.
        /// </summary>
        public override void FixPosition(Location position)
        {
            SetPositionOrientation(GetAccuratePosition(position), GetRelativeQuaternion());
        }

        /// <summary>
        /// Fixes this entity's orientation based on its attachment.
        /// Does nothing for this property.
        /// </summary>
        public override void FixOrientation(MathHelpers.Quaternion orientation)
        {
        }

        /// <summary>
        /// Handles the tick event.
        /// </summary>
        public void Tick()
        {
            FixPosition(AttachedTo.LastKnownPosition);
        }

        /// <summary>
        /// Handles the despawn event.
        /// </summary>
        public override void OnDespawn()
        {
            base.OnDespawn();
            Entity.OnTick -= Tick;
        }
    }
}
