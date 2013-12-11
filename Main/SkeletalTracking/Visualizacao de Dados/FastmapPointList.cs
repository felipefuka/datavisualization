using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using OctreeSearch;

namespace MetricSPlat_Kinect
{
class TFastmapPointList : List<TFastmapPoint> {
   Octree myOcTree;
   int iTargetDimension;
   int iSelectedElement;
   float fMinX, fMaxX, fMinY, fMaxY, fMinZ, fMaxZ;
   TFastmapProjectionWPF fpProjection;

   Auxiliar.ENUM_drawModes dmDrawMode;
   public Auxiliar.ENUM_drawModes dmADrawMode
   {
       set { dmDrawMode = value; }
       get { return dmDrawMode; }
   }
   public List<TPreProcessedRecord> ListOfIndexOfDataItemsToDraw{
       set { SetListOfIndexOfDataItemsToDraw(value); }
   }
   public float PointSize
   {
       set { SetPointSize(value); }
       get { return fPointSize; }
   }
   public int TargetDimension 
   {
       set { iTargetDimension = value; }
       get { return iTargetDimension; }
   }
   public bool DrawIdLabelsProperty
   {
       set { bDrawIdLabels = value; }
       get { return bDrawIdLabels; }
   }
   public int SelectedElement 
   {
       get { return iSelectedElement; }
   }

   List<TPreProcessedRecord> lListOfIndexOfDataItemsToDraw;
   List<TPreProcessedRecord> lPreProcessedRecordsForFastmapPivotChoosing;
   bool bDrawIdLabels;
   
   TPreProcessedData ppdData;
   TFastmapPoint sdAuxiliar;
   float fPointSize;

   bool bMappingCalculated;
   float fRelevanceValuesTimeStamp;
   float fFrequencyValuesTimeStamp;

   void SetDrawMode(Auxiliar.ENUM_drawModes dmADrawMode){
       dmDrawMode = dmADrawMode;
       foreach(TFastmapPoint item in this){
          item.DrawMode = this.dmDrawMode;
        }      
    }
   void SetPointSize(float fAPointSize)
   {
       foreach(TFastmapPoint item in this){
          item.PointSize = fAPointSize;
        }  
   }
   public TFastmapPointList() :base()
   {
       dmADrawMode = Auxiliar.ENUM_drawModes.drawmodeNormal;
       fPointSize = 3.0f;
       bMappingCalculated = false;
       iTargetDimension = 3;
       bDrawIdLabels = false;
       iSelectedElement = 0;
       lListOfIndexOfDataItemsToDraw = null;
       myOcTree = null;
   }
   public void Init(TFastmapProjectionWPF fpaProjection, TPreProcessedData aPreProcessedData)
   {
       this.Clear();
       fpProjection = fpaProjection;
       ppdData = aPreProcessedData;
       lPreProcessedRecordsForFastmapPivotChoosing = ppdData.ListOfPreprocessedRecords;
       TFastmapPoint sdTemp;
       /*Inicializa a lista de pontos projetados via Fastmap*/
       for (uint iCounter = 0; iCounter < ppdData.iRecordsNumber; iCounter++)
       {
           sdTemp = new TFastmapPoint();
           sdTemp.Init(iCounter, fpProjection);
           Add(sdTemp);
       }        
   }
   void SetListOfIndexOfDataItemsToDraw(List<TPreProcessedRecord> lAListToDraw){
        lListOfIndexOfDataItemsToDraw = lAListToDraw;
   }
   public int GetMaxResolution()
   {
       //myOcTree.getMaxLevel();
       return 1;
   }

   public void AutoDrawOctree(int iResolution)
   {
       TFastmapPoint FastmapPointTempToDraw = null;
       TPreProcessedRecord pprTemp;
       ArrayList myArrayList = new ArrayList();
       //myArrayList = myOcTree.GetNode(fMaxX, fMinX, fMaxY, fMinY, fMaxZ, fMinZ);
       //System.Windows.Forms.MessageBox.Show(myArrayList.ToString());
       //testing
       TOpenGLColor cor = new TOpenGLColor();
       cor.SetColor(1.0f, 0.0f, 0.0f);
       for (int iCounter = 0; iCounter < ppdData.RecordsNumber; iCounter++)
       {
           myArrayList = myOcTree.GetNode(fMaxX, fMinX, fMaxY, fMinY, fMaxZ, fMinZ);
           //pprTemp = this.ppdData.ListOfPreprocessedRecords[iCounter];
           FastmapPointTempToDraw = this[iCounter];
           FastmapPointTempToDraw.NormalDraw(cor);
       }
   }


   public void AutoDraw(int iResolution)
   {
       //myOcTree.autodraw(iResolution);
       //    return

       TFastmapPoint FastmapPointTempToDraw = null;
       TPreProcessedRecord pprTemp;

       if (lListOfIndexOfDataItemsToDraw != null)
       {
           /*Not working - disconsider*/
           for (int iCounter = 0; iCounter < lListOfIndexOfDataItemsToDraw.Count; iCounter++)
           {
               pprTemp = lListOfIndexOfDataItemsToDraw[iCounter];
                   FastmapPointTempToDraw = this[pprTemp.Index];

                   switch (dmDrawMode)
                   {
                       case Auxiliar.ENUM_drawModes.drawmodeNormal:
                           FastmapPointTempToDraw.SelectionDraw(pprTemp.Color);
                           break;
                   }
                   if (bDrawIdLabels)
                       FastmapPointTempToDraw.DrawIDLabel();               
           }
       }
       else
       {
           for (int iCounter = 0; iCounter < this.Count; iCounter++)
           {
               pprTemp = this.ppdData.ListOfPreprocessedRecords[iCounter];
                   FastmapPointTempToDraw = this[iCounter];
                   FastmapPointTempToDraw.NormalDraw(pprTemp.Color);

               /*switch (dmDrawMode)
               {
                   case Auxiliar.ENUM_drawModes.drawmodeNormal:
                       FastmapPointTempToDraw.NormalDraw(pprTemp.Color);
                       break;
               }
               if (bDrawIdLabels)
                   FastmapPointTempToDraw.DrawIDLabel();
                                   
                */


           }
       }
   }
   public void PerformMapping(){
       if(bMappingCalculated)
          return;

       if(ppdData.ListOfPreprocessedRecords.Count == 1){  /*Mapeamento de um unico elemento . vai para a origem*/
          (this[0]).oglvPoint.X = 0.0f;
          (this[0]).oglvPoint.Y = 0.0f;
          (this[0]).oglvPoint.Z = 0.0f;
          fMinX = 10.0f; fMaxX = 10.0f;
          fMinY = 10.0f; fMaxY = 10.0f;
          fMinZ = 10.0f; fMaxZ = 10.0f;
       }else{
          float[] fMapTemp = new float[3];
        
          TFastmapper fm = new TFastmapper(iTargetDimension);

            /*No VisTree, o indice da lista de elementos que serao exibidos - definido pela ordem da propria lista -
            e muito importante. No entanto, e importante frisar que o conjunto lPreProcessedRecordsForFastmapPivotChoosing
            nao sera exibido e, portanto, o indice dentro desta lista nao tem importancia para o VisTree. O indice aqui
            so tem importancia para a classe TFastMapper no mapa de TFastMapImage que ela usa.*/
          /*Escolha dos pivots . definicao do espaco.*/
          fm.ChoosePivots(lPreProcessedRecordsForFastmapPivotChoosing);

          /*Agora faz-se a projecao (mapeamento) do conjunto ppdData.ListOfPreprocessedRecords
            que define o que sera exibido de fato*/

          /*Para procurar os valores máximos e mínimos em X, Y e Z, inicializamos as respecitivas
            variaveis com os valores de mapeamento do primeiro elemento*/
          fm.Map(ppdData.ListOfPreprocessedRecords[0],fMapTemp);  /*Mapeia o 1o. elemento*/
             (this[0]).oglvPoint.X = fMapTemp[0];
             (this[0]).oglvPoint.Y = fMapTemp[1];
             (this[0]).oglvPoint.Z = fMapTemp[2];
             fMinX = fMapTemp[0]; fMaxX = fMapTemp[0];
             fMinY = fMapTemp[1]; fMaxY = fMapTemp[1];
             fMinZ = fMapTemp[2]; fMaxZ = fMapTemp[2];

          for(int iRecordCounter = 1; iRecordCounter < ppdData.RecordsNumber; iRecordCounter++){
             fm.Map(ppdData.ListOfPreprocessedRecords[iRecordCounter],fMapTemp);
             (this[iRecordCounter]).oglvPoint.X = fMapTemp[0];
             (this[iRecordCounter]).oglvPoint.Y = fMapTemp[1];
             (this[iRecordCounter]).oglvPoint.Z = fMapTemp[2];
             if(fMapTemp[0] < fMinX)
                fMinX = fMapTemp[0];
             if(fMapTemp[0] > fMaxX)
                fMaxX = fMapTemp[0];

             if(fMapTemp[1] < fMinY)
                fMinY = fMapTemp[1];
             if(fMapTemp[1] > fMaxY)
                fMaxY = fMapTemp[1];

             if(fMapTemp[2] < fMinZ)
                fMinZ = fMapTemp[2];
             if(fMapTemp[2] > fMaxZ)
                fMaxZ = fMapTemp[2];
          }

          fpProjection.SetglOrtho(fMinX, fMaxX, fMinY, fMaxY, fMinZ, fMaxZ);
          /*Projeção inicial com o elemento 0 centralizado*/
          this.CentralizeElement(0);

          bMappingCalculated = true;
       }
       myOcTree = new Octree(fMaxX, fMinX, fMaxY, fMinY, fMaxZ, fMinZ, 1, OctreeNode.NO_MIN_SIZE);
       for (int iRecordCounter = 0; iRecordCounter < ppdData.RecordsNumber; iRecordCounter++)
       {
           myOcTree.AddNode(this[iRecordCounter].oglvPoint.X, this[iRecordCounter].oglvPoint.Y, this[iRecordCounter].oglvPoint.Z, this[iRecordCounter]);
       }
    }

   void CentralizeElement(int iAnElementIndex)
   {
       int iIndexOfElementToCentralize = 0;
       TPreProcessedRecord pprTemp;
       for (int iCounter = 0; iCounter < this.ppdData.ListOfPreprocessedRecords.Count; iCounter++)
       {
           pprTemp = this.ppdData.ListOfPreprocessedRecords[iCounter];
           if (pprTemp.Label == Convert.ToString(iAnElementIndex))
           {
               iIndexOfElementToCentralize = pprTemp.Index;
               break;
           }
       }

       /*Ao termino deste procedimento o elemento iIndexOfElementToCentralize terá coordenadas (0,0,0) com os outros pontos a sua volta*/
       TFastmapPoint fpCenter = this[iIndexOfElementToCentralize];
       float[] fCoordinatesTemp = new float[3];
       fCoordinatesTemp[0] = fpCenter.oglvPoint.X;
       fCoordinatesTemp[1] = fpCenter.oglvPoint.Y;
       fCoordinatesTemp[2] = fpCenter.oglvPoint.Z;

       /*Translate every point*/
       TFastmapPoint fpTemp;
       for (int iRecordCounter = 0; iRecordCounter < this.Count; iRecordCounter++)
       {
           fpTemp = this[iRecordCounter];
           fpTemp.oglvPoint.X -= fCoordinatesTemp[0];
           fpTemp.oglvPoint.Y -= fCoordinatesTemp[1];
           fpTemp.oglvPoint.Z -= fCoordinatesTemp[2];
       }

       /*Also, translate the boundaries*/
       fMinX -= fCoordinatesTemp[0]; fMaxX -= fCoordinatesTemp[0];
       fMinY -= fCoordinatesTemp[1]; fMaxY -= fCoordinatesTemp[1];
       fMinZ -= fCoordinatesTemp[2]; fMaxZ -= fCoordinatesTemp[2];
       fpProjection.SetglOrtho(fMinX, fMaxX, fMinY, fMaxY, fMinZ, fMaxZ);
   }
    /* 

   
      TCustomPanel *cpDrawSpace;

      void CentralizeElement(int iAnElementIndex);
      void CentralizeSelectedElement();
      void ColorElementByIndex(iAnElementIndex);
      void Clear();
      void Refresh();
      void Init(TCustomPanel *scADrawSpace);
      void SetDataForFastmapPivotChoosing(TList* lAListOfPreprocessedRecordsForPivotChoosing);
      void PerformMapping(stBasicEuclideanMetricEvaluator <stBasicArrayObject <float, int> >* sbeeAnOptionalEvaluator = null);

      TFastmapPoint* GetFastmapPoint(int iAnElementIndex);
      void SelectPoint(int iAnId);
      void UnSelectPoint();
      void Select_Deselect_SetOfPointsByIndex(TList* lAListOfIndexes, bool bOption);
      void DrawLineBetweenPoints(int iAIndex, int AnotherIndex);
     */
}
}
