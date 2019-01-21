using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HG.MSICS;
using HG.MyJCZ;
using System.Windows.Forms;
namespace WindowsFormsApplication1.MarkInterface
{
    public  class HgMarkInterface
    {
      public  enum  Lasererr
      {
          lasernoneerr =0,
          initfail =1,
          loadmodelfail =2,
          changetextfail = 3,
          markedfail =4,
          flyfail = 5,
          setpresevemodefail =6
      }

      public  enum MarkCardType
      {
        markjcz,
        markmsi
        }
        public struct MarkCardinfoST
        {
         public MarkCardType markcard;
         public IntPtr ph;
         public string systemxmlfilepath;
         }
   
      private static MarkCardinfoST markcardinformationst;
      public  static MarkCardinfoST MarkCardInformationSt
      {
          set { markcardinformationst = value; }
          get { return markcardinformationst;  }
      }

      public static Lasererr InitLaser()
      {
          if (markcardinformationst.markcard == MarkCardType.markjcz)
          {
              if (MarkJcz.InitLaser(markcardinformationst.ph))
              {
                  return Lasererr.lasernoneerr;
              }
              else
              {
                  return Lasererr.initfail;
              }
          }
          else if (markcardinformationst.markcard == MarkCardType.markmsi)
          {
              if (MSI.Initialize())
              {
                  return Lasererr.lasernoneerr;
              }
              else
              {
                  return Lasererr.initfail;
              }
          }
          else
          {
              return Lasererr.initfail;
          }
      }
     static    public string nowmodelfile = "";
      public static int LoadtheModelFile(ref string path)
      {
          if (markcardinformationst.markcard == MarkCardType.markjcz)
          {
              if (MarkJcz.LoadEzdFile(ref path,false)!= " ")
              {
                  nowmodelfile = path;
                  return 11;
              }
              else
              {
                  return 10;
              }
          }
          else if (markcardinformationst.markcard == MarkCardType.markmsi)
          {
                nowmodelfile = path;
                return  MSI.Loadmodelfile(ref path);                   
          }
          else
          {
                nowmodelfile = "";
              return -1;
          }
      }
        public static void UnLoadtheModelFile(int mskindex)
        {
            MSI.UnLoadmodelfile(mskindex);
        }
      public static void DrawModelFileInPicture(ref PictureBox pic,int mksfileindex)
      {
         if (markcardinformationst.markcard == MarkCardType.markjcz)
          {
              MarkJcz.ShowPreviewBmp(pic);           
          }
         else if (markcardinformationst.markcard == MarkCardType.markmsi)
         {
             MSI.DrawMksFile(mksfileindex,pic.Handle);
         }
         else
         {

         }
      }

      public static void CloseLaser(int mskindex)
      {
          if (markcardinformationst.markcard == MarkCardType.markjcz)
          {
              MarkJcz.Close();
          }
          else if (markcardinformationst.markcard == MarkCardType.markmsi)
          {       
              MSI.Close(mskindex);
          }
          else
          {

          }
      }


      public static void ChangeNameByTxtstring(ref string names,ref string txtstring,int mskindex)
      {
          if (markcardinformationst.markcard == MarkCardType.markjcz)
          {
              MarkJcz.ChangeTextByName(ref names, ref txtstring);
          }
          else if (markcardinformationst.markcard == MarkCardType.markmsi)
          {
              MSI.ChangeTextByName(ref names, ref txtstring,mskindex);
          }
          else
          {

          }
      }
        public static bool ReadIO(int IO)
        {
            if (markcardinformationst.markcard == MarkCardType.markjcz)
            {
                return MarkJcz.ReadPort(IO);
            }
            return true;
        }
        public static bool WriteIO(int IO,bool onoff)
        {
            if (markcardinformationst.markcard == MarkCardType.markjcz)
            {
                return MarkJcz.WritePort(IO,onoff);
            }
            return true;
        }
        public static Lasererr MarkByObjectName(ref string objname, int mksindex)
        {
            if (markcardinformationst.markcard == MarkCardType.markjcz)
            {
                if(MarkJcz.MarkEntity(objname))
                {
                    return Lasererr.lasernoneerr;
                }else
                {
                    return Lasererr.markedfail;
                }

            }else
            {
                return Lasererr.markedfail;
            }
        }
      public static Lasererr Mark(bool fly,int mksindex)
      {
          if (markcardinformationst.markcard == MarkCardType.markjcz)
          {
              if (MarkJcz.Mark(fly))
              {
                  return Lasererr.lasernoneerr;
              }
              else
              {
                  return Lasererr.markedfail;
              }
          }
          else if (markcardinformationst.markcard == MarkCardType.markmsi)
          {
              if(MSI.Mark(mksindex))
              {
                  return Lasererr.lasernoneerr;
              }
              else
              {
                  return Lasererr.markedfail;
              }
          }
          else
          {
              return Lasererr.markedfail;
          }
      }
      public static Lasererr MsiSetPreseveMode(int mksindex)
      {
          if (markcardinformationst.markcard == MarkCardType.markmsi)
          {
              if (MSI.SetPresaveMode(mksindex))
              {
                  return Lasererr.lasernoneerr;
              }
              else
              {
                  return Lasererr.setpresevemodefail;
              }
          }
          else
          {
              return Lasererr.setpresevemodefail;
          }

      }


      public static void StopjczMark()
      {
          MarkJcz.StopMark();
      }
      public static int FlyMark(bool msiwithtrriger,int mksindex)
      {
          if (markcardinformationst.markcard == MarkCardType.markjcz)
          {
              if(MarkJcz.MarkFlyByInIOSignal())
              {
                  return 31;
              }
              else
              {
                  return 30;
              }
              
          }
          else if (markcardinformationst.markcard == MarkCardType.markmsi)
          {
              int RE = MSI.MarkFlyByInIOSignal(msiwithtrriger, mksindex);          
              return RE;            
          }
          else
          {
              return 44;
          }

      }

        public static bool Resetaxis(bool b0, bool b1)
        {
            return MarkJcz.AxisReset(b0, b1);
        }
        public static bool AxisCorrectOrigin(int n)
        {
            return MarkJcz.AxisCorrectOrigin(n);
        }

        public static double GetAxisCoor(int n)
        {
            return MarkJcz.GetAxisCoor(n);
        }

        public static bool AxisMoveTo(int n, double b)
        {
            return MarkJcz.AxisMoveTo(n, b);
        }
    }
}
