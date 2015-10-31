using ActionSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ConversationSystem
{
    class UITextManager : EventHandler
    {
        //static public UITextManager ConversationText;

        private TextMesh SpriteText;
        public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public double ScrollSpeed = 10;
        public int MaxLineLength = 20;

        private string NextPhrase = "sadassdasd";
        private ActionGroup grp = new ActionGroup();
        private Vector3 InitialPos = new Vector3();
        public double EaseTime = 1.5;

        public Vector3 FinalPos;
        // Use this for initialization
        public UITextManager()
        {
            

            
        }

        void Start()
        {

            //TextBackground = (GameObject)Instantiate(TextBackground, this.gameObject.transform.position + InitialTextOffsett, new Quaternion());
            //TextBackground.transform.localScale = InitialTextScale;
            //UITextManager.ConversationText = this;
            



        }

        public void Connect()
        {
            EventSystem.GlobalHandler.Connect(Events.UpdateText, OnUpdateText);
            EventSystem.GlobalHandler.Connect(Events.ActivateTextWindow, OnActivateWindowEvent);
            EventSystem.GlobalHandler.Connect(Events.DeactivateTextWindow, OnDeactivateWindowEvent);

            InitialPos = this.gameObject.transform.localPosition;
            SpriteText = this.gameObject.transform.FindChild("SpriteText").gameObject.GetComponent<TextMesh>();
        }


        public void Appear()
        {
            var seq = ActionSystem.Action.Sequence(grp);
            
            
            Action.Property(seq, this.gameObject.transform.GetProperty(o => o.localPosition), FinalPos, EaseTime, Curve);
        }

        public void Disappear()
        {
            var seq = ActionSystem.Action.Sequence(grp);
            var finalPos = InitialPos;
            
            Action.Property(seq, this.gameObject.transform.GetProperty(o => o.localPosition), finalPos, EaseTime, Curve);
            Action.Call(seq, gameObject.Destroy);
        }

        public void UpdateText(string newText)
        {
            NextPhrase = newText;
            UpdateText();
        }

        //Recieves a StringEvent
        public void OnUpdateText(EventData eventData)
        {
            var data = eventData as StringEvent;
            UpdateText(data.Message);
        }

        public void OnActivateWindowEvent(EventData eventData)
        {
            
            Appear();
        }

        private void Destroy(EventData data)
        {
            //EventSystem.GlobalHandler.Disconnect(Events.ActivateTextWindow, Destroy);
            gameObject.Destroy();
        }

        public void OnDeactivateWindowEvent(EventData eventData)
        {

            Disappear();
        }

        void UpdateText()
        {
            SpriteText.text = NextPhrase;
            //SpriteText.text = "";

            //var seq = Action.Sequence(grp);
            //int lastIndex = 0;
            //int length = 0;

            //List<string> lines = new List<string>();

            ////loop through the characters
            //for (int i = 0; i < NextPhrase.Count(); ++i)
            //{

            //    //if we encounter a space...
            //    if (NextPhrase[i] == ' ')
            //    {
            //        //check to see if our length from lasti index is greater than the
            //        //line length, (and if it is, make sure that it is not "negative")
            //        if (i - lastIndex > MaxLineLength && i - lastIndex > 0)
            //        {
            //            //if this is not a space right after a space when the max
            //            //length is 1...
            //            if (length == 0)
            //            {
            //                length = i - lastIndex;
            //            }
            //            //add the word, reset everything, and check this 
            //            //index again.
            //            lines.Add(NextPhrase.Substring(lastIndex, length));
            //            lastIndex = lastIndex + length + 1;
            //            length = 0;
            //            --i;
            //        }
            //        else
            //        {
            //            //recalculate the length of the current line
            //            if (i - lastIndex <= MaxLineLength)
            //            {
            //                length = i - lastIndex;
            //            }
            //        }
            //    }
            //}
            ////Check the curent word list to see if it should be 
            ////it's own line
            //if (NextPhrase.Count() - lastIndex > MaxLineLength)
            //{
            //    if (length == 0)
            //    {
            //        length = NextPhrase.Count() - lastIndex;
            //    }
            //    lines.Add(NextPhrase.Substring(lastIndex, length));
            //    lastIndex = lastIndex + length;
            //}
            ////Do one last check
            //if ((NextPhrase.Count() - lastIndex) != 0)
            //{
            //    if (NextPhrase[lastIndex] == ' ')
            //    {
            //        ++lastIndex;
            //    }

            //    lines.Add(NextPhrase.Substring(lastIndex, NextPhrase.Count() - lastIndex));
            //}
            //foreach(var i in lines)
            //{
            //    Action.Call(seq, AddWord, i + '\n');
            //    Action.Delay(seq, 1 / (ScrollSpeed * 10));

            //}
            //var words = NextPhrase.Split(' ');
            //foreach (var i in words)
            //{
            //    charNumber += i.Count();
            //    if(i.Contains("<"))
            //    {
            //        charNumber -= 7;
            //    }

            //    Action.Call(seq, AddWord, i);
            //    if (i.Contains('\n'))
            //    {
            //        charNumber = 0;
            //    }
            //    else if (charNumber >= MaxLineLength)
            //    {

            //        Action.Call(seq, AddCharacter, '\n');
            //        charNumber = 0;
            //    }
            //    else
            //    {
            //        Action.Call(seq, AddCharacter, ' ');
            //    }

            //    Action.Delay(seq, 1 / (ScrollSpeed * 10));

            //}
        }

        void CharacterByCharacter()
        {
            SpriteText.text = "";
            var seq = Action.Sequence(grp);
            int charNumber = 0;
            foreach(var i in NextPhrase)
            {
               
                if (charNumber == MaxLineLength)
                {
                    Action.Call(seq, AddCharacter, '\n');
                    Action.Delay(seq, 1 / (ScrollSpeed * 10));
                    charNumber = 0;
                }
                if(i == '\n')
                {
                    charNumber = 0;
                }
                Action.Call(seq, AddCharacter, i);
                Action.Delay(seq, 1/(ScrollSpeed * 10));
                ++charNumber;
            }
        }

        void AddWord(string word)
        {
            SpriteText.text += word;
        }

        void AddCharacter(char character)
        {
            SpriteText.text += character;
        }

        // Update is called once per frame
        void Update()
        {
            grp.Update(Time.smoothDeltaTime);
        }

        void OnDestroy()
        {
            EventSystem.GlobalHandler.Disconnect(Events.UpdateText, OnUpdateText);
            EventSystem.GlobalHandler.Disconnect(Events.ActivateTextWindow, OnActivateWindowEvent);
            EventSystem.GlobalHandler.Disconnect(Events.DeactivateTextWindow, OnDeactivateWindowEvent);
        }

    }
}
