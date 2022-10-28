using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinOrLoose : MonoBehaviour
{
    public void Win()
    {

    }

    public void Loose()
    {

    }

    void Update()
    {
        
    }
    /*class Fireworks
    {
        internal float timer { get; set; }
        internal bool explosed { get; set; }
        internal bool done { get; set; }
        internal List<Color> lstColors {get;set;}

        //------------------------------------------------------------
        internal Fireworks()
        {
            timer = 0;
            explosed = false;
            done = false;
            lstColors = new List<Color>() { Color.Red, Color.LimeGreen, Color.Blue, Color.Yellow, Color.Violet, Color.Aqua, Color.Orange, Color.Green };
            Tools.Get<EffectsManager>().whistle.Play();
        }
        //------------------------------------------------------------
        internal void Update(GameTime pGameTime)
        {
            timer += (1.0f / 60.0f);

            if (timer >= 1 && !explosed)
            {
                explosed = true;
                var icolor = Tools.Rnd(0, 5);
                var lclx = Tools.Rnd(Scenes.Slate[Scenes.SlateEdges.Left] + 100, Scenes.Slate[Scenes.SlateEdges.Right] - 100);
                var lcly = Tools.Rnd(Scenes.Slate[Scenes.SlateEdges.Up] + 100, Scenes.Slate[Scenes.SlateEdges.Up] + (Scenes.slate.Height / 2));
                Tools.Get<EffectsManager>().PlayExplosion(200, new Vector2(lclx, lcly), lstColors[icolor]);
                Tools.Get<EffectsManager>().firework.Play();
            }

            if (timer >= 3)
                done = true;
        }
    }
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * internal void PlayExplosion(short pNumber, Vector2 pPos, Color pColor)
        {
            for (short i = 0; i < pNumber; i++)
            {
                var pwr = (Tools.Rnd(50, 300)/100.0f);
                var angle = Tools.Rnd(0, 360);
                var vx = (float)Math.Cos(MathHelper.ToRadians(angle));
                var vy = (float)Math.Sin(MathHelper.ToRadians(angle));
                var frct = (vx > 0) ? 0.005f : -0.005f;
                var disp = (1.0f / Tools.Rnd(90, 180) );
                var dtlife = (Tools.Rnd(75, 150)) / 100;
                var lclscale = (Tools.Rnd(100, 200) / 100.0f);

                var part = new Particles(pColor, pPos, new Vector2(vx,vy), new Vector2(pwr,pwr), new Vector2(0,-0.01f), new Vector2(frct,0), 1.0f, disp, 0);
                part.scale = new Vector2(lclscale, lclscale);

                var lclaura = new Particles(pColor, new Vector2(pPos.X+1, pPos.Y+1), new Vector2(vx,vy), new Vector2(pwr,pwr), new Vector2(0,-0.01f), new Vector2(frct,0), 1.0f, disp, 0);
                lclaura.img = aura;
                lclaura.orgn = new Vector2(aura.Width / 2, aura.Height / 2);
                lclaura.scale = new Vector2(lclscale, lclscale);

                lstParticles.Add(part);
                lstParticles.Add(lclaura);
            }
        }

        //------------------------------------------------------------
*/
}
