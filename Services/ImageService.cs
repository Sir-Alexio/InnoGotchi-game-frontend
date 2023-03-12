using InnoGotchi_frontend.Models;
using InnoGotchi_frontend.Services.Abstract;
using System.Drawing;

namespace InnoGotchi_frontend.Services
{
    public class ImageService:IImageService
    {
        private ImageViewModel _images;
        public ImageViewModel Images
        {
            get
            {
                if (_images != null)
                {
                    return _images;
                }

                _images = new ImageViewModel();

                _images.Bodies = new List<ImageModel>() {new ImageModel {ImagePath = "body1.svg" },
                                                    new ImageModel { ImagePath = "body2.svg" },
                                                    new ImageModel { ImagePath = "body3.svg" },
                                                    new ImageModel { ImagePath = "body4.svg" },
                                                    new ImageModel { ImagePath = "body5.svg" }};

                _images.Eyes = new List<ImageModel>() {new ImageModel {ImagePath = "eyes1.svg" },
                                                    new ImageModel { ImagePath = "eyes2.svg" },
                                                    new ImageModel { ImagePath = "eyes3.svg" },
                                                    new ImageModel { ImagePath = "eyes4.svg" },
                                                    new ImageModel { ImagePath = "eyes5.svg" },
                                                    new ImageModel { ImagePath = "eyes6.svg" }};

                _images.Mouths = new List<ImageModel>() {new ImageModel {ImagePath = "mouth1.svg" },
                                                    new ImageModel { ImagePath = "mouth2.svg" },
                                                    new ImageModel { ImagePath = "mouth3.svg" },
                                                    new ImageModel { ImagePath = "mouth4.svg" },
                                                    new ImageModel { ImagePath = "mouth5.svg" }};

                _images.Noses = new List<ImageModel>() {new ImageModel {ImagePath = "nose1.svg" },
                                                    new ImageModel { ImagePath = "nose2.svg" },
                                                    new ImageModel { ImagePath = "nose3.svg" },
                                                    new ImageModel { ImagePath = "nose4.svg" },
                                                    new ImageModel { ImagePath = "nose5.svg" },
                                                    new ImageModel { ImagePath = "nose6.svg"}};
                
                return _images;
            }
        }
    }
}
