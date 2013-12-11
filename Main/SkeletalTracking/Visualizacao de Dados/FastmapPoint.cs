using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsGL.OpenGL;

namespace MetricSPlat_Kinect
{
class TFastmapPoint{
   TPreProcessedRecord pprRecord;
   Auxiliar.ENUM_drawModes dmDrawMode;
   TFastmapProjectionWPF fmProjection;


   public TPreProcessedRecord Record
   {
       get { return pprRecord; }
       set { pprRecord = value; }
   }

   public Auxiliar.ENUM_drawModes DrawMode
   {
       set { dmDrawMode = value; }
       get { return dmDrawMode; }
   }
   public float PointSize
        {
            set { oglvPoint.PointSize = value; }
        }

    public int FastmapPointIndex
    {
        get { return GetFastmapPointIndex(); }
    }

   bool bIsSelected;

   public TOpenGLVertex oglvPoint;
   public void SetPointSize(float fAPointSize)
   {
       oglvPoint.PointSize = fAPointSize;
}
   public int GetFastmapPointIndex()
   {
       return this.pprRecord.Index;
   }

   public TFastmapPoint()
   {
       dmDrawMode = Auxiliar.ENUM_drawModes.drawmodeNormal;
       bIsSelected = false;
    }

   public void Init(uint iARecordIndex, TFastmapProjectionWPF fmaProjection)
   {
       this.fmProjection = fmaProjection;
       oglvPoint = new TOpenGLVertex(iARecordIndex);
   }

   public void NormalDraw(TOpenGLColor ocAColor)
   {
       GL.glLoadName(oglvPoint.Id);
       if(bIsSelected){
          GL.glColor3f(ocAColor.C1,ocAColor.C2,ocAColor.C3);

          float[] fLightProperties = new float[4] {ocAColor.C1, ocAColor.C2, ocAColor.C3, 0.0f };
          GL.glMaterialfv(GL.GL_FRONT,  GL.GL_DIFFUSE, fLightProperties);
          GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, fLightProperties);
          //fLightProperties[0] = 1.0f/3.0f;
          GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, fLightProperties);
       }else{
          GL.glColor3f(ocAColor.C1,ocAColor.C2,ocAColor.C3);
          float[] fLightProperties = new float[4]{ocAColor.C1,ocAColor.C2,ocAColor.C3, 0.0f};
          GL.glMaterialfv(GL.GL_FRONT,  GL.GL_DIFFUSE, fLightProperties);
          GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, fLightProperties);
         // fLightProperties[2] = 1.0f/3.0f;
          GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, fLightProperties);
       }
       oglvPoint.Call(fmProjection.Mode3D, bIsSelected);
       oglvPoint.Call3D(false);
   }
   public void SelectionDraw(TOpenGLColor ocAColor)
   {
       GL.glColor3f(ocAColor.C1, ocAColor.C2, ocAColor.C3);
       float[] fLightProperties = new float[4]{ocAColor.C1, ocAColor.C2, ocAColor.C3, 1.0f };
       GL.glMaterialfv(GL.GL_FRONT,  GL.GL_DIFFUSE, fLightProperties);
       GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, fLightProperties);
       fLightProperties[0] = ocAColor.C1/3.0f;  fLightProperties[1] = ocAColor.C2/3.0f; fLightProperties[2] = ocAColor.C3/3.0f;
       GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, fLightProperties);

       GL.glLoadName(oglvPoint.Id);
       oglvPoint.Call(fmProjection.Mode3D, bIsSelected);
       oglvPoint.Call3D(false);
   }
   public void DrawIDLabel(){
       GL.glDisable(GL.GL_LIGHTING);
       GL.glColor3f(0.0f,1.0f,0.0f);
       float[] fLightProperties = new float[4] { 0.0f, 0.0f, 0.0f, 1.0f };
       GL.glMaterialfv(GL.GL_FRONT,  GL.GL_DIFFUSE, fLightProperties);
       GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, fLightProperties);
       GL.glMaterialfv(GL.GL_FRONT, GL.GL_EMISSION, fLightProperties);

       if(fmProjection.Mode3D)
           GL.glRasterPos3f((float)this.oglvPoint.X * 1.05f, (float)this.oglvPoint.Y, (float)this.oglvPoint.Z);
       else
           GL.glRasterPos3f((float)this.oglvPoint.X * 1.05f, (float)this.oglvPoint.Y, (float)0.0f);

       //this.fmProjection.Draw2DText(this.pprRecord.Label);
       GL.glEnable(GL.GL_LIGHTING);
   }

   public void CallGLVertex3D()
   {
      oglvPoint.CallGLVertex3D();
   }
};
}