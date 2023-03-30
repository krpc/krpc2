using System;
using KSP.Sim.impl;
using KRPC.Service.Attributes;
using KRPC.Utils;

namespace KRPC2.SpaceCenter
{
    /// <summary>
    /// Describes an orbit. For example, the orbit of a vessel, obtained by calling
    /// <see cref="Vessel.Orbit"/>.
    /// </summary>
    [KRPCClass(Service = "SpaceCenter2")]
    public class Orbit : Equatable<Orbit>
    {
        internal Orbit(VesselComponent vessel)
        {
            InternalOrbit = vessel.Orbit;
        }

        /// <summary>
        /// Construct a an orbit from a KSP orbit object.
        /// </summary>
        public Orbit(PatchedConicsOrbit orbit)
        {
            InternalOrbit = orbit;
        }

        /// <summary>
        /// Returns true if the objects are equal.
        /// </summary>
        public override bool Equals(Orbit other)
        {
            return !ReferenceEquals(other, null) && InternalOrbit == other.InternalOrbit;
        }

        /// <summary>
        /// Hash code for the object.
        /// </summary>
        public override int GetHashCode()
        {
            return InternalOrbit.GetHashCode();
        }

        /// <summary>
        /// The KSP orbit object.
        /// </summary>
        public PatchedConicsOrbit InternalOrbit { get; private set; }

        /// <summary>
        /// Gets the apoapsis of the orbit, in meters, from the center of mass
        /// of the body being orbited.
        /// </summary>
        /// <remarks>
        /// For the apoapsis altitude reported on the in-game map view,
        /// use <see cref="ApoapsisAltitude"/>.
        /// </remarks>
        [KRPCProperty]
        public double Apoapsis
        {
            get { return InternalOrbit.Apoapsis; }
        }

        /// <summary>
        /// The periapsis of the orbit, in meters, from the center of mass
        /// of the body being orbited.
        /// </summary>
        /// <remarks>
        /// For the periapsis altitude reported on the in-game map view,
        /// use <see cref="PeriapsisAltitude"/>.
        /// </remarks>
        [KRPCProperty]
        public double Periapsis
        {
            get { return InternalOrbit.Periapsis; }
        }

        /// <summary>
        /// The apoapsis of the orbit, in meters, above the sea level of the body being orbited.
        /// </summary>
        /// <remarks>
        /// This is equal to <see cref="Apoapsis"/> minus the equatorial radius of the body.
        /// </remarks>
        [KRPCProperty]
        public double ApoapsisAltitude
        {
            get { return InternalOrbit.ApoapsisArl; }
        }

        /// <summary>
        /// The periapsis of the orbit, in meters, above the sea level of the body being orbited.
        /// </summary>
        /// <remarks>
        /// This is equal to <see cref="Periapsis"/> minus the equatorial radius of the body.
        /// </remarks>
        [KRPCProperty]
        public double PeriapsisAltitude
        {
            get { return InternalOrbit.PeriapsisArl; }
        }

        /// <summary>
        /// The semi-major axis of the orbit, in meters.
        /// </summary>
        [KRPCProperty]
        public double SemiMajorAxis
        {
            get { return 0.5d * (Apoapsis + Periapsis); }
        }

        /// <summary>
        /// The semi-minor axis of the orbit, in meters.
        /// </summary>
        [KRPCProperty]
        public double SemiMinorAxis
        {
            get
            {
                var e = Eccentricity;
                return SemiMajorAxis * Math.Sqrt(1d - (e * e));
            }
        }

        /// <summary>
        /// Eccentricity of the orbit (from 0 to 1).
        /// </summary>
        [KRPCProperty]
        public double Eccentricity
        {
            get { return InternalOrbit.eccentricity; }
        }

        /// <summary>
        /// The current radius of the orbit, in meters. This is the distance between the center
        /// of mass of the object in orbit, and the center of mass of the body around which it
        /// is orbiting.
        /// </summary>
        /// <remarks>
        /// This value will change over time if the orbit is elliptical.
        /// </remarks>
        [KRPCProperty]
        public double Radius
        {
            get { return InternalOrbit.radius; }
        }
    }
}
