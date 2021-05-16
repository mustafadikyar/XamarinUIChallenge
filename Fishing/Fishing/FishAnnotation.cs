using Fishing.Helpers;
using Fishing.Models;
using SkiaSharp;
using System.Reflection;
using System.Threading.Tasks;

namespace Fishing
{
    internal class FishAnnotation
    {
        public SKBitmap OnscreenAnnotation { get; set; }
        public SKBitmap LeftAnnotation { get; set; }
        public SKBitmap RightAnnotation { get; set; }

        int width = 250, height = 125, padding = 25, radius = 25;

        SKPaint annotationBackground = new SKPaint()
        {
            Color = SKColors.White,
            Style = SKPaintStyle.StrokeAndFill,
            IsAntialias = true
        };

        public async Task CreateLeftAnnotation(FishModel fish)
        {
            SKBitmap bitmap = await CreateBaseAnnotation(fish);
            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                // draw the arrow
                SKPath path = new SKPath();
                var halfHeight = (float)(bitmap.Height * .5);
                path.MoveTo(0, halfHeight);
                path.LineTo(padding, halfHeight - padding);
                path.LineTo(padding, halfHeight + padding);
                path.LineTo(0, halfHeight);
                path.Close();
                canvas.DrawPath(path, annotationBackground);
            }
            LeftAnnotation = bitmap;
        }

        SKPaint headerPaint = new SKPaint()
        {
            Color = SKColors.Black,
            IsAntialias = true,
            TextSize = 40f,
            Typeface = GetTypeface("OpenSans-Bold.ttf")
        };

        SKPaint bodyPaint = new SKPaint()
        {
            Color = SKColors.Black,
            IsAntialias = true,
            TextSize = 30f,
            Typeface = GetTypeface("OpenSans-Regular.ttf")
        };

        SKPaint debugPaint = new SKPaint()
        {
            Color = SKColors.Red,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1,
        };

        public static SKTypeface GetTypeface(string fullFontName)
        {
            SKTypeface result;

            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("Fishing.Fonts." + fullFontName);
            if (stream == null)
                return null;

            result = SKTypeface.FromStream(stream);
            return result;
        }

        public async Task CreateOnScreenAnnotation(FishModel fish)
        {
            int bitmapHeight = height + (2 * padding);

            // measure header
            string headerText = fish.Name;
            SKRect headerBounds = new SKRect();
            headerPaint.MeasureText(headerText, ref headerBounds);

            string bodyText = fish.FishSize;
            SKRect bodyBounds = new SKRect();
            bodyPaint.MeasureText(bodyText, ref bodyBounds);

            // position our fish
            SKRect fishBounds = new SKRect(0, 0, (height - padding * 2), (height - padding * 2));
            fishBounds.Offset(2 * padding, 2 * padding);

            // position the text
            var lineSpacing = 10;
            var totalLineHeight = headerBounds.Height + lineSpacing + bodyBounds.Height;
            var topSpacing = (bitmapHeight - totalLineHeight) / 2;

            headerBounds.Location = new SKPoint(fishBounds.Right + padding, topSpacing);
            bodyBounds.Location = new SKPoint(fishBounds.Right + padding, headerBounds.Bottom + lineSpacing);

            var bitmapWidth = headerBounds.Width + fishBounds.Width + (5 * padding);

            SKBitmap bitmap = new SKBitmap((int)bitmapWidth, bitmapHeight);
            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                canvas.Clear();
                // draw a rounded rect
                SKRect outline = new SKRect(0, 0, bitmap.Width, bitmap.Height);
                outline.Inflate(-padding, -padding);
                canvas.DrawRoundRect(outline, radius, radius, annotationBackground);

                // draw our fish
                var fishImage = await BitmapHelper.LoadBitmapFromUrl(fish.Image);
                canvas.DrawBitmap(fishImage, fishBounds, BitmapStretch.AspectFit, BitmapAlignment.Center, BitmapAlignment.Center);

                // draw our text
                canvas.DrawText(headerText, headerBounds.Left, headerBounds.Bottom, headerPaint);
                canvas.DrawText(bodyText, bodyBounds.Left, bodyBounds.Bottom, bodyPaint);

                // draw an arrow
                SKPath path = new SKPath();
                var halfWidth = (float)(bitmap.Width * .5);
                path.MoveTo(halfWidth, bitmap.Height);
                path.LineTo(halfWidth - padding, bitmap.Height - padding);
                path.LineTo(halfWidth + padding, bitmap.Height - padding);
                path.LineTo(halfWidth, bitmap.Height);
                path.Close();
                canvas.DrawPath(path, annotationBackground);
            }
            OnscreenAnnotation = bitmap;
        }

        public async Task CreateRightAnnotation(FishModel fish)
        {
            SKBitmap bitmap = await CreateBaseAnnotation(fish);
            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                // draw the arrow
                SKPath path = new SKPath();
                var halfHeight = (float)(bitmap.Height * .5);
                path.MoveTo(bitmap.Width, halfHeight);
                path.LineTo(bitmap.Width - padding, halfHeight - padding);
                path.LineTo(bitmap.Width - padding, halfHeight + padding);
                path.LineTo(bitmap.Width, halfHeight);
                path.Close();
                canvas.DrawPath(path, annotationBackground);
            }
            RightAnnotation = bitmap;
        }

        internal async Task<SKBitmap> CreateBaseAnnotation(FishModel fish)
        {
            // create a bitmap
            SKBitmap bitmap = new SKBitmap(width + 2 * padding, height + 2 * padding);
            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                canvas.Clear();

                // draw a rounded rect
                SKRect outline = new SKRect(0, 0, bitmap.Width, bitmap.Height);
                outline.Inflate(-padding, -padding);
                canvas.DrawRoundRect(outline, radius, radius, annotationBackground);

                // draw a fish in the middle
                outline.Inflate(-padding, -padding);
                var fishImage = await BitmapHelper.LoadBitmapFromUrl(fish.Image);
                //canvas.DrawBitmap(fishImage, outline);
                canvas.DrawBitmap(fishImage, outline, BitmapStretch.AspectFit, BitmapAlignment.Center, BitmapAlignment.Center);
            }
            return bitmap;
        }
    }
}