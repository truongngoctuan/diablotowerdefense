using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml;

namespace TowerDefense
{
    /// <summary>
    /// ease like other screen, so i dont do this screen any more
    /// </summary>
    public class HighScoreScreen
    {
        SpriteFont spriteFont;
        Texture2D backgroundImage;
        string strHighScoresFile = @"Content\HighScores.xml";
        Vector2 _topLeft;

        Texture2D headBackground;
        Texture2D bodyBackground;
        Texture2D footBackground;

        enum HighScoreScreenState { Opening, Opened, Closing, Closed };
        HighScoreScreenState highScoreScreenState;

        List<Record> records;

        int _iRecord;
        int _iWidth;
        int _iHeight;
        int _iBody;
        int _nBody;

        int _iRecordWidth;
        int _iRecordHeight;

        public HighScoreScreen()
        {           
            records = new List<Record>();
            _iRecord = -1;
            _iBody = _nBody = 0;
        }

        public HighScoreScreen(int iRecord)
        {
            records = new List<Record>();
            _iRecord = iRecord;
            _iBody = _nBody = 0;
            _topLeft = new Vector2((GlobalVar.glViewport.X - headBackground.Width) / 2, GlobalVar.glViewport.Y / 4);
        }

        public void LoadResource()
        {
            backgroundImage = GlobalVar.glContentManager.Load<Texture2D>("HighScore_background");
            headBackground = GlobalVar.glContentManager.Load<Texture2D>("HighScore_head");
            bodyBackground = GlobalVar.glContentManager.Load<Texture2D>("HighScore_body");
            footBackground = GlobalVar.glContentManager.Load<Texture2D>("HighScore_foot");
            spriteFont = GlobalVar.glContentManager.Load<SpriteFont>(@"Menu\TimesNewRoman");
        }
        public void LoadContent()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(strHighScoresFile);

            XmlNodeList xmlNodeList = xmlDoc.FirstChild.ChildNodes;
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                switch (xmlNode.Name)
                {
                    case "Record":
                        {
                            string strPlayerName = xmlNode.Attributes["Player"].Value.ToString();
                            int iScore = int.Parse(xmlNode.Attributes["Score"].Value.ToString());
                            records.Add(new Record(strPlayerName, iScore));
                            break;
                        }
                }
            }

            _topLeft = new Vector2((GlobalVar.glViewport.X - headBackground.Width) / 2, GlobalVar.glViewport.Y / 4);
            _iWidth = headBackground.Width;
            _iHeight = headBackground.Height + footBackground.Height + records.Count * 50;
            _iRecordWidth = (int)spriteFont.MeasureString("W").X * Record.nMaxNameLength + (int)spriteFont.MeasureString("0").X * Record.nMaxScoreLength + 60;
            _iRecordHeight = (int)spriteFont.MeasureString("W").Y;

            _nBody = (_iHeight - headBackground.Height - footBackground.Height) / bodyBackground.Height + 1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, (int)GlobalVar.glViewport.X, (int)GlobalVar.glViewport.Y), Color.White);
            switch (highScoreScreenState)
            {
                case HighScoreScreenState.Opening:
                    {
                        // Draw Menu
                        spriteBatch.Draw(headBackground, _topLeft, Color.White);

                        for (int i = 0; i < _iBody; i++)
                            spriteBatch.Draw(bodyBackground, new Vector2(_topLeft.X, _topLeft.Y + headBackground.Height + bodyBackground.Height * i), Color.White);

                        spriteBatch.Draw(footBackground, new Vector2(_topLeft.X, _topLeft.Y + headBackground.Height + _iBody * bodyBackground.Height), Color.White);

                        if (_iBody == _nBody)
                            highScoreScreenState = HighScoreScreenState.Opened;
                        else
                            _iBody++;
                        break;
                    }
                case HighScoreScreenState.Opened:
                    {
                        // Draw Menu
                        spriteBatch.Draw(headBackground, _topLeft, Color.White);

                        for (int i = 0; i < _nBody; i++)
                            spriteBatch.Draw(bodyBackground, new Vector2(_topLeft.X, _topLeft.Y + headBackground.Height + bodyBackground.Height * i), Color.White);

                        spriteBatch.Draw(footBackground, new Vector2(_topLeft.X, _topLeft.Y + headBackground.Height + _nBody * bodyBackground.Height), Color.White);


                        // Draw MenuItems
                        Vector2 vt2RecordPosition = new Vector2();
                        for (int i = 0; i < records.Count; i++)
                        {
                            vt2RecordPosition.X = _topLeft.X + 30;
                            vt2RecordPosition.Y = _topLeft.Y + headBackground.Height + _iRecordHeight * i;
                            spriteBatch.DrawString(spriteFont, records[i].strPlayerName, vt2RecordPosition, Color.White);

                            string strScore = records[i].iScore.ToString();
                            vt2RecordPosition.X = _topLeft.X + _iWidth - spriteFont.MeasureString(strScore).X - 30;
                            spriteBatch.DrawString(spriteFont, strScore, vt2RecordPosition, Color.White);
                        }
                        break;
                    }
                case HighScoreScreenState.Closing:
                    {
                        // Draw Menu
                        spriteBatch.Draw(headBackground, _topLeft, Color.White);

                        for (int i = 0; i < _iBody; i++)
                            spriteBatch.Draw(bodyBackground, new Vector2(_topLeft.X, _topLeft.Y + headBackground.Height + bodyBackground.Height * i), Color.White);

                        spriteBatch.Draw(footBackground, new Vector2(_topLeft.X, _topLeft.Y + headBackground.Height + _iBody * bodyBackground.Height), Color.White);

                        if (_iBody == 0)
                            highScoreScreenState = HighScoreScreenState.Closed;
                        else
                            _iBody--;
                        break;
                    }
                case HighScoreScreenState.Closed:
                    {
                        break;
                    }
            }
        }
    }

    public class Record
    {
        public static int nMaxNameLength = 10;
        public static int nMaxScoreLength = 10;
        
        public string strPlayerName;
        public int iScore;

        public Record(string playerName, int score)
        {
            strPlayerName = playerName;
            iScore = score;
        }
    }
}
