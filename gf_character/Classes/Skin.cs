using System;
using System.Collections.Generic;
using System.Text;

namespace gf_character.Classes
{
    public class Skin
    {
        public bool isMale { get; set; }
        public List<HeadOverlay> headOverlay { get; set; }
        public List<ComponentVariation> componentVariation { get; set; }
        public float[] faceFeature { get; set; }
        public List<PropIndex> propIndex { get; set; }
        public int hairColor { get; set; }
        public int hairColor2 { get; set; }

        public Skin()
        {
            headOverlay = new List<HeadOverlay>();
            componentVariation = new List<ComponentVariation>();
            faceFeature = new float[20];
            propIndex = new List<PropIndex>();
        }

    }
}
