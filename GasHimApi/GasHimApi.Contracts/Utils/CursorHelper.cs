using System.Text;

namespace GasHimApi.Contracts.Utils
{
    public static class CursorHelper
    {
        public static string Encode(string name, int id)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            try
            {
                var escaped = Uri.EscapeDataString(name);
                var raw = $"{escaped}|{id}";
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(raw));
            }
            catch
            {
                return string.Empty;
            }
        }

        public static (string name, int id) Decode(string cursor)
        {
            if (string.IsNullOrEmpty(cursor))
                return (string.Empty, 0);

            try
            {
                var raw = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
                var parts = raw.Split('|');
                if (parts.Length != 2) 
                    return (string.Empty, 0);

                var name = Uri.UnescapeDataString(parts[0]);
                var ok = int.TryParse(parts[1], out var id);
                return (name, ok ? id : 0);
            }
            catch
            {
                return (string.Empty, 0);
            }
        }
    }
}
