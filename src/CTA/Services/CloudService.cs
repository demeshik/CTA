using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;

namespace CTA.Services
{
    public class CloudService : ICloudInterface
    {
        public Cloudinary Configuration()
        {
            return new Cloudinary("cloudinary://699756615932382:m5MHlmJJkZmGa_H_7VPdYpo6JyA@djrazor308");
        }
    }
}
