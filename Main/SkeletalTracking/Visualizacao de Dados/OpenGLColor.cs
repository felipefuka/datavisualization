using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsGL.OpenGL;

namespace MetricSPlat_Kinect
{
class TOpenGLColor
{
    public float C1
        {
            get { return glC1; }
            set { glC1 = value; }
        }

    public float C2
        {
            get { return glC2; }
            set { glC2 = value; }
        }
    public float C3
        {
            get { return glC3; }
            set { glC3 = value; }
        }

   public float glC1;
   public float glC2;
   public float glC3;

   public TOpenGLColor()
   {
          glC1 = 0.0f;
    glC2 = 1.0f;
    glC3 = 0.0f;
   }
   public TOpenGLColor(float glAC1, float glAC2, float glAC3)
   {
   glC1 = glAC1;
   glC2 = glAC2;
   glC3 = glAC3;
   }

   public void SetColor(float glAC1, float glAC2, float glAC3)
   {
       glC1 = glAC1;
       glC2 = glAC2;
       glC3 = glAC3;
   }
   public void SetColor(TOpenGLColor oglcAColor)
   {
       SetColor(oglcAColor.glC1, oglcAColor.glC2, oglcAColor.glC3);
   }
   public void Call()
   {
       GL.glColor3f(glC1, glC2, glC3);
   }
   static void CallHSI(float fAHueValue, float fASaturationValue, float fAnIntensityValue)
   {
       if (fASaturationValue == 0)
       {
           GL.glColor3f(fAnIntensityValue, fAnIntensityValue, fAnIntensityValue);
           return;
       }
       GL.glColor3f(HSIValue(fAHueValue + 0.0f, fASaturationValue, fAnIntensityValue),
                 HSIValue(fAHueValue + 4.0f, fASaturationValue, fAnIntensityValue),
                 HSIValue(fAHueValue + 2.0f, fASaturationValue, fAnIntensityValue));

   }
   static void CallHSIScaleWithIntesity(float fANormalizedValue, float fAIntensity)
   {
       CallHSI(fANormalizedValue * 6, fAIntensity, 1);
   }
   static void CallHSIScale(float fANormalizedValue)
   {
       CallHSI(fANormalizedValue * 6, fANormalizedValue, fANormalizedValue);
   }
   static void CallBlueToRedIntensityScale(float fAHueCoefficient, float fAnIntensityValue)
   {
       CallHSI(4.0f + 2 * fAHueCoefficient, (float)Math.Pow(fAnIntensityValue, 2), 1.0f); 
   }

   static float HSIValue(float fAHuePhase, float fASaturationValue, float fAnIntensityValue){
    float fPure = 0.5f*(1 + (float)Math.Cos(fAHuePhase * Math.PI/3));
    return(fAnIntensityValue*(1.0f - fASaturationValue*(1.0f-fPure)));
  }
};

}
