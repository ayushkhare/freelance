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
using Android.Preferences;
using Newtonsoft.Json;
using Android.Support.V7.App;
using Android.Content.PM;

namespace Kemin_Yolk_Sensor
{
    [Activity(Theme = "@style/MyTheme.ScoreList", Label = "Egg Yolk Scores", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SaveListActivity : AppCompatActivity
    {
        //setting up instances to be used later
        private ListView listView;
        private List<string> listOfScores;
        private List<string> numberedList;
        private ArrayAdapter<string> adapter;
        private string thescores;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ListLayout);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //assigns the listview
            listView = FindViewById<ListView>(Resource.Id.scorelistview);
            //get the list of data
            listOfScores = GetScoresFromPreferences();
            numberedList = new List<string>();
            //increments to assign a number to each data entry in the list
            int numberincrement = 1;
            //add a number to each entry in the list
            foreach (string score in listOfScores)
            {
                numberedList.Add(numberincrement + ". " + score);
                numberincrement++;
            }
            //store the list in the array adapter
            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemMultipleChoice, numberedList);
            listView.Adapter = adapter;
            //allow for multiple records in the list to be selected
            listView.ChoiceMode = ChoiceMode.Multiple;

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // set the menu layout on Main Activity  
            MenuInflater.Inflate(Resource.Menu.ListMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                //return back to camera
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                //selects all items in the list
                case Resource.Id.selectall:
                    for (int i = 0; i < listView.Adapter.Count; i++)
                    {
                        listView.SetItemChecked(i, true);
                    }
                    return true;

                //deletes selected items from the list
                case Resource.Id.deletebutton:
                    {
                        ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
                        var scores = sharedPreferences.GetString("scorelist", null);
                        //gets the list but with an added value telling if the entries have been selected
                        var pos = FindViewById<ListView>(Resource.Id.scorelistview).CheckedItemPositions;
                        StringBuilder sb = new StringBuilder();
                        string[] positionarray = pos.ToString().Split(',');
                        string positions = "";
                        foreach (string position in positionarray)
                        {
                            //if the entry is selected, add it to string positions 
                            if (position.Contains("true"))
                            {
                                positions += position;
                            }

                            positions = positions.Replace("=true", "");
                            positions = positions.Replace("{", "");
                            positions = positions.Replace("}", "");

                        }

                        //split the entries in string positions into an array
                        string[] positionsarray = positions.Split(' ');
                        if (pos.ToString() != "{}")
                        {

                            //convert the array into an int array to remove the entry by position
                            int[] positionsnumarray = Array.ConvertAll(positionsarray, s => int.Parse(s));


                            //if only one entry is selected, then remove it this way
                            if (positionsnumarray.Length == 1)
                            {
                                //remove object from the list
                                int increment = positionsnumarray[0];
                                listOfScores.RemoveAt(increment);


                            }

                            //if more than one entry is selected, remove them this way
                            if (positionsnumarray.Length > 1)
                            {
                                //remove selected objects from the list

                                //previous entry number
                                int prevnum = 0;
                                //current entry in the list
                                int currentnum = 0;
                                //increment
                                int check = 0;
                                foreach (int num in positionsnumarray)
                                {
                                    
                                        currentnum = (num - check);
                                    //remove the entry from the list
                                    listOfScores.RemoveAt(currentnum);

                                    prevnum = num;
                                    check++;

                                }

                            }

                            numberedList = new List<string>();
                            //reorder the list numbers
                            int numberincrement = 1;
                            foreach (string score in listOfScores)
                            {
                                numberedList.Add(numberincrement + ". " + score);
                                numberincrement++;
                            }
                            //update the adapter with the new list
                            adapter.Clear();
                            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemMultipleChoice, numberedList);
                            listView.Adapter = adapter;
                            listView.ChoiceMode = ChoiceMode.Multiple;
                            adapter.NotifyDataSetChanged();

                            //update the string builder stored in the sharedpreferences
                            foreach (string scoretext in listOfScores)
                            {
                                sb.Append('!').Append(scoretext);
                            }
                            if (!(sb.Length == 0))
                            {
                                sb.Remove(0, 1);
                            }
                            
                            ISharedPreferencesEditor editor = sharedPreferences.Edit();
                            editor.PutString("scorelist", sb.ToString());
                            editor.Commit();

                        }

                        Toast.MakeText(this, "Delete Successful", ToastLength.Long).Show();

                        return true;
                    }

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        //get the data saved from the sharedpreferences
        private List<string> GetScoresFromPreferences()
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            var scores = sharedPreferences.GetString("scorelist", null);
            if (scores == null)
            {
                return new List<string>();

            }
            thescores = scores.ToString();
            
            //extract the data from the string builder
            if (thescores.StartsWith("!"))
            {
                thescores = thescores.Remove(0, 1);
            }
            if (!(thescores.Length == 0))
            {
                listOfScores = thescores.Split('!').ToList();
            }

            if (thescores.Length == 0)
            {
                return new List<string>();
            }

            //return the list of data
            return listOfScores;
        }

        
    }
}