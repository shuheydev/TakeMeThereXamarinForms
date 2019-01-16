using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Color = System.Drawing.Color;


namespace TakeMeThereXamarinForms.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly double cycleTime = 2000;
        private Stopwatch stopwatch = new Stopwatch();
        private bool pageIsActive;
        private float t;

        public MainPage()
        {
            InitializeComponent();

            skCanvasViewCompass.PaintSurface += OnCanvasViewCompassPaintSurface;
            skCanvasViewTargetDirection.PaintSurface += OnCanvasViewTargetDirectionPaintSurface;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pageIsActive = true;
            stopwatch.Start();

            Device.StartTimer(TimeSpan.FromMilliseconds(33), () =>
            {
                t = (float)(stopwatch.Elapsed.TotalMilliseconds % cycleTime / (cycleTime));

                skCanvasViewTargetDirection.InvalidateSurface();

                if (!pageIsActive)
                    stopwatch.Stop();

                return pageIsActive;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            pageIsActive = false;
        }


        /// <summary>
        /// 目的地の方角を表示するcanvasの初期設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnCanvasViewTargetDirectionPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            using (SKPaint strokePaint = new SKPaint())
            {
                strokePaint.Style = SKPaintStyle.Stroke;
                strokePaint.Color = SKColors.LawnGreen;
                strokePaint.StrokeCap = SKStrokeCap.Square;
                strokePaint.StrokeWidth = 1;

                //座標変換
                canvas.Translate(info.Width / 2f, info.Height / 2f);
                canvas.Scale(Math.Min(info.Width / 200f, info.Height / 200f));


                for (int circle = 0; circle < 1; circle++)
                {
                    float radius = 9 * (circle + t);

                    strokePaint.Color = new SKColor(0x7c, 0xfc, 0x00, (byte)(255 * (circle == 0 ? (1 - t) : 1)));

                    canvas.DrawCircle(0, -90, radius, strokePaint);
                }
            }
        }


        /// <summary>
        /// 方位を表示するcanvasの初期設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnCanvasViewCompassPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            using (SKPaint strokePaint = new SKPaint())
            using (SKPaint textPaint = new SKPaint { Color = SKColors.Black, TextSize = 15 })
            {
                strokePaint.Style = SKPaintStyle.Stroke;
                strokePaint.Color = SKColors.Black;
                strokePaint.StrokeCap = SKStrokeCap.Square;


                //座標変換
                canvas.Translate(info.Width / 2f, info.Height / 2f);
                canvas.Scale(Math.Min(info.Width / 200f, info.Height / 200f));

                //テキスト準備
                SKRect textBounds = new SKRect();
                string directionText = "N";
                for (int angle = 0; angle < 360; angle += 6)
                {
                    strokePaint.StrokeWidth = 1;

                    var yBaseValue = -70;//y座標の基準値。メモリの長さやテキストの位置に使う
                    var yEndPoint = yBaseValue - 5;

                    if (angle == 0 || angle == 90 || angle == 180 || angle == 270)
                    {
                        //ちょっと長めに
                        yEndPoint = yBaseValue - 10;
                        //ちょっと太めに
                        strokePaint.StrokeWidth = 1.5f;

                        //東西南北
                        switch (angle)
                        {
                            case 0:
                                directionText = "N";
                                break;
                            case 90:
                                directionText = "E";
                                break;
                            case 180:
                                directionText = "S";
                                break;
                            case 270:
                                directionText = "W";
                                break;
                        }


                        var textWidth = textPaint.MeasureText(directionText, ref textBounds);

                        canvas.DrawText(directionText, -textWidth / 2f, yBaseValue - 15, textPaint);
                    }

                    canvas.DrawLine(0, yBaseValue, 0, yEndPoint, strokePaint);
                    canvas.RotateDegrees(6);
                }
            }
        }
    }
}