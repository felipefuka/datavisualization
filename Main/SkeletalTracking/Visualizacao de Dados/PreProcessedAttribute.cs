using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetricSPlat_Kinect
{
class TPreProcessedAttribute{
   public float fValue;
   public float Value{
       get {return fValue;}
   }
   public TPreProcessedAttribute(float fAValue){
       fValue = fAValue;
   }
};
}
