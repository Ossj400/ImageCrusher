using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageCrusher.ImageController;
using ImageCrusher.Inpainting;
using ExcelLibrary;
using ExcelLibrary.SpreadSheet;
using System.Security.AccessControl;
using System.Security.Principal;
using Emgu.CV.Structure;
using Emgu.CV;

namespace DataGeneratorSpace
{
    class DataGenerator
    {
        int del;
        string dateNow;
        string dirNew;
        string photoPath;
        string photoName;
        string maskPath;
        string maskName;
        string path0;
        Workbook workbook;
        Image<Bgr, byte> ImgOut;
        Image<Bgr, byte> img;
        string methodName = "0";

        public DataGenerator (ImageMenu image)
        {
            photoPath = image.imgPath;
            maskPath = image.maskPath;
            img = image.Img;
        }

        public string ReadPhotoName()
        {

            int f = 0;
            foreach (char ch in photoPath)
            {
                f++;
                if (ch == '^')
                    break;
            }
            photoName = string.Format(photoPath.Remove(0, f));
            del = f;

            return photoName;
        }

        public string ReadMaskName()
        {

            int f = 0;
            foreach (char ch in maskPath)
            {
                f++;
                if (ch == '^')
                    break;
            }
            maskName = string.Format(maskPath.Remove(0, f));
            del = f;
            return maskName;
        }

        public string CreateFolder()
        {
            path0 = string.Format(photoPath.Remove(del-2));
            dateNow = DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            dirNew = path0 + @"\Documents\ResultOf_" + dateNow;
            Directory.CreateDirectory(dirNew);
            DirectorySecurity sec = Directory.GetAccessControl(dirNew);
            SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid,null);
            sec.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.Modify | FileSystemRights.Synchronize, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            Directory.SetAccessControl(dirNew, sec);

            return dirNew;
        }
        public string CreateFile()
        {
            path0 = dirNew + @"\ResultOf_" + methodName + dateNow + ".xls";
            Workbook workbook1 = new Workbook();
            Worksheet worksheet = new Worksheet(dateNow);
            worksheet.Cells[0, 0] = new Cell("Photo name");
            worksheet.Cells[0, 1] = new Cell("Mask name");
            worksheet.Cells[0, 2] = new Cell("RMSE");
            worksheet.Cells[0, 3] = new Cell("PSNR");
            worksheet.Cells[0, 4] = new Cell("SSIM");
            worksheet.Cells[0, 5] = new Cell("GSMD");
            worksheet.Cells[0, 6] = new Cell("Algorithm Name");
            worksheet.Cells.ColumnWidth[0, 1] = 30;
            workbook1.Worksheets.Add(worksheet);
            workbook1.Save(path0);
           // workbook = workbook1;

            return path0;
        }

        public void GenerateValues(string photoName, string maskName, double rmse, double psnr, double ssim, double gmsd, string methodName)
        {
            var workbook = Workbook.Load(path0);
            var worksheet = workbook.Worksheets[0];
            int lastRow = worksheet.Cells.LastRowIndex;
            worksheet.Cells[lastRow + 1,0] = new Cell(photoName);
            worksheet.Cells[lastRow + 1, 1] = new Cell(maskName);
            worksheet.Cells[lastRow + 1, 2] = new Cell(rmse);
            worksheet.Cells[lastRow + 1, 3] = new Cell(psnr);
            worksheet.Cells[lastRow + 1, 4] = new Cell(ssim);
            worksheet.Cells[lastRow + 1, 5] = new Cell(gmsd);
            worksheet.Cells[lastRow + 1, 6] = new Cell(methodName);

            workbook.Worksheets.Add(worksheet);
            workbook.Save(path0);
        }

        public void Loope()
        {

            methodName = "Alexandru_Telea";
            ReadPhotoName();
            string enviroPh = string.Format(photoPath.Remove(del - 2));
            ReadMaskName();
            CreateFolder();
            CreateFile();
            int i = 0;
            string enviroMsk = string.Format(maskPath);
            foreach (object file in Directory.EnumerateFiles(enviroMsk.Remove(del - 2)))
            {
                foreach (object files in Directory.EnumerateFiles(enviroPh))
                {
                    i++;
                    ImageMenu newIm = new ImageMenu();
                    newIm.Img = new Image<Bgr, byte>(files.ToString());
                    Noise noise = new Noise(newIm);
                    noise.MaskLoaded = new Image<Gray, byte>(file.ToString());
                    AlexandruTeleaInpaint alexandruTelea = new AlexandruTeleaInpaint();
                    alexandruTelea.InpaintTel(newIm, noise, 1);
                    photoPath = files.ToString();
                    maskPath = file.ToString();
                    UseMethods(newIm, alexandruTelea, "Alexandru Telea");
                    alexandruTelea.ImageOutTelea.Save(path0 + methodName + i.ToString() + ".bmp");
                }
            }
        }
            public void Loope2()
       {
            methodName = "Navier_Stokes";
            ReadPhotoName();
            string enviroPh = string.Format(photoPath.Remove(del - 2));
            ReadMaskName();
            CreateFolder();
            CreateFile();
            int i = 0;
            string enviroMsk = string.Format(maskPath);
            foreach (object file in Directory.EnumerateFiles(enviroMsk.Remove(del - 2)))
            {
                foreach (object files in Directory.EnumerateFiles(enviroPh))
                {
                    i++;
                    ImageMenu newIm = new ImageMenu();
                    newIm.Img = new Image<Bgr, byte>(files.ToString());
                    Noise noise = new Noise(newIm);
                    noise.MaskLoaded = new Image<Gray, byte>(file.ToString());
                    NavierStokesInpaint navierStokes = new NavierStokesInpaint();
                    navierStokes.InpaintNav(newIm, noise, 1);
                    photoPath = files.ToString();
                    maskPath = file.ToString();
                    UseMethods(newIm, navierStokes, "Navier Stokes");
                    navierStokes.ImageOutNav.Save(path0 + methodName + i.ToString() + ".bmp");
                }
            }
         }

        public void Loope3()
        {
            int i = 0;
            methodName = "FSR_F";
            ReadPhotoName();
            ReadMaskName();
            CreateFolder();
            CreateFile();
            string enviroPh = string.Format(photoPath.Remove(del - 1));// + @"\Images";
            string enviroMsk = string.Format(maskPath);
            foreach (object file in Directory.EnumerateFiles(enviroMsk.Remove(del - 2)))
            {
                foreach (object files in Directory.EnumerateFiles(enviroPh))
                {
                    i++;
                    ImageMenu newIm = new ImageMenu();
                    newIm.Img = new Image<Bgr, byte>(files.ToString());
                    Noise noise = new Noise(newIm);
                    noise.MaskLoaded = new Image<Gray, byte>(file.ToString());
                    FrequencySelectiveReconstruction FSR = new FrequencySelectiveReconstruction();
                    newIm.imgPath = files.ToString();
                    FSR.InpaintFSR(newIm, noise,Emgu.CV.XPhoto.XPhotoInvoke.InpaintType.FsrFast);
                    photoPath = files.ToString();
                    maskPath = file.ToString();
                    UseMethods(newIm, FSR, "FsrFast");
                    FSR.ImageOutFSR.Save(path0 + methodName + i.ToString() + ".bmp");
                }
            }
        }

             public void Loope4()
             {
                 int i = 0;
                 methodName = "Shift_Map";
                 ReadPhotoName();
                 ReadMaskName();
                 CreateFolder();
                 CreateFile();
                 string enviroPh = string.Format(photoPath.Remove(del - 1));// + @"\Images";
                 string enviroMsk = string.Format(maskPath);
                foreach (object file in Directory.EnumerateFiles(enviroMsk.Remove(del - 2)))
                 {
                     foreach (object files in Directory.EnumerateFiles(enviroPh))
                     {
                         i++;
                         ImageMenu newIm = new ImageMenu();
                         newIm.Img = new Image<Bgr, byte>(files.ToString());
                         Noise noise = new Noise(newIm);
                         noise.MaskLoaded = new Image<Gray, byte>(file.ToString());
                         FrequencySelectiveReconstruction FSR = new FrequencySelectiveReconstruction();
                         newIm.imgPath = files.ToString();
                         FSR.InpaintFSR(newIm, noise, Emgu.CV.XPhoto.XPhotoInvoke.InpaintType.Shiftmap);
                         photoPath = files.ToString();
                         maskPath = file.ToString();
                         UseMethods(newIm, FSR, "Shift Map");
                         FSR.ImageOutFSR.Save(path0 + methodName + i.ToString() + ".bmp");
                     }
                 }
             }
        public void Loope5()
        {
            int i = 0;
            methodName = "FSR_B";
            ReadPhotoName();
            ReadMaskName();
            CreateFolder();
            CreateFile();
            string enviroPh = string.Format(photoPath.Remove(del - 1));// + @"\Images";
            string enviroMsk = string.Format(maskPath);
            foreach (object file in Directory.EnumerateFiles(enviroMsk.Remove(del - 2)))
            {
                foreach (object files in Directory.EnumerateFiles(enviroPh))
                {
                    i++;
                    ImageMenu newIm = new ImageMenu();
                    newIm.Img = new Image<Bgr, byte>(files.ToString());
                    Noise noise = new Noise(newIm);
                    noise.MaskLoaded = new Image<Gray, byte>(file.ToString());
                    FrequencySelectiveReconstruction FSR = new FrequencySelectiveReconstruction();
                    newIm.imgPath = files.ToString();
                    FSR.InpaintFSR(newIm, noise, Emgu.CV.XPhoto.XPhotoInvoke.InpaintType.FsrBest);
                    photoPath = files.ToString();
                    maskPath = file.ToString();
                    UseMethods(newIm, FSR, "FsrBest");
                    FSR.ImageOutFSR.Save(path0 + methodName + i.ToString() + ".bmp");
                }
            }
        }
        public virtual void UseMethods(ImageMenu image, NavierStokesInpaint navier, string methodName)
        {
            string phName = ReadPhotoName();
            string msName = ReadMaskName();
            Indicator indicator = new Indicator(image, navier);
            double rmse = indicator.RMSE();
            double psnr = indicator.PNSR();
            double ssim = indicator.SSIM();
            double gmsd = indicator.GMSD();
            GenerateValues(phName, msName, rmse, psnr, ssim, gmsd, methodName);
        }
        public void UseMethods(ImageMenu image, AlexandruTeleaInpaint alexandruTelea, string methodName)
        {
            string phName = ReadPhotoName();
            string msName = ReadMaskName();
            Indicator indicator = new Indicator(image, alexandruTelea);
            double rmse = indicator.RMSE();
            double psnr = indicator.PNSR();
            double ssim = indicator.SSIM();
            double gmsd = indicator.GMSD();
            GenerateValues(phName, msName, rmse, psnr, ssim, gmsd, methodName);
        }
        public void UseMethods(ImageMenu image, FrequencySelectiveReconstruction FSR, string methodName)
        {
            string phName = ReadPhotoName();
            string msName = ReadMaskName();
            Indicator indicator = new Indicator(image, FSR);
            double rmse = indicator.RMSE();
            double psnr = indicator.PNSR();
            double ssim = indicator.SSIM();
            double gmsd = indicator.GMSD();
            GenerateValues(phName, msName, rmse, psnr, ssim, gmsd, methodName);
        }

        public void UseMethods(ImageMenu image, Nans nans, string methodName)
        {
            string phName = ReadPhotoName();
            string msName = ReadMaskName();
            Indicator indicator = new Indicator(image, nans);
            double rmse = indicator.RMSE();
            double psnr = indicator.PNSR();
            double ssim = indicator.SSIM();
            double gmsd = indicator.GMSD();
            GenerateValues(phName, msName, rmse, psnr, ssim, gmsd, methodName);
        }

    }
}
