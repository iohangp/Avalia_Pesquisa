
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Provider;
using Android.Runtime;
using Android.Graphics;
using Java.IO;
using System;

using System.IO;
using Android;
using Android.Content.PM;
using Environment = Android.OS.Environment;
using Path = System.IO.Path;
using Android.Media;
using Android.Util;
using System.ComponentModel;
using static Android.Provider.MediaStore;
using Plugin.Media;

namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "Camera")]
    public class CameraActivity : BaseActivity
    {
        byte[] bitmapData;
        ImageView imageView;
        protected override int LayoutResource => Resource.Layout.Camera;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //SetContentView(Resource.Layout.Camera);
            var btnCamera = FindViewById<Button>(Resource.Id.btnCamera);
            imageView = FindViewById<ImageView>(Resource.Id.imageView);
            btnCamera.Click += BtnCamera_Click;
        }
       
        private async void BtnCamera_Click(object sender, System.EventArgs e)
        {

            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                try
                {
                    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        CompressionQuality = 80
                    });

                    if (file == null)
                        return;

                    Toast.MakeText(this, "File Location: "+file.Path, ToastLength.Long).Show();

                    string imgBase64String = GetBase64StringForImage(file.Path);



                }

                catch { Toast.MakeText(this, "Imagem não capturada", ToastLength.Long).Show(); }
            }

        }

        protected static string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }


        public void Salvar()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alerta = builder.Create();
            AvaliacaoService avaliacaoService = new AvaliacaoService();


                var avaliacao = new Avaliacao_Imagem
                {

                    Imagem = bitmapData.ToString(),
                    idAvaliacao = 1,
                    Data = DateTime.Now,
                    idUsuario = 1

                };

                try
                {
                avaliacaoService.SalvarAvaliacaoImagem(avaliacao); ;


                    alerta.SetTitle("Sucesso!");
                    alerta.SetIcon(Android.Resource.Drawable.IcInputAdd);
                    alerta.SetMessage("Imagem Salva com Sucesso!");
                    alerta.SetButton("OK", (s, ev) =>
                    {
                        alerta.Dismiss();
                    });
                    alerta.Show();

                }

                catch

                {
                    alerta.SetMessage("Erro ao salvar ");
                    alerta.SetTitle("ERRO!");
                    alerta.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    alerta.SetMessage("Erro ao salvar a Imagem!");
                    alerta.SetButton("OK", (s, ev) =>
                    {
                        alerta.Dismiss();
                    });
                    alerta.Show();
                }
            }

        }

}
