using System;
using KSP.Sim.impl;
using KSP.Sim.State;

using KRPC.Service;
using KRPC.Service.Attributes;
using KRPC.Utils;

// NOTE: This is basically just a reimplementation of KSP.Sim.impl/VesselVehicle with KRPC
// bindings. Implemented for consistency with krpc1. Also note that SpaceWarp
// VesselVehicleExtensions also provides something similar.
namespace KRPC2.SpaceCenter
{
    /// <summary>
    /// Used to manipulate the controls of a vessel. This includes adjusting the
    /// throttle, enabling/disabling systems such as SAS and RCS, or altering the
    /// direction in which the vessel is pointing.
    /// Obtained by calling <see cref="Vessel.Control"/>.
    /// </summary>
    /// <remarks>
    /// Control inputs (such as pitch, yaw and roll) are instantaneous (the same
    /// as tapping a button)
    /// </remarks>
    [KRPCClass(Service = "SpaceCenter2", GameScene = GameScene.Flight)]
    public class Control : Equatable<Control>
    {
        readonly Guid vesselId;

        internal Control(VesselComponent vessel)
        {
            // TODO: Don't hold on to vessel, object. Instead lookup from scope
            // by ID. See the setter for this property.
            InternalVessel = vessel;
            vesselId = vessel.GlobalId;
        }

        /// <summary>
        /// Returns true if the vesselIds are equal.
        /// </summary>
        public override bool Equals(Control other)
        {
            return !ReferenceEquals(other, null) && vesselId == other.vesselId;
        }

        /// <summary>
        /// Hash code for the object.
        /// </summary>
        public override int GetHashCode()
        {
            return vesselId.GetHashCode();
        }

        /// <summary>
        /// The KSP vessel object.
        /// </summary>
        public VesselComponent InternalVessel { get; private set; }

        private void AtomicSet(FlightCtrlStateIncremental flightControlUpdate)
        {
            InternalVessel.ApplyFlightCtrlState(flightControlUpdate);
        }

        /// <summary>
        /// The state of the throttle. A value between 0 and 1.
        /// </summary>
        [KRPCProperty]
        public float Throttle
        {
            get
            {
                return InternalVessel.flightCtrlState.mainThrottle;
            }
            set
            {
                var incremental = new FlightCtrlStateIncremental()
                {
                    mainThrottle = value
                };
                AtomicSet(incremental);
            }
        }

        /// <summary>
        /// Instantaneous change to the roll control, and current roll state. A value between -1 and 1.
        /// </summary>
        /// <remarks>
        /// Reading this value provides the instantaneous roll control value.
        /// Setting this value is the same as tapping q/e in stock KSP2.
        /// The roll experienced by the vehicle will be RollTrim + Roll. KSP
        /// will internally clamp this value between -1 and 1.
        /// </remarks>
        [KRPCProperty]
        public float Roll
        {
            get
            {
                return InternalVessel.flightCtrlState.roll;
            }
            set
            {
                var incremental = new FlightCtrlStateIncremental()
                {
                    roll = value
                };
                AtomicSet(incremental);
            }
        }

        /// <summary>
        /// Instantaneous change to the yaw control, and current yaw state. A value between -1 and 1.
        /// </summary>
        /// <remarks>
        /// Reading this value provides the instantaneous yaw control value.
        /// Setting this value is the same as tapping a/d in stock KSP2.
        /// The yaw experienced by the vehicle will be YawTrim + Yaw. KSP
        /// will internally clamp this value between -1 and 1.
        /// </remarks>
        [KRPCProperty]
        public float Yaw
        {
            get
            {
                return InternalVessel.flightCtrlState.yaw;
            }
            set
            {
                var incremental = new FlightCtrlStateIncremental()
                {
                    yaw = value
                };
                AtomicSet(incremental);
            }
        }

        /// <summary>
        /// Instantaneous change to the pitch control, and current pitch state. A value between -1 and 1.
        /// </summary>
        /// <remarks>
        /// Reading this value provides the instantaneous pitch control value.
        /// Setting this value is the same as tapping w/s in stock KSP2.
        /// The pitch experienced by the vehicle will be PitchTrim + Pitch. KSP
        /// will internally clamp this value between -1 and 1.
        /// </remarks>
        [KRPCProperty]
        public float Pitch
        {
            get
            {
                return InternalVessel.flightCtrlState.pitch;
            }
            set
            {
                var incremental = new FlightCtrlStateIncremental()
                {
                    pitch = value
                };
                AtomicSet(incremental);
            }
        }

        /// <summary>
        /// The state of the roll trim control. A value between -1 and 1.
        /// </summary>
        /// <remarks>
        /// This will set the roll control to have this value as a baseline.
        /// For an instantaneous roll change, see <see cref="Roll"/>.
        /// </remarks>
        [KRPCProperty]
        public float RollTrim
        {
            get
            {
                return InternalVessel.flightCtrlState.rollTrim;
            }
            set
            {
                var incremental = new FlightCtrlStateIncremental()
                {
                    rollTrim = value
                };
                AtomicSet(incremental);
            }
        }

        /// <summary>
        /// The state of the yaw trim control. A value between -1 and 1.
        /// </summary>
        /// <remarks>
        /// This will set the yaw control to have this value as a baseline.
        /// For an instantaneous yaw change, see <see cref="Yaw"/>.
        /// </remarks>
        [KRPCProperty]
        public float YawTrim
        {
            get
            {
                return InternalVessel.flightCtrlState.yawTrim;
            }
            set
            {
                var incremental = new FlightCtrlStateIncremental()
                {
                    yawTrim = value
                };
                AtomicSet(incremental);
            }
        }

        /// <summary>
        /// The state of the pitch trim control. A value between -1 and 1.
        /// </summary>
        /// <remarks>
        /// This will set the pitch control to have this value as a baseline.
        /// For an instantaneous pitch change, see <see cref="Pitch"/>.
        /// </remarks>
        [KRPCProperty]
        public float PitchTrim
        {
            get
            {
                return InternalVessel.flightCtrlState.pitchTrim;
            }
            set
            {
                var incremental = new FlightCtrlStateIncremental()
                {
                    pitchTrim = value
                };
                AtomicSet(incremental);
            }
        }
    }
}
