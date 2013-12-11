using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MetricSPlat_Kinect
{
class TPreProcessedData : List<KeyValuePair<String,TPreProcessedDimension>>{
   public int iRecordsNumber;
   public int iRelevanceDimensions;
   List<TPreProcessedRecord> lRecords;

   public int RecordsNumber { get{ return iRecordsNumber;}}
   public int NumberOfDimensions { get{return GetNumberOfDims();}}
   public List<TPreProcessedRecord> ListOfPreprocessedRecords { get { return lRecords; } }

   int GetNumberOfDims(){
    return this.Count;
   }

   public TPreProcessedData() : base()
   {
       iRecordsNumber = 0;
       iRelevanceDimensions = 0;
       lRecords = new List<TPreProcessedRecord>();
   }
   public bool SetTextFile(String sAFullFilePath){
       List<TPreProcessedAttribute> lTemp;
       string line;
       TPreProcessedDimension ppdiTemp;
       TPreProcessedRecord pprTemp;
       int iFieldsCounter = 0;
       System.IO.StreamReader file =  new System.IO.StreamReader(sAFullFilePath);
       line = file.ReadLine();

       /*First line*/
       if (line[0].CompareTo('#') != 0)
       {
           //sMessage = "Missing field names in text file, operation aborted.";
           //Application->MessageBox(sMessage.c_str(), NULL, MB_OK);
           //ifsTextFile.close();
           return false;
       }
       else
       {
           String[] words = line.Split(';');
           foreach (String word in words)
           {
               if (iFieldsCounter == 0)
               {
                   this.Add(
                       new KeyValuePair<String, TPreProcessedDimension>
                           (word.Substring(1, word.Length - 1), new TPreProcessedDimension(word.Substring(1, word.Length - 1), iFieldsCounter)));
                  // filew.Write(word.Substring(1, word.Length - 1).ToString());
               }
               else
               {
                   this.Add(
                       new KeyValuePair<String, TPreProcessedDimension>
                           (word, new TPreProcessedDimension(word, iFieldsCounter)));
                  // filew.Write(word.ToString());
               }
               iFieldsCounter++;
           }
       }

       /*Lê e processa os dados, preenchendo os objetos de dimensões préprocessadas*/
       iRecordsNumber = 0;
       while((line = file.ReadLine()) != null)
       {
           //if(line.StartsWith("%"))
             //  continue;
           this.ProcessTextLine(line);
           iRecordsNumber++;
       }
       lTemp = new List<TPreProcessedAttribute>();
       for(int iCounter = 0; iCounter < iRecordsNumber; iCounter++){
           lTemp.Clear();
            for(int iFieldCounter = 0; iFieldCounter < this.Count; iFieldCounter++){
                ppdiTemp = this[iFieldCounter].Value;
                lTemp.Add(ppdiTemp[iCounter]);
            }
            pprTemp = new TPreProcessedRecord(lTemp,iCounter);
            this.lRecords.Add(pprTemp);
       }           
       file.Close();
       this.Normalize();
       return true;
   }

    bool IsTextLineCorrect(String sATextLine){
       String[] words = Regex.Split(sATextLine, ";");
       if(words.Length != this.Count)
           return false;
        return true;
    }

   bool ProcessTextLine(String sATextLine){
       int iFieldsCounter = 0;
       /*Se a linha de texto não estiver correta ignora*/
       if(!IsTextLineCorrect(sATextLine))
          return false;
       TPreProcessedDimension ppdimTemp;
       String[] words = Regex.Split(sATextLine, ";");
       foreach(String word in words){
          /*Adiciona dado*/
          ppdimTemp = this[iFieldsCounter].Value;
          ppdimTemp.AddTextData(word);

          iFieldsCounter++;
       }
       return true;
   }
   void Normalize(bool bSearchForBoundaries = true){
       TPreProcessedDimension ppdimTemp;
       for (int iCounter = 0; iCounter < this.Count; iCounter ++){
          ppdimTemp = this[iCounter].Value;
          ppdimTemp.Normalize(bSearchForBoundaries);
       }
   }


/*


   void SetListOfPreProcessedRecords(TList *lAListOfPreProcessedRecords,
                                                TObject *oAPointerToTheOriginalPreProcessedData,
                                                bool bFullRange);

   void SetLabelsForEachRecord(List<String> lAListOfAnsiStrings);
   void AddDataForComposition(TPreProcessedData *oAPointerToTheOriginalPreProcessedData);
   void FinishDataComposition();
   
   void ExchangeDimensions(int iIndex1, int iIndex2);

 
   void SetPointAndAnalyzePreProcessedAttribute(int iADimensionIndex, float fARelevanceValue);
   void RemoveAllRelevancePoints();
   void SetDimMaxRelevDist(int iADimensionIndex, float fAMaxRelevanceDistance);

   float GetUnormalizedValue(int iADimensionIndex,float fAValue);
   float GetUnormalizedValue(int iADimensionIndex, int iAValueIndex);
   float GetValue(int iADimensionIndex, int iAValueIndex);
   float GetIthTimeValue(int iAValueIndex);
   float GetDimMaxRelevDist(int iADimensionIndex);
   float GetDimRelevValue(int iADimensionIndex);

   bool IsLabelDimension(int iADimensionIndex);
   String GetStringOfGivenValue(int iADimensionIndex,float fAValue);

   int GetDimParallelCoordsPosX(int iADimensionIndex);
   int GetDimScatterPlotsPosX(int iADimensionIndex);
   int GetDimScatterPlotsPosY(int iADimensionIndex);

   String GetDimName(int iADimensionIndex);
   int GetIndexByName(AnsiString asAName);
   float GetDimHighValue(int iADimensionIndex);
   float GetDimLowValue(int iADimensionIndex);
   TPreProcessedAttribute * GetAttrib(int iDimIndex, int iRecordIndex);
   void CalculateDataFrequency();
   void CalculateSubSetDataFrequency(TList *lListOfAttributes);
   void Clear();
   void SortRecordsBasedOnDimension(int iADimensionIndex, TList *lAListOfPreProcessedRecords);

   void SortRecordsByRelevance();
   TPreProcessedRecord * GetRecord(int iIndex);

   void TimeDetermineSetOfRecordsToShow(int iThTime);
   void ResetTimeExhibition(bool bShow);
   bool IsTextLineCorrect(char * cATextLine);

   void CalculateRecordsRelevance();
   static int RecordsComparisonFunction(void *Item1, void *Item2);
   static int RecordRelevanceComparisonFunction(void *Item1, void *Item2);
   int GetDummyValue(){return 1;};
   void SetDummyValue(int iAValue){};
 */
}
}
