﻿

using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Collections.Generic;
using LogoQuizGame.Adapter;
using System.Linq;
namespace LogoQuizGame
{
    [Activity(Label = "LogoQuizGame", MainLauncher = true, Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : Activity
    {
        public List<string> suggestSource = new List<string>();
        public GridViewAnswerAdapter answerAdapter;
        public GridViewSuggestAdapter suggestAdapter;
        public Button btnSubmit;
        public GridView gvAnswer, gvSuggest;
        public ImageView imgLogo;
        int[] img_list =
        {
            
            
            Resource.Drawable.facebook,
            
            Resource.Drawable.googleplus,
           
            Resource.Drawable.instagram,
            Resource.Drawable.kickstarter,
            Resource.Drawable.linkedin,
            
            Resource.Drawable.pinterest,
            
            Resource.Drawable.skype,
            Resource.Drawable.snapchat,
            Resource.Drawable.soundcloud,
           
            Resource.Drawable.whatsapp,
            
            Resource.Drawable.youtube
        };
        public char[] answer;
        string correct_answer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource  
            SetContentView(Resource.Layout.activity_main);
            InitViews();
        }
        private void InitViews()
        {
            gvAnswer = FindViewById<GridView>(Resource.Id.gvAnswer);
            gvSuggest = FindViewById<GridView>(Resource.Id.gvSuggest);
            imgLogo = FindViewById<ImageView>(Resource.Id.imgLogo);
            SetupList();
            btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            btnSubmit.Click += delegate
            {
                //Convert char Array to string  
                string result = new string(Common.Common.user_submit_answer);
                if (result.Equals(correct_answer))
                {
                    Toast.MakeText(ApplicationContext, "Finish ! This is" + result.ToUpper(), ToastLength.Short).Show();
                    //Result   
                    Common.Common.user_submit_answer = new char[correct_answer.Length];
                    //Update UI  
                    GridViewAnswerAdapter answerAdapter = new GridViewAnswerAdapter(SetupNullList(), this);
                    gvAnswer.Adapter = answerAdapter;
                    answerAdapter.NotifyDataSetChanged();
                    GridViewSuggestAdapter suggestAdapter = new GridViewSuggestAdapter(suggestSource, this, this);
                    gvSuggest.Adapter = suggestAdapter;
                    suggestAdapter.NotifyDataSetChanged();
                    SetupList();
                }
                else
                    Toast.MakeText(this, "Incorrect!!!", ToastLength.Short).Show();
            };
        }
        private char[] SetupNullList()
        {
            char[] result = new char[answer.Length];
            return result;
        }
        private void SetupList()
        {
            //Random Logo  
            Random random = new Random();
            int imageSelected = img_list[random.Next(img_list.Length)];
            imgLogo.SetImageResource(imageSelected);
            correct_answer = Resources.GetResourceName(imageSelected);
            correct_answer = correct_answer.Substring(correct_answer.LastIndexOf("/") + 1);
            answer = correct_answer.ToCharArray();
            Common.Common.user_submit_answer = new char[answer.Length];
            //Add Answer Character to List  
            suggestSource.Clear();
            foreach (char item in answer)
            {
                suggestSource.Add(Convert.ToString(item));
            }
            //Random characters from alphabet list and add to list  
            for (int i = answer.Length; i < answer.Length * 2; i++)
                suggestSource.Add(Common.Common.alphabet_characters[random.Next(Common.Common.alphabet_characters.Length)]);
            //Sort List  
            suggestSource = suggestSource.OrderBy(x => Guid.NewGuid()).ToList();
            //Set Adapter for Grid View  
            answerAdapter = new GridViewAnswerAdapter(SetupNullList(), this);
            suggestAdapter = new GridViewSuggestAdapter(suggestSource, this, this);
            answerAdapter.NotifyDataSetChanged();
            suggestAdapter.NotifyDataSetChanged();
            gvAnswer.Adapter = answerAdapter;
            gvSuggest.Adapter = suggestAdapter;
        }
    }
}