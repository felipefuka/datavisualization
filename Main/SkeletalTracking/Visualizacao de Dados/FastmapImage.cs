using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetricSPlat_Kinect
{
class TFastMapImage{
      int Dim;
      float[] Coords;
      TPreProcessedRecord Object;
      bool Mine;

      public TFastMapImage(TPreProcessedRecord obj, int dim, bool ownership = false){
           Object = obj;
           Mine = ownership;
           Dim = dim;
           Coords = new float[dim];
      }
      public TPreProcessedRecord GetObject(){
          return Object;
      }
      public void SetObject(TPreProcessedRecord obj, bool ownership = false){
           if ((Object != null) && (Mine)){
              Object = null;
           }//end if

           Object = obj;
           Mine = ownership;        
      }
      public float GetIthValue(int idx){
        return Coords[idx];
      }
      public void SetIthValue(int idx, float aValue)
      {
          Coords[idx] = aValue;
      }


      public void GetCoords(float[] coords){
        for(int i = 0; i< Dim; i++)
            coords[i] = Coords[i];
      }

      public float[] GetCoords(){
          return Coords;
      }
      public void SetCoords(float[] coords){
        for(int i = 0; i< Dim; i++)
            Coords[i] = coords[i];
      }
      public void SetCoordsTo(float value = 0)
      {
          for (int i = 0; i < Dim; i++)
              Coords[i] = value;
      }
      public int GetDimension()
      {
          return Dim;
      }
      public TFastMapImage Clone()
      {
          TFastMapImage c;

          c = new TFastMapImage(Object.Clone(), Dim, true);
          c.SetCoords(Coords);
          return c;
      }



};
}
