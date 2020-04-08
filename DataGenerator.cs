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

namespace DataGeneratorSpace
{
    class DataGenerator
    {
        //1. reading photoName
        //2. reading maskName
        //3. Writing new folder Named : Results+DateTimeNow
        //4. Writing to new folder file: xml, Results+DateTimeNow

        int del;
        string dateNow;
        string dirNew;
        string photoPath;
        string photoName;
        string maskPath;
        string maskName;
        string path0;
        Workbook workbook;
       // string method;
        public DataGenerator (ImageMenu image)
        {
            photoPath = image.imgPath;
            maskPath = image.maskPath;
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
            path0 = dirNew + @"\ResultOf_" + dateNow + ".xls";
            Workbook workbook1 = new Workbook();
            Worksheet worksheet = new Worksheet(dateNow);
            worksheet.Cells[0, 0] = new Cell("Photo name");
            worksheet.Cells[0, 1] = new Cell("Mask name");
            worksheet.Cells[0, 2] = new Cell("Corrupeted pixels percentage");
            worksheet.Cells[0, 3] = new Cell("RMSE");
            worksheet.Cells[0, 4] = new Cell("PSNR");
            worksheet.Cells[0, 5] = new Cell("SSIM");
            worksheet.Cells[0, 6] = new Cell("GSMD");
            worksheet.Cells.ColumnWidth[0, 1] = 30;
            workbook1.Worksheets.Add(worksheet);
            workbook1.Save(path0);
           // workbook = workbook1;

            return path0;
        }

        public void GenerateValues(int corruptedPxPercent, double rmse, double psnr, double ssim, double gsmd)
        {
            var workbook = Workbook.Load(path0);
            var worksheet = workbook.Worksheets[0];
            int lastRow = worksheet.Cells.LastRowIndex;
            worksheet.Cells[lastRow + 1,0] = new Cell(photoName);
            worksheet.Cells[lastRow + 1, 1] = new Cell(maskName);
            worksheet.Cells[lastRow + 1, 2] = new Cell(corruptedPxPercent);
            worksheet.Cells[lastRow + 1, 3] = new Cell(rmse);
            worksheet.Cells[lastRow + 1, 4] = new Cell(psnr);
            worksheet.Cells[lastRow + 1, 5] = new Cell(ssim);
            worksheet.Cells[lastRow + 1, 6] = new Cell(gsmd);

            workbook.Worksheets.Add(worksheet);
            workbook.Save(path0);
            Loope();
        }

        public void Loope()
        {
            ImageMenu MenuImg;
            NavierStokesInpaint NavierStokes;
            AlexandruTeleaInpaint AlexandruTelea;
            FrequencySelectiveReconstruction FSR;
            Nans NansAlg;
            string enviro = string.Format(photoPath.Remove(del - 2)) + @"\Images";
            foreach (object file in Directory.EnumerateFiles(enviro))
            {
                // zrobić robienie jednej maski 20% S&P i maski 40% S&P, scratch maska około 40%,  square 40 %
                // create method to use FSR and create make mask and use in FSRBest/FSR ?
                //

            }

        }


    }
}
