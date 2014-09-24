using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudyConsoleProject
{
    static class YounExtention
    {
        public static bool IsNullOrEmpty(this String str)
        {
            if (str == null || str == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNullList(this IEnumerable<string> list)
        {
            if (list == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
