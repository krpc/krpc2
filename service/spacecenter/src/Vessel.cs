using System;
using KSP.Sim.impl;
using KRPC.Service.Attributes;
using KRPC.Utils;

namespace KRPC2.SpaceCenter
{
    /// <summary>
    /// These objects are used to interact with vessels in KSP. This includes getting
    /// orbital and flight data, manipulating control inputs and managing resources.
    /// Created using <see cref="SpaceCenter.ActiveVessel"/> or <see cref="SpaceCenter.Vessels"/>.
    /// </summary>
    [KRPCClass (Service = "SpaceCenter2")]
    public class Vessel : Equatable<Vessel>
    {
        /// <summary>
        /// Construct from a VesselComponent.
        /// </summary>
        public Vessel (VesselComponent vessel)
        {
            if (ReferenceEquals (vessel, null))
                throw new ArgumentNullException (nameof (vessel));
            Id = vessel.GlobalId;
        }

        /// <summary>
        /// Returns true if the objects are equal.
        /// </summary>
        public override bool Equals (Vessel other)
        {
            return !ReferenceEquals (other, null) && Id == other.Id;
        }

        /// <summary>
        /// Hash code for the object.
        /// </summary>
        public override int GetHashCode ()
        {
            return Id.GetHashCode ();
        }

        /// <summary>
        /// The KSP vessel id.
        /// </summary>
        public IGGuid Id { get; private set; }
    }
}