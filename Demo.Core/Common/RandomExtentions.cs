using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Core.Common
{
    public static class RandomExtentions
    {
        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="len"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public  static List<string> GetRandString(int len, int count)
        {
            double max_value = Math.Pow(36, len);
            if (max_value > long.MaxValue)
            {
                //ShowError(string.Format("Math.Pow(36, {0}) 超出 long最大值！", len));
                return null;
            }
            long all_count = (long)max_value;
            long stepLong = all_count / count;
            if (stepLong > int.MaxValue)
            {
                //ShowError(string.Format("stepLong ({0}) 超出 int最大值！", stepLong));
                return null;
            }
            int step = (int)stepLong;
            if (step < 3)
            {
                //ShowError("step 不能小于 3!");
                return null;
            }
            long begin = 0;
            List<string> list = new List<string>();
            Random rand = new Random();
            while (true)
            {
                long value = rand.Next(1, step) + begin;
                begin += step;
                list.Add(GetChart(len, value));
                if (list.Count == count)
                {
                    break;
                }
            }
            list = SortByRandom(list);
            return list;
        }
        //将数字转化为字符串
        ////数字+字母
        private static  string CHAR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static  string GetRandonString(int lenght)
        {
            Random rand = new Random();
            string str = "";
            for (int i = 0; i < lenght; i++)
            {
                int value = rand.Next(0, 35);
                str += CHAR[value];
            }
            return str;
        }

        /// <summary>
        /// 将数字转化成字符串
        /// </summary>
        /// <param name="len"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static  string GetChart(int len, long value)
        {
            StringBuilder str = new StringBuilder();
            while (true)
            {
                str.Append(CHAR[(int)(value % 36)]);
                value = value / 36;
                if (str.Length == len)
                {
                    break;
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// 随机排序
        /// </summary>
        /// <param name="charList"></param>
        /// <returns></returns>
        private static List<string> SortByRandom(List<string> charList)
        {
            Random rand = new Random();
            for (int i = 0; i < charList.Count; i++)
            {
                int index = rand.Next(0, charList.Count);
                string temp = charList[i];
                charList[i] = charList[index];
                charList[index] = temp;
            }
            return charList;
        }
    }
}