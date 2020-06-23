using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace DefaultStylesNet5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var types 
                = typeof(Button)
                    .Assembly
                    .GetTypes()
                    .Where(t => Inherited(t, typeof(Control)))
                    .Select(t => t)
                    .OrderBy( t => t.Name )
                    .ToArray();

            m_List.ItemsSource = types;
        }

        private bool Inherited( Type type, Type checkedType )
        {
            Type? baseType = type;
            do
            {
                baseType = baseType.BaseType;
                if (baseType == checkedType) return true;

            } while (!(baseType is null) && baseType != typeof(Object));

            return false;
        }

        private void MainWindow_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0] is Type controlType)
            {
                var res = Application.Current.FindResource(controlType);

                if (res is {})
                {
                    var xmlSettings = new XmlWriterSettings
                    {
                        Indent = true,
                        Encoding = Encoding.UTF8
                    };

                    var sb = new StringBuilder();

                    using var w = XmlWriter.Create(sb, xmlSettings);

                    XamlWriter.Save(res, w);
                    m_Xaml.Text = sb.ToString();
                }


            }

        }
    }
}
