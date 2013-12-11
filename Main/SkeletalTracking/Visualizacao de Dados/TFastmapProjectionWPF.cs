using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsGL.OpenGL;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows;
using System.Collections;

namespace MetricSPlat_Kinect
{
    class TFastmapProjectionWPF
    {
        int iCurrentResolution;
        TPreProcessedData ppdData;
        int iSpaceWidth;
        int iSpaceHeight;
        int iLastX, iLastY, Y, X;
        bool jadesenhei;
        public Queue filaacoes;

        public TPreProcessedData PreProcessedData
        {
            get { return ppdData; }
            set
            {
                SetData(value);
            }
        }
        MainWindow sScreen;
        public MainWindow Screen
        {
            get { return sScreen; }
            set
            {
                sScreen = value;
                iSpaceWidth = (int)Math.Max(this.ppdData.iRecordsNumber, sScreen.OpenGLProjectionControl.Width);
                iSpaceHeight = (int)Math.Max(this.ppdData.iRecordsNumber, sScreen.OpenGLProjectionControl.Height);
            }
        }
        public bool Mode3D { get { return bMode3D; } }
        /*
       public float OpenGLAreaHorizontalRange { get{ return fOpenGLAreaHorizontalRange;};
       public float OpenGLAreaVerticalRange { get{ return fOpenGLAreaVerticalRange;};
       public int SelectedElementId { get{ return GetSelectedElementId;};
       public enum ENUM_panOperation ChoseOperation { get{ return paChoseOperation;};
    */
        //HGLRC hgCurrentContext;
        bool bPickingOnProcess;
        int iSphereDisplayList;
        int iTargetDimension;
        float fPointSize;
        int hits; uint[] iPickHitsbuffer;
        int[] view;
        int iScreenX, iScreenY;

        /*Limites de projeção*/
        float fMinX, fMaxX, fMinY, fMaxY, fMinZ, fMaxZ;
        float fAbsoluteMax;
        /*Faz a translação da origem do sistema, portanto, estas são as coordenadas do elemento central*/
        float fTransX, fTransY, fTransZ;
        float fRotAroundY, fRotAroundX;
        float fScale;

        //GL.GLUquadricObj gqoCenter;
        float fOpenGLAreaVerticalRange;
        float fOpenGLAreaHorizontalRange;

        bool bMode3D;
        bool bDrawTarget;
        bool bMouseDown;
        TFastmapPointList FastmapPointList;

        Auxiliar.ENUM_panOperation paChoseOperation;

        public TFastmapProjectionWPF(Window aWindow)       
        {
            iCurrentResolution = 0;
            sScreen = null;
            //this.KeyDown += new KeyEventHandler(OurView_OnKeyDown);
            iTargetDimension = 3;
            //Screen.Cursors[crRotateCursor] = LoadCursor(HInstance, "ROTATECURSOR");
            //Screen.Cursors[crPan] = LoadCursor(HInstance, "PAN_256");
            //Screen.Cursors[crZoomIn] = LoadCursor(HInstance, "ZOOM_IN_256");

            //formSetPointSize = NULL;
            fPointSize = 5.0f;

            paChoseOperation = Auxiliar.ENUM_panOperation.paRotate;

            FastmapPointList = new TFastmapPointList();
            FastmapPointList.TargetDimension = iTargetDimension;
            bMode3D = true;
            bMouseDown = false;
            bDrawTarget = true;
            //gqoCenter = NULL;

            fMinX = -10; fMaxX = 10; fMinY = -10; fMaxY = 10; fMinZ = -10; fMaxZ = 10;
            fTransX = 0; fTransY = 0; fTransZ = 0;
            fRotAroundY = 10; fRotAroundX = 10;
            fScale = 1;
            iLastX = 0; iLastY = 0;
            bPickingOnProcess = false;
            filaacoes = new Queue();
            
            //hgCurrentContext = NULL;

            //FDrawGraphFunction = NULL;
            //this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ProcessMouseMove);
        }
        public void glDraw()
        {
                Init();
                Luzes();
                //FastmapPointList.AutoDraw(iCurrentResolution);
                FastmapPointList.AutoDrawOctree(iCurrentResolution);

                DrawTarget();
                GL.glFlush();
            // TODO: Draw a box or something
            //this.SwapBuffer(); // swap Buffers

        }
        bool SetData(TPreProcessedData ppdaPreProcessedData)
        {
            this.SetSpacialData();
            if (ppdaPreProcessedData == null)
                return false;
            if (ppdData != null)
                ppdData = null;
            this.ppdData = ppdaPreProcessedData;
            this.FastmapPointList.Init(this, this.ppdData);
            return true;
        }
      //MEUS METODOS - UTILIZAR COM VRPN

        public void ProcessTransX(bool ladoEsquerdo)
        {
            if (ladoEsquerdo)
                fTransX -= (float)0.08;
            else
                fTransX += (float)0.08;
        }

        public void ProcessTransY(bool paraCima)
        {
            if (paraCima)
                fTransY -= (float)0.08;
            else
                fTransY += (float)0.08;
        }
        
        public void ProcessRotX(bool ladoEsquerdo)
        {
            if (ladoEsquerdo)
                fRotAroundX -= (float) 0.3;
            else
                fRotAroundX += (float)0.3;
        }

        public void ProcessRotY(bool paraBaixo)
        {
            if (paraBaixo)
                fRotAroundY -= (float) 0.3;
            else
                fRotAroundY += (float )0.3;
        }

        public void ProcessEscala(bool diminuir)
        {
            if (diminuir && (fScale > 0) ) fScale -= 0.006f;
            else if (fScale < 10) fScale += 0.006f;

        }

        //FIM MEUS METODOS

        void SetSpacialData()
        {
            int iSmallerDimension = (int)Math.Truncate((double)((Math.Min(this.iSpaceWidth, this.iSpaceHeight)) * 0.8));

            fOpenGLAreaVerticalRange = this.iSpaceHeight / 2;
            fOpenGLAreaHorizontalRange = this.iSpaceWidth / 2;
        }
        void Init()
        {
            GL.glClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);

            GL.glViewport(0, 0, (int)sScreen.openGLProjectionControl.Width, (int)sScreen.openGLProjectionControl.Height);

            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();

            if (bPickingOnProcess)
            {
                /*Defini-se uma area de 5x5 pixels para selecao de pontos do fastmap*/
                GL.gluPickMatrix(iScreenX, iScreenY, 5, 5, view);
            }

            GL.gluPerspective(60.0, (int)sScreen.openGLProjectionControl.Width / (int)sScreen.openGLProjectionControl.Height, 0.0, 2 * fAbsoluteMax);

            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();
            GL.gluLookAt(0 + fTransX * fAbsoluteMax, 0 + fTransY * fAbsoluteMax, 2 * fAbsoluteMax,
                      0 + fTransX * fAbsoluteMax, 0 + fTransY * fAbsoluteMax, 0,
                      0, 1, 0);
            GL.glRotatef(fRotAroundY, 0.0f, 1.0f, 0.0f);
            GL.glRotatef(fRotAroundX, 1.0f, 0.0f, 0.0f);
            GL.glScalef(fScale, fScale, fScale);

         
        }
        void Luzes()
        {
            if (bMode3D)
            {
                float[] light = { 1.0f, 1.0f, 1.0f, 1.0f };
                float[] mat_shininess = { 40.0f };
                float[] position1 = { 0.0f, 0.0f, -2 * fAbsoluteMax, 1.0f };

                GL.glEnable(GL.GL_LIGHTING);
                GL.glEnable(GL.GL_LIGHT0);

                GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, position1);
                GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, light);
                GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, light);
                GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, light);

                GL.glMaterialfv(GL.GL_FRONT, GL.GL_SHININESS, mat_shininess);
            }
            else
            {
                GL.glDisable(GL.GL_LIGHTING);
            }
        }
        protected void OurView_OnKeyDown(object Sender, System.Windows.Forms.KeyEventArgs kea)
        {
            if (kea.KeyCode == Keys.Q && kea.Modifiers == Keys.Shift)
            {
                //Application.Exit();
            }
        }
        public void InitGLContext()
        {
            GL.glShadeModel(GL.GL_SMOOTH);
            // Set Smooth Shading 
            GL.glClearColor(0.0f, 0.0f, 0.0f, 0.5f);
            // BackGround Color 
            GL.glClearDepth(1.0f);
            // Depth buffer setup 
            GL.glEnable(GL.GL_DEPTH_TEST);
            // Enables Depth Testing 
            GL.glDepthFunc(GL.GL_LEQUAL);
            // The Type Of Depth Test To Do 
            GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_HINT, GL.GL_NICEST);
            /* Really Nice Perspective Calculations */
        }
        public void Resize()
        {
            System.Drawing.Size s = new System.Drawing.Size((int)sScreen.openGLProjectionControl.Width, (int)sScreen.openGLProjectionControl.Height);
            double aspect_ratio = (double)s.Width / (double)s.Height;
            GL.glMatrixMode(GL.GL_PROJECTION); // Select The Projection Matrix
            GL.glLoadIdentity(); // Reset The Projection Matrix
            // Calculate The Aspect Ratio Of The Window
            GL.gluPerspective(45.0f, aspect_ratio, 0.1f, 100.0f);
            GL.glMatrixMode(GL.GL_MODELVIEW); // Select The Modelview Matrix
            GL.glLoadIdentity();// Reset The Modelview Matrix
        }
        public void SetglOrtho(float fMinX, float fMaxX, float fMinY, float fMaxY, float fMinZ, float fMaxZ)
        {
            this.fMinX = fMinX; this.fMaxX = fMaxX;
            this.fMinY = fMinY; this.fMaxY = fMaxY;
            this.fMinZ = fMinZ; this.fMaxZ = fMaxZ;
            fAbsoluteMax = Math.Max(Math.Max(Math.Abs(fMaxX), Math.Max(Math.Abs(fMaxY), Math.Abs(fMaxZ))), Math.Max(Math.Abs(fMinX), Math.Max(Math.Abs(fMinY), Math.Abs(fMinZ))));
            if (fAbsoluteMax == 0)
                fAbsoluteMax = 500;
        }
        public void PerformMapping()
        {
            FastmapPointList.PerformMapping();
        }
        void DrawTarget()
        {
            /*Este comando garante que os eixos (target) nao sejam confundidos com o ultimo elemento desenhado.*/
            GL.glLoadName(1);
            GL.glDisable(GL.GL_LIGHTING);

            GL.glLineWidth(1.0f);
            //Cor do Ambiente
            GL.glColor3f(0.0f, 0.0f, 0.0f);

            float[] fLightProperties = { 0.0f, 0.0f, 0.0f, 1.0f };
            GL.glMaterialfv(GL.GL_FRONT, GL.GL_DIFFUSE, fLightProperties);
            GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, fLightProperties);
            GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, fLightProperties);

            Circle(0.0f, 0.0f, fAbsoluteMax, 1);
            Circle(0.0f, 0.0f, fAbsoluteMax, 2);
            Circle(0.0f, 0.0f, fAbsoluteMax, 3);

            GL.glBegin(GL.GL_LINES);
            GL.glVertex3f(fAbsoluteMax / 2.0f, 0.0f, 0.0f); // origin of the line
            GL.glVertex3f(-fAbsoluteMax / 2.0f, 0.0f, 0.0f); // ending point of the line
            GL.glEnd();
            GL.glRasterPos3f(fAbsoluteMax / 2.0f, 0.0f, 0.0f);
            //this->Draw2DText("X");

            GL.glBegin(GL.GL_LINES);
            GL.glVertex3f(0.0f, fAbsoluteMax / 2.0f, 0.0f); // origin of the line
            GL.glVertex3f(0.0f, -fAbsoluteMax / 2.0f, 0.0f); // ending point of the line
            GL.glEnd();
            GL.glRasterPos3f(0.0f, fAbsoluteMax / 2.0f, 0.0f);
            //this->Draw2DText("Y");

            GL.glBegin(GL.GL_LINES);
            GL.glVertex3f(0.0f, 0.0f, fAbsoluteMax / 2.0f); // origin of the line
            GL.glVertex3f(0.0f, 0.0f, -fAbsoluteMax / 2.0f); // ending point of the line
            GL.glEnd();
            GL.glRasterPos3f(0.0f, 0.0f, fAbsoluteMax / 2.0f);
            //this->Draw2DText("Z");

            GL.glEnable(GL.GL_LIGHTING);
        }
        void Circle(float x, float y, float radius, int iPlane)
        {
            float da = getAngleIncrement(radius, 0.1f);
            GL.glBegin(GL.GL_LINE_STRIP);
            for (float a = 0.0f; a <= (2 * Math.PI + da); a += da)
            {
                if (iPlane == 1)
                    GL.glVertex3f(x + (float)Math.Cos(a) * radius, y + (float)Math.Sin(a) * radius, 0.0f);
                else if (iPlane == 2)
                    GL.glVertex3f(x + (float)Math.Cos(a) * radius, 0.0f, y + (float)Math.Sin(a) * radius);
                else
                    GL.glVertex3f(0.0f, x + (float)Math.Cos(a) * radius, y + (float)Math.Sin(a) * radius);
            }
            GL.glEnd();
        }

        float getAngleIncrement(float rad, float accuracy)
        {
            rad = Math.Max(rad, 1.0f);
            return Math.Min((2.0f * (float)Math.Asin(1.0f / rad) / accuracy), 0.1f);
        }
        public void MouseMoveProcess(object sender, System.Windows.Input.MouseEventArgs e)
        { //TShiftState Shift,
            int iX = (int)e.GetPosition(null).X;
            int iY = (int)e.GetPosition(null).Y;
            iY = (int)sScreen.openGLProjectionControl.Height - iY;
            this.iScreenX = iX;
            this.iScreenY = iY;

            iY = (int)(((float)iY) / ((float)sScreen.openGLProjectionControl.Height) * ((float)iSpaceHeight));
            iX = (int)(((float)iX) / ((float)sScreen.openGLProjectionControl.Width) * ((float)iSpaceWidth));
            iY = iY - ((int)fOpenGLAreaVerticalRange);
            iX = iX - ((int)fOpenGLAreaHorizontalRange);   
           // if (bMouseDown)
          //  {
            //    if (paChoseOperation == Auxiliar.ENUM_panOperation.paRotate)
           //     {

                    /*Alt move, gira apenas em torno de Y*/
                    if (System.Windows.Input.Keyboard.Modifiers == ModifierKeys.Alt)
                    {
                        if (iX > iLastX)
                            fRotAroundY += 3;
                        else if (iX < iLastX)
                            fRotAroundY -= 3;
                    }
                    else if (System.Windows.Input.Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        if (iY > iLastY)
                            fRotAroundX -= 3;
                        else if (iY < iLastY)
                            fRotAroundX += 3;
                    }
                    else if (System.Windows.Input.Keyboard.Modifiers == ModifierKeys.Shift)
                    {
                        if (iY > iLastY)
                            fScale += 0.05f;
                        else if (iY < iLastY)
                            fScale -= 0.05f;
                    }
                    else
                    {                    
                        if (iX > iLastX)
                            fRotAroundY += 3;
                        else if (iX < iLastX)
                            fRotAroundY -= 3;
                        if (iY > iLastY)
                            fRotAroundX -= 3;
                        else if (iY < iLastY)
                            fRotAroundX += 3;
                    }
            //    }
           // }
            /*Ctrl move, gira apenas em torno de X*/
            /*  if(!Shift.Contains(ssCtrl)){
                 if(iY > iLastY)
                    fRotAroundX -= 3;
                 else if(iY < iLastY)
                    fRotAroundX += 3;
              }
           }else if(paChoseOperation == Auxiliar.ENUM_panOperation.paScale){
              if(iY > iLastY)
                 fScale += 0.1;
              else if(iY < iLastY)
                 fScale -= 0.1;
           }else if(paChoseOperation == Auxiliar.ENUM_panOperation.paTranslate){
              if(iX > iLastX)
                 fTransX -= 0.05;
              else if(iX < iLastX)
                 fTransX += 0.05;
              if(iY > iLastY)
                 fTransY -= 0.05;
              else if(iY < iLastY)
                 fTransY += 0.05;
           }*/
            iLastX = iX; iLastY = iY;
            //Paint() --> fmProjectionView.glDraw(); ;
            /* }else{
                if(paChoseOperation == paSelectionMode){
                   bPickingOnProcess = true;
                   Paint();
                   if(hits > 0){
                      int iPicked = iPickHitsbuffer[0 * 4 + 3];
                      if(iPicked >= 0 && iPicked < this->FastmapPointList->Count){
                         this->FastmapPointList->SelectPoint(iPicked);
                      }
                      hits = 0;
                   }

                   bPickingOnProcess = false;
                   Paint();
                }*/
        }


        /*
           void  MouseDownProcess(TObject *Sender,TMouseButton Button, TShiftState Shift, int iX, int iY);
           void  MouseMoveProcess(TObject *Sender, TShiftState Shift, int iX, int iY);
           void  MouseUpProcess(TObject *Sender, TMouseButton Button,TShiftState Shift, int iX, int iY);
           void  SetSpacialData();

           void  PopupMenuOnPoupup(TObject *Sender);
           void  PopupMenuOperationsChange(TObject *Sender, TMenuItem *Source, bool Rebuild);
           void  TFastmapProjection::ResetSytem();

           void  SetPointSizeFormMouseUpProcess(TObject *Sender);

           void Luzes();
           void  SetDrawMode(ENUM_drawModes dmADrawMode);
           float  GetNormalizedClickedPosition(int iX, int iY);
           void  DrawTarget();
           float getAngleIncrement(float rad, float accuracy);
           void Circle(float x, float y, float radius, int iPlane);
           int  GetSelectedElementId();
        public:		// User declarations
            TFastmapProjection(TComponent* Owner);
           bool  SetData(TPreProcessedData *ppdlaPreProcessedData);
           void  SetPointSize(float fAPointSize);

           void  CentralizeElement(int iAnElementIndex);
           void  SetDataForFastmapPivotChoosing(TList* lAListOfPreprocessedRecords);
           void  SetDrawIdLabels(bool bAnOption);
           void SetListOfIndexOfDataItemsToDrawDeterminedFromSimilarityQuery(TList* lAListOfRecordsIndexes);
        */
    }
}
