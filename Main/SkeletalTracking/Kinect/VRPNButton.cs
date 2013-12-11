using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vrpn;
using System.Threading;

namespace MetricSPlat_Kinect.Kinect
{
    class VRPNButton
    {

        public ButtonRemote button;
        public bool[] botao, pausa;
        TFastmapProjectionWPF fastmap;

        public VRPNButton(String endereco, String tracker, TFastmapProjectionWPF janelaFast)
        {
            botao = new bool[8];
            button = new ButtonRemote("Button0@localhost");

            button.ButtonChanged += new ButtonChangeEventHandler(verificaBotao);
            button.MuteWarnings = true;
            fastmap = janelaFast;
            pausa = new bool[7];


        }

        public void verificaBotao(object sender, ButtonChangeEventArgs e)
        {
            {
                if (e.Button <= 2) atualizaVetores(e.Button, 1);
                else if (e.Button <= 4) atualizaVetores(e.Button, 2);
                else if (e.Button <= 6) atualizaVetores(e.Button, 3);
                else if (e.Button == 7) atualizaVetores(e.Button, 4);
            }
        }

        public void Botao1()
        {
            if (this.botao[1] && !this.botao[2])
            {
                Console.WriteLine("BOTAO 1");
                this.botao[1] = false;
                fastmap.ProcessRotY(false);
                if (this.pausa[2] == false) this.botao[1] = true;
                //Console.WriteLine("{0} {1} {2}", e.Time, e.Button, e.IsPressed ? "pressed" : "released");
            }
        }

        public void Botao2()
        {
            if (this.botao[2] && !this.botao[1])
            {
                Console.WriteLine("BOTAO 2");
                this.botao[2] = false;
                fastmap.ProcessRotY(true);
                if (this.pausa[1] == false) this.botao[2] = true;
                //Console.WriteLine("{0} {1} {2}", e.Time, e.Button, e.IsPressed ? "pressed" : "released");
            }

        }

        public void Botao3()
        {
            if (this.botao[3])
            {
                Console.WriteLine("BOTAO 3");
                this.botao[3] = false;
                fastmap.ProcessRotX(false);
                if (this.pausa[4] == false) this.botao[3] = true;
                //Console.WriteLine("{0} {1} {2}", e.Time, e.Button, e.IsPressed ? "pressed" : "released");
            }

        }

        public void Botao4()
        {
            if (this.botao[4])
            {
                Console.WriteLine("BOTAO 4");
                this.botao[4] = false;
                fastmap.ProcessRotX(true);
                if (this.pausa[3] == false) this.botao[4] = true;
                //Console.WriteLine("{0} {1} {2}", e.Time, e.Button, e.IsPressed ? "pressed" : "released");
            }

        }

        public void Botao5()
        {
            if (this.botao[5])
            {
                Console.WriteLine("BOTAO 5");
                this.botao[5] = false;
                fastmap.ProcessEscala(false);
                if (this.pausa[6] == false) this.botao[5] = true;
                //Console.WriteLine("{0} {1} {2}", e.Time, e.Button, e.IsPressed ? "pressed" : "released");
            }

        }

        public void Botao6()
        {
            if (this.botao[6])
            {
                Console.WriteLine("BOTAO 6");
                this.botao[6] = false;
                fastmap.ProcessEscala(true);
                if (this.pausa[5] == false) this.botao[6] = true;
                //Console.WriteLine("{0} {1} {2}", e.Time, e.Button, e.IsPressed ? "pressed" : "released");
            }

        }

        public void atualizaVetores(int botao, int pausa)
        {
            if (this.botao[botao] == true)
            {
                this.botao[botao] = false;
            }
            else
            {
                switch (botao)
                {
                    case 1:
                        if (!this.botao[2]) this.botao[botao] = true;
                        break;

                    case 2:
                        if (!this.botao[1]) this.botao[botao] = true;
                        break;

                    case 3:
                        if (!this.botao[4]) this.botao[botao] = true;
                        break;

                    case 4:
                        if (!this.botao[3]) this.botao[botao] = true;
                        break;

                    case 5:
                        if (!this.botao[6]) this.botao[botao] = true;
                        break;

                    case 6:
                        if (!this.botao[5]) this.botao[botao] = true;
                        break;

                    case 7:
                        this.botao[botao] = true;
                        break;
                }
            }

        }



        public void StartDetection()
        {
            while (true)
            {
                this.ChamaThreads(null);
                Thread.Sleep(1);
            }
        }

        private void ChamaThreads(Object stateInfo)
        {
            this.button.Update();
            Thread t1 = new Thread(this.Botao1);
            t1.Start();
            Thread t2 = new Thread(this.Botao2);
            t2.Start();
            Thread t3 = new Thread(this.Botao3);
            t3.Start();
            Thread t4 = new Thread(this.Botao4);
            t4.Start();
            Thread t5 = new Thread(this.Botao5);
            t5.Start();
            Thread t6 = new Thread(this.Botao6);
            t6.Start();
        }           
    }


}

