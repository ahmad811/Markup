
using System;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class MarkupVoiceCommands : MonoBehaviour {
    //audio
    public AudioSource Success;
    public AudioSource Fail;
    public AudioSource CommandRecognized;

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> keywords = new Dictionary<string, Action>();

    //private TextToSpeechManager ttsm;
    private void Awake()
    {
        keywords.Add("Red Color", () => {
            Coloring(Color.red);
        });
        keywords.Add("Blue Color", () => {
            Coloring(Color.blue);
        });
        keywords.Add("Green Color", () => {
            Coloring(Color.green);
        });
        keywords.Add("Black Color", () => {
            Coloring(Color.black);
        });
        keywords.Add("White Color", () => {
            Coloring(Color.white);
        });
        keywords.Add("Yello Color", () => {
            Coloring(Color.yellow);
        });
        keywords.Add("Gray Color", () => {
            Coloring(Color.gray);
        });

        keywords.Add("Show Colors", () => {
            if (CommandRecognized != null)
            {
                CommandRecognized.Play();
            }
            SendMessage("ShowColors");

        });
        keywords.Add("Hide Colors", () => {
            if (CommandRecognized != null)
            {
                CommandRecognized.Play();
            }
            SendMessage("HideColors");
        });
        
        keywords.Add("Show Markup", () => {
            if (CommandRecognized != null)
            {
                CommandRecognized.Play();
            }
            SendMessage("ShowMarkup");
        });
        keywords.Add("Hide Markup", () => {
            if (CommandRecognized != null)
            {
                CommandRecognized.Play();
            }
            SendMessage("HideMarkup");
        });

        keywords.Add("Line Mode", () => {
            if (CommandRecognized != null)
            {
                CommandRecognized.Play();
            }
            SendMessage("LineMode");
        });
        keywords.Add("Rect Mode", () => {
            if (CommandRecognized != null)
            {
                CommandRecognized.Play();
            }
            SendMessage("RectMode");
        });
        /*
        keywords.Add("Tap Mode", () => {
            if (CommandRecognized != null)
            {
                CommandRecognized.Play();
            }
            SendMessage("TapMode");
        });
       
        keywords.Add("Hand Mode", () => {
            if (CommandRecognized != null)
            {
                CommandRecognized.Play();
            }
            SendMessage("HandMode");
        });
        */
        

        keywords.Add("Thin", () => {
            if (CommandRecognized != null)
            {
                CommandRecognized.Play();
            }
            SendMessage("ThinPen");
        });
        keywords.Add("Thick", () => {
            if (CommandRecognized != null)
            {
                CommandRecognized.Play();
            }
            SendMessage("ThickPen");
        });
        keywords.Add("Exit", () => {
            Application.Quit();
        });
        keywords.Add("Quit ", () => {
            Application.Quit();
        });
        //
        if (keywordRecognizer == null)
        {
            string[] keys = new string[keywords.Keys.Count];
            keywords.Keys.CopyTo(keys, 0);
            keywordRecognizer = new KeywordRecognizer(keys);
            keywordRecognizer.OnPhraseRecognized += keywordRecognizer_OnPhraseRecognized;
        }
        keywordRecognizer.Start();

        //ttsm = GetComponent<TextToSpeechManager>();
    }
    private void Coloring(Color color)
    {
        if (CommandRecognized != null)
        {
            CommandRecognized.Play();
        }
        SendMessage("OnColorChange", color);
    }
    private void keywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Action action;
        if (keywords.TryGetValue(args.text, out action))
        {
            if (action != null)
            {
                action.Invoke();
            }
        }
    }

    protected void OnEnable()
    {
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Start();
        }

    }
    /*
    private void SpeakText(string text)
    {
        ttsm.SpeakText(text);
    }
    */
    protected void OnDisable()
    {
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
        }

    }

    private void OnDestroy()
    {
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Dispose();
        }
    }
}
