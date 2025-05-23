namespace Dewi.Helpers.Extentions
{
    public static class FileCreatingExtention
    {
        public static string FileCreating(this IFormFile file,string root,string foldername)
        {
            string extention=Path.GetExtension(file.FileName);  
            string orginalname=Path.GetFileNameWithoutExtension(file.FileName);
            string filename;
            if(orginalname.Length>64)
            {
                orginalname=orginalname.Substring(orginalname.Length-64);   
            }
            filename=Guid.NewGuid().ToString()+"-"+orginalname+extention;
            string path=Path.Combine(root,foldername, filename);
            using (FileStream stream = new FileStream(path,FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return filename;
        }

        public static void DeletingFile(this string filename,string root,string foldername)
        {
            string path=Path.Combine(root,foldername,filename); 
            if(File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
