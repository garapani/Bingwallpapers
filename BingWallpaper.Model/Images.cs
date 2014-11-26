using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BingWallpaper.Model
{
    public class H
    {
        public string desc { get; set; }
        public string link { get; set; }
        public string query { get; set; }
        public int locx { get; set; }
        public int locy { get; set; }
    }

    public class Image
    {
        public string startdate { get; set; }
        public string fullstartdate { get; set; }
        public string enddate { get; set; }
        public string url { get; set; }
        public string userPreferredurl { get; set; }
        public string url480x800 { get; set; }
        public string url768x1280 { get; set; }
        public string url720x1280 { get; set; }
        public string url1080x1920 { get; set; }
        public string urlbase { get; set; }
        public string copyright { get; set; }
        public string copyrightlink { get; set; }
        public bool wp { get; set; }
        public string hsh { get; set; }
        public int drk { get; set; }
        public int top { get; set; }
        public int bot { get; set; }
        public List<H> hs { get; set; }
        public List<object> msg { get; set; }

        public override bool Equals(object obj)
        {
            Image temp = obj as Image;
            if (url == temp.url)
            { return true; }
            else return false;
        }
    }

    public class Tooltips
    {
        public string loading { get; set; }
        public string previous { get; set; }
        public string next { get; set; }
        public string walle { get; set; }
        public string walls { get; set; }
    }

    public class RootObject
    {
        public List<Image> images { get; set; }
        public Tooltips tooltips { get; set; }
    }
}
