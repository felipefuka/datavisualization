using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsGL.OpenGL;

namespace MetricSPlat_Kinect
{
class TOpenGLVertex{
   float glfColorR;  float glfColorG;   float glfColorB;
   float glfX; float glfY; float glfZ;

   float fPointSize;
   uint iId;
   public float X
        {
            get { return glfX; }
            set { glfX = value; }
        }
   public float Y
        {
            get { return glfY; }
            set { glfY = value; }
        }
    
   public float Z
        {
            get { return glfZ; }
            set { glfZ = value; }
        }
   public float PointSize
        {
            set { fPointSize = value; }
        }   
   public uint Id
        {
            get { return iId; }
        }
   public TOpenGLVertex(uint iAnId)
   {
       glfX = 0; glfY = 0; glfZ = 0;
       glfColorR = 0; glfColorG = 0; glfColorB = 1;
       fPointSize = 4;
       iId = iAnId;
   }
   public TOpenGLVertex(float glfAX, float glfAY, float glfAZ, uint iAnId)
{
       glfX = glfAX;   glfY = glfAY;   glfZ = glfAZ;
       glfColorR = 0;   glfColorG = 0;   glfColorB = 1;
       fPointSize = 0.035f;
       iId = iAnId;
}
   void SetCoordinates(float glfAX, float glfAY, float glfAZ)
   {
       glfX = glfAX;
       glfY = glfAY;
       glfZ = glfAZ;
   }
   float GetCoordinate(int iAIndex)
   {
       if (iAIndex == 0)
       {
           return glfX;
       }
       else if (iAIndex == 1)
       {
           return glfY;
       }
       else
       {
           return glfZ;
       }
   }
   void SetCoordinate(int iAIndex, float glfACoordinate)
   {
       if (iAIndex == 0)
       {
           glfX = glfACoordinate;
       }
       else if (iAIndex == 1)
       {
           glfY = glfACoordinate;
       }
       else if (iAIndex == 2)
       {
           glfZ = glfACoordinate;
       }
   }
   public void Call(bool bMode3D = false, bool bSelected = false, int iDisplayListIndex = 0)
   {
       if (bMode3D)
           Call3D(bSelected, iDisplayListIndex);
       else
           Call2D(bSelected);
   }
   public void Call2D(bool bSelected)
   {
       if (bSelected)
           GL.glPointSize(fPointSize * 2);
       else
           GL.glPointSize(fPointSize);

       GL.glBegin(GL.GL_POINTS);
       GL.glVertex3f(glfX, glfY, 0.0f);
       GL.glEnd();
   }
   public void Call3D(bool bSelected, int iDisplayListIndex = 0)
   {
       if (bSelected)
       {
           GL.glPointSize(fPointSize * 2);
       }
       else
       {
           GL.glPointSize(fPointSize);
       }

       GL.glBegin(GL.GL_POINTS);

       GL.glVertex3f(glfX, glfY, glfZ);
       GL.glEnd();
   }
   void SetColor(float glfAColorR, float glfAColorG, float glfAColorB)
   {
       glfColorR = glfAColorR; glfColorG = glfAColorG; glfColorB = glfAColorB;
   }
  
   public void CallGLVertex3D(){
       GL.glVertex3f(glfX, glfY, glfZ);
   }
};

}
