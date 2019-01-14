//using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;

namespace Zh.Persistence
{
    public enum ImageSize
    {
        Full,
        Medium,
        Small
    }

    public class ItemImage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[ForeignKey("Article")]
        [Required]
        public Item Item { get; set; }
        [Required]
        public byte[] ImageData { get; set; }

        [NotMapped]
        public object LinkData => new { id = Id, hash = ImageHash() };

        public byte[] SizedData(ImageSize requestSize = ImageSize.Full)
        {
            if (requestSize == ImageSize.Full)
            {
                return ImageData;
            }
            else
            {
                using (var originalMS = new MemoryStream(ImageData))
                {
                    Bitmap originalBM = new Bitmap(originalMS);
                    int width, height;
                    switch (requestSize)
                    {
                        case ImageSize.Medium:
                            width = 1000;
                            break;
                        case ImageSize.Small:
                            width = 500;
                            break;
                        default:
                            width = originalBM.Width;
                            break;
                    }
                    height = (int) ((float)width / originalBM.Width * originalBM.Height);
                    Image result = new Bitmap(originalBM, width, height);
                    var jpg = System.Drawing.Imaging.ImageFormat.Jpeg;
                    using (var outStream = new MemoryStream())
                    {
                        result.Save(outStream, jpg);
                        return outStream.ToArray();
                    }
                }
            }
        }

        public string ImageHash()
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                return Convert.ToBase64String(sha1.ComputeHash(ImageData));
            }
        }
    }
}