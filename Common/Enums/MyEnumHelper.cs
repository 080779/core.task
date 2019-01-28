using DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Enums
{
    public static class MyEnumHelper
    {
        public static string GetEnumName<T>(this int enumTypeId)
        {
            return Enum.GetName(typeof(T), enumTypeId);
        }

        public static EnumDTO[] GetEnumList<T>()
        {
            var arrays = Enum.GetValues(typeof(T));
            List<EnumDTO> lists = new List<EnumDTO>();
            foreach (var item in arrays)
            {
                lists.Add(new EnumDTO { Id = (int)item, Name = item.ToString() });
            }
            return lists.ToArray();
        }
    }
}
