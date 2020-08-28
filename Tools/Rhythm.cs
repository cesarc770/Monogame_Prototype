using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Rapid_Prototype_1.Tools {
    class Rhythm {
        private double beatLength;

        private double timeSinceBeat = int.MaxValue;

        private int beatCount = 0;


        const double SECONDS_PER_MINUTE = 60;
        const double MILLISECONDS_PER_SECOND = 1000;
        const double FRAME_TIME = (1 / 60f) * MILLISECONDS_PER_SECOND;
        public Rhythm(float bpm) {
            beatLength = (SECONDS_PER_MINUTE * MILLISECONDS_PER_SECOND) / bpm;
        }

        public void Start() {
            timeSinceBeat = 0;
        }

        public void Update(GameTime gameTime) {
            timeSinceBeat += gameTime.ElapsedGameTime.TotalMilliseconds;

            double timeTillNextBeat = beatLength - timeSinceBeat;

            if (timeSinceBeat + FRAME_TIME / 2 > beatLength) {
                timeSinceBeat -= beatLength;

                Console.WriteLine("\nBEAT {0}, {1}", beatCount, timeSinceBeat);
                ++beatCount;
            }
            else {
                Console.Write('-');
            }

            //Console.WriteLine("{0} -> now -> {1}", timeSinceBeat, timeTillNextBeat);
        }
    }
}
