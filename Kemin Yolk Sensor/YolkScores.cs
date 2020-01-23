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
               {
                new YolkScores("1", 220, 185, 0),
                new YolkScores("1.2", 221, 185, 0),
                new YolkScores("1.4", 222, 184, 0),
                new YolkScores("1.6", 222, 183, 0),
                new YolkScores("1.8", 223, 183, 0),
                new YolkScores("2", 224, 182, 0),
                new YolkScores("2.2", 225, 182, 0),
                new YolkScores("2.4", 226, 182, 0),
                new YolkScores("2.6", 227, 183, 0),
                new YolkScores("2.8", 227, 183, 0),
                new YolkScores("3", 228, 183, 0),
                new YolkScores("3.2", 228, 182, 0),
                new YolkScores("3.4", 229, 182, 0),
                new YolkScores("3.6", 229, 181, 0),
                new YolkScores("3.8", 230, 180, 0),
                new YolkScores("4", 230, 179, 0),
                new YolkScores("4.2", 231, 177, 0),
                new YolkScores("4.4", 231, 176, 0),
                new YolkScores("4.6", 232, 174, 0),
                new YolkScores("4.8", 233, 172, 0),
                new YolkScores("5", 234, 170, 0),
                new YolkScores("5.2", 235, 168, 0),
                new YolkScores("5.4", 236, 167, 0),
                new YolkScores("5.6", 237, 165, 0),
                new YolkScores("5.8", 237, 164, 0),
                new YolkScores("6", 238, 163, 0),
                new YolkScores("6.2", 237, 162, 0),
                new YolkScores("6.4", 236, 160, 0),
                new YolkScores("6.6", 235, 158, 0),
                new YolkScores("6.8", 235, 157, 0),
                new YolkScores("7", 234, 155, 0),
                new YolkScores("7.2", 235, 152, 0),
                new YolkScores("7.4", 235, 150, 0),
                new YolkScores("7.6", 236, 147, 0),
                new YolkScores("7.8", 236, 144, 0),
                new YolkScores("8", 237, 142, 0),
                new YolkScores("8.2", 237, 140, 0),
                new YolkScores("8.4", 237, 139, 0),
                new YolkScores("8.6", 237, 138, 0),
                new YolkScores("8.8", 237, 137, 0),
                new YolkScores("9", 237, 136, 0),
                new YolkScores("9.2", 237, 134, 0),
                new YolkScores("9.4", 237, 132, 0),
                new YolkScores("9.6", 236, 130, 0),
                new YolkScores("9.8", 236, 128, 0),
                new YolkScores("10", 236, 127, 0),
                new YolkScores("10.2", 236, 126, 0),
                new YolkScores("10.4", 236, 125, 0),
                new YolkScores("10.6", 236, 124, 0),
                new YolkScores("10.8", 236, 122, 0),
                new YolkScores("11", 236, 119, 0),
                new YolkScores("11.2", 236, 118, 0),
                new YolkScores("11.4", 236, 117, 0),
                new YolkScores("11.6", 236, 116, 0),
                new YolkScores("11.8", 236, 114, 0),
                new YolkScores("12", 236, 113, 0),
                new YolkScores("12.2", 237, 112, 0),
                new YolkScores("12.4", 236, 110, 0),
                new YolkScores("12.6", 235, 109, 0),
                new YolkScores("12.8", 235, 107, 0),
                new YolkScores("13", 234, 106, 0),
                new YolkScores("13.2", 234, 104, 0),
                new YolkScores("13.4", 234, 102, 0),
                new YolkScores("13.6", 235, 101, 0),
                new YolkScores("13.8", 236, 99, 0),
                new YolkScores("14", 236, 97, 0),
                new YolkScores("14.2", 235, 92, 0),
                new YolkScores("14.4", 235, 86, 0),
                new YolkScores("14.6", 234, 81, 0),
                new YolkScores("14.8", 234, 75, 0),
                new YolkScores("15", 233, 70, 0),
            };

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