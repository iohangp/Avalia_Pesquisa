
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
using Plugin.Media.Abstractions;
using System.Collections;
using static Android.Provider.DocumentsContract;
using Newtonsoft.Json.Linq;

namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "Camera")]
    public class CameraActivity2 : BaseActivity
    {
        byte[] byteArray;
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



        private async void BtnCamera_Click(object sender, EventArgs e)
        {
            //var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            //using (var memoryStream = new MemoryStream())
            //{
            //    photo.GetStream().CopyTo(memoryStream);
            //    photo.Dispose();
            //    byteArray = memoryStream.ToArray();
            //}

            //Salvar();
            AvaliacaoService avaliacaoService = new AvaliacaoService();
            string s = avaliacaoService.GetImagem()[0].Imagem;
            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(s);

            MemoryStream mStream = new MemoryStream();
            mStream.Write(byteArray, 0, byteArray.Length);



            //nao retirar
            //byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(s);
            //Bitmap bmp = BitmapFactory.DecodeByteArray(byteArray, 0, byteArray.Length);
            // imageView.SetImageBitmap(bmp);

        }


        public void Salvar()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alerta = builder.Create();
            AvaliacaoService avaliacaoService = new AvaliacaoService();


            var avaliacao = new Avaliacao_Imagem
            {

                Imagem = byteArray.ToString(),
                idAvaliacao = 1,
                tratamento = 1,
                repeticao = 1,
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

