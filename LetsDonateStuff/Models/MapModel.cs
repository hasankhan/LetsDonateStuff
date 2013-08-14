using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsDonateStuff.Models
{
    public class MapModel
    {
        public string Title { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Description { get; set; }
        public bool Draggable { get; set; }

        public MapModel(string title, float latitude, float longitude, string description, bool draggable)
        {
            this.Title = title;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Description = description;
            this.Draggable = draggable;
        }
    }
}