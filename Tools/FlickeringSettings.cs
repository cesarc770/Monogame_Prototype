using System;
using System.Collections.Generic;
using System.Text;

namespace Rapid_Prototype_1
{
    public class FlickeringSettings
    {
        public string Name;
        public float BloomThreshold;
        public float BlurAmount;
        public float BloomIntensity;
        public float BaseIntensity;
        public float BloomSaturation;
        public float BaseSaturation;
        public FlickeringSettings(string name, float bloomThreshold, float blurAmount,
                                float bloomIntensity, float baseIntensity,
                                float bloomSaturation, float baseSaturation)
        {
            Name = name;
            BloomThreshold = bloomThreshold;
            BlurAmount = blurAmount;
            BloomIntensity = bloomIntensity;
            BaseIntensity = baseIntensity;
            BloomSaturation = bloomSaturation;
            BaseSaturation = baseSaturation;
        }     
        public static FlickeringSettings[] PresetSettings =
        {
        //                Name           Thresh  Blur Bloom  Base  BloomSat BaseSat
        new FlickeringSettings("Default",     0.25f,  4,   1.25f, 1,    1,       1),
        new FlickeringSettings("Soft",        0,      3,   1,     1,    1,       1),
        new FlickeringSettings("Desaturated", 0.5f,   8,   2,     1,    0,       1),
        new FlickeringSettings("Saturated",   0.25f,  4,   2,     1,    2,       0),
        new FlickeringSettings("Blurry",      0,      2,   1,     0.1f, 1,       1),
        new FlickeringSettings("Subtle",      0.5f,   2,   1,     1,    1,       1),
        new FlickeringSettings("No Glow",     0,   0,   0,     1,    0,       1),
    };
    }
}
