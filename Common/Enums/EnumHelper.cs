using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Enums
{
    public static class EnumHelper
    {
        public static string GetEnumName<T>(this int enumTypeId)
        {
            return Enum.GetName(typeof(T), enumTypeId);
        }

        public static EnumModel[] GetEnumList<T>()
        {
            var arrays = Enum.GetValues(typeof(T));
            List<EnumModel> lists = new List<EnumModel>();
            foreach (var item in arrays)
            {
                lists.Add(new EnumModel { Id = (int)item, Name = item.ToString() });
            }
            return lists.ToArray();
        }

        public class EnumModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
