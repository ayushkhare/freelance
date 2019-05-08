using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace Kemin_Yolk_Sensor
{
    public class YolkScores
    {
        //values for the RGB and its conversions


        public string ColourName { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
    


        public YolkScores()
        {
            
        }

        public YolkScores(string colourname, int R, int G, int B)
        {
            ColourName = colourname;
            Red = R;
            Green = G;
            Blue = B;
        }



        public YolkScores(int R, int G, int B)
        {
            Red = R;
            Green = G;
            Blue = B;
        }

        

        //compare 2 RGB values using Euclidiean distance
        public int CompareToEuc(YolkScores colourcompare)
        {
            double eucdistance = Math.Sqrt(Math.Pow((Red - colourcompare.Red), 2) + Math.Pow((Green - colourcompare.Green), 2) + Math.Pow((Blue - colourcompare.Blue), 2));
            string values = (Red.ToString() + ", " + colourcompare.Red.ToString()
                + ", " + Green.ToString() + ", " + colourcompare.Green.ToString()
                + ", " + Blue.ToString() + ", " + colourcompare.Blue.ToString()
                );
            return Convert.ToInt16(Math.Round(eucdistance)); ;
        }



        //get the RGB value from the reading and compare it to the list of RGB values calibrated to the fan
        public  string GetColour(int R, int G, int B)
        {
            //values to be used
            YolkScores selectedcolour = new YolkScores(R, G, B);
            int differencecheck = 101;
            string yolkscore = "";

       //List of calibration RGB values
            List<YolkScores> colorList = new List<YolkScores>()
               { new YolkScores("1", 220, 185, 0), new YolkScores("2", 224, 182, 0), new YolkScores("3", 228, 183, 0),
                  new YolkScores("4", 229, 181, 0), new YolkScores("5", 234, 170, 0), new YolkScores("6", 238, 163, 0),
                  new YolkScores("7", 234, 156, 0), new YolkScores("8", 237, 142, 0), new YolkScores("9", 237, 136, 0),
                  new YolkScores("10", 236, 127, 0), new YolkScores("11", 236, 119, 0), new YolkScores("12", 237, 113, 0),
                  new YolkScores("13", 234, 106, 0), new YolkScores("14", 236, 97, 0), new YolkScores("15", 233, 70, 0),};

            //compare RGB reading to all the RGB values in the list, selects the closest one
            foreach (YolkScores colorformula in colorList)
            {
                int colourdiff = selectedcolour.CompareToEuc(colorformula);
                if (colourdiff < differencecheck)
                {
                    differencecheck = colourdiff;
                    yolkscore = colorformula.ColourName;
                }

            }





            //return the score of the egg yolk from the reading
            return yolkscore;
        }
    }
}