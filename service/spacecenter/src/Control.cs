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
    /// Control inputs (such as pitch, yaw and roll) are zeroed when all clients
    /// that have set one or more of these inputs are no longer connected.
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
        /// The state of the roll control. A value between -1 and 1.
        /// </summary>
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
        /// The state of the yaw control. A value between -1 and 1.
        /// </summary>
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
        /// The state of the pitch control. A value between -1 and 1.
        /// </summary>
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

        /// <summary>
        /// The state of the wheel steer control. A value between -1 and 1.
        /// </summary>
        [KRPCProperty]
        public float WheelSteer
        {
            get
            {
                return InternalVessel.flightCtrlState.wheelSteer;
            }
            set
            {
                var incremental = new FlightCtrlStateIncremental()
                {
                    wheelSteer = value
                };
                AtomicSet(incremental);
            }
        }

        /// <summary>
        /// The state of the wheel steer trim control. A value between -1 and 1.
        /// </summary>
        [KRPCProperty]
        public float WheelSteerTrim
        {
            get
            {
                return InternalVessel.flightCtrlState.wheelSteerTrim;
            }
            set
            {
                var incremental = new FlightCtrlStateIncremental()
                {
                    wheelSteerTrim = value
                };
                AtomicSet(incremental);
            }
        }

        /// <summary>
        /// The state of the wheel throttle control. A value between -1 and 1.
        /// </summary>
        [KRPCProperty]
        public float WheelThrottle
        {
            get
            {
                return InternalVessel.flightCtrlState.wheelThrottle;
            }
            set
            {
                var incremental = new FlightCtrlStateIncremental()
                {
                    wheelThrottle = value
                };
                AtomicSet(incremental);
            }
        }

        /// <summary>
        /// The state of the wheel throttle trim control. A value between -1 and 1.
        /// </summary>
        [KRPCProperty]
        public float WheelThrottleTrim
        {
            get
            {
                return InternalVessel.flightCtrlState.wheelThrottleTrim;
            }
            set
            {
                var incremental = new FlightCtrlStateIncremental()
                {
                    wheelThrottleTrim = value
                };
                AtomicSet(incremental);
            }
        }

        /// <summary>
        /// Whether the gear up control is active.
        /// </summary>
        [KRPCProperty]
        public bool Gear
        {
            get
            {
                return InternalVessel.flightCtrlState.gearUp;
            }
            set
            {
                var incremental = new FlightCtrlStateIncremental()
                {
                    gearUp = value,
                    gearDown = !value
                };
                AtomicSet(incremental);
            }
        }

        /// <summary>
        /// Whether the light control is active.
        /// </summary>
        [KRPCProperty]
        public bool Lights
        {
            get
            {
                return InternalVessel.flightCtrlState.headlight;
            }
            set
            {
                var incremental = new FlightCtrlStateIncremental()
                {
                    headlight = value
                };
                AtomicSet(incremental);
            }
        }

        /// <summary>
        /// Whether the brakes control is active.
        /// </summary>
        [KRPCProperty]
        public bool Brakes
        {
            get
            {
                return InternalVessel.flightCtrlState.brakes;
            }
            set
            {
                var incremental = new FlightCtrlStateIncremental()
                {
                    brakes = value
                };
                AtomicSet(incremental);
            }
        }
    }
}
