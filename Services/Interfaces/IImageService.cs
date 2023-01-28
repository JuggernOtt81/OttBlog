
using OttBlog.ViewModels;
using OttBlog.Services;
using OttBlog.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OttBlog.Services.Interfaces
{
    public interface IImageService
    {
        public Task<byte[]> EncodeImageAsync(IFormFile file);
        public Task<byte[]> EncodeImageAsync(string fileName);
        string DecodeImage(byte[] data, string type);
        string ContentType(IFormFile file);
        int Size(IFormFile file);
    }
}