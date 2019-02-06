
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Provider;
using Android.Runtime;
using Android.Graphics;
namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "Camera")]
    public class CameraActivity : BaseActivity
    {

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
        }
        private void BtnCamera_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);
        }
    }
}