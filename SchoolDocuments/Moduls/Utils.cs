using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Moduls
{
    static class Utils
    {
        public static string toRTF(string input)
        {  /*Конвертирует текст, в текст пригодный для RTF документа */
            //                                                                                                             FF
StringBuilder sb = new StringBuilder(input.Length * 4);

            foreach (var c in input)
            {
                if (c > 127) //заменяем не-ASCII символы на их Unicode представление
                {
                    string escape = @"\u" + ((Int16)c).ToString() + "?";
                    sb.Append(escape);
                }
                else //ASCII добавляем без изменений
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }
}
