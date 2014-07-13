using Chronos.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chronos.Core.Service
{
    public class Clock : IService
    {

        public event EventHandler FixedTick;

        public event EventHandler VariableTick;

        /// <summary>
        /// Time spent processing all frames.
        /// </summary>
        public double Runtime { get; set; }

        /// <summary>
        /// The maximum frame time.  The frame time
        /// is override if it is greater than this value.
        /// </summary>
        public double MaxFrametime { get; set; }

        /// <summary>
        /// The duration of the latest variable tick.
        /// </summary>
        public double VariableTickDuration { get; set; }

        /// <summary>
        /// Total number of variable ticks.
        /// </summary>
        public long VariableTicks { get; set; }

        /// <summary>
        /// The duration of a fixed tick.
        /// </summary>
        public double FixedTickDuration { get; set; }

        /// <summary>
        /// Total number of fixed ticks.
        /// </summary>
        public long FixedTicks { get; set; }

        private double accumulator = 0.0d;

        public void Initialise(IContext context)
        {
            VariableTickDuration = 0.0d;
            FixedTickDuration = 1.0d / 30.0d;
            MaxFrametime = 0.25d;
            FixedTicks = 0;
            Runtime = 0.0d;
            VariableTicks = 0;
        }

        public void Update(double elapsedTime)
        {

            // Variable Tick:
            Runtime += elapsedTime;
            VariableTickDuration = elapsedTime;

            VariableTicks += 1;

            var variableTickEvt = VariableTick;
            if (variableTickEvt != null)
            {
                VariableTick(this, null);
            }

            // Fixed Tick:
            if (elapsedTime > MaxFrametime)
            {
                elapsedTime = 0.25;
            }

            accumulator += elapsedTime;

            while (accumulator >= FixedTickDuration)
            {
                // Fixed Updates
                accumulator -= FixedTickDuration;
                FixedTicks += 1;

                var fixedTickEvt = FixedTick;
                if (fixedTickEvt != null)
                {
                    FixedTick(this, null);
                }

            }

        }

    }
}
