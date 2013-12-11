using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vrpn;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows;

namespace MetricSPlat_Kinect.Kinect
{
    class VRPNTracker
    {
        // Constantes das posicoes do tracker
        const int MAO_DIREITA = 14;

        public TrackerRemote tracker;
        public VRPNButton button;
        TFastmapProjectionWPF fastmap;
        private double lastX, lastY, lastZ;
        private Window window;
        double smoothFactor;
        public VRPNTracker(String endereco, String trackerString, TFastmapProjectionWPF janelaFast, VRPNButton buttonArg, Window win)
        {
            this.button = buttonArg;
            this.window = win;

            tracker = new TrackerRemote("Tracker0@localhost");
            tracker.PositionChanged += new TrackerChangeEventHandler(mudouPosicao);
            win.MouseMove += new System.Windows.Input.MouseEventHandler(mudouPosicaoMouse);
            tracker.MuteWarnings = true;
            fastmap = janelaFast;
            this.tracker.Update();
            lastX = 0;
            lastY = 0;
            smoothFactor = 0.004;


        }


        private void mudouPosicao(object sender, TrackerChangeEventArgs e)
        {
            //Realiza a translacao baseado na posicao da mao direita

            if (button.botao[7] && (e.Sensor == MAO_DIREITA))
            {
                if (this.lastX == 0 && this.lastY == 0)
                {
                    this.lastX = e.Position.X;
                    this.lastY = e.Position.Y;
                
                }
                if (lastX > e.Position.X + smoothFactor) this.fastmap.ProcessTransX(true);
                if (lastX < e.Position.X - smoothFactor) this.fastmap.ProcessTransX(false);
                if (lastY > e.Position.Y + smoothFactor) this.fastmap.ProcessTransY(true);
                if (lastY < e.Position.Y - smoothFactor) this.fastmap.ProcessTransY(false);
                lastX = e.Position.X;
                lastY = e.Position.Y;
            
            }
        }

        private void mudouPosicaoMouse(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //Realiza a translacao baseado na posicao da mao direita

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (lastX > e.GetPosition(null).X) this.fastmap.ProcessTransX(false);
                if (lastX < e.GetPosition(null).X) this.fastmap.ProcessTransX(true);
                if (lastY > e.GetPosition(null).Y) this.fastmap.ProcessTransY(true);
                if (lastY < e.GetPosition(null).Y) this.fastmap.ProcessTransY(false);
                lastX = e.GetPosition(null).X;
                lastY = e.GetPosition(null).Y;

            }
        }
    }
}
