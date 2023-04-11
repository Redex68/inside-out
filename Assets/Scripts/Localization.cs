using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//0 English
//1 Hrvatski

namespace Localization
{
    public static class Loc
    {
        public static string loc(Languages lang)
        {
            switch(lang)
            {
                case Languages.English: return "English";
                case Languages.Hrvatski: return "Croatian";
            }

            return "";
        }

        public static string[] loc()
        {
            string[] s = { "English", "Hrvatski" };
            return s;
        }

        public static string loc(string[] txt)
        {
            return txt[(int)Settings.GameLanguage];
        }

        public static void locObj(GameObject obj, string[] txt)
        {
            obj.GetComponent<TMPro.TMP_Text>().text = loc(txt);
        }

        public static void locObj(Button obj, string[] txt)
        {
            var comp = obj.gameObject.GetComponent<TMPro.TMP_Text>();
            if(comp == null) 
                Debug.Log(string.Format("{0} doesnt have text", obj.gameObject.name));
            else comp.text = loc(txt);
        }

        public static void locChild(GameObject parent, string[] txt)
        {
            var comp = parent.GetComponentInChildren<TMPro.TMP_Text>();
            if(comp == null) 
                Debug.Log(string.Format("{0} doesnt have text in child", parent.name));

            parent.GetComponentInChildren<TMPro.TMP_Text>().text = loc(txt);
        }

        public static void locChild(Button parent, string[] txt)
        {
            parent.gameObject.GetComponentInChildren<TMPro.TMP_Text>().text = loc(txt);
        }
    }

    public static class MainMenuTxt
    {
        public static string[] Title =
        {
            "InsideOut",
            "InsideOut"
        };

        public static string[] Play =
        {
            "Play",
            "Igraj"
        };

        public static string[] Quiz =
        {
            "Quiz",
            "Kviz"
        };

        public static string[] Volume =
        {
            "Volume",
            "Glasnoća"
        };        
        
        public static string[] Back =
        {
            "Back",
            "Povratak"
        };

        public static string[] Language =
        {
            "Language",
            "Jezik"
        };

        public static string[] Settings =
        {
            "Settings",
            "Postavke"
        };

        public static string[] Exit =
        {
            "Exit",
            "Izlaz"
        };

        public static string[] Credits =
        {
            "Credits",
            "Zasluge"
        };

        public static string[] CreditsText =
        {
            @"Students:
Marin Petric
Bruno Perković
Branimir Tomeljak
Josipa Markić
Sven Leko

Mentors:
Mirko Sužnjević
Lea Skorin-Kapov
Maja Matijašević",
            @"Studenti:
Marin Petric
Bruno Perković
Branimir Tomeljak
Josipa Markić
Sven Leko

Mentori:
Mirko Sužnjević
Lea Skorin-Kapov
Maja Matijašević"
        };
    }

}