using System;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingAppAPI.Helpers;
using DatingAppAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DatingAppAPI.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config){

          var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
          _cloudinary = new Cloudinary(acc);

        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var imageUploadResult = new ImageUploadResult();

            if(file.Length > 0) {

                //convert file into stream of data.
                using var stream = file.OpenReadStream();
                
                //configure uplaod params
                var uploadParams = new ImageUploadParams {

                    //file.
                    File = new FileDescription(file.FileName),

                    //transform image. crop crops the image. fill is the square type. gravity tells what to crop.
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };

                //upload image in cloudinary.
                imageUploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return imageUploadResult;
        }

        public Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
           var deletionParams = new DeletionParams(publicId);
           
           var result = _cloudinary.DestroyAsync(deletionParams);

           return result;
        }
    }
}