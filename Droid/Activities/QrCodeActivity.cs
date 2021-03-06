﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Vision;
using Android.Gms.Vision.Barcodes;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using static Android.Gms.Vision.Detector;
using static Android.Text.Layout;

namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "Scanear Estudo")]
    public class QrCodeActivity : BaseActivity, ISurfaceHolderCallback , IProcessor
    {  
        SurfaceView surfaceView;
        TextView txtResult;
        BarcodeDetector barcodeDetector;
        CameraSource cameraSource;
        const int RequestCameraPermisionID = 1001;
        string activity;

        protected override int LayoutResource => Resource.Layout.QrCode;

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestCameraPermisionID:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
                            {
                                //Request Permision  
                                ActivityCompat.RequestPermissions(this, new string[]
                                {
                    Manifest.Permission.Camera
                                }, RequestCameraPermisionID);
                                return;
                            }
                            try
                            {
                                cameraSource.Start(surfaceView.Holder);
                            }
                            catch (InvalidOperationException)
                            {
                            }
                        }
                    }
                    break;
            }
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource  
           // SetContentView(Resource.Layout.QrCode);
            surfaceView = FindViewById<SurfaceView>(Resource.Id.cameraView);
            txtResult = FindViewById<TextView>(Resource.Id.txtResult);
            Bitmap bitMap = BitmapFactory.DecodeResource(ApplicationContext
                                                         .Resources, Resource.Drawable.logo_3m);
            barcodeDetector = new BarcodeDetector.Builder(this)
                .SetBarcodeFormats(BarcodeFormat.QrCode)
                .Build();
            cameraSource = new CameraSource
                .Builder(this, barcodeDetector)
                .SetRequestedPreviewSize(1600, 1024)
                .SetAutoFocusEnabled(true)
                .SetRequestedFps(24.0f)
                .Build();
            surfaceView.Holder.AddCallback(this);
            barcodeDetector.SetProcessor(this);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.OnBackPressed();
                    return true;

            }
            return base.OnOptionsItemSelected(item);
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
        }
        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
            {
                //Request Permision  
                ActivityCompat.RequestPermissions(this, new string[]
                {
                    Manifest.Permission.Camera
                }, RequestCameraPermisionID);
                return;
            }
            try
            {
                cameraSource.Start(surfaceView.Holder);
            }
            catch (InvalidOperationException)
            {
            }
        }
        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            cameraSource.Stop();
        }
        public void ReceiveDetections(Detections detections)
        {
            SparseArray qrcodes = detections.DetectedItems;
            if (qrcodes.Size() != 0)
            {
                txtResult.Post(() => {
                    Vibrator vibrator = (Vibrator)GetSystemService(Context.VibratorService);
                    vibrator.Vibrate(100);
                    txtResult.Text = ((Barcode)qrcodes.ValueAt(0)).RawValue;

                    Intent intent = new Intent();
                    intent.PutExtra("qrcode", txtResult.Text);
                    SetResult(Result.Ok,intent);

                    Finish();

                });

                
            }
        }
        public void Release()
        {

        }

        public override void OnBackPressed()
        {
            Finish();

        }

    }
  
}

