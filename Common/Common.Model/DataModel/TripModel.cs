using System;
using AzureMapsToolkit.Common;

namespace Common.Model.DataModel
{
    public class TripModel
    {
        public Coordinate[] Points { get; internal set; }
        public int Duration { get; internal set; }
        public int Distance { get; internal set; }
    }
}
