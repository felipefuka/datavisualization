using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetricSPlat_Kinect
{
    class TFastmapper{
      int Dim;
      int DLoopCount;
      float[] PivotDist,PivotDist2;
      TFastMapImage[] Pivots;

      public TFastmapper(int dim = 3, int dLoopCount = 5){
           Dim = dim;
           DLoopCount = dLoopCount;

           PivotDist = new float[Dim];
           PivotDist2 = new float[Dim];
           Pivots = new TFastMapImage[Dim * 2];
      }
      public float GetEuclideanDistance(TPreProcessedRecord rec1, TPreProcessedRecord rec2){
            float d = 0, tmp;
            for (int iCounter = 0; iCounter < rec1.Count; iCounter++){
                tmp = rec1[iCounter].fValue - rec2[iCounter].fValue;
                d += tmp * tmp;
            }
            return d;
      }

      public float GetPartialEuclideanDistance(float[] rec1, float[] rec2, int iPartial){
            float d = 0, tmp;
            for (int iCounter = 0; iCounter < iPartial; iCounter++){
                tmp = rec1[iCounter] - rec2[iCounter];
                d += tmp * tmp;
            }
            return d;
      }

      public void Map(TPreProcessedRecord obj, float[] map){
           for (int i = 0; i < Dim; i++){
              map[i] = Project(obj, map, i);
           }//end for
      }
      public float Project(TPreProcessedRecord obj, float[] map, int axis){
          int pid;
          pid = axis * 2;
          return (GetFMDistance2(obj, map, Pivots[pid].GetObject(),
                  Pivots[pid].GetCoords(), axis) +

                 (PivotDist2[axis]) -
                 GetFMDistance2(obj, map, Pivots[pid + 1].GetObject(),
                 Pivots[pid + 1].GetCoords(), axis)
                 )/
                 (2 * PivotDist[axis]);
      }

      float GetFMDistance2(TPreProcessedRecord o1, float[] map1,TPreProcessedRecord o2, float[] map2, int axis){
         float fDistanceTemp = GetEuclideanDistance(o1, o2);
         return fDistanceTemp - GetPartialEuclideanDistance(map1, map2, axis);        
      }

      int GetDimensions()
      {
          return Dim;
      }
      int GetPivotIdx(int axis, int name)
      {
          return (axis * 2) + name;
      }
      TPreProcessedRecord GetPivotObject(int idx)
      {
          if (Pivots[idx] != null)
          {
              return Pivots[idx].GetObject();
          }
          else
          {
              return null;
          }//end if
      }
      void GetPivotMap(int idx, float[] map)
      {
          if (Pivots[idx] != null)
          {
              Pivots[idx].GetCoords(map);
          }//end if
      }
      void SetPivot(int idx, TPreProcessedRecord obj, float[] map){
          // Create image if required.
          if (Pivots[idx] == null)
          {
              Pivots[idx] = new TFastMapImage(obj.Clone(), Dim);
          }
          else
          {
              Pivots[idx].SetObject(obj.Clone());
          }//end if

          // Replace image contents
          if (map != null)
          {
              Pivots[idx].SetCoords(map);
          }//end if
      }
      void UpdatePivotMaps()
      {
          int axis;            
          int pid;             
          int i;               

          pid = 0;
          for (axis = 0; axis < Dim; axis++)
          {
              PivotDist2[axis] = GetFMDistance2(
                    Pivots[pid].GetObject(), Pivots[pid].GetCoords(),
                    Pivots[pid + 1].GetObject(), Pivots[pid + 1].GetCoords(),
                    axis);
              PivotDist[axis] = (float)Math.Sqrt(PivotDist2[axis]);

              for (i = 0; i < Dim * 2; i++)
              {
                  if (i == pid)
                  {
                      Pivots[i].SetIthValue(axis, 0);
                  }
                  else if (i == pid + 1)
                  {
                      // I'm the pivot 2
                      Pivots[i].SetIthValue(axis, PivotDist[axis]);
                  }
                  else
                  {
                      Pivots[i].SetIthValue(axis, Project(Pivots[i].GetObject(),Pivots[i].GetCoords(), axis));
                  }//end if
              }//end for

              // Next pid
              pid = pid + 2;
          }//end for
      }
      bool IsReady()
      {
          return (Pivots[(Dim * 2) - 1] != null);
      }
      TFastMapImage[] CreateTmpImageVector(List<TPreProcessedRecord> lListOfPreprocessedRecordsForPivotChoosing)
      {
          TFastMapImage[] vec;
          int i;

          vec = new TFastMapImage[lListOfPreprocessedRecordsForPivotChoosing.Count];

          for (i = 0; i < lListOfPreprocessedRecordsForPivotChoosing.Count; i++)
          {
              vec[i] = new TFastMapImage(lListOfPreprocessedRecordsForPivotChoosing[i], Dim, false);
              vec[i].SetCoordsTo(0);
          }//end for

          return vec;
      }

      void FindPivots(TFastMapImage[] imgs, int n, int axis, ref int pa, ref int pb, ref float dist){
           float tmpdist;
           int oldpa;
           int step;
           int i;

           // Search for me!!
           pa = 0;
           oldpa = 0;
           step = 0;
           while (step < DLoopCount){
              // Locate the object wich is the most distant from pa
              dist = 0;
              for (i = 0; i < n; i++){
                 if (i != pa){
                    tmpdist = GetFMDistance2(
                          imgs[pa].GetObject(), imgs[pa].GetCoords(),
                          imgs[i].GetObject(), imgs[i].GetCoords(),
                          axis);
                    if (tmpdist < 0)
                        tmpdist = 0;
                    tmpdist = (float)Math.Sqrt(tmpdist);
                    if (tmpdist > dist){
                       dist = tmpdist;
                       pb = i;
                    }//end if
                 }//end if
              }//end for

              if (pb == oldpa){
                 // pb is the most distant from pa and vice versa
                 step = DLoopCount; // Terminate it now !
              }else{
                 // Prepare the next step
                 step++;
                 oldpa = pa;
                 pa = pb;
                 pb = oldpa;
              }//end it
           }//end while            
      }
      public void ChoosePivots(List<TPreProcessedRecord> lListOfPreprocessedRecordsForPivotChoosing)
      {
          int axis;
          int pid; 
          int i;   
          TFastMapImage[] images;    
          
          int[] pidxs;
          int iTotal;

          pidxs = new int[Dim * 2];

          images = CreateTmpImageVector(lListOfPreprocessedRecordsForPivotChoosing);

          pid = 0;
          for (axis = 0; axis < Dim; axis++)
          {
              // Choose pivots. It will also cache the distance beween them for later uses.
              FindPivots(images, lListOfPreprocessedRecordsForPivotChoosing.Count, axis, ref pidxs[pid], ref pidxs[pid + 1], ref PivotDist[axis]);
              PivotDist2[axis] = PivotDist[axis] * PivotDist[axis];

              // Add the new pivots.   pidxs[pid]
              SetPivot(pid, images[pidxs[pid]].GetObject(), images[pidxs[pid]].GetCoords());
              SetPivot(pid + 1, images[pidxs[pid + 1]].GetObject(), images[pidxs[pid + 1]].GetCoords());

              // Map everybody
              for (i = 0; i < lListOfPreprocessedRecordsForPivotChoosing.Count; i++)
              {
                  if (i == pidxs[pid])
                  {
                      images[i].SetIthValue(axis, 0);
                  }
                  else if (i == pidxs[pid + 1])
                  {
                      images[i].SetIthValue(axis, PivotDist[axis]);
                  }
                  else
                  {
                      images[i].SetIthValue(axis, Project(images[i].GetObject(), images[i].GetCoords(), axis));
                  }//end if
              }//end for

              // Update the pivot maps
              pid = pid + 2;
              for (i = 0; i < pid; i++)
              {
                  Pivots[i].SetIthValue(axis, images[i].GetIthValue(axis));
              }//end for
          }//end for
      }
};
}
