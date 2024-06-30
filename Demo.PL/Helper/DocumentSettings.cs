using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helper
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file , string folderName)
        {
            // 1. Get Location Folder Path
            //string folderpath = "C:\\Users\\moham\\Desktop\\Projects EF\\DemoMVC G04 Solution\\Demo.PL\\wwwroot\\files\\" + folderName;
            //string folderpath = Directory.GetCurrentDirectory() + "wwwroot\\files\\" + folderName;
            string folderpath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);

            // 2. Get File Name(Unique)
            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3. Get File Path===> FolderPath + FileName

            string filePath = Path.Combine(folderpath, fileName);

            // 4. Save File as Stream

            using var FileStream = new FileStream(filePath, FileMode.Create); 

            file.CopyTo(FileStream);

            return fileName;


        }
        public static void DeleteFile(string fileName ,  string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
