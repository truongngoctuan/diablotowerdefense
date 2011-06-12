using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections;

namespace TowerDefense
{
    public class LoadScreen:Base.BaseGameTemplate
    {
        public Loader loader;
        public IEnumerator<float> enumerator;
        Texture2D backgroundImage;
        Texture2D progressBar;
        Texture2D flowBar;
        Vector2 vt2ProgressBarPosition;
        Vector2 vt2ProgressBarSize;
        Vector2 vt2FlowBarPosition;
        Vector2 vt2FlowBarSize;

        public float fProgress;
        public LoadScreen()
        {
            loader = new Loader();            
            backgroundImage = GlobalVar.glContentManager.Load<Texture2D>("LoadScreen_background");
            progressBar = GlobalVar.glContentManager.Load<Texture2D>("LoadScreen_progressbar");
            flowBar = GlobalVar.glContentManager.Load<Texture2D>("LoadScreen_flowbar");

            vt2ProgressBarPosition = new Vector2(GlobalVar.glViewport.X / 2, GlobalVar.glViewport.Y * 3 / 4);
            vt2ProgressBarSize = new Vector2(progressBar.Width, progressBar.Height);
            vt2FlowBarPosition = new Vector2(vt2ProgressBarPosition.X - progressBar.Width / 2, vt2ProgressBarPosition.Y - vt2ProgressBarSize.Y / 2 + (vt2ProgressBarSize.Y - flowBar.Height) / 2);
            vt2FlowBarSize = new Vector2(0, flowBar.Height);
        }

        public void Update()
        {
            bool bIsCompleted = !(enumerator.MoveNext());
            if (bIsCompleted == true)
            {
                GlobalVar.SetGameStage(GameStage.SinglePlayer);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            fProgress = enumerator.Current;
            vt2FlowBarSize.X = fProgress * flowBar.Width;
            spriteBatch.Draw(backgroundImage, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(progressBar, vt2ProgressBarPosition, null, Color.White, 0.0f, new Vector2(progressBar.Width / 2, progressBar.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(flowBar, vt2FlowBarPosition, new Rectangle(0, 0, (int)vt2FlowBarSize.X, (int)vt2FlowBarSize.Y), Color.White);
        }

        #region BaseGameTemplate Members

        public void Initialize()
        {
            //throw new NotImplementedException();
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            //throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }

    public class Loader : IEnumerable<float>
    {
        int iLoadedItems;
        public Loader()
        {
            iLoadedItems = 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<float> GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<float> LoadCreepResource(List<Unit> unitCollections)
        {
            iLoadedItems = 0;
            foreach (Unit unit in unitCollections)
            {
                unit.LoadResource();
                iLoadedItems++;
                yield return (float)iLoadedItems * 1.0f / unitCollections.Count;
            }

            yield return 1.0f;
        }
    }
}
