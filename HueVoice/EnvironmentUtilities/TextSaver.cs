using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using HueVoice.HueInterface.Interfaces;

namespace HueVoice.EnvironmentUtilities
{
    public class TextSaver : ITextSaver
    {
        public async Task<string> ReadFileAsync(string fileName)
        {
            var storageFile = await Package.Current.InstalledLocation.GetFileAsync(fileName);
            using (var stream = await storageFile.OpenStreamForReadAsync())
            {
                using (var streamReader = new StreamReader(stream))
                {
                    var all = await streamReader.ReadToEndAsync();
                    return all;
                }
            }

        }

        public async Task<bool> SaveFileAsync(string fileName, string text)
        {
            var storageFile = await Package.Current.InstalledLocation.GetFileAsync(fileName);
            if (storageFile == null)
            {
                storageFile = await Package.Current.InstalledLocation.CreateFileAsync(fileName);
            }
            using (var stream = await storageFile.OpenStreamForWriteAsync())
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    await streamWriter.WriteAsync(text);
                    return true;
                }
            }
        }
    }
}
