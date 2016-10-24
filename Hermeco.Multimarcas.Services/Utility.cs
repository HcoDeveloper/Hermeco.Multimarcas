using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Common;

namespace Hermeco.Multimarcas.Services
{
    class Utility
    {
        public static DataSet DecompressedData(Object objectData)
        {
            try
            {
                DataSet dsData = new DataSet("ResultData");
                byte[] data = (byte[])objectData;
                MemoryStream stream = new MemoryStream(data);
                GZipStream zipStream = new GZipStream(stream, CompressionMode.Decompress);
                dsData.ReadXml(zipStream, XmlReadMode.ReadSchema);
                zipStream.Close();
                stream.Close();

                data = null;
                zipStream.Dispose();
                stream.Dispose();

                return dsData;
            }
            catch (Exception FileException)
            {
                //TODO: logging on WCF logging service
                return null;
            }
        }

        public static byte[] resizeImage(byte[] imageDB, int canvasWidth, int canvasHeight)
        {
            var brush = new SolidBrush(Color.White);
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
            Bitmap bitmap = (Bitmap)tc.ConvertFrom(imageDB);
            bitmap.SetResolution(96, 96);

            double scale = Math.Min((double) canvasWidth / bitmap.Width, (double) canvasHeight / bitmap.Height);

            var bmp = new Bitmap((int)canvasWidth, (int)canvasHeight);
            var graph = Graphics.FromImage(bmp);

            // uncomment for higher quality output
            graph.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graph.CompositingQuality = CompositingQuality.HighQuality;
            graph.SmoothingMode = SmoothingMode.AntiAlias;

            var scaleWidth = (int)(bitmap.Width * scale);
            var scaleHeight = (int)(bitmap.Height * scale);

            graph.FillRectangle(brush, new RectangleF(0, 0, canvasWidth, canvasHeight));
            graph.DrawImage(bitmap, new Rectangle(((int)canvasWidth - scaleWidth) / 2, ((int)canvasHeight - scaleHeight) / 2, scaleWidth, scaleHeight));

            EncoderParameters encoder_params = new EncoderParameters(1);
            encoder_params.Param[0] = new EncoderParameter( System.Drawing.Imaging.Encoder.Quality, 80L);

            ImageCodecInfo image_codec_info = GetEncoderInfo("image/jpeg");
            byte[] bytes;
            var stream = new System.IO.MemoryStream();
            bmp.Save(stream, image_codec_info, encoder_params);
            bytes = stream.ToArray();
           
            //ImageConverter converter = new ImageConverter();
            //byte[] bytes = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
            return bytes;
            //return Convert.ToBase64String(bytes); 
        }

        private static ImageCodecInfo GetEncoderInfo(string mime_type)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i <= encoders.Length; i++)
            {
                if (encoders[i].MimeType == mime_type) return encoders[i];
            }
            return null;
        }

        public static string GetMundo(string Genero, string Edad)
        {
            string strMundo = Edad;
            string strGenero = Genero;
            string NombreMundo = string.Empty;
            switch (strMundo)
            {
                case "NIÑOS":
                    switch (strGenero)
                    {
                        case "FEMENINO":
                            NombreMundo = "NIÑA";
                            break;
                        case "MASCULINO":
                            NombreMundo = "NIÑO";
                            break;
                        default:

                            break;
                    }
                    break;
                case "BEBES":
                    NombreMundo = "BEBE";
                    break;
                case "PRIMI":
                    NombreMundo = "PRIMI";
                    break;
                default:

                    break;
            }
            return NombreMundo;
        }

    }
}
