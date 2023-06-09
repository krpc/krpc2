using System;
using KSP.Sim.impl;
using KRPC.Service;
using KRPC.Service.Attributes;
using KRPC.Utils;

namespace KRPC2.SpaceCenter
{
    /// <summary>
    /// These objects are used to interact with vessels in KSP. This includes getting
    /// orbital and flight data, manipulating control inputs and managing resources.
    /// Created using <see cref="SpaceCenter.ActiveVessel"/> <!--or <see cref="SpaceCenter.Vessels"/>-->.
    /// </summary>
    [KRPCClass(Service = "SpaceCenter2")]
    public class Vessel : Equatable<Vessel>
    {

        /// <summary>
        /// Construct from a VesselComponent.
        /// </summary>
        public Vessel(VesselComponent vessel)
        {
            if (ReferenceEquals(vessel, null))
                throw new ArgumentNullException(nameof(vessel));
            Id = vessel.GlobalId;
            InternalVessel = vessel;
        }

        /// <summary>
        /// Returns true if the objects are equal.
        /// </summary>
        public override bool Equals(Vessel other)
        {
            return !ReferenceEquals(other, null) && Id == other.Id;
        }

        /// <summary>
        /// Hash code for the object.
        /// </summary>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// The KSP vessel id.
        /// </summary>
        public IGGuid Id { get; private set; }

        /// <summary>
        /// The KSP vessel object.
        /// </summary>
        public VesselComponent InternalVessel { get; private set; }

        /// <summary>
        /// The current orbit of the vessel.
        /// </summary>
        [KRPCProperty]
        public Orbit Orbit
        {
            get { return new Orbit(InternalVessel); }
        }

        /// <summary>
        /// Returns a <see cref="Control"/> object that can be used to manipulate
        /// the vessel's control inputs. For example, its pitch/yaw/roll controls,
        /// RCS and thrust.
        /// </summary>
        [KRPCProperty(GameScene = GameScene.Flight)]
        public Control Control
        {
            get { return new Control(InternalVessel); }
        }
    }
}
