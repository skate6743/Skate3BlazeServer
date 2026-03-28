using Newtonsoft.Json;
using System.Net;

namespace Servers.HTTP.CustomEndpoints
{
    public class SpoofUsernameEndpoint
    {
        private static readonly object _spoofLock = new();

        private static bool IPWhitelisted(IPEndPoint endpoint)
        {
            string IPsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "adminips.txt");
            if (File.Exists(IPsFilePath))
            {
                string[] IPs = File.ReadAllLines(IPsFilePath);
                string testIP = endpoint.Address.ToString();
                return IPs.Any(x => x == testIP);
            }
            else
            {
                return false;
            }
        }

        public static async Task<string> GetPanel(HttpListenerContext ctx)
        {
            if (!IPWhitelisted(ctx.Request.RemoteEndPoint))
                return "Unauthorized";

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "spoofed_usernames.json");
            Dictionary<string, string> spoofs = File.Exists(path)
                ? JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path))
                : new Dictionary<string, string>();

            if (spoofs == null)
                return "Unauthorized";

            string rows = string.Join("\n", spoofs.Select(kv =>
                $"<tr><td>{kv.Key}</td><td>{kv.Value}</td><td><button onclick=\"fetch('/removespoof?name={kv.Key}').then(()=>location.reload())\">Remove</button></td></tr>"));

            return $@"
                <html><body>
                <table border='1' cellpadding='5' cellspacing='0'>
                <tr><th>RPCN Name</th><th>Spoofed Name</th><th></th></tr>
                {rows}
                </table>
                <br>
                <input id='rpcn' placeholder='RPCN Name'>
                <input id='spoofed' placeholder='Spoofed Name'>
                <button onclick=""fetch('/spoofusername?name='+document.getElementById('rpcn').value+'&newname='+document.getElementById('spoofed').value).then(()=>setTimeout(()=>location.reload(),500))"">Add</button>
                </body></html>";
        }

        private static string FixName(string name)
        {
            return name.Replace("\"", "").Replace("'", "").Replace("%", "");
        }

        public static async Task SpoofUsername(HttpListenerContext ctx)
        {
            if (!IPWhitelisted(ctx.Request.RemoteEndPoint))
                return;

            string name = ctx.Request.QueryString["name"];
            string newName = ctx.Request.QueryString["newname"];
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(newName))
                return;

            name = FixName(name);
            newName = FixName(newName);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "spoofed_usernames.json");

            lock (_spoofLock)
            {
                var spoofs = File.Exists(path)
                    ? JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path))
                    : new Dictionary<string, string>();

                spoofs[name] = newName;
                File.WriteAllText(path, JsonConvert.SerializeObject(spoofs, Formatting.Indented));
            }
        }

        public static async Task RemoveSpoof(HttpListenerContext ctx)
        {
            if (!IPWhitelisted(ctx.Request.RemoteEndPoint))
                return;

            string name = ctx.Request.QueryString["name"];
            if (string.IsNullOrEmpty(name))
                return;

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "spoofed_usernames.json");

            lock (_spoofLock)
            {
                if (!File.Exists(path))
                    return;

                var spoofs = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
                spoofs.Remove(name);
                File.WriteAllText(path, JsonConvert.SerializeObject(spoofs, Formatting.Indented));
            }
        }
    }
}
