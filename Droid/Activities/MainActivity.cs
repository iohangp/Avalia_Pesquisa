//using Android.App;
//using Android.Content;
//using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using System.Threading;
using Avalia_Pesquisa.Droid.Helpers;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Avalia_Pesquisa.Droid.Activities;

namespace Avalia_Pesquisa.Droid
{
    [Activity(Label = "@string/app_name", Icon = "@mipmap/icon",
        LaunchMode = LaunchMode.SingleInstance,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : BaseActivity
    {
       // protected override int LayoutResource => Resource.Layout.activity_main;

        protected override int LayoutResource => Resource.Layout.Menu;
        ViewPager pager;
        TabsAdapter adapter;
        DataBase db;
        CloudDataStore CloudData;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            CriarBancoDados();


            adapter = new TabsAdapter(this, SupportFragmentManager);
            // pager = FindViewById<ViewPager>(Resource.Id.viewpager);
            /*   var tabs = FindViewById<TabLayout>(Resource.Id.tabs);
               pager.Adapter = adapter;
               tabs.SetupWithViewPager(pager);
               pager.OffscreenPageLimit = 3;

               pager.PageSelected += (sender, args) =>
               {
                   var fragment = adapter.InstantiateItem(pager, args.Position) as IFragmentVisible;

                   fragment?.BecameVisible();
               };

               Toolbar.MenuItemClick += (sender, e) =>
               {
                   var intent = new Intent(this, typeof(AddItemActivity)); ;
                   StartActivity(intent);
               };

               SupportActionBar.SetDisplayHomeAsUpEnabled(false);
               SupportActionBar.SetHomeButtonEnabled(false);
               */
            CheckInstalacao();

            var session = Settings.GeneralSettings;
            if (session == null)
            {
                var intent = new Intent(this, typeof(LoginActivity)); ;
                StartActivity(intent);
            }


            Button button = FindViewById<Button>(Resource.Id.BTAplicacao);
            button.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(AplicacaoActivity)); ;
                StartActivity(intent);
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_logoff:
                    Settings.GeneralSettings = null;
                    var intent = new Intent(this, typeof(LoginActivity)); ;
                    StartActivity(intent);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void CriarBancoDados()
        {
            db = new DataBase();
            db.CriarBancoDeDados();
        }

        private bool CheckInstalacao()
        {
            db = new DataBase();

            if (!db.CheckInstalacao())
            {

                var intent = new Intent(this, typeof(ConfigAparelhoActivity)); ;
                StartActivity(intent);

            }


            return true;
        }

    }

    class TabsAdapter : FragmentStatePagerAdapter
    {
        string[] titles;

        public override int Count => titles.Length;

        public TabsAdapter(Context context, Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
            titles = context.Resources.GetTextArray(Resource.Array.sections);
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position) =>
                            new Java.Lang.String(titles[position]);

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            switch (position)
            {
                case 0: return BrowseFragment.NewInstance();
                case 1: return AboutFragment.NewInstance();
            }
            return null;
        }

        public override int GetItemPosition(Java.Lang.Object frag) => PositionNone;
    }


}
