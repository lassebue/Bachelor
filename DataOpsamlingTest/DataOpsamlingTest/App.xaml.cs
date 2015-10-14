using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Parse;

namespace DataOpsamlingTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            ParseClient.Initialize("4MpkWjGUq6gngl88lAZL9GuBjqlwt5dywI0zaWYG", "U7wDQU90gDR32cK50a1rU7TFRhBWAHXArBcU8Wdx");
        }
        
    }
}
