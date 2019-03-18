using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using MeusPedidos.Activities;
using com.refractored.monodroidtoolkit.imageloader;
using Fragment = Android.Support.V4.App.Fragment;

namespace MeusPedidos.Fragments
{
    public class AboutAppFragment : Fragment
    {
        public AboutAppFragment()
        {
            this.RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.about_app, null);
           
            return view;
        }
    }
}
