using System.Threading.Tasks;

namespace HueVoice.HueInterface.Interfaces
{
    public interface ITextSaver
    {
        Task<bool> SaveFileAsync(string fileName, string text);
        Task<string> ReadFileAsync(string fileName);
    }
}
