using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;



namespace Front_End_WASP
{
    public class StringWithChoiceVM : DependencyObject
    {
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value", typeof(string), typeof(StringWithChoiceVM));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(
                     "IsSelected", typeof(bool), typeof(StringWithChoiceVM));
    }

    public class ListChoiceVM : DependencyObject
    {
        public ListChoiceVM(List<string> initial)
        {
            Values = new ObservableCollection<StringWithChoiceVM>();
            foreach (var n in initial)
                Values.Add(new StringWithChoiceVM() { Value = n, IsSelected = true });
        }

        public ObservableCollection<StringWithChoiceVM> Values { get; private set; }

        public IEnumerable<string> GetSelectedItems()
        {
            return Values.Where(v => v.IsSelected).Select(v => v.Value);
        }

        public IEnumerable<string> GetUnselectedItems()
        {
            return Values.Where(v => !v.IsSelected).Select(v => v.Value);
        }
    }
}
