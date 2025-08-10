namespace VirocGanpati.Helpers
{

    public class FileHelper
    {
        public static void EnsureDirectoriesExist(string documentsFolderPath, string signaturesFolderPath)
        {
            // Check if the documents directory exists, if not create it
            if (!Directory.Exists(documentsFolderPath))
            {
                Directory.CreateDirectory(documentsFolderPath);
            }

            // Check if the signatures directory exists, if not create it
            if (!Directory.Exists(signaturesFolderPath))
            {
                Directory.CreateDirectory(signaturesFolderPath);
            }
        }
    }
}
