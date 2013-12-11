using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;
using Microsoft.Samples.Kinect.WpfViewers;
using System.Threading;
using SharpGL;
using MetricSPlat_Kinect.Kinect;

namespace MetricSPlat_Kinect
{
    partial class MainWindow : Window
    {
        TFastmapProjectionWPF fmProjectionView;
        MetricSPlat_Kinect.Kinect.VRPNButton vrpnButton;
       
        public MainWindow()
        {
            fmProjectionView = null;
                        InitializeComponent();
            //Clo
        }

        public SharpGL.WPF.OpenGLControl OpenGLProjectionControl
        {
            get { return openGLProjectionControl; }
        }

        private void Form1_Closing()
        {
            System.Environment.Exit(0);

        }

        bool closing = false;
        const int skeletonCount = 6;
        Skeleton[] allSkeletons = new Skeleton[skeletonCount];
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            kinectSensorChooser1.KinectSensorChanged += new DependencyPropertyChangedEventHandler(kinectSensorChooser1_KinectSensorChanged);
            
            TPreProcessedData pData = new TPreProcessedData();
            // Get the current directory. 
            //string path = Directory.GetCurrentDirectory();
            //string target = @"c:\temp";
            //System.Windows.Forms.MessageBox.Show("The current directory is {0}", path);
            pData.SetTextFile("C:\\Users\\piderman\\Desktop\\IC\\IC Visualizacao de Dados\\C#\\Public\\Visualizacao de Dados\\Main\\Cancer.txt");
            fmProjectionView = new TFastmapProjectionWPF(this);
            vrpnButton = new VRPNButton("localhost", "Button0", fmProjectionView);
            track = new VRPNTracker("localhost", "Tracker0", fmProjectionView, vrpnButton, this);
            fmProjectionView.PreProcessedData = pData;
            fmProjectionView.Screen = this;
            fmProjectionView.PerformMapping(); 
            Thread vrpnthread = new Thread(vrpnButton.StartDetection);
            vrpnthread.Start();
           


        }

        void kinectSensorChooser1_KinectSensorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            KinectSensor old = (KinectSensor)e.OldValue;

            StopKinect(old);

            KinectSensor sensor = (KinectSensor)e.NewValue;

            if (sensor == null)
            {
                return;
            }

            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.3f,
                Correction = 0.0f,
                Prediction = 0.0f,
                JitterRadius = 1.0f,
                MaxDeviationRadius = 0.5f
            };
            sensor.SkeletonStream.Enable(parameters);

            sensor.SkeletonStream.Enable();

            sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(sensor_AllFramesReady);
            sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

            try
            {
                sensor.Start();
            }
            catch (System.IO.IOException)
            {
                kinectSensorChooser1.AppConflictOccurred();
            }
        }

        void sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            if (closing)
            {
                return;
            }

            //Get a skeleton
            Skeleton first = GetFirstSkeleton(e);

            if (first == null)
            {
                return;
            }



            //set scaled position
            //ScalePosition(headImage, first.Joints[JointType.Head]);

        }

        void GetCameraPoint(Skeleton first, AllFramesReadyEventArgs e)
        {

            using (DepthImageFrame depth = e.OpenDepthImageFrame())
            {
                if (depth == null ||
                    kinectSensorChooser1.Kinect == null)
                {
                    return;
                }


                //Map a joint location to a point on the depth map
                //head
                DepthImagePoint headDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.Head].Position);
                //left hand
                DepthImagePoint leftDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.HandLeft].Position);
                //right hand
                DepthImagePoint rightDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.HandRight].Position);


                //Map a depth point to a point on the color image
                //head
                ColorImagePoint headColorPoint =
                    depth.MapToColorImagePoint(headDepthPoint.X, headDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left hand
                ColorImagePoint leftColorPoint =
                    depth.MapToColorImagePoint(leftDepthPoint.X, leftDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //right hand
                ColorImagePoint rightColorPoint =
                    depth.MapToColorImagePoint(rightDepthPoint.X, rightDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);


                //Set location

             //   if (fmProjectionView != null)
                    //fmProjectionView.ProcessHandMove(rightColorPoint.X, rightColorPoint.Y);
            }
        }


        Skeleton GetFirstSkeleton(AllFramesReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null)
                {
                    return null;
                }


                skeletonFrameData.CopySkeletonDataTo(allSkeletons);

                //get the first tracked skeleton
                Skeleton first = (from s in allSkeletons
                                  where s.TrackingState == SkeletonTrackingState.Tracked
                                  select s).FirstOrDefault();

                return first;

            }
        }

        private void StopKinect(KinectSensor sensor)
        {
            if (sensor != null)
            {
                if (sensor.IsRunning)
                {
                    //stop sensor 
                    sensor.Stop();

                    //stop audio if not null
                    if (sensor.AudioSource != null)
                    {
                        sensor.AudioSource.Stop();
                    }


                }
            }
        }

        private void CameraPosition(FrameworkElement element, ColorImagePoint point)
        {
            //Divide by 2 for width and height so point is right in the middle 
            // instead of in top/left corner
            Canvas.SetLeft(element, point.X - element.Width / 2);
            Canvas.SetTop(element, point.Y - element.Height / 2);

        }

        private void ScalePosition(FrameworkElement element, Joint joint)
        {
            //convert the value to X/Y
            //Joint scaledJoint = joint.ScaleTo(1280, 720); 

            //convert & scale (.3 = means 1/3 of joint distance)
            Joint scaledJoint = joint.ScaleTo(1280, 720, .3f, .3f);

            Canvas.SetLeft(element, scaledJoint.Position.X);
            Canvas.SetTop(element, scaledJoint.Position.Y);

        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closing = true;
            StopKinect(kinectSensorChooser1.Kinect);
            Form1_Closing();
        }

        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.OpenGLEventArgs args)
        {
            //  Get the OpenGL instance that's been passed to us.
            OpenGL gl = args.OpenGL;
            fmProjectionView.glDraw();
        }

        private void OpenGLFirstControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            if (fmProjectionView != null)
                fmProjectionView.InitGLContext();
        }

        private void OpenGLFirstControl_Resized(object sender, OpenGLEventArgs args)
        {
            if (fmProjectionView != null)
                fmProjectionView.Resize();
        }

       // private void openGLProjectionControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
       // {
           // if (fmProjectionView != null)
              //  fmProjectionView.ProcessMouseMove(null, e);
       // }
        //rotate something
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.vrpnButton.atualizaVetores(1, 1);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.vrpnButton.atualizaVetores(2, 1);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            this.vrpnButton.atualizaVetores(3, 2);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            this.vrpnButton.atualizaVetores(4, 2);
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            this.vrpnButton.atualizaVetores(5, 3);

            /*Colocar menubar*/
            /*Verificar na solucao em C++, como usar o mouse para rotacionar o ambiente*/
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            this.vrpnButton.atualizaVetores(6, 3);
        }



        internal VRPNTracker track { get; set; }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            /*File  dialog*/
            //Stream myStream = null;
            //OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            //openFileDialog1.FilterIndex = 2;
            //openFileDialog1.RestoreDirectory = true;

            //if (openFileDialog1.ShowDialog() == DialogResult.Value)
            //{
            //    try
            //    {
            //        if ((myStream = openFileDialog1.OpenFile()) != null)
            //        {
            //            using (myStream)
            //            {
            //                // Insert code to read the stream here.
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        System.Windows.Forms.MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            //    }
            //}
        }

        private void openGLProjectionControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
           // fmProjectionView.MouseMoveProcess(sender, (int)e.GetPosition(null).X, (int)e.GetPosition(null).Y);
            fmProjectionView.MouseMoveProcess(sender, e);

            fmProjectionView.glDraw();
           // this.vrpnButton.atualizaVetores(1, 1);
        }

        private void openGLProjectionControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void button5_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Cancer"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                //System.Windows.Forms.MessageBox.Show(filename);
                TPreProcessedData pData = new TPreProcessedData();
                pData.SetTextFile(filename);
                fmProjectionView = new TFastmapProjectionWPF(this);
                vrpnButton = new VRPNButton("localhost", "Button0", fmProjectionView);
                track = new VRPNTracker("localhost", "Tracker0", fmProjectionView, vrpnButton, this);
                fmProjectionView.PreProcessedData = pData;
                fmProjectionView.Screen = this;
                fmProjectionView.PerformMapping();
                Thread vrpnthread = new Thread(vrpnButton.StartDetection);
                vrpnthread.Start();
            }
        }

        private void openGLProjectionControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void openGLProjectionControl_KeyDown_1(object sender, System.Windows.Input.KeyEventArgs e1, System.Windows.Input.MouseEventArgs e)
        {
           
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Control move: gira apenas em torno do eixo X\nAlt move: gira apenas em torno do eixo Y\nShift move: aumenta e diminui o zoom\nTranslação: clicar e arrastar");
        }
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("                    Visualização de Dados            \nAluno: Felipe Djun Fukakusa da Silva\nOrientador: Prof. Jose Fernando Rodrigues Jr.");
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
