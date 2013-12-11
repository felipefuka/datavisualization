using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetricSPlat_Kinect
{
class TPreProcessedRecord : List<TPreProcessedAttribute>{
   String sLabel;
   int iRecordIndex;
   TOpenGLColor oglcColor;

   public int Index
   {
       get{return iRecordIndex;}
   }
   public String Label{
       get{return GetLabel();}
       set{sLabel = value;}
   }
   public TOpenGLColor Color{
       get{return oglcColor;}
   }
   public TPreProcessedRecord(List<TPreProcessedAttribute> lListOfPreprocessedAttributes, int iARecordIndex)
   {
       oglcColor = new TOpenGLColor();
       //Muda cor para maligno ou benigno
      if (lListOfPreprocessedAttributes[10].Value == 1) oglcColor.SetColor(1.0f, 0.0f, 0.0f);
     else oglcColor.SetColor(0.0f,1.0f,0.0f);
       for (int iCounter = 0; iCounter < lListOfPreprocessedAttributes.Count; iCounter++){
          this.Add(lListOfPreprocessedAttributes[iCounter]);
       }
       iRecordIndex = iARecordIndex;
       sLabel = "";
   }
   public TPreProcessedRecord(int iARecordIndex)
   {
       oglcColor = new TOpenGLColor();
       oglcColor.SetColor(1.0f, 0.0f, 0.0f);
       iRecordIndex = iARecordIndex;
       sLabel = "";
   }

   public TPreProcessedAttribute GetAttrib(int iDimensionIndex)
   {
       return this[iDimensionIndex];
   }
   public TPreProcessedRecord Clone()
   {
       TPreProcessedRecord pprClone = new TPreProcessedRecord(this.Index);
       pprClone.iRecordIndex = this.iRecordIndex;
       pprClone.oglcColor = this.oglcColor;

       for(int iCounter = 0;iCounter < this.Count; iCounter++){
          pprClone.Add(this.GetAttrib(iCounter));
       }
       return pprClone;
   }
   public float Get(int iDimensionIndex)
   {
       return (this[iDimensionIndex]).Value;
   }
   public String GetLabel()
   {
       if (sLabel == "")
           sLabel = Convert.ToString(iRecordIndex);
       return sLabel;
   }
   public void SetColor(float glAC1, float glAC2, float glAC3)
   {
       oglcColor.SetColor(glAC1, glAC2, glAC3);
   }
   /*Este método é a ponte entre a VisTree e o FastMapper da arboretum. Ele converte
     um TPreprocessedRecord para um stBasicArrayObject <float, int>*/
   //stBasicArrayObject <float, int>* GetBasicArrayObject();
};
}
