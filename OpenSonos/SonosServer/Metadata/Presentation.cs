﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OpenSonos.SonosServer.Metadata
{
    [Serializable]
    [XmlRoot("Presentation")]
    public class Presentation
    {
        [XmlElement("PresentationMap")] 
        public List<PresentationMap> PresentationMaps;

        public Presentation()
        {
        }

        public Presentation(PresentationMap singleMap)
        {
            PresentationMaps = new List<PresentationMap>
            {
                PresentationMap.DefaultSonosSearch()
            };
        }
    }
}