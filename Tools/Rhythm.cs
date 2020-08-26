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
        private  double startTime = -1;
        public bool started { get => startTime != -1; }
        private int lastBeat = -1;


        const double SECONDS_PER_MINUTE = 60;
        const double MILLISECONDS_PER_SECOND = 1000;
        public Rhythm(float bpm) {
            beatLength = (SECONDS_PER_MINUTE * MILLISECONDS_PER_SECOND) / bpm;
        }

        public void Start(GameTime gameTime) {
            startTime = gameTime.TotalGameTime.TotalMilliseconds;
        }

        public void Update(GameTime gameTime) {
            double timeSinceStart = gameTime.TotalGameTime.TotalMilliseconds - startTime;

            double timeSinceBeat = timeSinceStart % (int)beatLength;
            double timeTillNextBeat = beatLength - timeSinceBeat;

            double lastBeatTime = timeSinceStart / beatLength;
            if ((int)lastBeatTime > lastBeat) {
                lastBeat = (int)lastBeatTime;
                Console.WriteLine("BEAT {0}", lastBeat);
            }

            //Console.WriteLine("{0} -> now -> {1}", timeSinceBeat, timeTillNextBeat);
        }
    }
}
