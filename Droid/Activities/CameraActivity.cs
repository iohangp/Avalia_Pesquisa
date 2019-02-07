
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
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {

            base.OnActivityResult(requestCode, resultCode, data);

            Bitmap bitmap = (Bitmap)data.Extras.Get("data");

            imageView.SetImageBitmap(bitmap);

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
            }
            Salvar();

        }
        private void BtnCamera_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);

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
