using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetricSPlat_Kinect
{
    class TPreProcessedDimension : List<TPreProcessedAttribute>
{
   bool bNormalized;
   String sDimensionName;
   int iDimensionIndex;

   float fHighValue;
   float fLowValue;
   float fRange;

   int iParallelCoordsPosX;
   int iScatterPlotsPosX;
   int iScatterPlotsPosY;
   
   public String DimensionName{
       get{return sDimensionName;}
   }
   public int DimensionIndex{
       get { return iDimensionIndex; }
   }
   public float HighValue{
       get { return fHighValue; }
   }
   public float LowValue{
       get { return fLowValue; }
   }
   int StringLexValue(String sAString){
      int iSum1 = 0;          int iSum2 = 0;
      String sValue = "";
      for(int iCounter = 1; iCounter <= sAString.Length; iCounter++){
        iSum1 += iCounter*sAString[iCounter]*sAString[iCounter];
        iSum2 += sAString[iCounter];
      }

      sValue = Convert.ToString(iSum1) + Convert.ToString(iSum2);
      return Convert.ToInt16(sValue.Substring(0,6));
   }

   public TPreProcessedDimension(String sADimensionName, int iADimensionIndex) : base()
   {
       bNormalized = false;
       this.iDimensionIndex = iADimensionIndex;
       this.sDimensionName = sADimensionName;
       /*Propriedades que receberão atribuição apenas no método Init*/
       this.iParallelCoordsPosX = 0;
       this.iScatterPlotsPosX = 0;
       this.iScatterPlotsPosY = 0;

       this.fHighValue = 0;
       this.fLowValue = 0;
       this.fRange = 0;
   }
   public void Init(int iAParallelCoordsPosX, int iAScatterPlotsPosX, int iAScatterPlotsPosY)
   {
       this.iParallelCoordsPosX = iAParallelCoordsPosX;
       this.iScatterPlotsPosX = iAScatterPlotsPosX;
       this.iScatterPlotsPosY = iAScatterPlotsPosY;
   }
   public float GetUnormalizedValue(float fAValue)
   {
       return (fAValue * this.fRange) + this.fLowValue;
   }

   public void AddTextData(String sAUnormalizedTextValue)
   {
        bNormalized = false;
        try{
          Convert.ToDouble(sAUnormalizedTextValue);
          this.Add(new TPreProcessedAttribute((float)Convert.ToDouble(sAUnormalizedTextValue)));
        }catch(Exception){
          this.Add(new TPreProcessedAttribute(this.Count));    
        }
   }

   public float GetUnormalizedValue(int iAValueIndex)
   {
        TPreProcessedAttribute ppaTemp = this[iAValueIndex];
        return GetUnormalizedValue(ppaTemp.fValue);
   }
   public float GetValue(int iAValueIndex)
   {
       TPreProcessedAttribute ppaTemp = (TPreProcessedAttribute)this[iAValueIndex];
       return ppaTemp.fValue;
   }
   public void Normalize(bool bSearchForBoundaries = true)
   {
       if (bNormalized)
           return;
       TPreProcessedAttribute ppaTemp;
       float fNormalizedValue = 0;

       if (bSearchForBoundaries)
       {
           ppaTemp = this[0];
           fHighValue = ppaTemp.fValue;
           fLowValue = ppaTemp.fValue;

           /*Percorre os demais valores buscando os maiores e menores valores*/
           for (int iCounter = 1; iCounter < this.Count; iCounter++)
           {
               ppaTemp = this[iCounter];
               if (ppaTemp.fValue > fHighValue)
                   this.fHighValue = ppaTemp.fValue;

               if (ppaTemp.fValue < fLowValue)
                   this.fLowValue = ppaTemp.fValue;
           }
       }

       /*Diferença entre o maior e o menor valor da dimensão*/
       this.fRange = this.fHighValue - this.fLowValue;
       if (this.fRange == 0)
           this.fRange = 1;

       /*Neste ponto já temos os maiores e menores valores da dimensão, podemos realizar
         a normalização propriamente dita*/
       for (int iCounter = 0; iCounter < this.Count; iCounter++)
       {
           ppaTemp = this[iCounter];
           /*É posível que a aproximação da conta abaixo gere valores ligeiramente acima de 1, usamos min()*/
           fNormalizedValue = Math.Min(((ppaTemp.fValue - this.fLowValue) / this.fRange), 1.0f);
           ppaTemp.fValue = fNormalizedValue;
       }
   }
   public void SetSpecificRange(float fALowValue, float fAHighValue)
   {
       fHighValue = fAHighValue;
       fLowValue = fALowValue;
   }
   public String GetStringOfGivenValue(float fAValue)
   {
       return Convert.ToString(fAValue);
   }
};
}
