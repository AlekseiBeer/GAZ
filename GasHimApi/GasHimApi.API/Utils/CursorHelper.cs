using System;
using System.Text;

namespace GasHimApi.API.Utils
{
    public static class CursorHelper
    {
        public static string Encode(string name, int id)
        {
            var raw = $"{name}|{id}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(raw));
        }

        public static (string name, int id) Decode(string cursor)
        {
            var raw = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
            var parts = raw.Split('|');
            if (parts.Length != 2) return (string.Empty, 0);
            var ok = int.TryParse(parts[1], out var id);
            return (parts[0], ok ? id : 0);
        }
    }
}


