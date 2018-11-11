    using System;

using Android.App;
using Android.OS;
using Android.Widget;
using Android.Support.Design.Widget;
using System.Threading;
using Android.Views;
using Android.Content;
using Android.Preferences;
using Avalia_Pesquisa.Droid.Helpers;

namespace Avalia_Pesquisa.Droid
{
    [Activity(Label = "Login")]
    public class LoginActivity : Activity
    {
        Button loginButton;
        EditText txtSenha, txtLogin;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            // Create your application here
            SetContentView(Resource.Layout.Login);

            loginButton = FindViewById<Button>(Resource.Id.BTLogin);
            txtSenha = FindViewById<EditText>(Resource.Id.EDSenha);
            txtLogin = FindViewById<EditText>(Resource.Id.EDLogin);

            loginButton.Click += LoginButton_Click;

            txtLogin.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    e.Handled = true;
                }
            };

            txtSenha.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    e.Handled = true;
                    loginButton.PerformClick();
                }
            };
        }

        protected internal void LoginButton_Click(object sender, EventArgs e)
        {
            UsuarioService userService = new UsuarioService();
            var user = userService.GetUsuario(txtLogin.Text, txtSenha.Text);
            if (user.Count > 0)
            {
                // AppSettings.AddOrUpdateValue(SettingsKey, value);
                Settings.GeneralSettings = user[0].IdUsuario.ToString();             
                var intent = new Intent(this, typeof(MainActivity)); ;
                StartActivity(intent);
                Finish();
            }
            else
            {
              /*  txtLogin.Text = null;
                txtSenha.Text = null;
                txtLogin.RequestFocus(); */
                Toast.MakeText(this, "Nenhum usuário encontrado!", ToastLength.Short).Show();
            }
            
        }
    }
}
