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

      public  enum markcardtype
      {
        markjcz,
        markmsi
        }
        public struct markcardinfoST
        {
         public    markcardtype markcard;
         public IntPtr ph;
         public string systemxmlfilepath;

         }
   
      private static markcardinfoST markcardinformationst;
      public markcardinfoST MarkCardInformationSt
      {
          set { markcardinformationst = value; }

          get { return markcardinformationst;  }

      }

      public static Lasererr InitLaser()
      {
          if (markcardinformationst.markcard == markcardtype.markjcz)
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
          else if (markcardinformationst.markcard == markcardtype.markmsi)
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

      public static Lasererr LoadtheModelFile(ref string path)
      {
          if (markcardinformationst.markcard == markcardtype.markjcz)
          {
              if (MarkJcz.LoadEzdFile(ref path,false)!= " ")
              {
                  return Lasererr.lasernoneerr;
              }
              else
              {
                  return Lasererr.loadmodelfail;
              }
          }
          else if (markcardinformationst.markcard == markcardtype.markmsi)
          {
              if (MSI.Loadmodelfile(ref path))
              {
                  return Lasererr.lasernoneerr;
              }
              else
              {
                  return Lasererr.loadmodelfail;
              }

          }
          else
          {
              return Lasererr.loadmodelfail;
          }
      }

      public static void DrawModelFileInPicture(ref PictureBox pic)
      {
         if (markcardinformationst.markcard == markcardtype.markjcz)
          {
              MarkJcz.ShowPreviewBmp(pic);           
          }
         else if (markcardinformationst.markcard == markcardtype.markmsi)
         {
             MSI.DrawMksFile(pic.Handle);
         }
         else
         {

         }

      }

      public static void CloseLaser()
      {
          if (markcardinformationst.markcard == markcardtype.markjcz)
          {
              MarkJcz.Close();
          }
          else if (markcardinformationst.markcard == markcardtype.markmsi)
          {
          
              MSI.Close();
          }
          else
          {

          }
      }


      public static void ChangeNameByTxtstring(ref string names,ref string txtstring)
      {
          if (markcardinformationst.markcard == markcardtype.markjcz)
          {
              MarkJcz.ChangeTextByName(ref names, ref txtstring);
          }
          else if (markcardinformationst.markcard == markcardtype.markmsi)
          {

              MSI.ChangeTextByName(ref names, ref txtstring);
          }
          else
          {

          }
      }
      public static Lasererr Mark(bool fly)
      {

          if (markcardinformationst.markcard == markcardtype.markjcz)
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
          else if (markcardinformationst.markcard == markcardtype.markmsi)
          {
              if(MSI.Mark())
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
      public static Lasererr MsiSetPreseveMode()
      {
          if (markcardinformationst.markcard == markcardtype.markmsi)
          {
              if (MSI.SetPresaveMode())
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

      public static Lasererr FlyMark(bool msiwithtrriger)
      {
          if (markcardinformationst.markcard == markcardtype.markjcz)
          {
              if (MarkJcz.MarkFlyByInIOSignal())
              {
                  return Lasererr.lasernoneerr;
              }
              else
              {
                  return Lasererr.flyfail;
              }
              
          }
          else if (markcardinformationst.markcard == markcardtype.markmsi)
          {
              if (MSI.MarkFlyByInIOSignal(msiwithtrriger))
              {
                  return Lasererr.lasernoneerr;
             }else
              {
                  return Lasererr.flyfail;
              }
          }
          else
          {
              return Lasererr.flyfail;
          }

      }
    }
}
