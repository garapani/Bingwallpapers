using System.Windows;
using System.Windows.Controls;

namespace BingWallpapers.Controls
{
    public class LongListSelector : Microsoft.Phone.Controls.LongListSelector
    {
        #region Static Properties

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem",
                typeof(object),
                typeof(LongListSelector),
                new PropertyMetadata(null, OnSelectedItemChanged)
            );

        #endregion

        #region Constructor

        public LongListSelector()
        {
            SelectionChanged += LongListSelectorSelectionChanged;
        }

        #endregion

        #region Properties

        public new object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        #endregion

        #region Event Handlers

        public void LongListSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItem = base.SelectedItem;
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = (LongListSelector)d;
            selector.SelectedItem = e.NewValue;
        }

        #endregion
    }
}
