using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Infrastructure.FileSystem
{
    public class LocalFileImageService : IImageService
    {
        private readonly IHostingEnvironment _environment;

        public LocalFileImageService(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public byte[] GetImageBytesById(int id)
        {
            try
            {
                var contentRoot = _environment.ContentRootPath + "//Pics";
                var path = Path.Combine(contentRoot, id + ".png");
                return File.ReadAllBytes(path);
            }
            catch (FileNotFoundException ex)
            {
                throw new CatalogImageMissingException(ex);
            }
        }
    }
}
