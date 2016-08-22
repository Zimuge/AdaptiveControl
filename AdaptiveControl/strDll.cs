using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace AdaptiveControl
{
    class strDll
    {
        [DllImport("str.dll", CharSet = CharSet.Ansi)]
        public static extern bool str_init(IntPtr handle);

        [DllImport("str.dll", CharSet = CharSet.Ansi)]
        public static extern bool str_exit(IntPtr handle);

        [DllImport("str.dll", CharSet = CharSet.Ansi)]
        public static extern double str_read();

        [DllImport("str.dll", CharSet = CharSet.Ansi)]
        public static extern bool str_write(double data1, double data2);
    }
}
