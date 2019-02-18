using Xamarin.Forms;
using TakeMeThereXamarinForms.Models;
using System.Collections.ObjectModel;

namespace TakeMeThereXamarinForms.Views
{
    public partial class SelectTargetPage : ContentPage
    {
        public SelectTargetPage()
        {
            InitializeComponent();
        }

        private void ListView_TargetList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //選択時のハイライトを無効にするための記述。
            var s = sender as ListView;

            if (s == null)
                return;

            s.SelectedItem = null;
        }
    }
}
